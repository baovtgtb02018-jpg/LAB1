using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace SeleniumTest
{
    [Ignore("Tam bo qua de chay CI")]
    public class Tests
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        [Test]
        public void TestPageTitle()
        {
            // Điều hướng đến trang web
            driver.Navigate().GoToUrl("https://example.com");
            
            // Lấy tiêu đề
            string title = driver.Title;
            Console.WriteLine("Page Title is: " + title);
            
            // Kiểm tra kết quả
            Assert.AreEqual("Example Domain", title);
        }

        [TearDown]
        public void Teardown()
        {
            // Đóng trình duyệt
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}