/*		Đề tài: B24
	Tìm hiểu về lập trình Socket: Viết chương trình tạo hai quá trình server
	và client ở hai máy khác nhau. Client nhận một dãy số từ người sử dụng
	và gửi cho quá trình server. Server sắp xếp chuỗi này theo thứ tự tăng
	dần và gửi trả lại client để hiển thị cho người dùng biết.
*/

/* Thông tin chi tiết của các thành viên nhóm thực hiện */

/* Thành viên 1:
		Họ Tên: Hoàng Văn Việt
		Mã SV: 17150214
		Ngày Sinh: 21/08/1999
		Lớp: TNCNTT16
		Khóa: K16
*/

/* Thành viên 2:
		Họ Tên: Phan Thanh Tùng
		Mã SV: 17150212
		Ngày Sinh: 03/10/1999
		Lớp: TNCNTT16
		Khóa: K16
*/

/*
	Hướng dẫn sử dụng: 	chạy FILE với Visual Studio 20xx có cài đặt C#  
		B1: Tải file rar có tên "B24-HVViet-PTTung.rar"
		B2: Giải nén file rar, ta được thư mục "B24-HVViet-PTTung"
		B3: Sau khi mở thư mục "B24-HVViet-PTTung", trong thư mục "Source":
			- Chạy file "Client-Server.sln" -> Start, ta khởi chạy được Server
			- Vào thư mục "\Source\CLIENT\bin\Debug", chọn mở file CLIENT.exe
				+ Lưu ý là phải chạy file Server trước khi chạy file Client
			- File code nguồn Client trong thư mục "Client" có tên: Client.cs
			- File code nguồn Server trong thư mục "Server" có tên Server.cs
		B4: Kiểm tra
	Có thể kiểm tra bằng các bộ test có sẵn do sinh viên đưa ra:
	1. "6   5   4 3    2 1"
	2. "5.6 6 5 4 3 2 1    "
	3. "5.6 6 5 4 a y t e 3 2 1"
*/