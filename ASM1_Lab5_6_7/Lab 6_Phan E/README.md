# Phần E (Lab 6) — AI sinh Test Case & Thẩm định

## 1. Prompt đã sử dụng
"Tôi đang kiểm thử trang web saucedemo.com. Hãy đóng vai một chuyên gia QA, liệt kê cho tôi 5 ca kiểm thử (Test Case) cho ô nhập 'Mã bưu điện' (Postal Code) ở bước thanh toán (Checkout). Yêu cầu bao gồm cả các ca biên (nhập chữ, nhập rỗng, nhập ký tự đặc biệt). Hãy viết câu Assert dưới dạng NUnit C# (sử dụng Assert.That) cho từng ca."

## 2. Kết quả AI trả về (Nguyên văn)

Dưới đây là 5 ca kiểm thử cho ô nhập "Mã bưu điện" (Postal Code) trên trang saucedemo.com, kèm theo câu Assert tương ứng bằng NUnit C#:

**Ca 1: Nhập mã bưu điện hợp lệ (số)**
- Hành động: Nhập "700000" và nhấn Continue.
- Kỳ vọng: Chuyển sang bước tiếp theo (trang Overview).
- Assert: `Assert.That(driver.Url, Does.Contain("checkout-step-two"));`

**Ca 2: Bỏ trống mã bưu điện**
- Hành động: Để trống ô Postal Code và nhấn Continue.
- Kỳ vọng: Hiển thị thông báo lỗi yêu cầu nhập.
- Assert: `Assert.That(driver.FindElement(By.CssSelector("h3[data-test='error']")).Text, Does.Contain("Postal Code is required"));`

**Ca 3: Nhập chữ cái vào ô mã bưu điện**
- Hành động: Nhập "ABCXYZ" và nhấn Continue.
- Kỳ vọng: Hệ thống báo lỗi định dạng (chỉ cho phép số).
- Assert: `Assert.That(driver.FindElement(By.CssSelector("h3[data-test='error']")).Text, Does.Contain("Invalid format"));`

**Ca 4: Nhập ký tự đặc biệt**
- Hành động: Nhập "!@#$%^" và nhấn Continue.
- Kỳ vọng: Hệ thống báo lỗi.
- Assert: `Assert.That(driver.FindElement(By.CssSelector("h3[data-test='error']")).Text, Does.Contain("Error"));`

**Ca 5: Kiểm tra độ dài tối đa**
- Hành động: Nhập chuỗi 100 ký tự số.
- Kỳ vọng: Ô nhập bị giới hạn chỉ cho phép nhập tối đa 10 ký tự.
- Assert: `Assert.That(postalCodeInput.GetAttribute("value").Length, Is.LessThanOrEqualTo(10));`


## 3. Thẩm định: 2 ca KHÔNG dùng được và giải thích

Mặc dù AI sinh ra cú pháp NUnit C# rất chuẩn, nhưng khi áp dụng thực tế vào trang **saucedemo.com**, có 2 ca không thể dùng được:

### ❌ Ca 3: "Nhập chữ cái vào ô mã bưu điện" (Kỳ vọng báo lỗi định dạng)
- **Lý do không dùng được:** Trang `saucedemo.com` là một trang web giả lập (dummy site) để học automation. Ô Postal Code ở trang này **KHÔNG HỀ CÓ validation** (kiểm tra định dạng). Nếu bạn nhập chữ "ABCXYZ" và bấm Continue, trang web vẫn chấp nhận và chuyển thẳng sang trang Overview (bước 2). Do đó, câu `Assert.That(..., Does.Contain("Invalid format"))` sẽ bị **Failed** vì không có thông báo lỗi nào xuất hiện. AI đã áp đặt logic của một trang thương mại điện tử thực tế vào một trang demo.

### ❌ Ca 5: "Kiểm tra độ dài tối đa" (Kỳ vọng giới hạn 10 ký tự)
- **Lý do không dùng được:** AI tự bịa ra một quy tắc nghiệp vụ (Business Rule) là "chỉ cho phép nhập tối đa 10 ký tự". Trên thực tế, thẻ `<input>` của trường Postal Code trên saucedemo **không hề có thuộc tính `maxlength`**. Bạn có thể gõ 100 ký tự thoải mái. Vì vậy, câu Assert `Is.LessThanOrEqualTo(10)` sẽ luôn bị **Failed**. 

*(Ngoài ra, Ca 4 dùng `Does.Contain("Error")` quá chung chung, nếu chạy thật có thể sẽ pass do bắt được chữ "Error" ở các chỗ khác, nhưng nó không phản ánh đúng thông điệp lỗi cụ thể của hệ thống).*