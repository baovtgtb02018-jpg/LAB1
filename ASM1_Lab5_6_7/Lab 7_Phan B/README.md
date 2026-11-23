# Phần B (Lab 7) — Chọn lọc & Thẩm định Test Case

## 1. Danh sách Test Case do AI đề xuất (từ Phần A)
Dưới đây là 6 test case AI sinh ra cho tính năng "Thêm sản phẩm vào giỏ hàng" trên saucedemo.com:
1. **TC1 (Ca thường):** Thêm 1 sản phẩm vào giỏ, kiểm tra icon giỏ hàng tăng lên 1.
2. **TC2 (Ca thường):** Thêm 2 sản phẩm khác nhau, kiểm tra số lượng trong giỏ là 2.
3. **TC3 (Ca biên):** Thêm sản phẩm, sau đó xóa sản phẩm đó khỏi giỏ, kiểm tra giỏ hàng trống.
4. **TC4 (Ca lỗi):** Nhấn nút "Add to cart" 100 lần liên tiếp, kiểm tra giỏ hàng vẫn chỉ có 1 sản phẩm (chống spam click).
5. **TC5 (Ca lỗi):** Thêm sản phẩm khi đã hết hàng (Out of stock), kiểm tra nút "Add to cart" bị vô hiệu hóa.
6. **TC6 (Ca biên):** Thêm tối đa 99 sản phẩm vào giỏ hàng, kiểm tra giỏ hàng hiển thị đúng 99.

---

## 2. Bảng phân loại GIỮ / BỎ / SỬA

| Mã TC | Tên Ca Kiểm Thử | Quyết định | Lý do |
| :--- | :--- | :---: | :--- |
| **TC1** | Thêm 1 sản phẩm, check icon giỏ hàng | **GIỮ** | Áp dụng tốt cho saucedemo. Hành vi trang thực tế: badge giỏ hàng hiển thị số "1". |
| **TC2** | Thêm 2 sản phẩm khác nhau | **GIỮ** | Áp dụng tốt. Số lượng badge sẽ là "2". |
| **TC3** | Thêm rồi xóa sản phẩm khỏi giỏ | **GIỮ** | Áp dụng tốt, kiểm tra luồng xóa (Remove). |
| **TC4** | Spam click 100 lần nút "Add to cart" | **BỎ** | *(Xem giải thích bên dưới)* |
| **TC5** | Thêm sản phẩm khi đã hết hàng | **BỎ** | *(Xem giải thích bên dưới)* |
| **TC6** | Thêm tối đa 99 sản phẩm | **SỬA** | *(Xem giải thích bên dưới)* |

---

## 3. Giải thích lý do BỎ (và Sửa) các ca không dùng được

### ❌ Lý do BỎ TC4: "Nhấn nút Add to cart 100 lần liên tiếp"
- **Đối chiếu thực tế:** Khi nhấn nút "Add to cart" trên saucedemo, nút đó sẽ ngay lập tức đổi thành nút "Remove" (màu đỏ). 
- **Vì sao BỎ:** Bạn không thể click 100 lần vào nút "Add to cart" vì nó đã biến thành nút "Remove" sau lần click đầu tiên. Nếu code automation cố tình tìm nút "Add to cart" để click 100 lần, nó sẽ bị lỗi `NoSuchElementException` ngay ở lần thứ 2. Ca này không phù hợp với thiết kế UI của trang.

### ❌ Lý do BỎ TC5: "Thêm sản phẩm khi đã hết hàng (Out of stock)"
- **Đối chiếu thực tế:** Trang saucedemo.com là trang giả lập, **tất cả 6 sản phẩm đều luôn luôn trong trạng thái "Còn hàng"**. Không có sản phẩm nào hiển thị chữ "Out of stock".
- **Vì sao BỎ:** Không có dữ liệu (sản phẩm hết hàng) để test. Nếu viết code tìm kiếm phần tử "Out of stock" hay kiểm tra nút bị vô hiệu hóa, test sẽ luôn bị **Failed** vì trang web không có tính năng này. AI đã nhầm lẫn sang một trang thương mại điện tử thực tế (như Shopee, Tiki).

### ❌ Lý do BỎ TC6 (Sửa lại): "Thêm tối đa 99 sản phẩm"
- **Đối chiếu thực tế:** Giỏ hàng của saucedemo chỉ có đúng 6 sản phẩm (vì trang chỉ bán 6 món). Không có cách nào thêm được 99 sản phẩm.
- **Vì sao BỎ (hoặc Sửa):** 
  - **Nếu BỎ:** Vì dữ liệu đầu vào (99 sản phẩm) là không tưởng trên trang này.
  - **Nếu SỬA:** Ta phải sửa lại thành *"Thêm toàn bộ 6 sản phẩm có sẵn vào giỏ hàng, kiểm tra badge hiển thị số 6"*. Đây mới là ca biên (Max boundary) thực tế của trang web.