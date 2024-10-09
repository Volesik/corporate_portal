namespace CorporatePortal.Common.Constants;

public static class WorkerQueueConstants
{
    private const string DefaultQueueName = "default";
    private const string UserImportQueueName = "userimport";

    public static readonly List<string> QueuesForRegistration = new()
    {
        DefaultQueueName,
        UserImportQueueName
    };
}