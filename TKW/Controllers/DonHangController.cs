using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TKW.Models;

namespace TKW.Controllers
{
    public class DonHangController : Controller
    {
        WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // ============================================
        // 📌 1) Danh sách yêu cầu liên hệ (Admin)
        // ============================================
        public ActionResult Index()
        {
            var ds = db.LienHes
                        .OrderByDescending(x => x.NgayGui)
                        .ToList();
            return View(ds);
        }

        // ============================================
        // 📌 2) Chi tiết liên hệ (Admin xem)
        // ============================================
        public ActionResult ChiTiet(int id)
        {
            var lh = db.LienHes.FirstOrDefault(x => x.IdLienHe == id);
            if (lh == null) return HttpNotFound();

            return View(lh);
        }

        // ============================================
        // 📌 3) Người dùng gửi yêu cầu xem xe
        // ============================================
        [HttpPost]
        public ActionResult GuiLienHe(string idXe, string hoTen, string email, string dienThoai, string noiDung)
        {
            var xe = db.Xes.Find(idXe);
            if (xe == null)
                return Json(new { success = false, message = "Xe không tồn tại!" });

            var lh = new LienHe();
            lh.IdXe = idXe;
            lh.TenNguoiMua = hoTen;
            lh.Email = email;
            lh.DienThoai = dienThoai;
            lh.NoiDung = noiDung;
            lh.NgayGui = DateTime.Now;

            db.LienHes.Add(lh);
            db.SaveChanges();

            return Json(new { success = true, message = "Gửi liên hệ thành công!" });
        }

        // ============================================
        // 📌 4) Lưu tin nhắn từ trang Chi tiết xe
        // ============================================
        [HttpPost]
        public ActionResult LienHeXe(string idXe, string message)
        {
            var xe = db.Xes.Find(idXe);
            if (xe == null)
                return Json(new { success = false });

            var lh = new LienHe
            {
                IdXe = idXe,
                TenNguoiMua = (Session["UserName"] ?? "Khách").ToString(),
                Email = "",
                DienThoai = "",
                NoiDung = message,
                NgayGui = DateTime.Now
            };

            db.LienHes.Add(lh);
            db.SaveChanges();

            return Json(new { success = true, message = "Gửi tin nhắn thành công!" });
        }

        // ============================================
        // 📌 5) Xóa liên hệ (Admin)
        // ============================================
        public ActionResult Xoa(int id)
        {
            var lh = db.LienHes.FirstOrDefault(x => x.IdLienHe == id);
            if (lh != null)
            {
                db.LienHes.Remove(lh);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
