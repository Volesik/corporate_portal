using System.Text;
using Bgc.Web.Common.HttpClients;
using Bgc.Web.Common.Models;
using CorporatePortal.BL.Interfaces;
using CorporatePortal.Common.Constants;
using Microsoft.Extensions.Configuration;
using Refit;

namespace CorporatePortal.BL.Services;

public class UserPhotoService : IUserPhotoService
{
    private readonly IConfiguration _configuration;
    private readonly IUsersPhotoClient _userPhotoClient;
    
    public UserPhotoService(
        IConfiguration configuration,
        IUsersPhotoClient userPhotoClient)
    {
        _configuration = configuration;
        _userPhotoClient = userPhotoClient;
    }

    public async Task<UserPhotoResponseModel?> SendAsync(string guid)
    {
        var payload = CreatePayload(guid);

        try
        {
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(
                $"{_configuration["PhotoServiceIntegrationSettings:Login"]!}:{_configuration["PhotoServiceIntegrationSettings:Password"]!}"));
            var authHeader = $"Basic {authToken}";
            var result = await _userPhotoClient.SendRequestAsync(payload, authHeader);

            return result;
        }
        catch (ApiException e)
        {
            Console.WriteLine(e);
            throw;
        }
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
    
    private CreateUserPhotoRequestPayload CreatePayload(string guid)
    {
        Console.WriteLine($"Name is {_configuration["PhotoServiceIntegrationSettings:Name"]!}");
        return new CreateUserPhotoRequestPayload
        {
            Kod = guid,
            Name = _configuration["PhotoServiceIntegrationSettings:Name"]!,
            Request = _configuration["PhotoServiceIntegrationSettings:Request"]!,
            Sign = _configuration["PhotoServiceIntegrationSettings:Sign"]!,
            Type = _configuration["PhotoServiceIntegrationSettings:Type"]!,
            KeyAccount = _configuration["PhotoServiceIntegrationSettings:KeyAccount"]!,
            NameFl = guid,
        };
    }

}