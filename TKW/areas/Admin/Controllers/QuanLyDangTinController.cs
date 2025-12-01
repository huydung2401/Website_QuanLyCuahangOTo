using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TKW.Models;                       // Model của TKW

namespace TKW.Areas.Admin.Controllers
{
    public class QuanLyDangTinController : BaseAdminController
    {
        // DbContext của dự án TKW
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // =======================================
        // 1. DANH SÁCH TIN CHỜ DUYỆT
        // =======================================
        public ActionResult Index()
        {
            var listChoDuyet = db.Xes
                                  .Where(x => x.TrangThaiTin == "Chờ duyệt")
                                  .OrderBy(x => x.NgayDang)
                                  .ToList();

            return View(listChoDuyet);
        }

        // =======================================
        // 2. DUYỆT TIN
        // =======================================
        public ActionResult DuyetTin(string id)
        {
            var xe = db.Xes.Find(id);

            if (xe != null)
            {
                xe.TrangThaiTin = "Đã duyệt";
                db.SaveChanges();

                TempData["ThongBao"] = "Đã duyệt tin xe của khách: " + xe.NguoiDung.HoTen;
                TempData["LoaiThongBao"] = "alert-success";
            }

            return RedirectToAction("Index");
        }

        // =======================================
        // 3. TỪ CHỐI TIN
        // =======================================
        public ActionResult TuChoiTin(string id)
        {
            var xe = db.Xes.Find(id);

            if (xe != null)
            {
                xe.TrangThaiTin = "Từ chối";
                db.SaveChanges();

                TempData["ThongBao"] = "Đã từ chối tin đăng: " + xe.TieuDe;
                TempData["LoaiThongBao"] = "alert-warning";
            }

            return RedirectToAction("Index");
        }

        // =======================================
        // 4. XEM CHI TIẾT TIN
        // =======================================
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var xe = db.Xes.Find(id);

            if (xe == null)
                return HttpNotFound();

            return View(xe);
        }
    }
}
