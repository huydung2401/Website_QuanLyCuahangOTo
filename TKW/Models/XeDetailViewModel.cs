using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class XeDetailViewModel
    {
        // ===============================
        // THÔNG TIN XE
        // ===============================
        public Xe Xe { get; set; }
        public string IdXe { get; set; }
        public List<XeHinhAnh> HinhAnh { get; set; }

        // ===============================
        // THÔNG TIN NGƯỜI BÁN
        // ===============================
        public NguoiDung NguoiBan { get; set; }

        // ===============================
        // LOẠI, HÃNG, DÒNG XE
        // ===============================
        public DanhMucXe DanhMuc { get; set; }
        public HangXe HangXe { get; set; }
        public DongXe DongXe { get; set; }

        // ===============================
        // ĐÁNH GIÁ
        // ===============================
        public List<DanhGia> DanhSachDanhGia { get; set; }

        // ⭐ Tính trung bình sao
        //public double DiemTrungBinh
        //{
        //    get
        //    {
        //        if (DanhSachDanhGia == null || DanhSachDanhGia.Count == 0)
        //            return 0;

        //        return DanhSachDanhGia.Average(x => x.SoSao);
        //    }
        //}

        // ===============================
        // XE TƯƠNG TỰ
        // ===============================
        public List<Xe> XeTuongTu { get; set; }

        // ===============================
        // KIỂM TRA USER CÓ ĐƯỢC PHÉP ĐÁNH GIÁ KHÔNG
        // ===============================
        public bool CoTheDanhGia { get; set; }
    }
}
