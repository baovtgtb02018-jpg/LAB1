using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AutomationTests.Pages
{
    public class BasePage
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); // YC3
        }

        public IWebElement WaitForElement(By locator)
        {
            return wait.Until(d => d.FindElement(locator)); // YC3
        }
    }
}