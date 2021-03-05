using System;
using WebAPI.IntegrationTest.Support;
using Xunit;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTest
{
    public class UnitTest1: IClassFixture<WebAPIAppFactory<Startup,WebAPIAppFactoryOptions>>
    {

        private readonly WebAPIAppFactory<Startup, WebAPIAppFactoryOptions> _factory;

        public UnitTest1(ITestOutputHelper output,
            WebAPIAppFactory<Startup, WebAPIAppFactoryOptions> factory)
        {
            _factory = factory.Configure(o =>
           {
               o.ClassName = nameof(UnitTest1);
               o.AppSettingsTemplate = "TestAppSettings/appsettings.test.json";
           });
        }


        [Fact]
        public async void Test1()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/weatherforecast");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(
                "application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString()
            );
        }
    }
}
