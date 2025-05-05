using CorporatePortal.BL.Interfaces;
using CorporatePortal.Workers.Interfaces;
using Hangfire;

namespace CorporatePortal.Workers.Workers;

public class UserInfoDownloadWorker(
    IBackgroundJobClient backgroundJobClient,
    IExternalUserDataService externalUserDataService) : IWorker
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

    public async Task ProcessAsync()
    {
        await externalUserDataService.SendUserDataRequestAsync();
    }
}