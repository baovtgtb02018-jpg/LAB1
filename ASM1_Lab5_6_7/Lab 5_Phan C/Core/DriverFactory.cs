using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using SeleniumFramework.Config;

namespace SeleniumFramework.Core;

public static class DriverFactory
{
    public static IWebDriver Create(TestConfig cfg)
    {
        return cfg.Browser.ToLower() switch
        {
            "chrome" => CreateChrome(cfg),
            "firefox" => CreateFirefox(cfg),
            _ => throw new ArgumentException(
                $"Trình duyệt '{cfg.Browser}' không được hỗ trợ. " +
                $"Chỉ chấp nhận: chrome, firefox.")
        };
    }

    private static IWebDriver CreateChrome(TestConfig cfg)
    {
        var opts = new ChromeOptions();
        if (cfg.Headless)
        {
            opts.AddArgument("--headless=new");
            opts.AddArgument($"--window-size={cfg.WindowWidth},{cfg.WindowHeight}");
        }
        // Thêm dòng này để ép Selenium tự động tìm/tải driver
        var service = ChromeDriverService.CreateDefaultService();
        return new ChromeDriver(service, opts);
    }

    private static IWebDriver CreateFirefox(TestConfig cfg)
    {
        var opts = new FirefoxOptions();
        if (cfg.Headless)
        {
            opts.AddArgument("--headless");
            opts.AddArgument($"--width={cfg.WindowWidth}");
            opts.AddArgument($"--height={cfg.WindowHeight}");
        }
        // Thêm dòng này để ép Selenium tự động tìm/tải driver
        var service = FirefoxDriverService.CreateDefaultService();
        return new FirefoxDriver(service, opts);
    }
}