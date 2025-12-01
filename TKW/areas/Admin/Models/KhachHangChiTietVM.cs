using System.Collections.Generic;
using TKW.Models;

namespace TKW.Areas.Admin.Models
{
    public class KhachHangChiTietVM
    {
        public NguoiDung NguoiDung { get; set; }

        public List<Xe> TinDaDang { get; set; }

        public List<DatCoc> LichSuDatCoc { get; set; }

        public List<LaiThu> LichSuLaiThu { get; set; }

        public List<DanhGia> LichSuDanhGia { get; set; }
    }
}
