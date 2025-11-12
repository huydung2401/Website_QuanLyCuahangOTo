using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TKW.Models;

namespace TKW.Controllers
{
    public class DonHangController : Controller
    {
        QLMohoDBEntities2 db = new QLMohoDBEntities2();

        // =============================
        // GET: DonHang/Index
        // =============================
        public ActionResult Index()
        {
            // Lấy danh sách hóa đơn (đơn hàng)
            var donHangs = db.HoaDons
                             .OrderByDescending(h => h.NgayDat)
                             .ToList();

            return View(donHangs);
        }

        // =============================
        // GET: DonHang/ChiTiet/5
        // =============================
        public ActionResult ChiTiet(int id)
        {
            // Lấy chi tiết hóa đơn theo IdHoaDon
            var hoaDon = db.HoaDons
                           .Include("NguoiDung")
                           .Include("ChiTietHoaDons.SanPham")
                           .FirstOrDefault(h => h.IdHoaDon == id);

            if (hoaDon == null)
                return HttpNotFound();

            return View(hoaDon);
        }

        // =============================
        // POST: DonHang/CapNhatTrangThai
        // =============================
        [HttpPost]
        public ActionResult CapNhatTrangThai(int id, string trangThai)
        {
            var hoaDon = db.HoaDons.FirstOrDefault(h => h.IdHoaDon == id);
            if (hoaDon == null)
            {
                TempData["Message"] = "Không tìm thấy hóa đơn!";
                return RedirectToAction("Index");
            }

            try
            {
                hoaDon.TrangThai = trangThai;
                db.SaveChanges();
                TempData["Message"] = "Cập nhật trạng thái thành công!";
            }
            catch (Exception)
            {
                TempData["Message"] = "Cập nhật thất bại!";
            }

            return RedirectToAction("Index");
        }
    }
}