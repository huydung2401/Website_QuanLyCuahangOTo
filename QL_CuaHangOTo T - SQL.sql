CREATE DATABASE WebsiteMuaBanOtoDB;
GO
USE WebsiteMuaBanOtoDB;
GO

/* =====================================================
   1. BẢNG DANH MỤC XE (Loại xe) - Bảng cha
   ===================================================== */
CREATE TABLE DanhMucXe (
    IdDanhMuc VARCHAR(10) NOT NULL,
    TenDanhMuc NVARCHAR(100) NOT NULL,       -- Sedan, SUV, Bán tải...
    MoTa NVARCHAR(500),

    CONSTRAINT PK_DanhMucXe PRIMARY KEY (IdDanhMuc)
);
GO

/* =====================================================
   2. BẢNG HÃNG XE - Bảng cha
   ===================================================== */
CREATE TABLE HangXe (
    IdHangXe VARCHAR(10) NOT NULL,
    TenHang NVARCHAR(100) NOT NULL,          -- Toyota, Honda...
    QuocGia NVARCHAR(50),

    CONSTRAINT PK_HangXe PRIMARY KEY (IdHangXe)
);
GO

/* =====================================================
   3. BẢNG NGƯỜI DÙNG - Bảng cha
   ===================================================== */
CREATE TABLE NguoiDung (
    IdNguoiDung VARCHAR(10) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    MatKhau NVARCHAR(200) NOT NULL,
    DienThoai NVARCHAR(20),
    DiaChi NVARCHAR(200),
    VaiTro NVARCHAR(20) DEFAULT 'User',     -- User / Seller / Admin
    NgayTao DATETIME DEFAULT GETDATE(),
    TrangThai BIT DEFAULT 1,

    CONSTRAINT PK_NguoiDung PRIMARY KEY (IdNguoiDung)
);
GO

/* =====================================================
   4. BẢNG YÊU CẦU TƯ VẤN - Bảng độc lập
   ===================================================== */
CREATE TABLE YeuCauTuVan (
    IdTuVan INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100),
    SoDienThoai VARCHAR(20),
    
    -- 3 Câu hỏi khảo sát
    MucLuong NVARCHAR(100),        
    DongXeYeuThich NVARCHAR(100), 
    MucGiaMongMuon NVARCHAR(100), 
    
    -- Phần trả lời của Admin
    PhanHoiCuaAdmin NVARCHAR(MAX), 
    
    -- Trạng thái
    TrangThai NVARCHAR(50) DEFAULT N'Chờ tư vấn', -- 'Chờ tư vấn' hoặc 'Đã tư vấn'
    NgayGui DATETIME DEFAULT GETDATE()
);
GO

/* =====================================================
   5. BẢNG DÒNG XE (Model) - Phụ thuộc HangXe
   ===================================================== */
CREATE TABLE DongXe (
    IdDongXe VARCHAR(10) NOT NULL,
    TenDong NVARCHAR(100) NOT NULL,          -- Vios, Camry, Civic...
    IdHangXe VARCHAR(10) NOT NULL,

    CONSTRAINT PK_DongXe PRIMARY KEY (IdDongXe),
    CONSTRAINT FK_DongXe_HangXe FOREIGN KEY (IdHangXe) REFERENCES HangXe(IdHangXe)
);
GO

/* =====================================================
   6. BẢNG LỊCH SỬ ĐĂNG NHẬP - Phụ thuộc NguoiDung
   ===================================================== */
CREATE TABLE LichSuDangNhap (
    IdLichSu INT IDENTITY(1,1) PRIMARY KEY,
    IdNguoiDung VARCHAR(10),
    DiaChiIP NVARCHAR(100),
    ThietBi NVARCHAR(100),
    ThoiGian DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (IdNguoiDung) REFERENCES NguoiDung(IdNguoiDung)
);
GO

/* =====================================================
   7. BẢNG XE - Phụ thuộc NguoiDung, DanhMuc, HangXe, DongXe
   ===================================================== */
CREATE TABLE Xe (
    IdXe VARCHAR(10) NOT NULL,
    TieuDe NVARCHAR(200) NOT NULL,
    Gia DECIMAL(18,2) NOT NULL,
    NamSX INT,
    SoKM INT,

    HopSo NVARCHAR(50),
    NhienLieu NVARCHAR(50),
    MauSac NVARCHAR(50),

    DongCo NVARCHAR(100),               -- 1.5L, 2.0 Turbo...
    CongSuat NVARCHAR(100),             -- 150hp...
    KichThuoc NVARCHAR(200),            -- DxRxC
    XuatXu NVARCHAR(100),               -- Lắp ráp / Nhập khẩu

    MoTaNgan NVARCHAR(500),
    MoTaChiTiet NVARCHAR(MAX),
    DiaDiem NVARCHAR(200),

    TrangThaiTin NVARCHAR(20) DEFAULT N'Chờ duyệt',  -- Chờ duyệt, Đã duyệt, Từ chối, Đã bán
    NgayDang DATETIME DEFAULT GETDATE(),

    IdNguoiBan VARCHAR(10) NOT NULL,
    IdDanhMuc VARCHAR(10) NOT NULL,
    IdHangXe VARCHAR(10) NOT NULL,
    IdDongXe VARCHAR(10) NOT NULL,

    CONSTRAINT PK_Xe PRIMARY KEY (IdXe),
    CONSTRAINT FK_Xe_NguoiDung FOREIGN KEY (IdNguoiBan) REFERENCES NguoiDung(IdNguoiDung),
    CONSTRAINT FK_Xe_DanhMuc FOREIGN KEY (IdDanhMuc) REFERENCES DanhMucXe(IdDanhMuc),
    CONSTRAINT FK_Xe_HangXe FOREIGN KEY (IdHangXe) REFERENCES HangXe(IdHangXe),
    CONSTRAINT FK_Xe_DongXe FOREIGN KEY (IdDongXe) REFERENCES DongXe(IdDongXe)
);
GO

