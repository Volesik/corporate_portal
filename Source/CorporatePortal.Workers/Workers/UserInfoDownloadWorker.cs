using CorporatePortal.BL.Interfaces;
using CorporatePortal.Workers.Interfaces;
using Hangfire;

namespace CorporatePortal.Workers.Workers;

public class UserInfoDownloadWorker : IWorker
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IDownloader _downloader;

    public UserInfoDownloadWorker(IBackgroundJobClient backgroundJobClient, IDownloader downloader)
    {
        _backgroundJobClient = backgroundJobClient;
        _downloader = downloader;
    }

    public async Task ExecuteAsync(int bulkSize)
    {
        try
        {
            _backgroundJobClient.Enqueue(() => ProcessAsync());
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occured: {e.Message}");
        }
    }

    public async Task ProcessAsync()
    {
        await _downloader.DownloadUserInfoDataAsync();
    }
}