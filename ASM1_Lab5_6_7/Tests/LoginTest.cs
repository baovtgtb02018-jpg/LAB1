using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NUnit.Framework;
using ASM.Core;
using ASM.Models;
using ASM.Pages;

namespace ASM.Tests
{
    [TestFixture]
    [Category("Login")]
    public class LoginTest : BaseTest
    {
        public static IEnumerable<TestCaseData> LayDuLieuDangNhap()
        {
            string dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "users.json");
            if (!File.Exists(dataPath))
            {
                throw new FileNotFoundException($"Khong tim thay file du lieu: {dataPath}", dataPath);
            }

            string json = File.ReadAllText(dataPath);
            var users = JsonSerializer.Deserialize<List<UserLoginData>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<UserLoginData>();

            for (int i = 0; i < users.Count; i++)
            {
                UserLoginData u = users[i];
                string nhanTen = FormatTenCaTest(u.Username);
                yield return new TestCaseData(u)
                    .SetName($"DangNhap_{i + 1:00}_{nhanTen}");
            }
        }

        private static string FormatTenCaTest(string username)
        {
            if (string.IsNullOrEmpty(username)) return "Trong";
            string trimmed = username.Trim();
            if (username != trimmed) return "KhoangTrangThua";
            return trimmed.Length > 20 ? trimmed[..20] : trimmed;
        }

        [Test]
        [Category("Smoke")]
        [TestCaseSource(nameof(LayDuLieuDangNhap))]
        public void KiemTraDangNhap_DataDriven(UserLoginData data)
        {
            var loginPage = new LoginPage(driver, config.TimeoutSeconds);
            var inventoryPage = loginPage.DangNhap(data.Username, data.Password);

            if (data.ThanhCong)
            {
                Assert.That(inventoryPage.DaHienThi(), Is.True,
                    $"Ca '{data.GhiChu}' ky vong thanh cong nhung khong vao duoc trang san pham.");
            }
            else
            {
                Assert.That(inventoryPage.DaHienThi(), Is.False,
                    $"Ca '{data.GhiChu}' ky vong that bai nhung lai vao duoc trang san pham.");
                Assert.That(loginPage.LayThongBaoLoi(), Is.Not.Empty,
                    $"Ca '{data.GhiChu}' phai hien thi thong bao loi.");
            }
        }
    }
}
