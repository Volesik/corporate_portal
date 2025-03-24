using CorporatePortal.BL.Interfaces;
using CorporatePortal.Workers.Interfaces;
using Hangfire;

namespace CorporatePortal.Workers.Workers;

public class UserInfoDismissReadWorker(
    IBackgroundJobClient backgroundJobClient,
    IExternalUserDataService externalUserDataService,
    IUserInfoService userInfoService)
    : IWorker
{
    public Task ExecuteAsync(int bulkSize)
    {
        try
        {
            backgroundJobClient.Enqueue(() => ProcessAsync());
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occured: {e.Message}");
        }

        return Task.CompletedTask;
    }

    private async Task ProcessAsync()
    {
        await userInfoService.CountAsync(null, CancellationToken.None);
    }
}