using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTest
{
    [TestFixture]
    public class LoginTests
    {
        private IWebDriver driver = null!;
        private LoginPage loginPage = null!;
        private InventoryPage inventoryPage = null!;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            loginPage = new LoginPage(driver);
        }

        [Test]
        public void Login_ValidCredentials_ShouldRedirectToInventory()
        {
            inventoryPage = loginPage.DangNhap("standard_user", "secret_sauce");
            Assert.That(inventoryPage.DaHienThi(), Is.True, "Không chuyển hướng đến trang inventory!");
        }

        [Test]
        public void Login_InvalidCredentials_ShouldShowErrorMessage()
        {
            loginPage.DangNhap("locked_out_user", "secret_sauce");
            string errorText = loginPage.LayThongBaoLoi();
            Assert.That(errorText, Does.Contain("Sorry, this user has been locked out"), "Nội dung thông báo lỗi không đúng!");
        }

        [Test]
        public void Test_GetProducts_And_Assert()
        {
            inventoryPage = loginPage.DangNhap("standard_user", "secret_sauce");
            int productCount = inventoryPage.DemSoLuongSanPham();
            Assert.That(productCount, Is.EqualTo(6), "Số lượng sản phẩm hiển thị không đúng 6 cái!");
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
            driver?.Dispose();
        }
    }
}