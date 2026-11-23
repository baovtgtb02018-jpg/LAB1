using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using ASM.Config;

namespace ASM.Core
{
    public static class DriverFactory
    {
        public static IWebDriver Create(TestConfig cfg)
        {
            if (cfg == null)
            {
                throw new ArgumentNullException(nameof(cfg), "TestConfig khong duoc null.");
            }

            string rawBrowser = cfg.Browser ?? string.Empty;
            string browser = rawBrowser.Trim().ToLowerInvariant();

            IWebDriver driver = browser switch
            {
                "chrome" => new ChromeDriver(ChromeOpts(cfg)),
                "edge" => new EdgeDriver(EdgeOpts(cfg)),
                "firefox" => new FirefoxDriver(FirefoxOpts(cfg)),
                _ => throw new ArgumentException(
                    $"Trinh duyet '{rawBrowser}' khong duoc ho tro. Vui long chon: Chrome, Edge hoac Firefox.",
                    nameof(cfg))
            };

            if (!cfg.Headless && cfg.WindowWidth > 0 && cfg.WindowHeight > 0)
            {
                driver.Manage().Window.Size = new Size(cfg.WindowWidth, cfg.WindowHeight);
            }

            return driver;
        }

        private static ChromeOptions ChromeOpts(TestConfig cfg)
        {
            var options = new ChromeOptions();
            if (cfg.Headless)
            {
                options.AddArgument("--headless=new");
            }

            options.AddArgument($"--window-size={cfg.WindowWidth},{cfg.WindowHeight}");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            return options;
        }

        private static EdgeOptions EdgeOpts(TestConfig cfg)
        {
            var options = new EdgeOptions();
            if (cfg.Headless)
            {
                options.AddArgument("--headless=new");
            }

            options.AddArgument($"--window-size={cfg.WindowWidth},{cfg.WindowHeight}");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            return options;
        }

        private static FirefoxOptions FirefoxOpts(TestConfig cfg)
        {
            var options = new FirefoxOptions();
            if (cfg.Headless)
            {
                options.AddArgument("--headless");
            }

            options.AddArgument($"--width={cfg.WindowWidth}");
            options.AddArgument($"--height={cfg.WindowHeight}");
            return options;
        }
    }
}
