using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace LAB5.Config
{
    public static class ConfigReader
    {
        public static TestConfig LoadConfig()
        {
            string basePath = AppContext.BaseDirectory;
            string configPath = Path.Combine(basePath, "Config", "appsettings.json");

            IConfigurationBuilder builder = new ConfigurationBuilder();
            if (File.Exists(configPath))
            {
                builder = builder.AddJsonFile(configPath, optional: false, reloadOnChange: false);
            }

            IConfigurationRoot configuration = builder.Build();
            TestConfig config = new TestConfig();
            configuration.Bind(config);

            if (string.IsNullOrWhiteSpace(config.BaseUrl))
            {
                config.BaseUrl = "https://www.saucedemo.com/";
            }
            if (string.IsNullOrWhiteSpace(config.Browser))
            {
                config.Browser = "Chrome";
            }
            if (config.TimeoutSeconds <= 0)
            {
                config.TimeoutSeconds = 5;
            }
            if (config.WindowWidth <= 0)
            {
                config.WindowWidth = 1280;
            }
            if (config.WindowHeight <= 0)
            {
                config.WindowHeight = 800;
            }

            return config;
        }
    }
}