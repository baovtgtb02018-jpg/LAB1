using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace LAB5.Pages
{
    public class InventoryPage
    {
        private IWebDriver driver;
        private By inventoryContainerLocator = By.Id("inventory_container");

        public InventoryPage(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        public bool DaHienThi()
        {
            try
            {
                if (driver == null)
                {
                    return false;
                }

                ReadOnlyCollection<IWebElement> elements = driver.FindElements(inventoryContainerLocator);
                if (elements.Count > 0 && elements[0].Displayed)
                {
                    return true;
                }

                return false;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}