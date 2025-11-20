using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class ChiTietSanPham
    {
        // 1. Các thuộc tính cơ bản
        public SanPham SanPham { get; set; }
        public DanhMuc DanhMuc { get; set; }
        public List<SanPham> SanPhamLienQuan { get; set; }

        // 2. Thuộc tính của Code Đang Chạy (Gallery ảnh + Biến thể)
        public List<SanPhamHinhAnh> HinhAnhChiTiet { get; set; }
        public virtual List<BienTheSanPham> BienThes { get; set; }

        // 3. Thuộc tính của Code Mới (Đánh giá)
        public List<DanhGia> DanhSachDanhGia { get; set; }

        // Constructor: Khởi tạo tất cả các list để tránh lỗi Null
        public ChiTietSanPham()
        {
            SanPhamLienQuan = new List<SanPham>();
            HinhAnhChiTiet = new List<SanPhamHinhAnh>();
            BienThes = new List<BienTheSanPham>();
            DanhSachDanhGia = new List<DanhGia>();
        }
    }
}