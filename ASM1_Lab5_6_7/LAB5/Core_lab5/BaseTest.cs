using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using LAB5.Config;
using LAB5.Utilities;

namespace LAB5.Core
{
    public abstract class BaseTest
    {
        protected IWebDriver driver;
        protected TestConfig config;

        [SetUp]
        public void SetUp()
        {
            config = ConfigReader.LoadConfig();
            driver = DriverFactory.Create(config);

            if (config != null)
            {
                if (config.TimeoutSeconds > 0)
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(config.TimeoutSeconds);
                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(config.TimeoutSeconds);
                }

                if (string.IsNullOrWhiteSpace(config.BaseUrl) == false)
                {
                    driver.Navigate().GoToUrl(config.BaseUrl);
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                TestStatus status = TestContext.CurrentContext.Result.Outcome.Status;
                if (status == TestStatus.Failed)
                {
                    string testName = TestContext.CurrentContext.Test.Name;
                    string savedPath = ScreenshotHelper.CaptureFailure(driver, testName);
                    if (string.IsNullOrEmpty(savedPath) == false)
                    {
                        TestContext.WriteLine("Da luu anh loi: " + savedPath);
                    }
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Loi khi chup anh man hinh: " + ex.Message);
            }
            finally
            {
                if (driver != null)
                {
                    try
                    {
                        driver.Quit();
                    }
                    finally
                    {
                        driver.Dispose();
                        driver = null;
                    }
                }
            }
        }
    }
}