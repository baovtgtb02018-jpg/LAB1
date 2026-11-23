using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using LAB5.Core;
using LAB5.Pages;

namespace LAB5.Tests
{
    public class LoginUserData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool ThanhCong { get; set; }
        public string GhiChu { get; set; }
    }

    [TestFixture]
    public class LoginTests : BaseTest
    {
        private By usernameInput = By.Id("user-name");
        private By passwordInput = By.Id("password");
        private By loginButton = By.Id("login-button");

        public static IEnumerable<TestCaseData> LayDuLieuDangNhap()
        {
            string jsonPath = Path.Combine(AppContext.BaseDirectory, "Data", "users.json");
            string jsonContent = File.ReadAllText(jsonPath);
            List<LoginUserData> users = JsonSerializer.Deserialize<List<LoginUserData>>(jsonContent);

            List<TestCaseData> testCases = new List<TestCaseData>();
            if (users == null)
            {
                return testCases;
            }

            int index = 0;
            while (index < users.Count)
            {
                LoginUserData u = users[index];
                string safeNamePart = u.Username;
                if (string.IsNullOrWhiteSpace(safeNamePart))
                {
                    safeNamePart = "EmptyUser";
                }
                else
                {
                    safeNamePart = safeNamePart.Trim();
                }

                string indexText = index.ToString("D2");
                string caseName = "DangNhap_" + indexText + "_" + safeNamePart;

                TestCaseData dataItem = new TestCaseData(u.Username, u.Password, u.ThanhCong, u.GhiChu);
                dataItem = dataItem.SetName(caseName);
                testCases.Add(dataItem);

                index = index + 1;
            }

            return testCases;
        }

        [TestCaseSource(nameof(LayDuLieuDangNhap))]
        public void KiemTraDangNhap(string username, string password, bool thanhCong, string ghiChu)
        {
            IWebElement userBox = driver.FindElement(usernameInput);
            userBox.Clear();
            if (username != null)
            {
                userBox.SendKeys(username);
            }

            IWebElement passBox = driver.FindElement(passwordInput);
            passBox.Clear();
            if (password != null)
            {
                passBox.SendKeys(password);
            }

            driver.FindElement(loginButton).Click();

            InventoryPage inventoryPage = new InventoryPage(driver);
            bool actualResult = inventoryPage.DaHienThi();

            Assert.That(actualResult, Is.EqualTo(thanhCong), ghiChu);
        }
    }
}