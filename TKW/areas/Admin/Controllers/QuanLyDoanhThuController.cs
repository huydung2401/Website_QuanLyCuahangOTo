using System;
using System.Linq;
using System.Web.Mvc;
using TKW.Models;                       // DbContext + Models
using TKW.Areas.Admin.Models;          // DoanhThuVM

namespace TKW.Areas.Admin.Controllers
{
    public class QuanLyDoanhThuController : BaseAdminController
    {
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // =============================================================
        // 1. TRANG CHÍNH - BÁO CÁO DOANH THU
        // =============================================================
        public ActionResult Index()
        {
            // 1. Lọc ra các đơn đã thanh toán thành công 
            // Lưu ý: Chỉ tính tiền những đơn đã thu, không tính đơn chờ/hủy
            var donThanhCong = db.DatCocs.Where(d => d.TrangThai == "Đã cọc");

            // 2. Tính con số tổng quát 
            decimal tongDoanhThu = donThanhCong.Sum(d => (decimal?)d.SoTienCoc) ?? 0;
            int tongDonHang = donThanhCong.Count();

            ViewBag.TongDoanhThu = tongDoanhThu;
            ViewBag.TongDonHang = tongDonHang;

            // 3. Thống kê chi tiết theo từng Tháng/Năm
            var baoCaoThang = donThanhCong
                .ToList() // Tải về bộ nhớ để xử lý ngày tháng dễ hơn
                .GroupBy(d => new { d.NgayDat.Value.Month, d.NgayDat.Value.Year })
                .Select(g => new DoanhThuVM
                {
                    Thang = g.Key.Month,
                    Nam = g.Key.Year,
                    SoDonHang = g.Count(),
                    TongTien = g.Sum(x => (decimal?)x.SoTienCoc) ?? 0

                })
                .OrderByDescending(x => x.Nam)
                .ThenByDescending(x => x.Thang)
                .ToList();

            return View(baoCaoThang);
        }
    }
}
