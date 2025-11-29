using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class GioHang
    {
        public string IdXe { get; set; }       // Mã xe
        public string TenXe { get; set; }      // Tên xe
        public string HinhAnh { get; set; }    // Ảnh đại diện
        public int SoLuong { get; set; }       // Mỗi lần thêm vào, tăng số lượng
        public decimal Gia { get; set; }       // Giá bán

        // Thành tiền = số lượng * giá
        public decimal ThanhTien
        {
            get { return SoLuong * Gia; }
        }
    }
}