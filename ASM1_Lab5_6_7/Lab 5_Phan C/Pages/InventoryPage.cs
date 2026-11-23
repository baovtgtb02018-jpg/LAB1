using OpenQA.Selenium;

namespace SeleniumFramework.Pages;

public class InventoryPage
{
    private readonly IWebDriver _driver;

    public InventoryPage(IWebDriver driver) => _driver = driver;

    /// <summary>
    /// Trả về TRUE nếu đang ở trang sản phẩm.
    /// KHÔNG được ném lỗi — dùng FindElements (số nhiều) và try/catch.
    /// </summary>
    public bool DaHienThi()
    {
        try
        {
            // Dấu hiệu trang Inventory: class .inventory_list hoặc #inventory_container
            var elements = _driver.FindElements(By.ClassName("inventory_list"));
            return elements.Count > 0;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }
}