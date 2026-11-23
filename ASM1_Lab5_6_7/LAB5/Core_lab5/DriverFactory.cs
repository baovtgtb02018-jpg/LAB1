using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using LAB5.Config;

namespace LAB5.Core
{
    public static class DriverFactory
    {
        public static IWebDriver Create(TestConfig config)
        {
            string browserName = "Chrome";
            bool isHeadless = false;
            int width = 1280;
            int height = 800;

            if (config != null)
            {
                if (string.IsNullOrWhiteSpace(config.Browser) == false)
                {
                    browserName = config.Browser.Trim();
                }
                isHeadless = config.Headless;
                if (config.WindowWidth > 0)
                {
                    width = config.WindowWidth;
                }
                if (config.WindowHeight > 0)
                {
                    height = config.WindowHeight;
                }
            }

            string normalized = browserName.ToLowerInvariant();
            if (normalized == "chrome")
            {
                ChromeOptions chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--window-size=" + width.ToString() + "," + height.ToString());
                if (isHeadless)
                {
                    chromeOptions.AddArgument("--headless=new");
                }
                return new ChromeDriver(chromeOptions);
            }

            if (normalized == "firefox")
            {
                FirefoxOptions firefoxOptions = new FirefoxOptions();
                firefoxOptions.AddArgument("--width=" + width.ToString());
                firefoxOptions.AddArgument("--height=" + height.ToString());
                if (isHeadless)
                {
                    firefoxOptions.AddArgument("--headless");
                }
                return new FirefoxDriver(firefoxOptions);
            }

            throw new ArgumentException("Khong ho tro trinh duyet: " + browserName);
        }
    }
}