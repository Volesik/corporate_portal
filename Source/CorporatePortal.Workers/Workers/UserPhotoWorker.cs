using System.ComponentModel;
using CorporatePortal.BL.Interfaces;
using CorporatePortal.Common.Constants;
using CorporatePortal.Workers.Interfaces;
using Hangfire;
using Hangfire.Tags.Attributes;

namespace CorporatePortal.Workers.Workers;

public abstract class UserPhotoWorker(
    IBackgroundJobClient backgroundJobClient,
    IExternalUserDataService externalUserDataService,
    IUserInfoService userInfoService)
    : IWorker
{
    [DisableConcurrentExecution(60 * 5)]
    [Tag("Scheduled job")]
    [Queue(WorkerQueueConstants.PhotoDownloadQueueName)]
    public async Task ExecuteAsync(int bulkSize)
    {
        try
        {
            var users = await userInfoService.GetAsync(null, CancellationToken.None);
            
            foreach (var user in users)
            {
                backgroundJobClient.Enqueue(() => ProcessAsync(user.UniqueId.ToString()));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occured: {e.Message}");
        }
    }

    [DisableConcurrentExecution(60 * 5)]
    [Tag("Create Location")]
    [DisplayName("Download photo for user with guid: {0}")]
    [Queue(WorkerQueueConstants.PhotoDownloadQueueName)]
    private async Task ProcessAsync(string guid)
    {
        var result = await externalUserDataService.SendPhotoRequestAsync(guid);
        
        await externalUserDataService.SavePhotoAsync(result, guid);
    }
}