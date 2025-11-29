using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TKW.Models;


namespace TKW.Controllers
{
    public class HomeController : Controller
    {
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // =============================
        // TRANG CHỦ
        // =============================
        public ActionResult Index()
        {
            // 1) Xe mới đăng gần đây
            var xeMoi = db.Xes
                          .Include("XeHinhAnhs")
                          .OrderByDescending(x => x.NgayDang)
                          .Take(4)
                          .ToList();

            // 2) Xe giá tốt (giá < 1 tỷ)
            var giaTot = db.Xes
                           .Include("XeHinhAnhs")
                           .Where(x => x.Gia < 1000000000)
                           .OrderBy(x => x.Gia)
                           .Take(4)
                           .ToList();

            // 3) Xe theo danh mục “Sedan”
            var sedan = db.Xes
                          .Include("XeHinhAnhs")
                          .Where(x => x.IdDanhMuc == "DM01")
                          .Take(4)
                          .ToList();

            // 4) Lấy 12 xe bất kỳ hiển thị chính
            var xe = db.Xes
                       .Include("XeHinhAnhs")
                       .OrderBy(x => x.IdXe)
                       .Take(12)
                       .ToList();

            ViewBag.XeMoi = xeMoi;
            ViewBag.GiaTot = giaTot;
            ViewBag.Sedan = sedan;

            return View(xe);   // Model chính
        }


        // =============================
        // Cửa hàng
        // =============================
        public ActionResult CuaHang()
        {
            return View();
        }

        // =============================
        // Giới thiệu
        // =============================
        public ActionResult About()
        {
            ViewBag.Message = "Trang thông tin về website.";
            return View();
        }

        // =============================
        // Liên hệ
        // =============================
        public ActionResult Contact()
        {
            ViewBag.Message = "Trang liên hệ.";
            return View();
        }

        // =============================
        // Giải phóng tài nguyên
        // =============================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GioiThieu()
        {
            return View();
        }
    }
}
