using Microsoft.Extensions.Configuration;
using SuperShortLink;
using SuperShortLink.Cache;
using SuperShortLink.Charts;
using SuperShortLink.Repository;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddShortLink(this IServiceCollection services, IConfiguration configuration, Action<ShortLinkOptions> configure = null, string sectionName = null)
        {
            sectionName ??= ShortLinkOptions.SectionName;

            services.AddOptions<ShortLinkOptions>()
                .Bind(configuration.GetSection(sectionName))
                .ValidateDataAnnotations();

            return services.AddService(configure);
        }

        public static IServiceCollection AddShortLink(this IServiceCollection services, Action<ShortLinkOptions> configure)
        {

            services.AddOptions<ShortLinkOptions>()
                .Configure(configure)
                .ValidateDataAnnotations();
            return services.AddService(configure);
        }

        private static IServiceCollection AddService(this IServiceCollection services, Action<ShortLinkOptions> configure)
        {
            services.PostConfigure<ShortLinkOptions>(x =>
            {
                configure?.Invoke(x);
                var defaultOption = ShortLinkOptions.GenDefault();
                if (string.IsNullOrWhiteSpace(x.LoginAcount) || string.IsNullOrWhiteSpace(x.LoginPassword))
                {
                    x.LoginAcount = defaultOption.LoginAcount;
                    x.LoginPassword = defaultOption.LoginPassword;
                }
                if (!x.CacheCountLimit.HasValue)
                {
                    x.CacheCountLimit = defaultOption.CacheCountLimit;
                }
                //var suportDatabase = new List<DatabaseType>() { DatabaseType.PostgreSQL, DatabaseType.MySQL };
                //if (!suportDatabase.Contains(x.DbType))
                //{
                //    throw new ArgumentException("暂不支持该数据库");
                //}
            });

            services.AddTransient<IShortLinkService, ShortLinkService>();
            services.AddTransient<IShortLinkRepository, ShortLinkRepository>();

            services.AddTransient<IApplicationService, ApplicationService>();
            services.AddTransient<IApplicationRepository, ApplicationRepository>();

            services.AddSingleton<IMemoryCaching, MemoryCaching>();
            services.AddTransient<Base62Converter>();

            services.AddTransient<ChartFactory>();
            services.AddTransient<IChart, HourChart>();
            services.AddTransient<IChart, DayChart>();
            services.AddTransient<IChart, WeekChart>();
            services.AddTransient<IChart, MonthChart>();

            return services;
        }
    }
}