/* =====================================================
   8. BẢNG LỊCH LÁI THỬ - Phụ thuộc Xe, NguoiDung
   ===================================================== */
CREATE TABLE LaiThu (
    IdLaiThu INT IDENTITY(1,1) PRIMARY KEY,
    IdXe VARCHAR(10) NOT NULL,
    IdNguoiDung VARCHAR(10) NOT NULL,  -- Người đăng ký
    NgayHen DATETIME NOT NULL,         -- Ngày giờ muốn lái thử
    GhiChu NVARCHAR(MAX),
    TrangThai NVARCHAR(50) DEFAULT N'Chờ xác nhận', -- Chờ xác nhận, Đã xác nhận, Đã hủy, Đã xong
    NgayTao DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_LaiThu_Xe FOREIGN KEY (IdXe) REFERENCES Xe(IdXe),
    CONSTRAINT FK_LaiThu_NguoiDung FOREIGN KEY (IdNguoiDung) REFERENCES NguoiDung(IdNguoiDung)
);
GO

/* =====================================================
    9. BẢNG ĐẶT CỌC - Phụ thuộc Xe, NguoiDung
   ===================================================== */
CREATE TABLE DatCoc (
    IdDatCoc INT IDENTITY(1,1) PRIMARY KEY,
    IdXe VARCHAR(10) NOT NULL,
    IdNguoiDung VARCHAR(10) NOT NULL,  -- Người đặt cọc
    SoTienCoc DECIMAL(18,2) NOT NULL,  -- Ví dụ: 5,000,000
    PhuongThucTT NVARCHAR(50),         -- Chuyển khoản / Tiền mặt
    GhiChu NVARCHAR(MAX),
    TrangThai NVARCHAR(50) DEFAULT N'Chờ thanh toán', -- Chờ thanh toán, Đã cọc, Hoàn tiền
    NgayDat DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_DatCoc_Xe FOREIGN KEY (IdXe) REFERENCES Xe(IdXe),
    CONSTRAINT FK_DatCoc_NguoiDung FOREIGN KEY (IdNguoiDung) REFERENCES NguoiDung(IdNguoiDung)
);
GO

ALTER TABLE DatCoc
ADD LyDoHuy NVARCHAR(MAX) NULL,
    NgayHuy DATETIME NULL;

/* =====================================================
   📌 10. BẢNG HÌNH ẢNH XE - Phụ thuộc Xe
   ===================================================== */
CREATE TABLE XeHinhAnh (
    IdHinhAnh INT IDENTITY(1,1) PRIMARY KEY,
    IdXe VARCHAR(10) NOT NULL,
    HinhAnh NVARCHAR(255) NOT NULL,

    CONSTRAINT FK_XeHinhAnh_Xe FOREIGN KEY (IdXe) REFERENCES Xe(IdXe)
);
GO

/* =====================================================
   11. BẢNG LIÊN HỆ - Phụ thuộc Xe
   ===================================================== */
CREATE TABLE LienHe (
    IdLienHe INT IDENTITY(1,1) PRIMARY KEY,
    IdXe VARCHAR(10) NOT NULL,
    TenNguoiMua NVARCHAR(100),
    DienThoai NVARCHAR(20),
    Email NVARCHAR(100),
    NoiDung NVARCHAR(MAX),
    NgayGui DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_LienHe_Xe FOREIGN KEY (IdXe) REFERENCES Xe(IdXe)
);
GO

/* =====================================================
   12. BẢNG ĐÁNH GIÁ XE - Phụ thuộc Xe, NguoiDung
   ===================================================== */
CREATE TABLE DanhGia (
    IdDanhGia INT IDENTITY(1,1) PRIMARY KEY,
    IdXe VARCHAR(10) NOT NULL,
    IdNguoiDung VARCHAR(10),
    SoSao INT CHECK (SoSao BETWEEN 1 AND 5),
    NoiDung NVARCHAR(MAX),
    NgayDanhGia DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_DanhGia_Xe FOREIGN KEY (IdXe) REFERENCES Xe(IdXe),
    CONSTRAINT FK_DanhGia_NguoiDung FOREIGN KEY (IdNguoiDung) REFERENCES NguoiDung(IdNguoiDung)
);
GO

/* =====================================================
   13. BẢNG YÊU THÍCH - Phụ thuộc Xe, NguoiDung
   ===================================================== */
CREATE TABLE YeuThich (
    IdYeuThich INT IDENTITY(1,1) PRIMARY KEY,
    IdNguoiDung VARCHAR(10) NOT NULL,
    IdXe VARCHAR(10) NOT NULL,
    NgayLuu DATETIME DEFAULT GETDATE(),

    CONSTRAINT UQ_YeuThich UNIQUE (IdNguoiDung, IdXe),
    CONSTRAINT FK_YeuThich_User FOREIGN KEY (IdNguoiDung) REFERENCES NguoiDung(IdNguoiDung),
    CONSTRAINT FK_YeuThich_Xe FOREIGN KEY (IdXe) REFERENCES Xe(IdXe)
);
GO

/* =====================================================
   14. BẢNG LỊCH SỬ DUYỆT TIN - Phụ thuộc Xe, NguoiDung
   ===================================================== */
