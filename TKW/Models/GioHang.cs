using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class GioHang
    {
        public string IdSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }
        public decimal? GiaKhuyenMai { get; set; }

        public decimal ThanhTien
        {
            get { return (GiaKhuyenMai ?? Gia) * SoLuong; }
        }
    }
}