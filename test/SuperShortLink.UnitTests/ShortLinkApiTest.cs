using Microsoft.Extensions.DependencyInjection;
using SuperShortLink.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SuperShortLink.UnitTests
{
    public class ApiTest
    {
        private readonly IShortLinkApiService _apiService;

        public ApiTest()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddShortLinkApi(option =>
            {
                option.ApiDomain = "https://localhost:5000";
                option.AppSecret = "C93E7EF47F8832FAE1578F16D10E902D";
                option.AppCode = "test_code";
            });
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            _apiService = serviceProvider.GetService<IShortLinkApiService>();
        }

        [Fact]
        public async Task Api_Should_Be_Valid()
        {
            var shortLink = await _apiService.GenerateAsync(new Api.Models.ShortLinkGenerateRequest()
            {
                origin_url = "https://www.baidu.com"
            });
            Assert.NotNull(shortLink);
        }
    }
}
