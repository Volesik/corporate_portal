using Bgc.Web.Common.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Bgc.Web.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebCommonServices(this IServiceCollection services, ConfigurationManager manager)
    {
        services.AddRefitClients(manager);

        return services;
    }
    
    private static void AddRefitClients(this IServiceCollection services, ConfigurationManager manager)
    {
        services.AddRefitClient<IUsersPhotoClient>()
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(manager["PhotoServiceIntegrationSettings:BaseUrl"]!);
            });
    }
}