CREATE TABLE LichSuDuyetTin (
    IdDuyet INT IDENTITY(1,1) PRIMARY KEY,
    IdXe VARCHAR(10) NOT NULL,
    IdAdmin VARCHAR(10) NOT NULL,
    NoiDung NVARCHAR(MAX),
    TrangThai NVARCHAR(20),             -- Đã duyệt / Từ chối
    NgayDuyet DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_DuyetTin_Xe FOREIGN KEY (IdXe) REFERENCES Xe(IdXe),
    CONSTRAINT FK_DuyetTin_Admin FOREIGN KEY (IdAdmin) REFERENCES NguoiDung(IdNguoiDung)
);
GO

/* =====================================================
    15. INSERT DỮ LIỆU CÁC BẢNG CHA
   ===================================================== */
INSERT INTO DanhMucXe VALUES
('DM01', N'Sedan', N'Xe 4 chỗ, gầm thấp'),
('DM02', N'SUV',   N'Xe gầm cao, 5–7 chỗ'),
('DM03', N'Thai tải', N'Pickup mạnh mẽ'),
('DM04', N'MPV',   N'Xe gia đình 7 chỗ'),
('DM05', N'Luxury', N'Xe sang hạng cao');
GO

INSERT INTO HangXe VALUES
('HX01', N'Toyota', N'Nhật'),
('HX02', N'Honda',  N'Nhật'),
('HX03', N'BMW',    N'Đức'),
('HX04', N'Mercedes', N'Đức'),
('HX05', N'VinFast', N'Việt Nam'),
('HX06', N'Ford', N'Mỹ'),
('HX07', N'Peugeot', N'Pháp');
GO

INSERT INTO DongXe VALUES
('DX01','Vios','HX01'),
('DX02','Camry','HX01'),
('DX03','Fortuner','HX01'),
('DX04','Civic','HX02'),
('DX05','CR-V','HX02'),
('DX06','3 Series','HX03'),
('DX07','5 Series','HX03'),
('DX08','C-Class','HX04'),
('DX09','GLC','HX04'),
('DX10','VF 8','HX05');
GO

INSERT INTO NguoiDung VALUES
('ND001', N'Admin', 'admin@oto.vn', '123456', '0900000000', N'Hà Nội', 'Admin', GETDATE(), 1),
('ND002', N'Nguyễn Văn A', 'seller@oto.vn', '123456', '0911002200', N'HCM', 'Seller', GETDATE(), 1),
('ND003', N'Trần Thị B', 'buyer@oto.vn', '123456', '0933302200', N'Đà Nẵng', 'User', GETDATE(), 1),
('ND004', N'Lê Văn C', 'user4@oto.vn', '123456', '0988776655', N'Hải Phòng', 'User', GETDATE(), 1);
GO

INSERT INTO NguoiDung VALUES
('ND005', N'Hoàng Thị D', 'user5@oto.vn', '123456', '0977665544', N'HCM', 'User', GETDATE(), 1),
('ND006', N'Phạm Văn E', 'user6@oto.vn', '123456', '0966554433', N'Hà Nội', 'User', GETDATE(), 1);
GO

/* =====================================================
   16. INSERT DỮ LIỆU BẢNG XE (Sau khi đã có NguoiDung, HangXe, DongXe)
   ===================================================== */
INSERT INTO Xe VALUES
('XE001', N'Toyota Vios 2020 số tự động', 450000000, 2020, 35000,
 N'Tự động', N'Xăng', N'Trắng', N'1.5L', N'107hp', N'4420 x 1700 x 1475', N'Nhật',
 N'Xe gia đình đi giữ gìn', N'Toyota Vios 2020 bản E số tự động, không đâm đụng', N'Hồ Chí Minh',
 N'Đã duyệt', GETDATE(), 'ND002','DM01','HX01','DX01'),

('XE002', N'Honda Civic 2019 Turbo', 620000000, 2019, 42000,
 N'Tự động', N'Xăng', N'Đỏ', N'1.5 Turbo', N'170hp', N'4584 x 1799 x 1433', N'Nhật',
 N'Xe đẹp không lỗi nhỏ', N'Civic bản RS thể thao mạnh mẽ', N'Hà Nội',
 N'Đã duyệt', GETDATE(), 'ND002','DM01','HX02','DX04'),

('XE003', N'Toyota Camry 2021', 1020000000, 2021, 28000,
 N'Tự động', N'Xăng', N'Đen', N'2.5L', N'181hp', N'4885 x 1840 x 1445', N'Nhật',
 N'Cực mới 98%', N'Camry 2021 full option', N'Đà Nẵng',
 N'Đã duyệt', GETDATE(), 'ND002','DM01','HX01','DX02'),

('XE004', N'VinFast VF8 2023 Eco', 950000000, 2023, 5000,
 N'Tự động', N'Điện', N'Xanh', N'150kW', N'201hp', N'4750 x 1900 x 1660', N'Việt Nam',
 N'Xe lướt như mới', N'VF8 Eco chạy ít, pin thuê', N'Hà Nội',
 N'Đã duyệt', GETDATE(), 'ND002','DM02','HX05','DX10'),

('XE005', N'Mercedes GLC 300 4Matic', 2350000000, 2020, 32000,
 N'Tự động', N'Xăng', N'Trắng', N'2.0 Turbo', N'258hp', N'4670 x 1900 x 1640', N'Đức',
 N'Sang trọng, xe cá nhân', N'Mercedes GLC 300 động cơ mạnh mẽ', N'HCM',
 N'Chờ duyệt', GETDATE(), 'ND002','DM05','HX04','DX09');
GO

INSERT INTO Xe VALUES
('XE006', N'Hyundai Tucson 2021 ', 820000000, 2021, 25000,
 N'Tự động', N'Xăng', N'Đỏ', N'2.0L', N'155hp', N'4475 x 1850 x 1660', N'Hàn Quốc',
 N'Xe gia đình sử dụng, không taxi', N'Tucson 2021 bản đặc biệt nhiều option', N'Hà Nội',
 N'Đã duyệt', GETDATE(), 'ND002','DM03','HX03','DX07'),

