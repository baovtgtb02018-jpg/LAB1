# Hướng dẫn chạy Project

Dưới đây là các lệnh cần thiết để thiết lập và kiểm thử dự án:

1. Khôi phục các thư viện phụ thuộc:
`dotnet restore`

2. Chạy kiểm thử tự động:
`dotnet test`

Trí tuệ nhân tạo thực sự là một trợ lý đắc lực để tăng tốc quy trình phát triển phần mềm, đặc biệt hữu ích khi cần tạo nhanh cấu trúc mã boilerplate, viết kịch bản kiểm thử tự động hay khoanh vùng lỗi cú pháp. Tuy nhiên, AI thường tỏ ra thiếu chính xác và dễ bị "ảo giác" thông tin khi phải xử lý các luồng logic nghiệp vụ phức tạp hoặc cấu hình framework đặc thù của từng dự án. Nó đôi khi gọi sai phiên bản thư viện, đề xuất các hàm không tồn tại, hoặc bỏ qua tính bảo mật của luồng dữ liệu. Do đó, AI cực kỳ hiệu quả để giải quyết các tác vụ lặp đi lặp lại và gợi ý hướng đi, nhưng người lập trình luôn phải kiểm chứng lại code để đảm bảo ứng dụng hoạt động chính xác.

| Phần tử   | Locator 1 (Ưu tiên)   | Locator 2                          | Lý do chọn                    |
| :-------- | :-------------------- | :--------------------------------- | :---------------------------- |
| Username  | #user-name            | [data-test="username"]             | id là duy nhất, dễ đọc.       |
| Password  | #password             | [data-test="password"]             | id là duy nhất.               |
| Nút Login | #login-button         | [data-test="login-button"]         | id là duy nhất.               |
| Tên SP    | #item_4_title_link    | [data-test="inventory-item-name"]  | id duy nhất cho từng SP.      |
| Giá SP    | .inventory_item_price | [data-test="inventory-item-price"] | Class dùng chung cho mọi giá. |

Nhận xét kết quả của AI:
AI đã đề xuất rất chính xác việc ưu tiên sử dụng thuộc tính data-test làm CSS Selector vì đây là thuộc tính sinh ra để phục vụ automation test, giúp locator không bị gãy khi UI thay đổi. Khi tôi copy locator [data-test='add-to-cart-sauce-labs-backpack'] dán vào tab Elements và tìm bằng Ctrl+F, kết quả trả về đúng 1 of 1 như mong đợi. Tuy nhiên, AI chưa nhắc đến điểm trừ là locator này bị gắn cứng (hardcode) với tên một sản phẩm cụ thể, nếu muốn tái sử dụng cho nút Add to cart khác thì cần phải tham số hóa.