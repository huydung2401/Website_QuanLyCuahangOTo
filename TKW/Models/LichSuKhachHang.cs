using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class LichSuKhachHang
    {
        public NguoiDung ThongTinUser { get; set; }
        public List<LaiThu> LichSuLaiThu { get; set; }
        public List<DatCoc> LichSuDatCoc { get; set; }
    }
}