('XE007', N'Mazda CX-5 2018 Luxury', 720000000, 2018, 60000,
 N'Tự động', N'Xăng', N'Xanh', N'2.0L', N'154hp', N'4550 x 1840 x 1680', N'Nhật',
 N'Xe chạy kỹ, bảo dưỡng đầy đủ', N'CX-5 2018 Luxury, tiết kiệm nhiên liệu', N'Hồ Chí Minh',
 N'Đã duyệt', GETDATE(), 'ND003','DM03','HX01','DX03'),

('XE008', N'Ford Ranger Wildtrak 2020', 860000000, 2020, 45000,
 N'Tự động', N'Dầu', N'Cam', N'2.0L Bi-Turbo', N'213hp', N'5362 x 1860 x 1830', N'Mỹ',
 N'Bán tải mạnh mẽ, không offroad', N'Ranger Wildtrak 2020 bản cao cấp', N'Đồng Nai',
 N'Đã duyệt', GETDATE(), 'ND002','DM04','HX06','DX08'),

('XE009', N'Kia Seltos 2022 Premium', 690000000, 2022, 18000,
 N'Tự động', N'Xăng', N'Vàng', N'1.4 Turbo', N'138hp', N'4385 x 1800 x 1645', N'Hàn Quốc',
 N'Xe mới 98%, nội thất đẹp', N'Seltos Premium 2022 bản cao', N'Cần Thơ',
 N'Đã duyệt', GETDATE(), 'ND004','DM03','HX03','DX06'),

('XE010', N'Mitsubishi Xpander 2019', 540000000, 2019, 52000,
 N'Tự động', N'Xăng', N'Bạc', N'1.5L', N'104hp', N'4475 x 1750 x 1730', N'Nhật',
 N'Xe gia đình, giữ gìn', N'Xpander 2019 bản AT tiết kiệm', N'Hải Phòng',
 N'Đã duyệt', GETDATE(), 'ND002','DM02','HX01','DX05'),

('XE011', N'BMW 320i Sport Line 2017', 1180000000, 2017, 75000,
 N'Tự động', N'Xăng', N'Trắng', N'2.0 Turbo', N'184hp', N'4633 x 1811 x 1429', N'Đức',
 N'Chạy chuẩn 75k km, nội thất sang trọng', N'BMW 320i Sport Line, xe đẹp không lỗi', N'Hồ Chí Minh',
 N'Đã duyệt', GETDATE(), 'ND005','DM05','HX04','DX09'),

('XE012', N'Audi Q5 2018 Quattro', 1600000000, 2018, 50000,
 N'Tự động', N'Xăng', N'Đen', N'2.0 Turbo', N'252hp', N'4663 x 1893 x 1659', N'Đức',
 N'Xe nhập Châu Âu, bảo dưỡng hãng', N'Audi Q5 Quattro 2018 cao cấp', N'Đà Nẵng',
 N'Đã duyệt', GETDATE(), 'ND002','DM05','HX04','DX09'),

('XE013', N'Peugeot 3008 2020 Active', 890000000, 2020, 30000,
 N'Tự động', N'Xăng', N'Cam', N'1.6 Turbo', N'165hp', N'4450 x 1840 x 1620', N'Pháp',
 N'Nội thất đẹp, form mới', N'Peugeot 3008 Active 2020 giá tốt', N'Hà Nội',
 N'Đã duyệt', GETDATE(), 'ND002','DM03','HX07','DX06'),

('XE014', N'Mercedes C200 Exclusive 2019', 1390000000, 2019, 40000,
 N'Tự động', N'Xăng', N'Đen', N'1.5 Turbo', N'184hp', N'4686 x 1810 x 1442', N'Đức',
 N'Xe sang chạy ít, còn rất mới', N'C200 Exclusive 2019 full option', N'Hồ Chí Minh',
 N'Đã duyệt', GETDATE(), 'ND006','DM05','HX04','DX09'),

('XE015', N'VinFast Fadil 2021 tiêu chuẩn', 340000000, 2021, 22000,
 N'Tự động', N'Xăng', N'Đỏ', N'1.4L', N'98hp', N'3676 x 1632 x 1495', N'Việt Nam',
 N'Xe lướt tiết kiệm xăng', N'Fadil 2021 chạy êm bền bỉ', N'Nghệ An',
 N'Đã duyệt', GETDATE(), 'ND002','DM02','HX05','DX10'),

('XE016', N'Toyota Altis 2017 1.8G', 595000000, 2017, 68000,
 N'Tự động', N'Xăng', N'Trắng', N'1.8L', N'138hp', N'4620 x 1775 x 1460', N'Nhật',
 N'Xe zin không đâm đụng', N'Corolla Altis 1.8G 2017, bền bỉ tiết kiệm', N'Hồ Chí Minh',
 N'Đã duyệt', GETDATE(), 'ND002','DM01','HX01','DX02'),

-- ===== TOYOTA =====
('XE017', N'Toyota Camry 2019 2.0G', 950000000, 2019, 52000,
 N'Tự động', N'Xăng', N'Đen', N'2.0L', N'165hp',
 N'4885 x 1840 x 1445', N'Nhật',
 N'Sedan bền bỉ', N'Camry 2.0G 2019 xe gia đình',
 N'Hà Nội', N'Đã duyệt', GETDATE(), 'ND002','DM01','HX01','DX02'),

('XE018', N'Toyota Camry 2020 2.5Q', 1150000000, 2020, 43000,
 N'Tự động', N'Xăng', N'Trắng', N'2.5L', N'207hp',
 N'4885 x 1840 x 1445', N'Nhật',
 N'Full option', N'Camry 2.5Q 2020 cao cấp',
 N'HCM', N'Đã duyệt', GETDATE(), 'ND002','DM01','HX01','DX02'),

