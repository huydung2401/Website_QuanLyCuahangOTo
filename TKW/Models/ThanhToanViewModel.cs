using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class ThanhToanViewModel
    {
        // 1. Thông tin khách hàng (Mapping với các cột mới trong bảng HoaDon)
        public string HoTen { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string GhiChu { get; set; }

        // Quan trọng: Để nhận giá trị từ Radio Button (COD/Banking)
        public string PhuongThucThanhToan { get; set; }

        // 2. Tổng tiền đơn hàng
        public decimal TongTien { get; set; }

        // 3. Danh sách sản phẩm mua (để hiển thị lại trang xác nhận hoặc lưu DB)
        public List<ChiTietDonHangItem> SanPhamMua { get; set; }

        public ThanhToanViewModel()
        {
            SanPhamMua = new List<ChiTietDonHangItem>();
        }
    }

    // Class phụ để chứa thông tin từng món hàng trong ViewModel
    public class ChiTietDonHangItem
    {
        public string IdSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string HinhAnh { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }

        // Tính thành tiền của từng món (Gia * SoLuong)
        public decimal ThanhTien => Gia * SoLuong;
    }
}