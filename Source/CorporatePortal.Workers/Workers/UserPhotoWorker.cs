using System.ComponentModel;
using CorporatePortal.BL.Interfaces;
using CorporatePortal.Common.Constants;
using CorporatePortal.Workers.Interfaces;
using Hangfire;
using Hangfire.Tags.Attributes;

namespace CorporatePortal.Workers.Workers;

public class UserPhotoWorker : IWorker
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IUserPhotoService _userPhotoService;
    private readonly IUserInfoService _userInfoService;
    
    public UserPhotoWorker(
        IBackgroundJobClient backgroundJobClient,
        IUserPhotoService userPhotoService,
        IUserInfoService userInfoService)
    {
        _backgroundJobClient = backgroundJobClient;
        _userPhotoService = userPhotoService;
        _userInfoService = userInfoService;
    }
    
    [DisableConcurrentExecution(60 * 5)]
    [Tag("Scheduled job")]
    [Queue(WorkerQueueConstants.PhotoDownloadQueueName)]
    public async Task ExecuteAsync(int bulkSize)
    {
        try
        {
            var users = await _userInfoService.GetAllAsync(null, 0, CancellationToken.None);
            foreach (var user in users)
            {
                _backgroundJobClient.Enqueue(() => ProcessAsync(user.Id, user.UniqueId.ToString()));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occured: {e.Message}");
        }
    }

    [DisableConcurrentExecution(60 * 5)]
    [Tag("Create Location")]
    [DisplayName("Download photo for user with guid: {1}")]
    [Queue(WorkerQueueConstants.PhotoDownloadQueueName)]
    public async Task ProcessAsync(long userId, string guid)
    {
        var result = await _userPhotoService.SendAsync(guid);
        await _userPhotoService.SavePhotoAsync(result, guid);
    }
}