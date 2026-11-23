using NUnit.Framework;
using ASM.Core;
using ASM.Pages;

namespace ASM.Tests
{
    [TestFixture]
    [Category("Inventory")]
    public class InventoryTests : BaseTest
    {
        [Test]
        [Category("Smoke")]
        public void Test_GetProducts_And_Assert()
        {
            var loginPage = new LoginPage(driver, config.TimeoutSeconds);
            var inventoryPage = loginPage.DangNhap("standard_user", "secret_sauce");

            Assert.That(inventoryPage.DaHienThi(), Is.True, "Khong vao duoc trang san pham!");
            Assert.That(inventoryPage.DemSoLuongSanPham(), Is.EqualTo(6), "So luong san pham hien thi khong dung 6 cai!");
        }

        [Test]
        [Category("Sort")]
        public void SapXepGia_ThapDenCao_TangDan()
        {
            var gia = new LoginPage(driver, config.TimeoutSeconds)
                .DangNhap("standard_user", "secret_sauce")
                .SapXep("lohi")
                .LayDanhSachGia();

            Assert.That(gia, Has.Count.EqualTo(6));
            Assert.That(gia, Is.Ordered.Ascending);
        }
    }
}
