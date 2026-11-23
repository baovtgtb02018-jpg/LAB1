using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ASM.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;

        protected BasePage(IWebDriver driver, int timeoutSeconds = 10)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
            this.wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        }

        protected IWebElement Tim(By locator)
        {
            return wait.Until(d =>
            {
                var element = d.FindElements(locator).FirstOrDefault(e => e.Displayed);
                return element;
            })!;
        }

        protected IReadOnlyCollection<IWebElement> TimNhieu(By locator)
        {
            wait.Until(d => d.FindElements(locator).Count > 0);
            return driver.FindElements(locator);
        }

        protected void EnterText(By locator, string text)
        {
            var element = Tim(locator);
            element.Clear();
            if (!string.IsNullOrEmpty(text))
            {
                element.SendKeys(text);
            }
        }

        protected void ClickElement(By locator)
        {
            Tim(locator).Click();
        }
    }
}
