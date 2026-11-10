# Hướng dẫn chạy Project

Dưới đây là các lệnh cần thiết để thiết lập và kiểm thử dự án:

1. Khôi phục các thư viện phụ thuộc:
`dotnet restore`

2. Chạy kiểm thử tự động:
`dotnet test`

Trí tuệ nhân tạo thực sự là một trợ lý đắc lực để tăng tốc quy trình phát triển phần mềm, đặc biệt hữu ích khi cần tạo nhanh cấu trúc mã boilerplate, viết kịch bản kiểm thử tự động hay khoanh vùng lỗi cú pháp. Tuy nhiên, AI thường tỏ ra thiếu chính xác và dễ bị "ảo giác" thông tin khi phải xử lý các luồng logic nghiệp vụ phức tạp hoặc cấu hình framework đặc thù của từng dự án. Nó đôi khi gọi sai phiên bản thư viện, đề xuất các hàm không tồn tại, hoặc bỏ qua tính bảo mật của luồng dữ liệu. Do đó, AI cực kỳ hiệu quả để giải quyết các tác vụ lặp đi lặp lại và gợi ý hướng đi, nhưng người lập trình luôn phải kiểm chứng lại code để đảm bảo ứng dụng hoạt động chính xác.

| Phần tử   | Locator 1 (Ưu tiên)   | Locator 2                          | Lý do chọn                    |
| :-------- | :-------------------- | :--------------------------------- | :---------------------------- |
| Username  | #user-name            | [data-test="username"]             | id là duy nhất, dễ đọc.       |
| Password  | #password             | [data-test="password"]             | id là duy nhất.               |
| Nút Login | #login-button         | [data-test="login-button"]         | id là duy nhất.               |
| Tên SP    | #item_4_title_link    | [data-test="inventory-item-name"]  | id duy nhất cho từng SP.      |
| Giá SP    | .inventory_item_price | [data-test="inventory-item-price"] | Class dùng chung cho mọi giá. |

Nhận xét kết quả của AI:
AI đã đề xuất rất chính xác việc ưu tiên sử dụng thuộc tính data-test làm CSS Selector vì đây là thuộc tính sinh ra để phục vụ automation test, giúp locator không bị gãy khi UI thay đổi. Khi tôi copy locator [data-test='add-to-cart-sauce-labs-backpack'] dán vào tab Elements và tìm bằng Ctrl+F, kết quả trả về đúng 1 of 1 như mong đợi. Tuy nhiên, AI chưa nhắc đến điểm trừ là locator này bị gắn cứng (hardcode) với tên một sản phẩm cụ thể, nếu muốn tái sử dụng cho nút Add to cart khác thì cần phải tham số hóa.

Phần A giải thích
Ở test không có Wait, Selenium tìm phần tử #finish ngay sau khi bấm Start. Tuy nhiên, phần tử này được JavaScript tạo ra sau vài giây, nên Selenium không tìm thấy và quăng lỗi NoSuchElementException. Ở test có Wait, Selenium kiên nhẫn chờ đến khi phần tử xuất hiện (tối đa 10 giây) nên đã tìm thấy và test Passed thành công.

Nhận xét AI
AI đã xác định đúng nguyên nhân gây ra lỗi NoSuchElementException là do Selenium tìm phần tử quá sớm. Gợi ý sử dụng WebDriverWait kết hợp Until là phù hợp với yêu cầu bài Lab 3. Sau khi thêm Explicit Wait, tôi kiểm chứng bằng cách chạy lại test và nhận thấy test có thể Pass ổn định hơn. AI cũng đề xuất một số cách chờ khác, tuy nhiên Thread.Sleep không phù hợp với yêu cầu của đề. Qua bài tập này, tôi hiểu rõ hơn nguyên nhân gây ra Flaky Test và cách sử dụng Explicit Wait để khắc phục.

file code test
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge; // Đã đổi sang thư viện của Edge
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;

