using CorporatePortal.Workers.Interfaces;
using CorporatePortal.Workers.Workers;
using Hangfire;

namespace CorporatePortal.Workers.Extensions;

public static class AppConfigurationExtensions
{
    public static WebApplication? RegisterRecurringJobs(this WebApplication? app)
    {
        if (app == null)
        {
            return null;
        }
        
        RegisterOrDeregisterScheduledJob<UserInfoReadWorker>(app.Configuration);
        RegisterOrDeregisterScheduledJob<UserInfoDownloadWorker>(app.Configuration);
        RegisterOrDeregisterScheduledJob<UserPhotoWorker>(app.Configuration);

        return app;
    }
    
    private static void RegisterOrDeregisterScheduledJob<T>(IConfiguration configuration)
        where T : IWorker
    {
        var className = typeof(T).Name;
        if (configuration.GetValue<bool>($"Workers:{className}:Enabled"))
        {
            RecurringJob.AddOrUpdate<T>(
                className, 
                x => x.ExecuteAsync(configuration.GetValue<int>($"Workers:{className}:BulkSize")), 
                configuration.GetValue<string>($"Workers:{className}:Schedule"));
        }
        else
        {
            RecurringJob.RemoveIfExists(className);
        }
    }
}