('XE019', N'Toyota Fortuner 2019 2.4AT', 980000000, 2019, 68000,
 N'Tự động', N'Dầu', N'Trắng', N'2.4L', N'147hp',
 N'4795 x 1855 x 1835', N'Nhật',
 N'SUV gia đình', N'Fortuner 2019 máy dầu',
 N'Đà Nẵng', N'Đã duyệt', GETDATE(), 'ND002','DM02','HX01','DX03'),

('XE020', N'Toyota Fortuner 2020 Legender', 1180000000, 2020, 52000,
 N'Tự động', N'Dầu', N'Đen', N'2.4L', N'147hp',
 N'4795 x 1855 x 1835', N'Nhật',
 N'Bản thể thao', N'Fortuner Legender 2020',
 N'HCM', N'Đã duyệt', GETDATE(), 'ND002','DM02','HX01','DX03'),

-- ===== HONDA =====
('XE021', N'Honda Civic 2018 1.8G', 560000000, 2018, 72000,
 N'Tự động', N'Xăng', N'Đỏ', N'1.8L', N'139hp',
 N'4648 x 1799 x 1416', N'Nhật',
 N'Sedan thể thao', N'Civic 1.8G 2018',
 N'Hà Nội', N'Đã duyệt', GETDATE(), 'ND002','DM01','HX02','DX04'),

('XE022', N'Honda Civic 2019 RS', 680000000, 2019, 55000,
 N'Tự động', N'Xăng', N'Xanh', N'1.5L', N'170hp',
 N'4648 x 1799 x 1416', N'Nhật',
 N'Bản RS', N'Civic RS 2019',
 N'HCM', N'Đã duyệt', GETDATE(), 'ND002','DM01','HX02','DX04'),

('XE023', N'Honda CR-V 2020 L', 920000000, 2020, 46000,
 N'Tự động', N'Xăng', N'Trắng', N'1.5L', N'188hp',
 N'4623 x 1855 x 1679', N'Nhật',
 N'SUV bán chạy', N'CR-V L 2020',
 N'Hà Nội', N'Đã duyệt', GETDATE(), 'ND002','DM02','HX02','DX05'),

-- ===== BMW =====
('XE024', N'BMW 320i 2017 Sport', 1150000000, 2017, 78000,
 N'Tự động', N'Xăng', N'Trắng', N'2.0L', N'184hp',
 N'4633 x 1811 x 1429', N'Đức',
 N'Sedan Đức', N'BMW 320i Sport',
 N'HCM', N'Đã duyệt', GETDATE(), 'ND005','DM05','HX03','DX06'),

('XE025', N'BMW 320i 2018 Luxury', 1250000000, 2018, 65000,
 N'Tự động', N'Xăng', N'Đen', N'2.0L', N'184hp',
 N'4633 x 1811 x 1429', N'Đức',
 N'Xe sang', N'BMW 320i Luxury',
 N'Hà Nội', N'Đã duyệt', GETDATE(), 'ND006','DM05','HX03','DX06'),

('XE026', N'BMW 520i 2019 Luxury', 1380000000, 2019, 52000,
 N'Tự động', N'Xăng', N'Xanh', N'2.0L', N'184hp',
 N'4963 x 1868 x 1479', N'Đức',
 N'Sedan cao cấp', N'BMW 520i 2019',
 N'HCM', N'Đã duyệt', GETDATE(), 'ND006','DM05','HX03','DX07'),

-- ===== MERCEDES =====
('XE027', N'Mercedes C200 2019', 1250000000, 2019, 48000,
 N'Tự động', N'Xăng', N'Đen', N'1.5L', N'184hp',
 N'4686 x 1810 x 1442', N'Đức',
 N'Sedan sang', N'C200 2019',
 N'HCM', N'Đã duyệt', GETDATE(), 'ND002','DM05','HX04','DX08'),

('XE028', N'Mercedes C200 2020', 1350000000, 2020, 42000,
 N'Tự động', N'Xăng', N'Trắng', N'1.5L', N'184hp',
 N'4686 x 1810 x 1442', N'Đức',
 N'Xe đẹp', N'C200 2020',
 N'Hà Nội', N'Đã duyệt', GETDATE(), 'ND002','DM05','HX04','DX08'),

('XE029', N'Mercedes GLC 300 2019', 1950000000, 2019, 52000,
 N'Tự động', N'Xăng', N'Đen', N'2.0L', N'258hp',
 N'4670 x 1900 x 1640', N'Đức',
 N'SUV sang', N'GLC 300 2019',
 N'HCM', N'Đã duyệt', GETDATE(), 'ND002','DM05','HX04','DX09'),

-- ===== VINFAST =====
('XE030', N'VinFast VF8 2023 Eco', 950000000, 2023, 8000,
 N'Tự động', N'Điện', N'Xanh', N'150kW', N'201hp',
 N'4750 x 1900 x 1660', N'Việt Nam',
 N'Xe điện', N'VF8 Eco 2023',
 N'Hà Nội', N'Đã duyệt', GETDATE(), 'ND002','DM02','HX05','DX10'),

('XE031', N'VinFast VF8 2023 Plus', 1150000000, 2023, 6000,
 N'Tự động', N'Điện', N'Đen', N'300kW', N'402hp',
 N'4750 x 1900 x 1660', N'Việt Nam',
 N'Bản cao', N'VF8 Plus 2023',
 N'HCM', N'Đã duyệt', GETDATE(), 'ND002','DM02','HX05','DX10'),

 -- ===== FORD =====
