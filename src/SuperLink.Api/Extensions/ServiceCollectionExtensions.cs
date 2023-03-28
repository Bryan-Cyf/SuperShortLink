using SuperShortLink.Api;
using SuperShortLink.Api.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddShortLinkApi(this IServiceCollection services, Action<ShortLinkApiOptions> configure)
        {

            services.AddOptions<ShortLinkApiOptions>()
                .Configure(configure);

            services.AddTransient<IShortLinkApiService, ShortLinkApiService>();
            services.AddTransient<ShortLinkHttpRequest>();
            return services;
        }
    }
}
