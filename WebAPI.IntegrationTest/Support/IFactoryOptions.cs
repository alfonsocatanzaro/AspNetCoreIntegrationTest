using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTest.Support
{
    public interface IFactoryOptions
    {
        void Apply(IHostBuilder builder);
        ITestOutputHelper TestOutputHelper { get; set; }
    }
}
