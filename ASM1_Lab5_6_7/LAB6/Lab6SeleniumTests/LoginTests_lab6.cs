using NUnit.Framework;
using OpenQA.Selenium;

namespace Lab6SeleniumTests
{
    [TestFixture]
    [Category("Login")]
    public class LoginTests : BaseTest
    {
        [Test]
        [Category("Smoke")]
        public void Login_ValidStandardUser_OpensInventoryPage()
        {
            IWebElement usernameInput = Driver.FindElement(By.Id("user-name"));
            usernameInput.Clear();
            usernameInput.SendKeys("standard_user");

            IWebElement passwordInput = Driver.FindElement(By.Id("password"));
            passwordInput.Clear();
            passwordInput.SendKeys("secret_sauce");

            IWebElement loginButton = Driver.FindElement(By.Id("login-button"));
            loginButton.Click();

            string currentUrl = Driver.Url;
            Assert.That(currentUrl, Does.Contain("inventory.html"));
        }

        [Test]
        public void Login_InvalidPassword_ShowsErrorMessage()
        {
            IWebElement usernameInput = Driver.FindElement(By.Id("user-name"));
            usernameInput.Clear();
            usernameInput.SendKeys("standard_user");

            IWebElement passwordInput = Driver.FindElement(By.Id("password"));
            passwordInput.Clear();
            passwordInput.SendKeys("wrong_password");

            IWebElement loginButton = Driver.FindElement(By.Id("login-button"));
            loginButton.Click();

            IWebElement errorBox = Driver.FindElement(By.CssSelector("[data-test='error']"));
            Assert.That(errorBox.Displayed, Is.True);
        }
    }
}