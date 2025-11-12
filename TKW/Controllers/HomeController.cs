using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TKW.Models; 

namespace TKW.Controllers
{
    public class HomeController : Controller
    {
        // Khởi tạo DbContext
        private QLMohoDBEntities2 db = new QLMohoDBEntities2();

        // Trang chủ: hiển thị danh sách sản phẩm
        public ActionResult Index()
        {
            // Lấy 8 sản phẩm mới nhất (nếu có trường NgayThem)
            var sanPhams = db.SanPhams
                             .OrderByDescending(sp => sp.NgayThem)
                             .Take(8)
                             .ToList();

            return View(sanPhams);
        }
        public ActionResult BrandStory()
        {
            return View();
        }
        public ActionResult ThietKe_ThiCong()
        {
            return View();
        }

        // Trang giới thiệu
        public ActionResult About()
        {
            ViewBag.Message = "Trang thông tin về cửa hàng.";
            return View();
        }

        // Trang liên hệ
        public ActionResult Contact()
        {
            ViewBag.Message = "Trang liên hệ cửa hàng.";
            return View();
        }

        // Giải phóng tài nguyên
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