('XE032', N'Ford Ranger Wildtrak 2021', 890000000, 2021, 42000,
 N'Tự động', N'Dầu', N'Cam', N'2.0L', N'213hp',
 N'5362 x 1860 x 1830', N'Mỹ',
 N'Bán tải cao cấp', N'Ranger Wildtrak 2021 máy Bi-Turbo',
 N'Bình Dương', N'Đã duyệt', GETDATE(), 'ND002','DM03','HX06','DX10'),

('XE033', N'Ford Everest 2020 Titanium', 1120000000, 2020, 56000,
 N'Tự động', N'Dầu', N'Đen', N'2.0L', N'210hp',
 N'4914 x 1860 x 1837', N'Mỹ',
 N'SUV 7 chỗ', N'Everest Titanium 2020',
 N'Hà Nội', N'Đã duyệt', GETDATE(), 'ND002','DM02','HX06','DX10'),

-- ===== PEUGEOT =====
('XE034', N'Peugeot 3008 2020 Active', 860000000, 2020, 48000,
 N'Tự động', N'Xăng', N'Trắng', N'1.6L', N'165hp',
 N'4450 x 1840 x 1620', N'Pháp',
 N'SUV châu Âu', N'Peugeot 3008 Active 2020',
 N'HCM', N'Đã duyệt', GETDATE(), 'ND002','DM02','HX07','DX05'),

('XE035', N'Peugeot 5008 2021 Allure', 980000000, 2021, 39000,
 N'Tự động', N'Xăng', N'Xanh', N'1.6L', N'165hp',
 N'4641 x 1844 x 1646', N'Pháp',
 N'SUV gia đình', N'Peugeot 5008 Allure 2021',
 N'Đà Nẵng', N'Đã duyệt', GETDATE(), 'ND002','DM02','HX07','DX05'),

-- ===== TOYOTA =====
('XE036', N'Toyota Vios 2021 G CVT', 525000000, 2021, 35000,
 N'Tự động', N'Xăng', N'Trắng', N'1.5L', N'107hp',
 N'4420 x 1700 x 1475', N'Nhật',
 N'Sedan phổ thông', N'Vios G 2021 tiết kiệm',
 N'Cần Thơ', N'Đã duyệt', GETDATE(), 'ND002','DM01','HX01','DX01'),

('XE037', N'Toyota Vios 2020 E CVT', 465000000, 2020, 48000,
 N'Tự động', N'Xăng', N'Bạc', N'1.5L', N'107hp',
 N'4420 x 1700 x 1475', N'Nhật',
 N'Xe gia đình', N'Vios E CVT 2020',
 N'HCM', N'Đã duyệt', GETDATE(), 'ND002','DM01','HX01','DX01'),

('XE038', N'Toyota Fortuner 2021 2.8AT', 1320000000, 2021, 41000,
 N'Tự động', N'Dầu', N'Đen', N'2.8L', N'201hp',
 N'4795 x 1855 x 1835', N'Nhật',
 N'SUV cao cấp', N'Fortuner 2.8AT 2021',
 N'Hà Nội', N'Đã duyệt', GETDATE(), 'ND002','DM02','HX01','DX03'),

-- ===== HONDA =====
('XE039', N'Honda Civic 2020 RS', 725000000, 2020, 44000,
 N'Tự động', N'Xăng', N'Đỏ', N'1.5L', N'170hp',
 N'4648 x 1799 x 1416', N'Nhật',
 N'Bản thể thao', N'Civic RS 2020',
 N'HCM', N'Đã duyệt', GETDATE(), 'ND002','DM01','HX02','DX04'),

('XE040', N'Honda CR-V 2019 G', 820000000, 2019, 63000,
 N'Tự động', N'Xăng', N'Xanh', N'2.0L', N'154hp',
 N'4584 x 1855 x 1679', N'Nhật',
 N'SUV gia đình', N'CR-V G 2019',
 N'Hải Phòng', N'Đã duyệt', GETDATE(), 'ND002','DM02','HX02','DX05'),

-- ===== MERCEDES =====
('XE041', N'Mercedes C300 AMG 2020', 1680000000, 2020, 39000,
 N'Tự động', N'Xăng', N'Trắng', N'2.0L', N'258hp',
 N'4686 x 1810 x 1442', N'Đức',
 N'AMG thể thao', N'C300 AMG 2020',
 N'HCM', N'Đã duyệt', GETDATE(), 'ND002','DM05','HX04','DX08'),

('XE042', N'Mercedes GLC 200 2021', 1750000000, 2021, 32000,
 N'Tự động', N'Xăng', N'Đen', N'2.0L', N'197hp',
 N'4670 x 1900 x 1640', N'Đức',
 N'SUV sang', N'GLC 200 2021',
 N'Hà Nội', N'Đã duyệt', GETDATE(), 'ND002','DM05','HX04','DX09'),

-- ===== BMW =====
('XE043', N'BMW 530i 2020 Luxury', 1980000000, 2020, 45000,
 N'Tự động', N'Xăng', N'Xanh', N'2.0L', N'252hp',
 N'4963 x 1868 x 1479', N'Đức',
 N'Sedan hạng sang', N'BMW 530i Luxury 2020',
 N'HCM', N'Đã duyệt', GETDATE(), 'ND002','DM05','HX03','DX07'),

 -- ===== BMW =====
('XE044', N'BMW 530i 2025 Luxury', 2980000000, 2025, 55000,
 N'Tự động', N'Xăng', N'Xanh', N'2.0L', N'252hp',
 N'4963 x 1868 x 1479', N'Đức',
 N'Sedan hạng sang', N'BMW 530i Luxury 2020',
 N'HCM', N'Đã duyệt', GETDATE(), 'ND002','DM05','HX03','DX07');

/* =====================================================
   17. CẬP NHẬT DỮ LIỆU XE
   ===================================================== */
