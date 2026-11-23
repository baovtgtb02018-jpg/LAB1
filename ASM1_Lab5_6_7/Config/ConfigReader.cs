using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ASM.Config
{
    public static class ConfigReader
    {
        public static TestConfig Read()
        {
            string configPath = Path.Combine(AppContext.BaseDirectory, "Config", "appsettings.json");
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException(
                    $"Khong tim thay file cau hinh tai: {configPath}. Kiem tra CopyToOutputDirectory trong AutomationTests.csproj.",
                    configPath);
            }

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(Path.Combine("Config", "appsettings.json"), optional: false, reloadOnChange: false)
                .Build();

            TestConfig? config = configuration.Get<TestConfig>();
            if (config == null || string.IsNullOrWhiteSpace(config.BaseUrl))
            {
                throw new InvalidOperationException("Thieu hoac sai gia tri BaseUrl trong Config/appsettings.json.");
            }

            return config;
        }
    }
}
