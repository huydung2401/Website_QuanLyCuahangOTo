using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TKW.Models;

namespace TKW.Controllers
{
    public class TaiKhoanController : Controller
    {
        // GET: TaiKhoan
        QLMohoDBEntities2 db = new QLMohoDBEntities2();

        // ========================
        // GET: Đăng nhập
        // ========================
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(string Email, string MatKhau)
        {
            var user = db.NguoiDungs
                         .FirstOrDefault(x => x.Email == Email && x.MatKhau == MatKhau);

            if (user == null)
            {
                TempData["Error"] = "Sai email hoặc mật khẩu!";
                return RedirectToAction("DangNhap");
            }

            // Lưu session
            Session["User"] = user;
            Session["UserName"] = user.HoTen;
            Session["UserId"] = user.IdNguoiDung;

            return RedirectToAction("Index", "Home");
        }

        // ========================
        // GET: Đăng ký
        // ========================
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(NguoiDung model)
        {
            // Check email tồn tại
            if (db.NguoiDungs.Any(u => u.Email == model.Email))
            {
                TempData["Error"] = "Email này đã được sử dụng!";
                return RedirectToAction("DangKy");
            }

            // Tạo mã ND tự động
            model.IdNguoiDung = "ND" + (db.NguoiDungs.Count() + 1).ToString("00");
            model.LaAdmin = false;


            db.NguoiDungs.Add(model);
            db.SaveChanges();

            TempData["Success"] = "Đăng ký thành công! Hãy đăng nhập.";

            return RedirectToAction("DangNhap");
        }

        // ========================
        // Đăng xuất
        // ========================
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}