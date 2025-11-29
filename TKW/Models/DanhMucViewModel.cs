using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class DanhMucXeViewModel
    {
        // ===== 1. Thông tin danh mục =====
        public DanhMucXe DanhMuc { get; set; }
        public List<DanhMucXe> AllDanhMuc { get; set; }

        // ===== 2. Danh sách xe trong danh mục =====
        public List<Xe> XeList { get; set; }

        // ===== 3. Bộ lọc nâng cao =====
        public string SortBy { get; set; }        // new, priceAsc, priceDesc
        public string HangXe { get; set; }        // HX01, HX02...
        public string DongXe { get; set; }        // DX01, DX02...
        public string NhienLieu { get; set; }     // Xăng, Dầu, Điện
        public string HopSo { get; set; }         // Tự động / Số sàn
        public string XuatXu { get; set; }        // Nhật, Đức, VN
        public string PriceFilter { get; set; }   // <500tr, 500–1 tỷ, >1 tỷ
        public int? NamSX { get; set; }           // Lọc theo năm

        public DanhMucXeViewModel()
        {
            AllDanhMuc = new List<DanhMucXe>();
            XeList = new List<Xe>();
        }
    }
}