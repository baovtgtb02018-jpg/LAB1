using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace SeleniumTest
{
    public class AdvancedInteractionTests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        // 1. TRANG JAVASCRIPT_ALERTS
        [Test]
        public void Test_JS_Alert_Accept()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");
            driver.FindElement(By.CssSelector("button[onclick='jsAlert()']")).Click();
            
            IAlert alert = driver.SwitchTo().Alert();
            Assert.That(alert.Text, Is.EqualTo("I am a JS Alert"));
            alert.Accept(); // Bấm OK
            
            // Assert kết quả ở #result
            Assert.That(driver.FindElement(By.Id("result")).Text, Is.EqualTo("You successfully clicked an alert"));
        }

        [Test]
        public void Test_JS_Confirm_Dismiss()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");
            driver.FindElement(By.CssSelector("button[onclick='jsConfirm()']")).Click();
            
            IAlert alert = driver.SwitchTo().Alert();
            Assert.That(alert.Text, Is.EqualTo("I am a JS Confirm"));
            alert.Dismiss(); // Bấm Cancel
            
            // Assert kết quả ở #result
            Assert.That(driver.FindElement(By.Id("result")).Text, Is.EqualTo("You clicked: Cancel"));
        }

        [Test]
        public void Test_JS_Prompt_SendKeys()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");
            driver.FindElement(By.CssSelector("button[onclick='jsPrompt()']")).Click();
            
            IAlert alert = driver.SwitchTo().Alert();
            Assert.That(alert.Text, Is.EqualTo("I am a JS prompt"));
            
            string input = "Hello";
            alert.SendKeys(input); // Gõ chữ vào Prompt
            alert.Accept(); // Bấm OK
            
            // Assert kết quả ở #result
            Assert.That(driver.FindElement(By.Id("result")).Text, Is.EqualTo($"You entered: {input}"));
        }

        // 2. TRANG IFRAME
        [Test]
        public void Test_Iframe_TypeText()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/iframe");
            
            // Switch vào iframe
            driver.SwitchTo().Frame("mce_0_ifr");
            
            // Gõ chữ
            IWebElement editor = driver.FindElement(By.Id("tinymce"));
            editor.Clear();
            editor.SendKeys("Xin chào");
            
            // Thoát ra DefaultContent
            driver.SwitchTo().DefaultContent();
            
            // Assert (kiểm tra một element ở trang chính để chắc chắn đã thoát iframe thành công)
            Assert.That(driver.FindElement(By.TagName("h3")).Text, Does.Contain("An iFrame containing"));
        }

        [TearDown]
        public void Teardown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }
    }

    
}