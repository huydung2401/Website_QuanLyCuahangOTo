using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TKW.Models;

namespace TKW.Controllers
{
    public class QuanTriController : Controller
    {
        // GET: QuanTri
        QLMohoDBEntities2 db = new QLMohoDBEntities2();

        // 🔸 Trang Dashboard (Admin Home)
        public ActionResult Index()
        {
            ViewBag.TongSanPham = db.SanPhams.Count();
            ViewBag.TongNguoiDung = db.NguoiDungs.Count();
            ViewBag.TongHoaDon = db.HoaDons.Count();
            ViewBag.TongDoanhThu = db.HoaDons.Sum(h => (decimal?)h.TongTien) ?? 0;

            return View();
        }

        // 🔸 Quản lý Sản phẩm
        public ActionResult SanPham()
        {
            var sanPhams = db.SanPhams.ToList();
            return View(sanPhams);
        }

        // 🔸 Quản lý Danh mục (Loại sản phẩm)
        public ActionResult LoaiSanPham()
        {
            var danhMucs = db.DanhMucs.ToList();
            return View(danhMucs);
        }

        // 🔸 Quản lý Người dùng
        public ActionResult TaiKhoan()
        {
            var nguoiDungs = db.NguoiDungs.ToList();
            return View(nguoiDungs);
        }

        // 🔸 Quản lý Đơn hàng
        public ActionResult DonHang()
        {
            var hoaDons = db.HoaDons
                            .OrderByDescending(h => h.NgayDat)
                            .ToList();
            return View(hoaDons);
        }
    }
}