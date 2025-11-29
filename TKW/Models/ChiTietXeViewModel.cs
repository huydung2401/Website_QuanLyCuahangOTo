using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class ChiTietXeViewModel
    {
        public Xe Xe { get; set; }
        public DanhMucXe DanhMuc { get; set; }
        public HangXe HangXe { get; set; }
        public DongXe DongXe { get; set; }

        public List<XeHinhAnh> HinhAnh { get; set; }
        public List<DanhGia> DanhSachDanhGia { get; set; }

        public List<Xe> XeLienQuan { get; set; }
    }
}