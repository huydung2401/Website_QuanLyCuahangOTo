using System.Linq;
using System.Web.Mvc;
using TKW.Models;   // Dùng model của dự án TKW

namespace TKW.Areas.Admin.Controllers
{
    public class QuanLyTuVanController : BaseAdminController
    {
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // ===========================================
        // 1. DANH SÁCH YÊU CẦU TƯ VẤN
        // ===========================================
        public ActionResult Index()
        {
            var list = db.YeuCauTuVans
                         .OrderBy(x => x.TrangThai != "Chờ tư vấn")
                         .ThenByDescending(x => x.NgayGui)
                         .ToList();

            return View(list);
        }

        // ===========================================
        // 2. FORM TRẢ LỜI (GET)
        // ===========================================
        public ActionResult TraLoi(int id)
        {
            var item = db.YeuCauTuVans.Find(id);
            return View(item);
        }

        // ===========================================
        // 3. XỬ LÝ TRẢ LỜI (POST)
        // ===========================================
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TraLoi(int id, string noidung)
        {
            var item = db.YeuCauTuVans.Find(id);

            if (item != null)
            {
                item.PhanHoiCuaAdmin = noidung;
                item.TrangThai = "Đã tư vấn";
                db.SaveChanges();

                TempData["ThongBao"] = "Đã gửi phản hồi tư vấn cho khách hàng thành công!";
                TempData["LoaiThongBao"] = "alert-success";
            }

            return RedirectToAction("Index");
        }
    }
}
