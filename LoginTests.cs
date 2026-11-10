using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;

namespace FirstTests 
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

        // Hàm dùng chung đăng nhập
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
                string name = item.FindElement(By.CssSelector(".inventory_item_name")).Text;
                string price = item.FindElement(By.CssSelector(".inventory_item_price")).Text;

                Console.WriteLine($"Tên SP: {name} | Giá: {price}");

                Assert.That(string.IsNullOrEmpty(name), Is.False, "Lỗi: Tên sản phẩm bị rỗng!");
                Assert.That(price.StartsWith("$"), Is.True, $"Lỗi: Giá của {name} không bắt đầu bằng $!");
            }
        }

 // --- PHẦN D: TEST CỐ TÌNH FAIL ĐỂ TỰ ĐỘNG CHỤP ẢNH ---
        [Test]
        [Ignore("Tam bo qua ca test nay de pipeline CI chay xanh")]
        public void Test_IntentionalFail_ToCaptureScreenshot()
        {
            Login("standard_user", "secret_sauce");
            
            // Cố tình assert sai tiêu đề để test rớt và kích hoạt [TearDown] chụp ảnh
            Assert.That(driver.Title, Is.EqualTo("Sai Tieu De Co Tinh"), "Cố tình làm sai để sinh ảnh minh chứng Phần D!");
        }

        // --- PHẦN D: [TearDown] TỰ ĐỘNG CHỤP ẢNH KHI THẤT BẠI ---
        [TearDown]
        public void TearDown()
        {
            // Kiểm tra nếu ca test bị thất bại (Failed) thì tiến hành chụp ảnh
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                try
                {
                    var testName = TestContext.CurrentContext.Test.Name;
                    var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    
                    // Tạo thư mục screenshots trong thư mục dự án nếu chưa có
                    var dir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "screenshots");
                    Directory.CreateDirectory(dir);
                    
                    var filePath = Path.Combine(dir, $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    screenshot.SaveAsFile(filePath);
                    
                    // Đính kèm ảnh vào NUnit TestContext
                    TestContext.AddTestAttachment(filePath);
                }
                catch (Exception ex)
                {
                    TestContext.WriteLine("Lỗi khi chụp ảnh màn hình: " + ex.Message);
                }
            }

            // Đóng và giải phóng driver
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}