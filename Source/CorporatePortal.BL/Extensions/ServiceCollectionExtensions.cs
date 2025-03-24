using Microsoft.Extensions.DependencyInjection;
using CorporatePortal.Web.Common.HttpClients;
using Microsoft.Extensions.Configuration;
using Refit;

namespace CorporatePortal.BL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebCommonServices(this IServiceCollection services, ConfigurationManager manager)
    {
        services.AddRefitClients(manager);

        return services;
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