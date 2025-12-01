using System;
using System.Linq;
using System.Web.Mvc;
using TKW.Models;   // Model TKW

namespace TKW.Areas.Admin.Controllers
{
    public class QuanLyDatCocController : BaseAdminController
    {
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // =============================================================
        // 1. DANH SÁCH ĐƠN CỌC
        // =============================================================
        public ActionResult Index()
        {
            var listDatCoc = db.DatCocs
                               .OrderByDescending(x => x.NgayDat)
                               .ToList();

            return View(listDatCoc);
        }

        // =============================================================
        // 2. XÁC NHẬN THANH TOÁN (ĐÃ THU TIỀN)
        // =============================================================
        public ActionResult XacNhanThanhToan(int id)
        {
            var don = db.DatCocs.Find(id);

            if (don != null)
            {
                don.TrangThai = "Đã cọc";
                db.SaveChanges();

                TempData["ThongBao"] = "Xác nhận thu tiền thành công cho đơn: " + id;
                TempData["LoaiThongBao"] = "alert-success";
            }

            return RedirectToAction("Index");
        }

        // =============================================================
        // 3. HỦY ĐƠN ĐẶT CỌC
        // =============================================================
        public ActionResult HuyDon(int id)
        {
            var don = db.DatCocs.Find(id);

            if (don != null)
            {
                don.TrangThai = "Đã hủy";
                db.SaveChanges();

                TempData["ThongBao"] = "Đã hủy đơn đặt cọc số: " + id;
                TempData["LoaiThongBao"] = "alert-warning";
            }

            return RedirectToAction("Index");
        }

        // =============================================================
        // 4. IN HÓA ĐƠN / HỢP ĐỒNG ĐẶT CỌC
        // =============================================================
        public ActionResult InHoaDon(int id)
        {
            var don = db.DatCocs.Find(id);

            if (don == null)
                return HttpNotFound();

            return View(don);
        }

        // =============================================================
        // 5. XEM CHI TIẾT ĐƠN CỌC
        // =============================================================
        public ActionResult Details(int id)
        {
            var don = db.DatCocs.Find(id);

            if (don == null)
                return HttpNotFound();

            return View(don);
        }
    }
}
