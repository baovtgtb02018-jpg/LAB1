using NUnit.Framework;
using SeleniumFramework.Core;
using SeleniumFramework.Pages;

namespace SeleniumFramework.Tests;

[TestFixture]
public class LoginTests : BaseTest
{
    [Test]
    public void DangNhap_ThanhCong()
    {
        var loginPage = new LoginPage(Driver);
        loginPage.DangNhap("standard_user", "secret_sauce");

        var inventory = new InventoryPage(Driver);
        Assert.That(inventory.DaHienThi(), Is.True,
            "Đáng lẽ phải vào được trang sản phẩm");
    }

    [Test]
    public void DangNhap_SaiMatKhau_ThatBai()
    {
        var loginPage = new LoginPage(Driver);
        loginPage.DangNhap("standard_user", "sai_mat_khau");

        var inventory = new InventoryPage(Driver);
        Assert.That(inventory.DaHienThi(), Is.False,
            "Đáng lẽ không được vào trang sản phẩm");
    }
}