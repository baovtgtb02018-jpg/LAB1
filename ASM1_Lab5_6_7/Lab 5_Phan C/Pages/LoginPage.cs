using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumFramework.Pages;

public class LoginPage
{
    private readonly IWebDriver _driver;

    public LoginPage(IWebDriver driver) => _driver = driver;

    private IWebElement UsernameInput =>
        _driver.FindElement(By.Id("user-name"));

    private IWebElement PasswordInput =>
        _driver.FindElement(By.Id("password"));

    private IWebElement LoginButton =>
        _driver.FindElement(By.Id("login-button"));

    public void DangNhap(string username, string password)
    {
        UsernameInput.Clear();
        UsernameInput.SendKeys(username);
        PasswordInput.Clear();
        PasswordInput.SendKeys(password);
        LoginButton.Click();
    }
}