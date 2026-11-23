# Framework Kiểm Thử Tự Động SauceDemo (.NET 10) — Nâng cấp Lab 5, Lab 6 & Lab 7

Dự án nâng cấp từ khung `ASM1` (`AutomationTests.csproj`, namespace `ASM.*`), đáp ứng trọn vẹn các quy chuẩn kiến trúc **Lab 5** (Config & Data-Driven, Phần B DriverFactory), **Lab 6** (Logging vào BaseTest, `[Category]`, chạy song song thread-safe), và **Lab 7** (Sinh code Giỏ hàng đúng chuẩn POM/BaseTest, bắt lỗi AI sai & thẩm định Checklist 6 điểm).

---

## 📂 Cấu trúc thư mục chuẩn

```text
ASM1_Lab5_6_7/
├── Config/
│   ├── appsettings.json      # Chứa BaseUrl, Browser, TimeoutSeconds, Headless, WindowWidth, WindowHeight
│   ├── TestConfig.cs         # POCO cấu hình (KHÔNG viết cứng URL trong .cs)
│   └── ConfigReader.cs       # Đọc Config/appsettings.json qua AppContext.BaseDirectory
├── Core/
│   ├── DriverFactory.cs      # (Lab 5 Phần B) Tạo IWebDriver đa trình duyệt (Chrome, Edge, Firefox)
│   └── BaseTest.cs           # (Lab 5 + Lab 6 Phần B) Quản lý vòng đời, Log [SetUp]/[TearDown], Screenshot, Parallel
├── Data/
│   └── users.json            # Bộ 6 dữ liệu Data-Driven (thường, nghịch & 4 ca biên)
├── Models/
│   └── UserLoginData.cs      # Model ánh xạ Username, Password, ThanhCong, GhiChu
├── Pages/
│   ├── BasePage.cs           # WebDriverWait (Ignore NoSuchElement + StaleElement), không Thread.Sleep
│   ├── LoginPage.cs          # POM trang đăng nhập
│   ├── InventoryPage.cs      # POM trang sản phẩm (DaHienThi dùng FindElements trả false khi timeout)
│   └── CartPage.cs           # (Lab 7 Phần C) POM trang giỏ hàng (/cart.html)
├── Utilities/
│   ├── Log.cs                # (Lab 6 Phần B) Info / Error kèm dấu thời gian dùng TestContext.Progress.WriteLine
│   └── ScreenshotHelper.cs   # Chụp ảnh màn hình khi test Failed
├── Tests/
│   ├── LoginTest.cs          # Data-Driven [TestCaseSource] static với tên DangNhap_01..06
│   ├── InventoryTests.cs     # Kiểm tra số lượng sản phẩm và sắp xếp giá
│   └── CartTests.cs          # (Lab 7 Phần C) Kiểm tra thêm & xóa sản phẩm trong giỏ hàng
├── Screenshots_Lab5/         # Ảnh minh chứng Lab 5 Phần B
├── Screenshots_Lab6/         # Ảnh minh chứng Lab 6 Phần B
├── Screenshots_Lab7/         # Ảnh minh chứng Lab 7 Phần C
└── AutomationTests.csproj
```

---

## ✅ Tổng hợp thực hiện theo từng Lab

### Lab 5 — Phần B: `Core/DriverFactory.cs` (Đa trình duyệt & Đa hình `IWebDriver`)
- Phương thức `public static IWebDriver Create(TestConfig cfg)` trả về `IWebDriver`.
- Hỗ trợ **Chrome** (`--headless=new`, `--window-size`), **Edge** (`--headless=new`, `--window-size`), và **Firefox** (`--headless`, `--width`/`--height`). Không dùng `Maximize()`.
- Trình duyệt gõ sai (vd `"Chromee"`) ném `ArgumentException` nêu rõ tên bị gõ sai.
- Chạy thực tế trên cùng mã nguồn (chỉ đổi `"Browser"` trong `Config/appsettings.json`):
  - `"Browser": "Chrome"` → **8/8 Passed**.
  - `"Browser": "Edge"` (`CoreCompile` skipped) → **8/8 Passed**.

### Lab 6 — Phần B: Logging vào `BaseTest` & Chạy song song Thread-Safe
- `Utilities/Log.cs` dùng `TestContext.Progress.WriteLine` ghi `[yyyy-MM-dd HH:mm:ss.fff] [INFO]` và `[ERROR]`.
- `Core/BaseTest.cs` ghi log bắt đầu ở `[SetUp]` và kết quả (`OK` / `ROT` + đường dẫn ảnh) ở `[TearDown]`. Các file test trong `Tests/` **không tự viết log**.
- Bật `[assembly: Parallelizable(ParallelScope.Fixtures)]`, `[assembly: LevelOfParallelism(3)]`, `"Headless": true`, field `driver` là biến instance (không `static`).

### Lab 7 — Phần C: Sinh code Giỏ hàng đúng chuẩn & Thẩm định AI
- **Prompt 4 thành phần** (Vai trò · Ngữ cảnh framework · Yêu cầu rõ · Định dạng đầu ra) sinh `Pages/CartPage.cs` và `Tests/CartTests.cs`.
- **Lỗi AI sai đã bắt được trước khi commit:**
  1. Bịa locator không tồn tại (`By.Id("cart-badge-count")`, `By.ClassName("cart_item_title")`) → sửa thành `By.ClassName("shopping_cart_badge")` và `By.ClassName("inventory_item_name")` theo DOM thật trên `saucedemo.com`.
  2. Dùng `Thread.Sleep(1500)` khi mở giỏ hàng → thay bằng `wait.Until(d => d.Url.Contains("cart.html"))`.
  3. Đặt `Assert` trong `CartPage` và viết `[SetUp]` trong `CartTests` → tách đúng trách nhiệm POM và kế thừa `BaseTest`.
- **Checklist 6 điểm:** Locator thật · Assert đúng nghiệp vụ · Dùng wait · Đúng chuẩn framework · Không thừa · Hiểu từng dòng (bao gồm xử lý lỗi thực tế `StaleElementReferenceException` trên React DOM khi điều hướng từ `/inventory.html` sang `/cart.html`).

---

## 🚀 Lệnh chạy kiểm thử

```bash
dotnet restore
dotnet build
dotnet test -l "console;verbosity=detailed" --logger "trx;LogFileName=TestResults.trx"
```