UPDATE Xe
SET MoTaChiTiet = N'🚘 Volvo XC90 Inscription B6 AWD – Sản xuất 2021, Model 2021
Volvo XC90 Inscription B6 AWD là mẫu SUV hạng sang 7 chỗ nổi tiếng với khả năng vận hành mạnh mẽ, độ an toàn hàng đầu thế giới và thiết kế sang trọng tinh tế theo phong cách Bắc Âu. Xe thuộc phiên bản Inscription cao cấp nhất, trang bị đầy đủ tiện nghi, phù hợp cho gia đình cũng như doanh nhân cần một chiếc SUV ổn định – đẳng cấp – an toàn tuyệt đối.
📌 **Thông tin tổng quan**
- Năm sản xuất: 2021  
- Model: 2021  
- Số km đã đi (ODO): 50.000 km  
- Tình trạng: Xe cá nhân, chạy giữ gìn, bảo dưỡng đầy đủ  
- Động cơ: Mild Hybrid B6 – tăng áp mạnh mẽ, tiết kiệm nhiên liệu  
- Hệ dẫn động: AWD – 4 bánh toàn thời gian  
- Màu sắc: Sang trọng, phù hợp khách hàng yêu xe châu Âu  

💰 **Giá bán: 2 tỷ 795 triệu**
⚡ **Cam kết của Carpla Phạm Văn Đồng**
- Xe **không đâm đụng**, **không ngập nước**, kiểm tra CARCHECK minh bạch.  
- Động cơ – hộp số **nguyên bản**, chưa từng đại tu.  
- Kiểm tra kỹ thuật toàn diện trước khi giao.  
- Hỗ trợ sang tên trong ngày, hỗ trợ trả góp ngân hàng lãi suất tốt.  
- **Bảo hành động cơ, hộp số lên đến 1 năm hoặc 20.000 km.**

🎯 **Tiện nghi & công nghệ nổi bật**
- Nội thất da Nappa cao cấp, thiết kế Scandinavian sang trọng.  
- Ghế chỉnh điện nhiều hướng, nhớ vị trí, thông gió – sưởi.  
- Màn hình trung tâm 9 inch, hỗ trợ Apple CarPlay & Android Auto.  
- Điều hòa tự động 4 vùng độc lập.  
- Hệ thống âm thanh cao cấp Harman Kardon sống động.  
- Hỗ trợ giữ làn đường, giữ khoảng cách, cảnh báo điểm mù.  
- Camera 360 độ sắc nét, hỗ trợ đỗ xe tự động.  
- Cốp điện, đá cốp thông minh.  

🔐 **Hệ thống an toàn tiêu chuẩn Volvo**
- City Safety – tự phanh khẩn cấp thông minh.  
- Hỗ trợ lái Pilot Assist.  
- 10 túi khí an toàn.  
- Cảnh báo va chạm, hỗ trợ chống lật, chống trượt.  
- Khung gầm chắc chắn đạt chuẩn an toàn 5 sao Châu Âu.  

🛠 **Tình trạng xe**
- Xe chạy kỹ, bảo dưỡng đúng lịch tại hãng.  
- Nội thất mới 95%, không ám mùi, không trầy xước nhiều.  
- Lốp còn ~80%, phanh – giảm xóc hoạt động êm ái.  
- Đi thử thực tế cực kỳ chắc chắn và mượt mà.

📞 **Liên hệ ngay để xem xe – lái thử trực tiếp** Xe đang trưng bày tại showroom Carpla Phạm Văn Đồng, hỗ trợ xem xe tại nhà theo yêu cầu.'
WHERE IdXe IN (
	'XE001','XE002','XE003','XE004', 'XE005', 'XE006', 'XE007','XE008', 
	'XE009','XE010', 'XE011','XE012', 'XE013', 'XE014', 'XE015', 'XE016',
    'XE017','XE018','XE019','XE020','XE021','XE022','XE023','XE024','XE025',
    'XE026','XE027','XE028','XE029','XE030','XE031','XE032','XE033','XE034',
    'XE035','XE036','XE037','XE038','XE039','XE040','XE041','XE042','XE043','XE044'
);



UPDATE Xe
SET TieuDe = N'Hyundai Tucson 2021'
WHERE IdXe = 'XE006';

/* =====================================================
   18. INSERT DỮ LIỆU CÁC BẢNG CON
   ===================================================== */
INSERT INTO XeHinhAnh (IdXe, HinhAnh) VALUES
('XE001','001.jpg'), 
('XE002','002.jpg'), 
('XE003','003.jpg'),
('XE004','004.jpg'),
('XE005','005.jpg');
GO

INSERT INTO XeHinhAnh (IdXe, HinhAnh) VALUES
('XE006','006.jpg'),
('XE007','007.jpg'),
('XE008','008.jpg'),
('XE009','009.jpg'),
('XE010','010.jpg'),
('XE011','011.jpg'),
('XE012','012.jpg'),
('XE013','013.jpg'),
('XE014','014.jpg'),
('XE015','015.jpg'),
('XE016','016.jpg'),
('XE017','017.jpg'),
('XE018','018.jpg'),
('XE019','019.jpg'),
('XE020','020.jpg'),
('XE021','021.jpg'),
('XE022','022.jpg'),
('XE023','023.jpg'),
('XE024','024.jpg'),
('XE025','025.jpg'),
('XE026','026.jpg'),
('XE027','027.jpg'),
('XE028','028.jpg'),
('XE029','029.jpg'),
('XE030','030.jpg'),
('XE031','031.jpg'),
('XE032','032.jpg'),
('XE033','033.jpg'),
('XE034','034.jpg'),
('XE035','035.jpg'),
('XE036','036.jpg'),
('XE037','037.jpg'),
('XE038','038.jpg'),
('XE039','039.jpg'),
('XE040','040.jpg'),
('XE041','041.jpg'),
('XE042','042.jpg'),
('XE043','043.jpg'),
('XE044','044.jpg');
GO


