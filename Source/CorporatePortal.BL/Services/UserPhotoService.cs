using System.Text;
using CorporatePortal.BL.Interfaces;
using CorporatePortal.Common.Constants;
using CorporatePortal.Web.Common.HttpClients;
using CorporatePortal.Web.Common.Models;
using CorporatePortal.Web.Common.Models.Photo;
using Microsoft.Extensions.Configuration;
using Refit;

namespace CorporatePortal.BL.Services;

public class UserPhotoService(
    IConfiguration configuration,
    IUserServiceApiClient userPhotoClient)
    : IUserPhotoService
{
    public async Task<UserPhotoResponseModel?> SendAsync(string guid)
    {
        var payload = CreatePayload(guid);

        try
        {
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(
                $"{configuration["PhotoServiceIntegrationSettings:Login"]!}:{configuration["PhotoServiceIntegrationSettings:Password"]!}"));
            var authHeader = $"Basic {authToken}";
            var result = await userPhotoClient.SendPhotoRequestAsync(payload, authHeader);

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
    
    private UserPhotoPayload CreatePayload(string guid)
    {
        Console.WriteLine($"Name is {configuration["PhotoServiceIntegrationSettings:Name"]!}");
        return new UserPhotoPayload
        {
            Kod = guid,
            Name = configuration["PhotoServiceIntegrationSettings:Name"]!,
            Request = configuration["PhotoServiceIntegrationSettings:Request"]!,
            Sign = configuration["PhotoServiceIntegrationSettings:Sign"]!,
            Type = configuration["PhotoServiceIntegrationSettings:Type"]!,
            KeyAccount = configuration["PhotoServiceIntegrationSettings:KeyAccount"]!,
            NameFl = guid,
        };
    }

}