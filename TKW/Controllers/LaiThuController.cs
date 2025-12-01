using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;                // Dùng Include
using TKW.Models;                        // EDMX entities

namespace TKW.Controllers
{
    public class LaiThuController : Controller
    {
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // ================================
        // 1. LỊCH SỬ ĐĂNG KÝ LÁI THỬ
        // ================================
        public ActionResult LichSuDangKy()
        {
            // Kiểm tra đăng nhập
            var user = Session["User"] as NguoiDung;
            if (user == null)
            {
                // Chưa đăng nhập → đưa tới trang đăng nhập
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            // Lấy danh sách lịch lái thử của user
            var dsLaiThu = db.LaiThus
                             .Include(l => l.Xe)
                             .Where(l => l.IdNguoiDung == user.IdNguoiDung)
                             .OrderByDescending(l => l.NgayTao)
                             .ToList();

            return View(dsLaiThu);
        }

        // Trang mặc định
        public ActionResult Index()
        {
            return View();
        }
    }
}
