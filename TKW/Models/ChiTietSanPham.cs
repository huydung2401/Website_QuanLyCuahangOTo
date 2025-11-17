using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class ChiTietSanPham
    {
        public SanPham SanPham { get; set; }
        public DanhMuc DanhMuc { get; set; }
        public List<SanPham> SanPhamLienQuan { get; set; }

        public ChiTietSanPham()
        {
            SanPhamLienQuan = new List<SanPham>();
        }
    }
}