using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TKW.Areas.Admin.Models
{
    public class XeInputViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề xe")]
        public string TieuDe { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Gia { get; set; }

        [Required]
        public int NamSX { get; set; }
        public string TrangThaiTin { get; set; }

        public string HopSo { get; set; }     // Số sàn / Tự động
        public string NhienLieu { get; set; } // Xăng / Dầu / Điện
        public int SoCho { get; set; }        // Ví dụ: 4, 5, 7 chỗ

        // ------- Dropdown -------

        [Required(ErrorMessage = "Vui lòng chọn hãng xe")]
        public string IdHangXe { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn dòng xe")]
        public string IdDongXe { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại xe")]
        public string IdDanhMuc { get; set; }

        // Mô tả chi tiết
        public string Mota { get; set; }

        // ---------- ẢNH (quan trọng) ----------
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
