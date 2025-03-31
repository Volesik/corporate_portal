using System.ComponentModel;
using CorporatePortal.BL.Interfaces;
using CorporatePortal.Common.Constants;
using CorporatePortal.Workers.Interfaces;
using Hangfire;
using Hangfire.Tags.Attributes;

namespace CorporatePortal.Workers.Workers;

public class UserInfoDismissReadWorker(
    IBackgroundJobClient backgroundJobClient,
    IExternalUserDataService externalUserDataService,
    IUserInfoService userInfoService)
    : IWorker
{
    public async Task ExecuteAsync(int bulkSize)
    {
        try
        {
            var dismissedUsers = await externalUserDataService.SendUserDataDismissAsync();
            if (dismissedUsers?.Data != null)
            {
                foreach (var data in dismissedUsers.Data)
                {
                    backgroundJobClient.Enqueue(() => ProcessAsync(data.Kod!));
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occured: {e.Message}");
        }
    }

    [DisableConcurrentExecution(60 * 5)]
    [Tag("Disable ser")]
    [DisplayName("Disable user with guid: {0}")]
    [Queue(WorkerQueueConstants.UserDismissed)]
    public async Task ProcessAsync(string guid)
    {
        await userInfoService.DisableUser(guid, CancellationToken.None);
    }
}