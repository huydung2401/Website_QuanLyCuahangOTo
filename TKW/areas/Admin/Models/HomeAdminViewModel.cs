using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Areas.Admin.Models
{
    public class HomeAdminViewModel
    {
        // =============================
        //  THỐNG KÊ CARD TRÊN DASHBOARD
        // =============================
        public int TongXe { get; set; }
        public int TinChoDuyet { get; set; }
        public int TongNguoiDung { get; set; }
        public int TongNhanVien { get; set; }
        public int TongDanhMuc { get; set; }
        public decimal TongDoanhThu { get; set; }
        public int LienHeMoi { get; set; }

        // =============================
        // BIỂU ĐỒ LOẠI XE
        // =============================
        public List<string> LoaiXe { get; set; }
        public List<int> SoLuongTheoLoai { get; set; }

        // =============================
        // BIỂU ĐỒ HÃNG XE
        // =============================
        public List<string> HangXe { get; set; }
        public List<int> SoLuongTheoHang { get; set; }

        // =============================
        // XE NHIỀU LƯỢT YÊU THÍCH
        // =============================
        public List<string> XeYeuThich { get; set; }
        public List<int> LuotYeuThich { get; set; }
    }
}
