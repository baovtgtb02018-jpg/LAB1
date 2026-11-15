using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SeleniumTest
{
    public class InventoryPage : BasePage
    {
        private readonly By tieuDeTrang = By.ClassName("title");
        private readonly By danhSachSanPham = By.ClassName("inventory_item");
        private readonly By oChonSapXep = By.ClassName("product_sort_container");
        private readonly By danhSachGia = By.ClassName("inventory_item_price");

        public InventoryPage(IWebDriver driver) : base(driver)
        {
        }

        public bool DaHienThi()
        {
            return driver.Url.Contains("inventory.html") && Tim(tieuDeTrang).Displayed;
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
    }
}