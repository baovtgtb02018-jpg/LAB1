using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ASM.Pages
{
    public class CartPage : BasePage
    {
        private readonly By tieuDeTrang = By.ClassName("title");
        private readonly By danhSachMucGioHang = By.ClassName("cart_item");
        private readonly By tenSanPhamTrongGio = By.ClassName("inventory_item_name");
        private readonly By giaSanPhamTrongGio = By.ClassName("inventory_item_price");
        private readonly By nutXoaBackpack = By.Id("remove-sauce-labs-backpack");
        private readonly By nutTiepTucMuaSam = By.Id("continue-shopping");
        private readonly By nutThanhToan = By.Id("checkout");

        public CartPage(IWebDriver driver, int timeoutSeconds = 10) : base(driver, timeoutSeconds)
        {
        }

        public bool DaHienThi()
        {
            try
            {
                var shortWait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                shortWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
                return shortWait.Until(d =>
                    d.Url.Contains("cart.html") &&
                    d.FindElements(tieuDeTrang).Any(e => e.Displayed && e.Text == "Your Cart"));
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public int DemSoLuongSanPhamTrongGio()
        {
            return driver.FindElements(danhSachMucGioHang).Count;
        }

        public List<string> LayDanhSachTenSanPham()
        {
            return TimNhieu(tenSanPhamTrongGio)
                .Select(e => e.Text.Trim())
                .ToList();
        }

        public List<double> LayDanhSachGiaSanPham()
        {
            return TimNhieu(giaSanPhamTrongGio)
                .Select(e => double.Parse(e.Text.Replace("$", "").Trim(), CultureInfo.InvariantCulture))
                .ToList();
        }

        public CartPage XoaBackpackKhoiGio()
        {
            ClickElement(nutXoaBackpack);
            return this;
        }

        public InventoryPage TiepTucMuaSam()
        {
            ClickElement(nutTiepTucMuaSam);
            wait.Until(d => d.Url.Contains("inventory.html"));
            return new InventoryPage(driver);
        }

        public bool NutThanhToanDaSanSang()
        {
            var btn = Tim(nutThanhToan);
            return btn.Displayed && btn.Enabled;
        }
    }
}
