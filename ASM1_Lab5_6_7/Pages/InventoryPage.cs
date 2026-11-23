using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ASM.Pages
{
    public class InventoryPage : BasePage
    {
        private readonly By tieuDeTrang = By.ClassName("title");
        private readonly By danhSachSanPham = By.ClassName("inventory_item");
        private readonly By oChonSapXep = By.ClassName("product_sort_container");
        private readonly By danhSachGia = By.ClassName("inventory_item_price");
        private readonly By nutThemBackpack = By.Id("add-to-cart-sauce-labs-backpack");
        private readonly By badgeGioHang = By.ClassName("shopping_cart_badge");
        private readonly By nutMoGioHang = By.ClassName("shopping_cart_link");

        public InventoryPage(IWebDriver driver, int timeoutSeconds = 10) : base(driver, timeoutSeconds)
        {
        }

        public bool DaHienThi()
        {
            try
            {
                var shortWait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                shortWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
                return shortWait.Until(d =>
                    d.Url.Contains("inventory.html") &&
                    d.FindElements(tieuDeTrang).Any(e => e.Displayed));
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public int DemSoLuongSanPham()
        {
            return TimNhieu(danhSachSanPham).Count;
        }

        public InventoryPage SapXep(string value)
        {
            var select = new SelectElement(Tim(oChonSapXep));
            select.SelectByValue(value);
            return this;
        }

        public List<double> LayDanhSachGia()
        {
            return TimNhieu(danhSachGia)
                .Select(e => double.Parse(e.Text.Replace("$", "").Trim(), CultureInfo.InvariantCulture))
                .ToList();
        }

        public InventoryPage ThemBackpackVaoGio()
        {
            ClickElement(nutThemBackpack);
            return this;
        }

        public int LaySoLuongBadgeGioHang()
        {
            var badges = driver.FindElements(badgeGioHang);
            if (badges.Count == 0)
            {
                return 0;
            }

            return int.Parse(badges[0].Text.Trim(), CultureInfo.InvariantCulture);
        }

        public CartPage MoGioHang()
        {
            ClickElement(nutMoGioHang);
            wait.Until(d => d.Url.Contains("cart.html"));
            return new CartPage(driver);
        }
    }
}
