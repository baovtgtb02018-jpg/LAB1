using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using ASM.Config;
using ASM.Utilities;

[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(3)]

namespace ASM.Core
{
    public abstract class BaseTest
    {
        // Field thuong (khong static) de moi instance/luong so huu WebDriver rieng, dam bao thread-safe
        protected IWebDriver driver = null!;
        protected TestConfig config = null!;

        [SetUp]
        public void SetUp()
        {
            config = ConfigReader.Read();
            string testName = TestContext.CurrentContext.Test.Name;
            Log.Info($"Bat dau: {testName} | Browser={config.Browser} | Headless={config.Headless} | Thread={Environment.CurrentManagedThreadId}");

            driver = DriverFactory.Create(config);
            driver.Navigate().GoToUrl(config.BaseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            string testName = TestContext.CurrentContext.Test.Name;
            TestStatus status = TestContext.CurrentContext.Result.Outcome.Status;

            try
            {
                if (status == TestStatus.Failed)
                {
                    string errorMessage = TestContext.CurrentContext.Result.Message ?? "Khong co thong diep loi";
                    Log.Error($"Ket qua ROT (FAILED): {testName} | Chi tiet: {errorMessage}");

                    if (driver != null)
                    {
                        string path = ScreenshotHelper.Capture(driver, testName);
                        Log.Info($"Da chup anh man hinh loi: {path}");
                    }
                }
                else
                {
                    Log.Info($"Ket qua OK ({status}): {testName}");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Khong the xu ly chup anh/ghi log sau test '{testName}': {ex.Message}");
            }
            finally
            {
                driver?.Quit();
                driver?.Dispose();
            }
        }
    }
}
