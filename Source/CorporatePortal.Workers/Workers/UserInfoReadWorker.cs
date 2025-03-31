using System.ComponentModel;
using System.Text.Json;
using CorporatePortal.BL.Interfaces;
using CorporatePortal.Common.Constants;
using CorporatePortal.DL.EntityFramework.Models;
using CorporatePortal.Workers.Interfaces;
using CorporatePortal.Workers.Mappers;
using CorporatePortal.Workers.Models;
using Hangfire;
using Hangfire.Tags.Attributes;

namespace CorporatePortal.Workers.Workers;

public class UserInfoReadWorker(
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
                
                backgroundJobClient.Enqueue(() => ProcessAsync(userInfo.UniqueId.ToString(), userInfo));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }
    }

    [DisableConcurrentExecution(60 * 5)]
    [Tag("Upsert user")]
    [DisplayName("Upsert user with guid: {0}")]
    [Queue(WorkerQueueConstants.UserImportQueueName)]
    public async Task ProcessAsync(string guid, UserInfo user)
    {
        await userInfoService.UpsertAsync(user, CancellationToken.None);
    }
}