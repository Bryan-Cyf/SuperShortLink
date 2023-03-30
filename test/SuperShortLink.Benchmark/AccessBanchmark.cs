using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using Microsoft.Extensions.DependencyInjection;
using SuperShortLink.Cache;
using SuperShortLink.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShortLink.Benchmark
{
    [MaxColumn, MinColumn, MemoryDiagnoser]
    [Config(typeof(Config))]
    public class AccessBanchmark
    {
        private readonly IShortLinkService _shortLinkService;
        private readonly Base62Converter _converter;
        public AccessBanchmark()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddShortLink(option =>
            {
                option.ConnectionString = "Server=127.0.0.1;Port=5432;User Id=uid;Password=pwd;Database=test_db;";
                option.DbType = DatabaseType.PostgreSQL;
                option.Secrect = "s9LFkgy5RovixI1aOf8UhdY3r4DMplQZJXPqebE0WSjBn7wVzmN2Gc6THCAKut";
                option.CodeLength = 6;
            });
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            _shortLinkService = serviceProvider.GetService<IShortLinkService>();
            _converter = serviceProvider.GetService<Base62Converter>();
        }

        [Params(10, 100, 200, 500, 1000)]
        public int ThreadCount;

        [Benchmark]
        public async Task Generate()
        {
            var index = 0;
            var tasks = new List<Task>();
            for (int i = 0; i < ThreadCount; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    Interlocked.Increment(ref index);
                    var shortKey = _converter.Confuse(index);
                    _ = _shortLinkService.AccessAsync(shortKey).Result;
                }));
            }
            await Task.WhenAll(tasks);
        }

        private class Config : ManualConfig
        {
            public Config()
            {
                AddJob(Job.Default.WithStrategy(RunStrategy.Monitoring));
            }
        }
    }
}
