using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using CorporatePortal.BL.Interfaces;
using CorporatePortal.Common.Constants;
using CorporatePortal.Web.Common.HttpClients;
using CorporatePortal.Web.Common.Models;
using CorporatePortal.Web.Common.Models.Photo;
using CorporatePortal.Web.Common.Models.UserData;
using Microsoft.Extensions.Configuration;

namespace CorporatePortal.BL.Services;

public class ExternalUserDataService : IExternalUserDataService
{
    private readonly IConfiguration _configuration;
    private readonly IUserServiceApiClient _userPhotoClient;
    private readonly string _authHeader;
    
    public ExternalUserDataService(IConfiguration configuration, IUserServiceApiClient userPhotoClient)
    {
        _configuration = configuration;
        _userPhotoClient = userPhotoClient;
        _authHeader = CreateAuthHeader();
    }
    
    public async Task<UserPhotoResponseModel?> SendPhotoRequestAsync(string guid)
    {
        var payload = CreateUserPhotoPayload(guid);
        var result = await _userPhotoClient.SendPhotoRequestAsync(payload, _authHeader);

        return result;
    }
    
    public async Task<bool> SavePhotoAsync(UserPhotoResponseModel? userPhotoResponseModel, string name)
    {
        var fileName = $"{name}.jpg";
        var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var fullPath = Path.Combine(currentDirectory, PathConstants.EmployeeImgPath, fileName);
        
        var base64String = userPhotoResponseModel?.Data?.FirstOrDefault()?.Photocontent;
        if (base64String == null)
        {
            return false;
        }

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        
        var imageBytes = Convert.FromBase64String(base64String);
        
        await File.WriteAllBytesAsync(fullPath, imageBytes);

        return true;
    }

    public async Task SendUserDataRequestAsync()
    {
        var payload = CreateUserDataPayload(UserDataApiConstants.UserDataMethodName);
        var result = await _userPhotoClient.SendUserDataRequestAsync(payload, _authHeader);
        
        using var doc = JsonDocument.Parse(result);
        var data = doc.RootElement.GetProperty(UserDataApiConstants.DataRootName);
        
        var dataJson = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        
        var currentDirectory = AppContext.BaseDirectory;
        var userInfosJsonFile = Path.Combine(currentDirectory, "test.json");
        
        await File.WriteAllTextAsync(userInfosJsonFile, dataJson);
    }

    public async Task<UserDataDismissResponseModel?> SendUserDataDismissAsync()
    {
        var payload = CreateUserDataPayload(UserDataApiConstants.UserDataDismissMethodName);
        var result = await _userPhotoClient.SendUserDismissDataRequestAsync(payload, _authHeader);

        return result;
    }
    
    private UserPhotoPayload CreateUserPhotoPayload(string guid)
    {
        return new UserPhotoPayload
        {
            Request = _configuration[UserDataApiConstants.Request]!,
            Sign = _configuration[UserDataApiConstants.Sign]!,
            Type = _configuration[UserDataApiConstants.Type]!,
            KeyAccount = _configuration[UserDataApiConstants.KeyAccount]!,
            Name = UserDataApiConstants.PhotoMethodName,
            Kod = guid,
            NameFl = guid
        };
    }

    private BaseUserDataPayload CreateUserDataPayload(string methodName)
    {
        return new BaseUserDataPayload
        {
            Request = _configuration[UserDataApiConstants.Request]!,
            Sign = _configuration[UserDataApiConstants.Sign]!,
            Type = _configuration[UserDataApiConstants.Type]!,
            KeyAccount = _configuration[UserDataApiConstants.KeyAccount]!,
            Name = methodName
        };
    }

    private string CreateAuthHeader()
    {
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(
            $"{_configuration[UserDataApiConstants.Login]!}:{_configuration[UserDataApiConstants.Password]!}"));
        
        return $"{UserDataApiConstants.AuthMethod} {authToken}";
    }
}