namespace SauceDemoTests
{
    public class Lab3Tests
    {
        IWebDriver driver;
        WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            // Khởi tạo EdgeDriver thay vì ChromeDriver để fix lỗi Win32Exception
            driver = new EdgeDriver();
            driver.Manage().Window.Maximize();
            // Chỉ dùng Explicit Wait 10s, KHÔNG dùng Implicit Wait theo yêu cầu
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        // ================= PHẦN A =================
        [Test]
        public void PartA_Step1_NoWait_ShouldFail()
        {
            // Cố tình để test này RỚT để lấy ảnh chụp màn hình cho Phần D
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");
            driver.FindElement(By.CssSelector("#start button")).Click();
            
            // Tìm ngay phần tử khi JavaScript chưa kịp tạo ra -> Sẽ văng NoSuchElementException
            var finishText = driver.FindElement(By.Id("finish")).Text;
        }

        [Test]
        public void PartA_Step2_ExplicitWait_ShouldPass()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");
            driver.FindElement(By.CssSelector("#start button")).Click();
            
            // Dùng Explicit Wait chờ đến khi phần tử #finish xuất hiện
            var finishElement = wait.Until(d => d.FindElement(By.Id("finish")));
            Assert.That(finishElement.Text, Is.EqualTo("Hello World!"));
        }

        // ================= PHẦN B =================
        [Test]
        public void PartB_DropdownAndAssertPrice()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();

            var sortDropdown = driver.FindElement(By.ClassName("product_sort_container"));
            var select = new SelectElement(sortDropdown);
            select.SelectByText("Price (low to high)");

            var priceElements = driver.FindElements(By.ClassName("inventory_item_price"));
            List<double> prices = new List<double>();

            foreach (var element in priceElements)
            {
                string priceText = element.Text.Replace("$", "").Trim();
                double price = double.Parse(priceText, System.Globalization.CultureInfo.InvariantCulture);
                prices.Add(price);
            }

            // Assert giá thực sự tăng dần
            Assert.That(prices, Is.Ordered.Ascending, "Lỗi: Danh sách giá không được sắp xếp tăng dần!");
        }

        // ================= PHẦN C =================
        [Test]
        public void PartC_AlertHandling()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");
            
            // 1. Alert (Accept)
            driver.FindElement(By.XPath("//button[text()='Click for JS Alert']")).Click();
            driver.SwitchTo().Alert().Accept();
            Assert.That(driver.FindElement(By.Id("result")).Text, Is.EqualTo("You successfully clicked an alert"));
            
            // 2. Confirm (Dismiss)
            driver.FindElement(By.XPath("//button[text()='Click for JS Confirm']")).Click();
            driver.SwitchTo().Alert().Dismiss();
            Assert.That(driver.FindElement(By.Id("result")).Text, Is.EqualTo("You clicked: Cancel"));
            
            // 3. Prompt (SendKeys + Accept)
            driver.FindElement(By.XPath("//button[text()='Click for JS Prompt']")).Click();
            var prompt = driver.SwitchTo().Alert();
            prompt.SendKeys("Phạm Thành Đạt");
            prompt.Accept();
            Assert.That(driver.FindElement(By.Id("result")).Text, Is.EqualTo("You entered: Phạm Thành Đạt"));
        }

        [Test]
        public void PartC_IframeHandling()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/iframe");
            
            // Chuyển vào Iframe
            driver.SwitchTo().Frame("mce_0_ifr");
            var editor = driver.FindElement(By.Id("tinymce"));
            editor.Clear();
            editor.SendKeys("Đạt đang test iframe!");
            
            // Trở về trang gốc
            driver.SwitchTo().DefaultContent();
        }

        // ================= PHẦN D =================
        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                // Chỉ chụp ảnh khi test thất bại
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                {
                    var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    string testName = TestContext.CurrentContext.Test.Name;
                    
                    // Tạo thư mục screenshots nếu chưa có
                    string screenshotDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "screenshots");
                    Directory.CreateDirectory(screenshotDir);
                    
                    string path = Path.Combine(screenshotDir, $"{testName}.png");
                    screenshot.SaveAsFile(path);
                    
                    // Đính kèm vào báo cáo test
                    TestContext.AddTestAttachment(path);
                }
                
                driver.Quit();
                driver.Dispose(); 
            }
        }
    }
}
// Cap nhat boi nhanh feature-conflict