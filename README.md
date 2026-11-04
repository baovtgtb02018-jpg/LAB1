# Hướng dẫn chạy Project

Dưới đây là các lệnh cần thiết để thiết lập và kiểm thử dự án:

1. Khôi phục các thư viện phụ thuộc:
`dotnet restore`

2. Chạy kiểm thử tự động:
`dotnet test`

Trí tuệ nhân tạo thực sự là một trợ lý đắc lực để tăng tốc quy trình phát triển phần mềm, đặc biệt hữu ích khi cần tạo nhanh cấu trúc mã boilerplate, viết kịch bản kiểm thử tự động hay khoanh vùng lỗi cú pháp. Tuy nhiên, AI thường tỏ ra thiếu chính xác và dễ bị "ảo giác" thông tin khi phải xử lý các luồng logic nghiệp vụ phức tạp hoặc cấu hình framework đặc thù của từng dự án. Nó đôi khi gọi sai phiên bản thư viện, đề xuất các hàm không tồn tại, hoặc bỏ qua tính bảo mật của luồng dữ liệu. Do đó, AI cực kỳ hiệu quả để giải quyết các tác vụ lặp đi lặp lại và gợi ý hướng đi, nhưng người lập trình luôn phải kiểm chứng lại code để đảm bảo ứng dụng hoạt động chính xác.