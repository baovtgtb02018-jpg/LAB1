using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace FirstTests // Đảm bảo đúng tên namespace project Bài 1 của bạn
{
    public class LoginTests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        // Hàm dùng chung đăng nhập (tái sử dụng cho cả Phần B và Phần C)[cite: 2]
        private void Login(string username, string password)
        {
            driver.FindElement(By.Id("user-name")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);
            driver.FindElement(By.Id("login-button")).Click();
        }

        // --- PHẦN B: CA 1 - LUỒNG THUẬN ---
        [Test]
        public void Login_ValidCredentials_ShouldRedirectToInventory()
        {
            Login("standard_user", "secret_sauce");
            Assert.That(driver.Url, Does.Contain("inventory.html"), "Không chuyển hướng đến trang inventory!");
        }

        // --- PHẦN B: CA 2 - LUỒNG NGHỊCH ---
        [Test]
        public void Login_InvalidCredentials_ShouldShowErrorMessage()
        {
            Login("locked_out_user", "secret_sauce");
            IWebElement errorMessage = driver.FindElement(By.CssSelector("h3[data-test='error']"));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị!");
            Assert.That(errorMessage.Text, Does.Contain("Sorry, this user has been locked out"), "Nội dung thông báo lỗi không đúng!");
        }

        // --- PHẦN C: THU THẬP DỮ LIỆU SẢN PHẨM ---
        [Test]
        public void Test_GetProducts_And_Assert()
        {
            Login("standard_user", "secret_sauce");

            var products = driver.FindElements(By.CssSelector(".inventory_item"));
            Assert.That(products.Count, Is.EqualTo(6), "Số lượng sản phẩm hiển thị không đúng 6 cái!");

            foreach (var item in products)
            {
                // Bắt buộc dùng item.FindElement(...) để lấy thông tin bên trong mỗi sản phẩm[cite: 2]
                string name = item.FindElement(By.CssSelector(".inventory_item_name")).Text;
                string price = item.FindElement(By.CssSelector(".inventory_item_price")).Text;

                Console.WriteLine($"Tên SP: {name} - Giá: {price}");

                // Assert chặt theo yêu cầu đề bài[cite: 2]
                Assert.That(string.IsNullOrEmpty(name), Is.False, "Lỗi: Tên sản phẩm bị rỗng!");
                Assert.That(price.StartsWith("$"), Is.True, $"Lỗi: Giá của {name} không bắt đầu bằng $!");
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}