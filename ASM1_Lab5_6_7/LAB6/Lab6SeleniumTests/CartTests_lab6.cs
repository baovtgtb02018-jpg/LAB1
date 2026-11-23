using NUnit.Framework;
using OpenQA.Selenium;

namespace Lab6SeleniumTests
{
    [TestFixture]
    [Category("Cart")]
    public class CartTests : BaseTest
    {
        [Test]
        [Category("Smoke")]
        public void Cart_AddBackpack_BadgeShowsOne()
        {
            Driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            Driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            Driver.FindElement(By.Id("login-button")).Click();

            Driver.FindElement(By.Id("add-to-cart-sauce-labs-backpack")).Click();

            IWebElement badge = Driver.FindElement(By.ClassName("shopping_cart_badge"));
            Assert.That(badge.Text, Is.EqualTo("1"));
        }

        [Test]
        [Category("Slow")]
        public void Checkout_CompleteOrder_ShowsThankYouMessage()
        {
            Driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            Driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            Driver.FindElement(By.Id("login-button")).Click();

            Driver.FindElement(By.Id("add-to-cart-sauce-labs-backpack")).Click();
            Driver.FindElement(By.ClassName("shopping_cart_link")).Click();
            Driver.FindElement(By.Id("checkout")).Click();

            Driver.FindElement(By.Id("first-name")).SendKeys("Dat");
            Driver.FindElement(By.Id("last-name")).SendKeys("Pham");
            Driver.FindElement(By.Id("postal-code")).SendKeys("70000");
            Driver.FindElement(By.Id("continue")).Click();
            Driver.FindElement(By.Id("finish")).Click();

            IWebElement completeHeader = Driver.FindElement(By.ClassName("complete-header"));
            Assert.That(completeHeader.Text, Does.Contain("Thank you for your order!"));
        }
    }
}