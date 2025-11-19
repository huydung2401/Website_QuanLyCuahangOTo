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
            var bep = db.SanPhams
              .Where(s => s.IdDanhMuc == "DM04")   // chỉ lấy bếp
              .OrderByDescending(s => s.DaBan)      // sắp xếp theo đã bán
              .Take(4)                              // hiển thị 4 sản phẩm
              .ToList();


            // 🔥 Lọc những sản phẩm giảm giá mạnh (giá khuyến mãi < giá gốc)
            var giaTot = db.SanPhams
                          .Where(sp => sp.GiaKhuyenMai > 0 && sp.GiaKhuyenMai < sp.Gia)
                          .OrderByDescending(sp => (sp.Gia - sp.GiaKhuyenMai))
                          .Take(4)
                          .ToList();

            // 🔥 4 sản phẩm bán chạy nhất
            var banChay = db.SanPhams
                            .OrderByDescending(sp => sp.DaBan)
                            .Take(4)
                            .ToList();

            // 🔥 Tất cả sản phẩm
            var sanPhams = db.SanPhams
                             .OrderBy(sp => sp.IdSanPham)
                             .ToList();

            // Gửi dữ liệu ra View

            ViewBag.GiaTot = giaTot;
            ViewBag.BanChay = banChay;
            ViewBag.Bep = bep;

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


        public ActionResult CuaHang()
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
