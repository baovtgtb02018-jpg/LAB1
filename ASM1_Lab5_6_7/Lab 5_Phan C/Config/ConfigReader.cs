using Microsoft.Extensions.Configuration;

namespace SeleniumFramework.Config;

public static class ConfigReader
{
    public static TestConfig Load()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        return config.Get<TestConfig>()
               ?? throw new InvalidOperationException("Không đọc được appsettings.json");
    }
}