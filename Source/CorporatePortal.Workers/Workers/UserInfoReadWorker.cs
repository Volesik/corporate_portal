using System.Text.Json;
using CorporatePortal.BL.Interfaces;
using CorporatePortal.DL.EntityFramework.Models;
using CorporatePortal.Workers.Interfaces;
using CorporatePortal.Workers.Mappers;
using CorporatePortal.Workers.Models;
using Hangfire;

namespace CorporatePortal.Workers.Workers;

public abstract class UserInfoReadWorker(
    IBackgroundJobClient backgroundJobClient,
    IUserInfoService userInfoService,
    UserInfoMapper userInfoMapper) : IWorker
{
    public async Task ExecuteAsync(int bulkSize)
    {
        try
        {
            var currentDirectory = AppContext.BaseDirectory;
            var userInfosJsonFile = Path.Combine(currentDirectory, "test.json");
            var jsonString = await File.ReadAllTextAsync(userInfosJsonFile);
            var importUsers = JsonSerializer.Deserialize<ImportUserDto[]>(jsonString);

            foreach (var importUser in importUsers!)
            {
                var userInfo = userInfoMapper.ToUserInfo(importUser);
                
                backgroundJobClient.Enqueue(() => ProcessAsync(userInfo));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }
    }

    private async Task ProcessAsync(UserInfo user)
    {
        await userInfoService.UpsertAsync(user, CancellationToken.None);
    }
}