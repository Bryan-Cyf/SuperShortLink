using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SuperShortLink.Repository;
using System.IO;
using Xunit;

namespace SuperShortLink.UnitTests
{
    public class ServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddShortLinkExtensions_Should_Get_Configuration_Succeed()
        {
            var appsettings = "{\"ShortLink\":{\"Secrect\":\"s9LFkgy5RovixI1aOf8UhdY3r4DMplQZJXPqebE0WSjBn7wVzmN2Gc6THCAKut\",\"CodeLength\":6,\"DbType\":\"PostgreSQL\",\"ConnectionString\":\"Server=Server=127.0.0.1;Port=5432;User Id=uid;Password=pwd;Database=test_db;\",\"LoginAcount\":\"admin\",\"LoginPassword\":\"123456\"}}";
            var path = TestHelper.CreateTempFile(appsettings);
            var directory = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(directory);
            configurationBuilder.AddJsonFile(fileName);
            var config = configurationBuilder.Build();

            var services = new ServiceCollection();
            services.AddOptions();
            services.AddShortLink(config);

            var serviceProvider = services.BuildServiceProvider();
            var shortLinkOption = serviceProvider.GetService<IOptionsSnapshot<ShortLinkOptions>>();
            Assert.NotNull(shortLinkOption);
            Assert.Equal(6, shortLinkOption.Value.CodeLength);
            Assert.Equal("s9LFkgy5RovixI1aOf8UhdY3r4DMplQZJXPqebE0WSjBn7wVzmN2Gc6THCAKut", shortLinkOption.Value.Secrect);
            Assert.Equal(DatabaseType.PostgreSQL, shortLinkOption.Value.DbType);
            Assert.Equal("admin", shortLinkOption.Value.LoginAcount);
            Assert.Equal("123456", shortLinkOption.Value.LoginPassword);
        }
    }
}