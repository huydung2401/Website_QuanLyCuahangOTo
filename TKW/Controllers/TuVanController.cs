using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TKW.Models;   // EDMX của dự án TKW

namespace TKW.Controllers
{
    public class TuVanController : Controller
    {
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // Hàm lấy user đang đăng nhập
        private NguoiDung GetCurrentUser()
        {
            return Session["User"] as NguoiDung;
        }

        // ==========================================
        // 1. DANH SÁCH PHIẾU TƯ VẤN (Lịch sử)
        // ==========================================
        public ActionResult Index()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                TempData["ThongBaoLoi"] = "Bạn cần đăng nhập để xem lịch sử yêu cầu tư vấn.";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var listPhieu = db.YeuCauTuVans
                              .Where(x => x.SoDienThoai == user.DienThoai)
                              .OrderByDescending(x => x.NgayGui)
                              .ToList();

            return View(listPhieu);
        }

        // ==========================================
        // 2. FORM TẠO PHIẾU (GET)
        // ==========================================
        public ActionResult Create()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                TempData["ThongBaoLoi"] = "Bạn cần đăng nhập để gửi phiếu tư vấn.";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var model = new YeuCauTuVan
            {
                HoTen = user.HoTen,
                SoDienThoai = user.DienThoai
            };

            return View(model);
        }

        // ==========================================
        // 3. SUBMIT PHIẾU (POST)
        // ==========================================
        [HttpPost]
        public ActionResult Create(YeuCauTuVan model)
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("DangNhap", "TaiKhoan");

            if (ModelState.IsValid)
            {
                model.NgayGui = DateTime.Now;
                model.TrangThai = "Chờ tư vấn";
                model.PhanHoiCuaAdmin = null;

                // đảm bảo không ai sửa được thông tin
                model.HoTen = user.HoTen;
                model.SoDienThoai = user.DienThoai;

                db.YeuCauTuVans.Add(model);
                db.SaveChanges();

                TempData["ThongBaoThanhCong"] =
                    "Gửi phiếu tư vấn thành công! Chúng tôi sẽ liên hệ trong thời gian sớm nhất.";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // ==========================================
        // 4. CHI TIẾT PHIẾU TƯ VẤN
        // ==========================================
        public ActionResult Details(int id)
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("DangNhap", "TaiKhoan");

            var phieu = db.YeuCauTuVans.Find(id);
            if (phieu == null)
                return HttpNotFound();

            // bảo mật: chỉ xem phiếu thuộc về mình
            if (phieu.SoDienThoai != user.DienThoai)
                return new HttpUnauthorizedResult("Bạn không có quyền xem phiếu này.");

            return View(phieu);
        }
    }
}
