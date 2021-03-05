using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTest.Support
{
    public class WebAPIAppFactoryOptions : IFactoryOptions
    {
        public readonly string SourceAppSettingsRelativePath = "..\\..\\..\\";

        public WebAPIAppFactoryOptions()
        {
            TempFileSuffix = Path.GetFileNameWithoutExtension(Path.GetTempFileName());
        }

        public ITestOutputHelper TestOutputHelper { get; set; }
        public string ClassName { get; internal set; }
        public string AppSettingsTemplate { get; internal set; }
        public string TempFileSuffix { get; internal set; }


        internal string GetSourceAppSettingsPath()
        {
            return Path.Combine(SourceAppSettingsRelativePath, AppSettingsTemplate);
        }



        public void Apply(IHostBuilder builder)
        {
            string jsonFile = CopyAndEditJsonConfig();
            builder.ConfigureAppConfiguration((context, conf) =>
            {
                conf.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), jsonFile));
            });
            builder.UseEnvironment("development");
        }

        private string CopyAndEditJsonConfig()
        {
            TryDeleteFiles($"appsettings.{ClassName}.*.json");
            string jsonPath = GetSourceAppSettingsPath();
            string json = File.ReadAllText(jsonPath);

            // do some substitutions

            string outputJsonFileName = $"appsettings.{ClassName}.{TempFileSuffix}.json";
            File.WriteAllText(outputJsonFileName, json);
            return outputJsonFileName;
        }
        private void TryDeleteFiles(string pattern)
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), pattern);
            foreach (var oldAppSettingsFile in files)
                TryDelete(oldAppSettingsFile);
        }
        private bool TryDelete(string filename)
        {
            try
            {
                File.Delete(filename);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }




    }
}
