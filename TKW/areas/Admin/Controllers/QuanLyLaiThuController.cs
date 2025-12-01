using System;
using System.Linq;
using System.Web.Mvc;
using TKW.Models;

namespace TKW.Areas.Admin.Controllers
{
    public class QuanLyLaiThuController : BaseAdminController
    {
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // =====================================================
        // 1. DANH SÁCH LỊCH HẸN
        // =====================================================
        public ActionResult Index()
        {
            // Ưu tiên hiển thị lịch "Chờ xác nhận" trước
            var listLich = db.LaiThus
                             .OrderBy(x => x.TrangThai != "Chờ xác nhận")
                             .ThenByDescending(x => x.NgayHen)
                             .ToList();

            return View(listLich);
        }

        // =====================================================
        // 2. XEM CHI TIẾT
        // =====================================================
        public ActionResult Details(int id)
        {
            var lich = db.LaiThus.Find(id);
            if (lich == null) return HttpNotFound();

            return View(lich);
        }

        // =====================================================
        // 3. XÁC NHẬN LỊCH
        // =====================================================
        public ActionResult XacNhanLich(int id)
        {
            var lich = db.LaiThus.Find(id);

            if (lich != null)
            {
                lich.TrangThai = "Đã xác nhận";
                db.SaveChanges();

                TempData["ThongBao"] = "Đã xác nhận lịch hẹn #" + id;
                TempData["LoaiThongBao"] = "alert-primary";
            }

            return RedirectToAction("Index");
        }

        // =====================================================
        // 4. HOÀN TẤT LỊCH
        // =====================================================
        public ActionResult HoanTatLich(int id)
        {
            var lich = db.LaiThus.Find(id);

            if (lich != null)
            {
                lich.TrangThai = "Đã xong";
                db.SaveChanges();

                TempData["ThongBao"] = "Đã hoàn tất lịch hẹn #" + id;
                TempData["LoaiThongBao"] = "alert-success";
            }

            return RedirectToAction("Index");
        }

        // =====================================================
        // 5. HỦY LỊCH
        // =====================================================
        public ActionResult HuyLich(int id)
        {
            var lich = db.LaiThus.Find(id);

            if (lich != null)
            {
                lich.TrangThai = "Đã hủy";
                db.SaveChanges();

                TempData["ThongBao"] = "Đã hủy lịch hẹn #" + id;
                TempData["LoaiThongBao"] = "alert-danger";
            }

            return RedirectToAction("Index");
        }

        // =====================================================
        // 6. XÓA LỊCH
        // =====================================================
        public ActionResult Delete(int id)
        {
            var lich = db.LaiThus.Find(id);

            if (lich != null)
            {
                db.LaiThus.Remove(lich);
                db.SaveChanges();

                TempData["ThongBao"] = "Đã xóa lịch hẹn #" + id;
                TempData["LoaiThongBao"] = "alert-warning";
            }

            return RedirectToAction("Index");
        }
    }
}
