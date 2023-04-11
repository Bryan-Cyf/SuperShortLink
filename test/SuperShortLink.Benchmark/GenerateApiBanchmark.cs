using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using Microsoft.Extensions.DependencyInjection;
using SuperShortLink.Api;
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
    public class GenerateApiBanchmark
    {
        private readonly IShortLinkApiService _apiService;

        public GenerateApiBanchmark()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddShortLinkApi(option =>
            {
                option.ApiDomain = "https://localhost:5000";
                option.AppSecret = "81FEB50B681F196D278D79A536B63A57";
                option.AppCode = "test_code";
            });
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            _apiService = serviceProvider.GetService<IShortLinkApiService>();
        }

        [Params(10, 50, 100)]
        public int ThreadCount;

        [Benchmark]
        public async Task Generate()
        {
            var index = 0;
            var url = "https://www.baidu.com/";
            var tasks = new List<Task>();
            for (int i = 0; i < ThreadCount; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    Interlocked.Increment(ref index);
                    _ = _apiService.GenerateAsync($"{url}{index}").Result;
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
