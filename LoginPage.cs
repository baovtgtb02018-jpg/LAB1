using OpenQA.Selenium;

namespace SeleniumTest
{
    public class LoginPage : BasePage
    {
        private readonly By oTaiKhoan = By.Id("user-name");
        private readonly By oMatKhau = By.Id("password");
        private readonly By nutDangNhap = By.Id("login-button");
        private readonly By khungThongBaoLoi = By.CssSelector("[data-test='error']");

        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public InventoryPage DangNhap(string tk, string mk)
        {
            var userElement = Tim(oTaiKhoan);
            userElement.Clear();
            userElement.SendKeys(tk);

            var passElement = Tim(oMatKhau);
            passElement.Clear();
            passElement.SendKeys(mk);

            Tim(nutDangNhap).Click();
            return new InventoryPage(driver);
        }

        public string LayThongBaoLoi()
        {
            return Tim(khungThongBaoLoi).Text;
        }
    }
}