using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using TKW.Models;

namespace TKW.Controllers
{
    public class TaiKhoanController : Controller
    {
        WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

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
                         .FirstOrDefault(x => x.Email == Email && x.MatKhau == MatKhau && x.TrangThai == true);

            if (user == null)
            {
                TempData["Error"] = "Sai email hoặc mật khẩu!";
                return RedirectToAction("DangNhap");
            }

            Session["User"] = user;
            Session["UserName"] = user.HoTen;
            Session["UserId"] = user.IdNguoiDung;
            Session["Role"] = user.VaiTro;

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
            if (string.IsNullOrWhiteSpace(model.HoTen) ||
                string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.MatKhau))
            {
                TempData["Error"] = "Họ tên, Email và Mật khẩu là bắt buộc!";
                return RedirectToAction("DangKy");
            }

            if (db.NguoiDungs.Any(u => u.Email == model.Email))
            {
                TempData["Error"] = "Email này đã được sử dụng!";
                return RedirectToAction("DangKy");
            }

            // Sinh ID dạng ND001, ND002
            string newId = "ND001";
            var last = db.NguoiDungs.OrderByDescending(x => x.IdNguoiDung).FirstOrDefault();
            if (last != null)
            {
                int num = int.Parse(last.IdNguoiDung.Substring(2)) + 1;
                newId = "ND" + num.ToString("000");
            }

            model.IdNguoiDung = newId;

            // Gán giá trị mặc định theo database mới
            model.VaiTro = "User";
            model.NgayTao = DateTime.Now;
            model.TrangThai = true;

            try
            {
                db.NguoiDungs.Add(model);
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                return RedirectToAction("DangKy");
            }

            TempData["Success"] = "Đăng ký thành công! Hãy đăng nhập.";
            return RedirectToAction("DangNhap");
        }

        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
