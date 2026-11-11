using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;

// Bật chạy song song theo từng Class (YC6)
[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace FirstTests 
{
    [TestFixture]
    public class LoginTests
    {
        private IWebDriver _driver;
        private BasePage _basePage;

        // YC5: Hàm đọc file JSON
        private static IEnumerable<LoginTestData> GetLoginData()
        {
            // Đường dẫn trỏ tới thư mục bin/Debug/netX.0/Data/LoginData.json
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "LoginData.json");
            string jsonString = File.ReadAllText(jsonPath);
            var data = JsonSerializer.Deserialize<List<LoginTestData>>(jsonString);
            
            foreach (var item in data)
            {
                yield return item;
            }
        }

        [SetUp]
        public void Setup()
        {
            // 1. Ghi log tiến trình bắt đầu test (YC6)
            TestContext.Progress.WriteLine($"[START] Bat dau test: {TestContext.CurrentContext.Test.Name} luc {DateTime.Now:HH:mm:ss}");

            // 2. Cấu hình Chrome Headless để chạy được trên GitHub Actions (YC9) và chạy nhanh hơn (YC6)
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--window-size=1920,1080");

            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        // Hàm dùng chung đăng nhập
        private void Login(string username, string password)
        {
            TestContext.Progress.WriteLine($"[STEP] Dang nhap voi tai khoan: {username}");
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
            TestContext.Progress.WriteLine($"[INFO] Tim thay {products.Count} san pham tren trang.");
            Assert.That(products.Count, Is.EqualTo(6), "Số lượng sản phẩm hiển thị không đúng 6 cái!");

            foreach (var item in products)
            {
                string name = item.FindElement(By.CssSelector(".inventory_item_name")).Text;
                string price = item.FindElement(By.CssSelector(".inventory_item_price")).Text;

                TestContext.Progress.WriteLine($"Tên SP: {name} | Giá: {price}");

                Assert.That(string.IsNullOrEmpty(name), Is.False, "Lỗi: Tên sản phẩm bị rỗng!");
                Assert.That(price.StartsWith("$"), Is.True, $"Lỗi: Giá của {name} không bắt đầu bằng $!");
            }
        }

        // --- PHẦN D: TEST CỐ TÌNH FAIL ĐỂ TỰ ĐỘNG CHỤP ẢNH & LẤY CI ĐỎ ---
        [Test]
        public void Test_IntentionalFail_ToCaptureScreenshot()
        {
            Login("standard_user", "secret_sauce");
            
            // Cố tình assert sai tiêu đề để test rớt và kích hoạt [TearDown] chụp ảnh
            Assert.That(driver.Title, Is.EqualTo("Sai Tieu De Co Tinh"), "Cố tình làm sai để sinh ảnh minh chứng Phần D!");
        }

        // --- PHẦN D: [TearDown] GHI LOG KẾT THÚC & TỰ ĐỘNG CHỤP ẢNH KHI THẤT BẠI ---
        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            TestContext.Progress.WriteLine($"[END] Ket thuc test: {TestContext.CurrentContext.Test.Name} -> Trang thai: {status}");

            // Kiểm tra nếu ca test bị thất bại (Failed) thì tiến hành chụp ảnh
            if (status == TestStatus.Failed)
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
                    TestContext.Progress.WriteLine($"[SCREENSHOT] Da chup anh man hinh loi: {filePath}");
                }
                catch (Exception ex)
                {
                    TestContext.WriteLine("Lỗi khi chụp ảnh màn hình: " + ex.Message);
                }
            }

            // Đóng và giải phóng driver
            if (driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
            }
        }
    }
}