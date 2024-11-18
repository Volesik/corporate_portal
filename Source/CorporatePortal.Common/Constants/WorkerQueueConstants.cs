namespace CorporatePortal.Common.Constants;

public static class WorkerQueueConstants
{
    public const string DefaultQueueName = "default";
    public const string UserImportQueueName = "userimport";
    public const string PhotoDownloadQueueName = "downloadphoto";

    public static readonly List<string> QueuesForRegistration = new()
    {
        DefaultQueueName,
        UserImportQueueName,
        PhotoDownloadQueueName
    };
}