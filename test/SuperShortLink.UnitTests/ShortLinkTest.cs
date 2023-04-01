using Microsoft.Extensions.DependencyInjection;
using SuperShortLink.Cache;
using SuperShortLink.Repository;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SuperShortLink.UnitTests
{
    public class ShortLinkTest
    {
        private readonly IMemoryCaching _memory;
        private readonly IShortLinkService _shortLinkService;
        private readonly IShortLinkRepository _repository;
        private readonly Base62Converter _converter;
        public ShortLinkTest()
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
            _repository = serviceProvider.GetService<IShortLinkRepository>();
            _memory = serviceProvider.GetService<IMemoryCaching>();
            _converter = serviceProvider.GetService<Base62Converter>();
        }

        [Fact]
        public void Generate_Key_Be_Valid()
        {
            var key = _converter.GenerateSecret();
            Assert.NotNull(key);
        }

        [Fact]
        public async Task Generate_Should_Be_Valid()
        {
            //有效链接
            var shortLink = await _shortLinkService.GenerateAsync("http://www.baidu.com");
            Assert.NotNull(shortLink);

            //无效链接
            shortLink = await _shortLinkService.GenerateAsync("abcd");
            Assert.Null(shortLink);
        }

        [Fact]
        public async Task Count_Should_Add_When_Aceess()
        {
            var shortLink = await _shortLinkService.GenerateAsync("http://www.baidu.com");
            Assert.NotNull(shortLink);

            var reId = _converter.ReCoverConfuse(shortLink);
            Assert.True(reId > 0);

            var model = await _repository.GetAsync(reId);
            Assert.NotNull(model);
            Assert.True(model.access_count == 0);

            var accessCount = 100;
            for (int i = 0; i < accessCount; i++)
            {
                await _shortLinkService.AccessAsync(shortLink);
            }

            model = await _repository.GetAsync(reId);
            Assert.NotNull(model);
            Assert.True(model.access_count == accessCount);
        }

        [Fact]
        public void Cache_Should_Succeed()
        {
            var key = "key";
            var value = "value";
            var result = _memory.Set(key, value);
            Assert.True(result);

            var cache = _memory.Get<string>(key);
            Assert.False(cache.IsNull);
            Assert.Equal(value, cache.Value);
        }

        [Fact]
        public void Base64_Convert_Should_Be_Same()
        {
            var convert = _converter.Encode(10);
            Assert.NotEmpty(convert);

            var digit = _converter.Decode(convert);
            Assert.NotEqual(10L, digit);
        }

        [Fact]
        public void Convert_Should_Be_Same()
        {
            var initDigit = int.MaxValue;
            var convert = _converter.Confuse(initDigit);

            var digit = _converter.ReCoverConfuse(convert);
            Assert.Equal(initDigit, digit);

        }
    }
}