DELETE FROM DanhGia;
INSERT INTO DanhGia (IdXe, IdNguoiDung, SoSao, NoiDung) VALUES
('XE001','ND003',5,N'Xe chạy êm, tiết kiệm xăng'),
('XE002','ND003',4,N'Dáng đẹp, máy mạnh'),
('XE003','ND003',5,N'Rất sang trọng, chạy sướng');
GO

INSERT INTO YeuThich (IdNguoiDung, IdXe) VALUES
('ND003','XE001'),
('ND003','XE003');
GO

INSERT INTO LienHe (IdXe, TenNguoiMua, DienThoai, Email, NoiDung) VALUES
('XE001', N'Phạm Dũng','0901234567','khach@gmail.com',N'Tôi muốn xem xe'),
('XE003', N'Lê Minh','0912345678','minh@gmail.com',N'Xe còn không?');
GO

INSERT INTO LichSuDuyetTin (IdXe, IdAdmin, NoiDung, TrangThai) VALUES
('XE001','ND001',N'Duyệt tin hợp lệ','Đã duyệt'),
('XE002','ND001',N'Hình ảnh rõ ràng','Đã duyệt'),
('XE005','ND001',N'Thiếu thông tin hình ảnh','Từ chối');
GO

INSERT INTO LichSuDangNhap (IdNguoiDung, DiaChiIP, ThietBi) VALUES
('ND001','192.168.1.10','Chrome'),
('ND002','192.168.1.11','iPhone'),
('ND003','192.168.1.12','Windows');
GO

/* =====================================================
   19. CẬP NHẬT THÊM THÔNG TIN XE
   ===================================================== */
UPDATE Xe
SET HopSo = N'Tự động'
WHERE IdXe = 'XE001';

UPDATE Xe
SET HopSo = N'Số sàn'
WHERE IdXe = 'XE002';

UPDATE Xe
SET HopSo = N'Số sàn'
WHERE IdXe = 'XE003';

UPDATE Xe
SET HopSo = N'Tự động'
WHERE IdXe = 'XE004';

UPDATE Xe
SET HopSo = N'Số sàn'
WHERE IdXe = 'XE005';

UPDATE Xe
SET DiaDiem  = N'Hồ Chí Minh'
WHERE IdXe = 'XE005';

UPDATE Xe
SET TieuDe= N'Mercedes GLC 300 4Matic'
WHERE IdXe = 'XE005';


USE WebsiteMuaBanOtoDB;
GO

/* =====================================================
   20. THÊM DỮ LIỆU BẢNG LÁI THỬ
   ===================================================== */
INSERT INTO LaiThu (IdXe, IdNguoiDung, NgayHen, GhiChu, TrangThai, NgayTao)
VALUES 
('XE001', 'ND003', DATEADD(day, 1, GETDATE()), N'Tôi muốn lái thử buổi chiều tầm 2h', N'Chờ xác nhận', GETDATE()),

('XE002', 'ND004', DATEADD(day, 2, GETDATE()), N'Gọi trước cho tôi 30p nhé', N'Đã xác nhận', GETDATE()),

('XE004', 'ND005', DATEADD(day, -1, GETDATE()), N'Tôi bận đột xuất xin hủy', N'Đã hủy', DATEADD(day, -2, GETDATE())),

('XE005', 'ND006', DATEADD(day, -5, GETDATE()), N'Xe rất êm, đang suy nghĩ thêm', N'Đã xong', DATEADD(day, -6, GETDATE())),

-- 5. Khách ND003 quay lại hẹn lái thử Tucson
('XE006', 'ND003', DATEADD(day, 3, GETDATE()), N'Mang xe qua nhà tôi được không?', N'Chờ xác nhận', GETDATE());
GO


/* =====================================================
   21. THÊM DỮ LIỆU BẢNG ĐẶT CỌC
   ===================================================== */
INSERT INTO DatCoc (IdXe, IdNguoiDung, SoTienCoc, PhuongThucTT, GhiChu, TrangThai, NgayDat)
VALUES
('XE001', 'ND003', 5000000, N'Chuyển khoản', N'Cọc giữ xe trong 3 ngày', N'Đã cọc', GETDATE()),

('XE002', 'ND004', 10000000, N'Chuyển khoản', N'Đã chuyển khoản qua Vietcombank, check giúp em', N'Chờ thanh toán', GETDATE()),

('XE003', 'ND005', 20000000, N'Tiền mặt', N'Đã nhận đủ tiền tại showroom', N'Đã cọc', DATEADD(day, -1, GETDATE())),

('XE004', 'ND006', 15000000, N'Chuyển khoản', N'Khách đổi ý không mua nữa, đã hoàn tiền lại', N'Hoàn tiền', DATEADD(day, -3, GETDATE())),

('XE005', 'ND003', 50000000, N'Chuyển khoản', N'Giữ xe tuần sau tôi qua lấy', N'Chờ thanh toán', GETDATE());
GO
sp_help DanhGia
/* =====================================================
   22. SELECT KIỂM TRA
   ===================================================== */
select * from DanhGia
select * from DanhMucXe
select * from DongXe
select * from HangXe
select * from LichSuDangNhap
select * from LichSuDuyetTin
select * from LienHe
select * from NguoiDung
select * from Xe
select * from XeHinhAnh
select * from YeuThich


SELECT * FROM NguoiDung;


SELECT IdXe, LEN(IdXe) FROM Xe WHERE IdXe = 'XE044'
SELECT * FROM Xe WHERE IdXe = 'XE044';
