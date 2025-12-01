using System;
using System.Linq;
using System.Web.Mvc;
using TKW.Models;                   // Model của TKW
using TKW.Areas.Admin.Controllers; // BaseAdminController

namespace TKW.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        // Database context của TKW
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            // ================================
            // 1. THỐNG KÊ XE
            // ================================
            ViewBag.TongSoXe = db.Xes.Count();
            ViewBag.XeMoiChoDuyet = db.Xes.Where(x => x.TrangThaiTin == "Chờ duyệt").Count();

            // ================================
            // 2. THỐNG KÊ DOANH THU CỌC
            // ================================
            decimal tongDoanhThu = db.DatCocs
                                     .Where(x => x.TrangThai == "Đã cọc")
                                     .Sum(x => (decimal?)x.SoTienCoc) ?? 0;

            ViewBag.TongDoanhThu = tongDoanhThu;

            // ================================
            // 3. LỊCH LÁI THỬ MỚI
            // ================================
            ViewBag.LichLaiThuMoi = db.LaiThus
                                      .Where(x => x.TrangThai == "Chờ xác nhận")
                                      .Count();

            // ================================
            // 4. THỐNG KÊ THÀNH VIÊN
            // ================================
            ViewBag.TongThanhVien = db.NguoiDungs
                                      .Where(u => u.VaiTro == "User")
                                      .Count();

            return View();
        }
    }
}
