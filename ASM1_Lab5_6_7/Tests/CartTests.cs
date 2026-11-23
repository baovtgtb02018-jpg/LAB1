using NUnit.Framework;
using ASM.Core;
using ASM.Pages;

namespace ASM.Tests
{
    [TestFixture]
    [Category("Cart")]
    public class CartTests : BaseTest
    {
        [Test]
        [Category("Smoke")]
        public void ThemSanPhamVaoGio_HienThiDungTrongGioHang()
        {
            var inventoryPage = new LoginPage(driver, config.TimeoutSeconds)
                .DangNhap("standard_user", "secret_sauce")
                .ThemBackpackVaoGio();

            Assert.That(inventoryPage.LaySoLuongBadgeGioHang(), Is.EqualTo(1),
                "Badge gio hang phai hien thi so luong la 1 sau khi them 1 san pham.");

            var cartPage = inventoryPage.MoGioHang();

            Assert.That(cartPage.DaHienThi(), Is.True, "Khong mo duoc trang gio hang (/cart.html).");
            Assert.That(cartPage.DemSoLuongSanPhamTrongGio(), Is.EqualTo(1), "So luong dong san pham trong gio khong dung.");
            Assert.That(cartPage.LayDanhSachTenSanPham(), Does.Contain("Sauce Labs Backpack"));
            Assert.That(cartPage.LayDanhSachGiaSanPham(), Is.EqualTo(new[] { 29.99 }));
            Assert.That(cartPage.NutThanhToanDaSanSang(), Is.True, "Nut Checkout phai san sang thao tac.");
        }

        [Test]
        [Category("Cart")]
        public void XoaSanPhamKhoiGio_GioHangTrong()
        {
            var cartPage = new LoginPage(driver, config.TimeoutSeconds)
                .DangNhap("standard_user", "secret_sauce")
                .ThemBackpackVaoGio()
                .MoGioHang()
                .XoaBackpackKhoiGio();

            Assert.That(cartPage.DaHienThi(), Is.True, "Van phai o lai trang gio hang sau khi xoa san pham.");
            Assert.That(cartPage.DemSoLuongSanPhamTrongGio(), Is.EqualTo(0), "Gio hang phai trong sau khi xoa san pham duy nhat.");

            var inventoryPage = cartPage.TiepTucMuaSam();
            Assert.That(inventoryPage.DaHienThi(), Is.True, "Bam Continue Shopping phai quay ve trang san pham.");
            Assert.That(inventoryPage.LaySoLuongBadgeGioHang(), Is.EqualTo(0), "Badge gio hang phai bien mat khi gio trong.");
        }
    }
}
