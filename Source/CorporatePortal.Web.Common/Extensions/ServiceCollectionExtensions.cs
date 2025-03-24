using CorporatePortal.Web.Common.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace CorporatePortal.Web.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddWebCommonServices(this IServiceCollection services, ConfigurationManager manager)
    {
        services.AddRefitClients(manager);
    }
    
    private static void AddRefitClients(this IServiceCollection services, ConfigurationManager manager)
    {
        services.AddRefitClient<IUserServiceApiClient>()
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(manager["PhotoServiceIntegrationSettings:BaseUrl"]!);
            });
    }
}