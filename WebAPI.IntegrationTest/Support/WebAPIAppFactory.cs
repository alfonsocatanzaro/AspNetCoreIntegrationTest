using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTest.Support
{
    public class WebAPIAppFactory<TStartup, TFactoryOptions>
     : WebApplicationFactory<TStartup>
        where TStartup : class
        where TFactoryOptions : IFactoryOptions, new()
    {
        private readonly TFactoryOptions options = new TFactoryOptions();

        public WebAPIAppFactory<TStartup, TFactoryOptions> Configure(Action<TFactoryOptions> setupAction)
        {
            setupAction?.Invoke(options);
            return this;
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            string[] args = new string[] { };
            IHostBuilder builder = Program.CreateHostBuilder(args);
            options.Apply(builder);
            return builder;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            if (options.TestOutputHelper != null)
                RegisterLogger(builder);
            return base.CreateHost(builder);
        }

        private void RegisterLogger(IHostBuilder builder)
        {
            builder.ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.Services.AddSingleton<ILoggerProvider>(
                    serviceProvider => new XUnitLoggerProvider(options.TestOutputHelper)
                );
            });
        }

        protected override void ConfigureClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


    }
}
