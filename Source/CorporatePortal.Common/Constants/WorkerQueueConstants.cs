namespace CorporatePortal.Common.Constants;

public static class WorkerQueueConstants
{
    public const string DefaultQueueName = "default";
    public const string UserImportQueueName = "userimport";
    public const string PhotoDownloadQueueName = "downloadphoto";
    public const string UserDismissed = "userdismissed";

    public static readonly List<string> QueuesForRegistration =
    [
        DefaultQueueName,
        UserImportQueueName,
        PhotoDownloadQueueName,
        UserDismissed
    ];
}