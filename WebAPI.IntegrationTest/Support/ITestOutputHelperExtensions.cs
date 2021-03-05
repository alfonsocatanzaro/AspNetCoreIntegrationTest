using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTest.Support
{
    public static class ITestOutputHelperExtensions
    {

        public static void WriteFormattedJson(this ITestOutputHelper _output, object obj)
        {
            _output.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}
