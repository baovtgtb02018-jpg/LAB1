using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace SeleniumTest
{
    public abstract class BasePage
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;

        protected BasePage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        protected IWebElement Tim(By locator)
        {
            return wait.Until(d =>
            {
                var element = d.FindElement(locator);
                return (element != null && element.Displayed) ? element : null;
            });
        }

        protected IReadOnlyCollection<IWebElement> TimNhieu(By locator)
        {
            wait.Until(d => d.FindElements(locator).Count > 0);
            return driver.FindElements(locator);
        }
    }
}