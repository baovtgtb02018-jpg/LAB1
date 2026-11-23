using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ASM.Pages
{
    public class LoginPage : BasePage
    {
        private readonly By oTaiKhoan = By.Id("user-name");
        private readonly By oMatKhau = By.Id("password");
        private readonly By nutDangNhap = By.Id("login-button");
        private readonly By khungThongBaoLoi = By.CssSelector("[data-test='error']");

        public LoginPage(IWebDriver driver, int timeoutSeconds = 10) : base(driver, timeoutSeconds)
        {
        }

        public InventoryPage DangNhap(string tk, string mk)
        {
            EnterText(oTaiKhoan, tk);
            EnterText(oMatKhau, mk);
            ClickElement(nutDangNhap);
            return new InventoryPage(driver);
        }

        public string LayThongBaoLoi()
        {
            return Tim(khungThongBaoLoi).Text;
        }

        public bool ThongBaoLoiDaHienThi()
        {
            try
            {
                var shortWait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                shortWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
                return shortWait.Until(d => d.FindElements(khungThongBaoLoi).Any(e => e.Displayed));
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}
