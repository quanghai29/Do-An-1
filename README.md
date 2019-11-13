# MiniProject-Batch-Rename

Thông tin nhóm thực hiện
- Mai Linh Đồng		-		1712349
- Nguyễn Hữu Duy  -   1712384
- Lê Quang Hải		-		1712407

Một số điểm đặc biệt cần lưu ý:
+ Phần Xử Lý các chức năng: có đầy đủ các chức năng Replace, newCase, moveISBN, uniqueName, fullName normalize
+ Phần Thêm File, Thêm Thư mục: thêm tất cả các file có trong thư mục được chọn, thêm tất cả các thư mục con của thư mục được chọn
+ Phần tiện ích trong sử dụng: 
  . Người dùng có thể thêm nhiều hành động vào list method
  . Người dùng có thể edit // xóa từng method
  . Người dùng có thể xem trước kết quả trên màn hình, lỗi trùng file , ngay sau khi nhấn start
  . Người 
  
+ Phần moveISBN có sử dụng biểu thức Regular Expression để kiểm tra chuỗi tên file/folder có đúng định dạng ISBN-Name hoặc Name-ISBN hay không.
+ Phần uniqueName sử dụng Guid để tạo chuỗi ngẫu nhiên
+Phần NewCase cho phép người dùng chọn 1 trong 3 option: Upper case, Lower case, Cammel case.
  .Trong màn hình Edit của NewCase nếu người dùng không chọn bất kì option nào mà nhấn OK thì sẽ xuất hiện hộp thoại nhắc nhở.
  .Nếu người dùng đã Add method Newcase mà chưa chọn option để rename mà nhấn Start thì sẽ được nhắc nhở phải chọn 1 option để hệ thống biết nên áp hành động nào lên đối tượng cần rename.
  .Trong màn hình Edit ngoài nút OK còn có cả nút Cancel để người dùng có thể hủy lựa chọn của mình nếu muốn.
  .Với Cammel case: kí tự đầu tiên của một từ được xác định khi trước nó là một kí tự không thuộc bộ chữ cái la tinh.
+Phần Fullname Normalize và Cammel case(thuộc New case) có sử dụng chung một hàm thay thế kí tự đầu tiên của một từ từ lower sang upper
