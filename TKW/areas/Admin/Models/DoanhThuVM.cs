using System;

namespace TKW.Areas.Admin.Models
{
    public class DoanhThuVM
    {
        public int Thang { get; set; }
        public int Nam { get; set; }

        public int SoDonHang { get; set; }
        public decimal TongTien { get; set; }

        public string TenThang
        {
            get
            {
                return $"Tháng {Thang}/{Nam}";
            }
        }
    }
}
