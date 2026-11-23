using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using SeleniumFramework.Config;
using SeleniumFramework.Utilities;

namespace SeleniumFramework.Core;

public abstract class BaseTest
{
    protected TestConfig Config = null!;
    protected IWebDriver Driver = null!;

    [SetUp]
    public void SetUp()
    {
        // 1. Đọc config
        Config = ConfigReader.Load();

        // 2. Tạo driver
        Driver = DriverFactory.Create(Config);

        // 3. Mở URL
        Driver.Navigate().GoToUrl(Config.BaseUrl);
    }

    [TearDown]
    public void TearDown()
    {
        try
        {
            // Chỉ chụp khi test Failed
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var path = ScreenshotHelper.Capture(
                    Driver, TestContext.CurrentContext.Test.Name);

                if (!string.IsNullOrEmpty(path))
                {
                    TestContext.AddTestAttachment(path, "Screenshot on failure");
                }
            }
        }
        finally
        {
            // LUÔN đóng trình duyệt, kể cả khi chụp ảnh ném lỗi
            if (Driver != null)
            {
                try { Driver.Quit(); } catch { /* ignore */ }
                try { Driver.Dispose(); } catch { /* ignore */ }
            }
        }
    }
}