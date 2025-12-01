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
                         .FirstOrDefault(x => x.Email == Email
                                          && x.MatKhau == MatKhau
                                          && x.TrangThai == true);

            if (user == null)
            {
                TempData["Error"] = "Sai email hoặc mật khẩu, hoặc tài khoản bị khóa!";
                return RedirectToAction("DangNhap");
            }

            // Lưu Session
            Session["User"] = user;
            Session["UserName"] = user.HoTen;
            Session["Role"] = user.VaiTro;

            // Chuẩn hóa vai trò để tránh lỗi so sánh
            string role = user.VaiTro.Trim().ToLower();

            // Admin + Seller vào trang Admin
            if (role == "admin" || role == "seller")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            // User bình thường vào trang Home
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

        public ActionResult HoSoCuaToi()
        {
            var userSession = Session["User"] as NguoiDung;
            if (userSession == null) return RedirectToAction("DangNhap");

            // Lấy lại dữ liệu mới nhất từ DB đề phòng session cũ
            var user = db.NguoiDungs.Find(userSession.IdNguoiDung);
            return View(user);
        }

        [HttpPost]
        public ActionResult CapNhatHoSo(NguoiDung model)
        {
            var userSession = Session["User"] as NguoiDung;
            if (userSession == null) return RedirectToAction("DangNhap");

            var user = db.NguoiDungs.Find(userSession.IdNguoiDung);
            if (user != null)
            {
                user.HoTen = model.HoTen;
                user.DienThoai = model.DienThoai;
                user.DiaChi = model.DiaChi;
                // Không cho sửa Email và MatKhau ở đây cho an toàn

                db.SaveChanges();
                Session["User"] = user; // Cập nhật lại session
                TempData["Success"] = "Cập nhật thông tin thành công!";
            }
            return RedirectToAction("HoSoCuaToi");
        }

        // ==========================================
        // 2. TRANG ĐƠN HÀNG CỦA TÔI (Lái thử + Cọc)
        // ==========================================
        public ActionResult DonHangCuaToi()
        {
            var userSession = Session["User"] as NguoiDung;
            if (userSession == null) return RedirectToAction("DangNhap");

            LichSuKhachHang model = new LichSuKhachHang();

            // Lấy thông tin user
            model.ThongTinUser = userSession;

            // Lấy danh sách lái thử của user đó
            model.LichSuLaiThu = db.LaiThus.Include("Xe")
                                           .Where(l => l.IdNguoiDung == userSession.IdNguoiDung)
                                           .OrderByDescending(l => l.NgayTao)
                                           .ToList();

            // Lấy danh sách đặt cọc của user đó
            model.LichSuDatCoc = db.DatCocs.Include("Xe")
                                           .Where(d => d.IdNguoiDung == userSession.IdNguoiDung)
                                           .OrderByDescending(d => d.NgayDat)
                                           .ToList();

            return View(model);
        }
    }
}
