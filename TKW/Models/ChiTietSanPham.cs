using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class ChiTietXe
    {
        // 1) Thông tin cơ bản
        public Xe Xe { get; set; }
        public DanhMucXe DanhMuc { get; set; }
        public HangXe HangXe { get; set; }
        public DongXe DongXe { get; set; }

        // 2) Danh sách xe liên quan
        public List<Xe> XeLienQuan { get; set; }

        // 3) Hình ảnh gallery
        public List<XeHinhAnh> HinhAnhChiTiet { get; set; }

        // 4) Danh sách đánh giá xe
        public List<DanhGia> DanhSachDanhGia { get; set; }

        // Constructor
        public ChiTietXe()
        {
            XeLienQuan = new List<Xe>();
            HinhAnhChiTiet = new List<XeHinhAnh>();
            DanhSachDanhGia = new List<DanhGia>();
        }
    }
}