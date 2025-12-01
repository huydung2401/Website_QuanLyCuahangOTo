using System;
using System.Linq;
using System.Web.Mvc;
using TKW.Models;  // Model của dự án TKW

namespace TKW.Areas.Admin.Controllers
{
    public class QuanLyNhanVienController : BaseAdminController
    {
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // ============================================================
        // 1. DANH SÁCH NHÂN VIÊN (Chỉ hiện Admin & Seller)
        // ============================================================
        public ActionResult Index()
        {
            var staff = db.NguoiDungs
                          .Where(u => u.VaiTro == "Admin" || u.VaiTro == "Seller")
                          .OrderByDescending(u => u.VaiTro)
                          .ToList();

            return View(staff);
        }

        // ============================================================
        // 2. TẠO NHÂN VIÊN (GET)
        // ============================================================
        public ActionResult Create()
        {
            return View();
        }

        // ============================================================
        // 3. CREATE (POST)
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NguoiDung model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra email trùng
                if (db.NguoiDungs.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng!");
                    return View(model);
                }

                model.IdNguoiDung = GenerateUserId();
                model.NgayTao = DateTime.Now;
                model.TrangThai = true; // Hoạt động

                db.NguoiDungs.Add(model);
                db.SaveChanges();

                TempData["ThongBao"] = "Đã thêm nhân viên mới: " + model.HoTen;
                TempData["LoaiThongBao"] = "alert-success";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // ============================================================
        // 4. EDIT (GET)
        // ============================================================
        public ActionResult Edit(string id)
        {
            var user = db.NguoiDungs.Find(id);
            if (user == null) return HttpNotFound();

            return View(user);
        }

        // ============================================================
        // 5. EDIT (POST)
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NguoiDung model)
        {
            if (ModelState.IsValid)
            {
                var user = db.NguoiDungs.Find(model.IdNguoiDung);
                if (user == null) return HttpNotFound();

                user.HoTen = model.HoTen;
                user.DienThoai = model.DienThoai;
                user.DiaChi = model.DiaChi;
                user.VaiTro = model.VaiTro;
                user.TrangThai = model.TrangThai;

                // Nếu đổi mật khẩu
                if (!string.IsNullOrEmpty(model.MatKhau))
                {
                    user.MatKhau = model.MatKhau;
                }

                db.SaveChanges();

                TempData["ThongBao"] = "Cập nhật nhân viên thành công!";
                TempData["LoaiThongBao"] = "alert-success";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // ============================================================
        // 6. XÓA NHÂN VIÊN
        // ============================================================
        public ActionResult Delete(string id)
        {
            var currentUser = (NguoiDung)Session["User"]; // bạn dùng session "User"

            // Không cho xoá chính mình
            if (currentUser != null && currentUser.IdNguoiDung == id)
            {
                TempData["ThongBao"] = "Bạn không thể tự xóa tài khoản của chính mình!";
                TempData["LoaiThongBao"] = "alert-danger";
                return RedirectToAction("Index");
            }

            var user = db.NguoiDungs.Find(id);
            if (user != null)
            {
                // Kiểm tra nhân viên có bài đăng xe không
                if (db.Xes.Any(x => x.IdNguoiBan == id))
                {
                    TempData["ThongBao"] = "Nhân viên này đang có bài đăng xe, không thể xóa!";
                    TempData["LoaiThongBao"] = "alert-warning";
                    return RedirectToAction("Index");
                }

                db.NguoiDungs.Remove(user);
                db.SaveChanges();

                TempData["ThongBao"] = "Đã xóa nhân viên: " + user.HoTen;
                TempData["LoaiThongBao"] = "alert-success";
            }

            return RedirectToAction("Index");
        }

        // ============================================================
        // HÀM SINH MÃ NDxxx
        // ============================================================
        private string GenerateUserId()
        {
            var lastUser = db.NguoiDungs
                             .OrderByDescending(u => u.IdNguoiDung)
                             .FirstOrDefault();

            if (lastUser == null) return "ND001";

            try
            {
                int num = int.Parse(lastUser.IdNguoiDung.Substring(2));
                return "ND" + (num + 1).ToString("D3");
            }
            catch
            {
                return "ND" + DateTime.Now.Ticks.ToString().Substring(10);
            }
        }
    }
}
