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
        WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // ============================
        // 📌 DASHBOARD ADMIN
        // ============================
        public ActionResult Index()
        {
            ViewBag.TongXe = db.Xes.Count();
            ViewBag.TongNguoiDung = db.NguoiDungs.Count();
            ViewBag.TongDanhGia = db.DanhGias.Count();
            ViewBag.TongYeuThich = db.YeuThiches.Count();

            // Đếm số tin CHỜ DUYỆT
            ViewBag.TinChoDuyet = db.Xes.Count(x => x.TrangThaiTin == "Chờ duyệt");

            return View();
        }

        // ============================
        // 📌 QUẢN LÝ XE
        // ============================
        public ActionResult Xe()
        {
            var ds = db.Xes.OrderByDescending(x => x.NgayDang).ToList();
            return View(ds);
        }

        // ============================
        // 📌 QUẢN LÝ DANH MỤC XE
        // ============================
        public ActionResult DanhMucXe()
        {
            var ds = db.DanhMucXes.ToList();
            return View(ds);
        }

        // ============================
        // 📌 QUẢN LÝ HÃNG XE
        // ============================
        public ActionResult HangXe()
        {
            var ds = db.HangXes.ToList();
            return View(ds);
        }

        // ============================
        // 📌 QUẢN LÝ DÒNG XE
        // ============================
        public ActionResult DongXe()
        {
            var ds = db.DongXes.ToList();
            return View(ds);
        }

        // ============================
        // 📌 QUẢN LÝ NGƯỜI DÙNG
        // ============================
        public ActionResult TaiKhoan()
        {
            var ds = db.NguoiDungs.OrderByDescending(x => x.NgayTao).ToList();
            return View(ds);
        }

        // ============================
        // 📌 QUẢN LÝ ĐÁNH GIÁ XE
        // ============================
        public ActionResult DanhGia()
        {
            var ds = db.DanhGias
                        .OrderByDescending(x => x.NgayDanhGia)
                        .ToList();
            return View(ds);
        }

        // ============================
        // 📌 QUẢN LÝ YÊU THÍCH
        // ============================
        public ActionResult YeuThich()
        {
            var ds = db.YeuThiches.ToList();
            return View(ds);
        }

        // ============================
        // 📌 QUẢN LÝ LIÊN HỆ (khách liên hệ người bán)
        // ============================
        public ActionResult LienHe()
        {
            var ds = db.LienHes
                        .OrderByDescending(x => x.NgayGui)
                        .ToList();
            return View(ds);
        }

        // ============================
        // 📌 QUẢN LÝ DUYỆT TIN ĐĂNG XE
        // ============================
        public ActionResult DuyetTin()
        {
            var ds = db.Xes
                        .Where(x => x.TrangThaiTin == "Chờ duyệt")
                        .OrderByDescending(x => x.NgayDang)
                        .ToList();

            return View(ds);
        }
    }
}
