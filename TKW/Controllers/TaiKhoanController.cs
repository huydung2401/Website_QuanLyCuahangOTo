using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
            // VALIDATION TỐI THIỂU
            if (string.IsNullOrWhiteSpace(model.HoTen) ||
                string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.MatKhau))
            {
                TempData["Error"] = "Họ tên, Email và Mật khẩu là bắt buộc!";
                return RedirectToAction("DangKy");
            }

            // CHECK EMAIL TRÙNG
            if (db.NguoiDungs.Any(u => u.Email == model.Email))
            {
                TempData["Error"] = "Email này đã được sử dụng!";
                return RedirectToAction("DangKy");
            }

            // TẠO MÃ ID KHÔNG BAO GIỜ TRÙNG
            model.IdNguoiDung = "ND" + DateTime.Now.Ticks.ToString().Substring(10);

            model.LaAdmin = false;

            try
            {
                db.NguoiDungs.Add(model);
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var e in ex.EntityValidationErrors)
                {
                    foreach (var v in e.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            "Lỗi: " + v.PropertyName + " - " + v.ErrorMessage
                        );
                    }
                }

                TempData["Error"] = "Dữ liệu không hợp lệ! Kiểm tra lại thông tin.";
                return RedirectToAction("DangKy");
            }

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