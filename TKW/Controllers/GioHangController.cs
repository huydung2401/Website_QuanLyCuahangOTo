using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TKW.Models;

namespace TKW.Controllers
{
    public class GioHangController : Controller
    {
        WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // Lấy giỏ hàng trong Session
        private List<GioHang> LayGioHang()
        {
            var gio = Session["GioHang"] as List<GioHang>;
            if (gio == null)
            {
                gio = new List<GioHang>();
                Session["GioHang"] = gio;
            }
            return gio;
        }

        // ============================
        // THÊM VÀO GIỎ
        // ============================
        [HttpPost]
        public ActionResult ThemGioHang(string id)
        {
            var xe = db.Xes.Find(id);
            if (xe == null)
                return Json(new { success = false });

            // Ảnh đầu tiên hoặc ảnh mặc định
            var hinh = db.XeHinhAnhs
                         .Where(h => h.IdXe == id)
                         .Select(h => h.HinhAnh)
                         .FirstOrDefault() ?? "no-image.jpg";

            var gio = LayGioHang();
            var item = gio.FirstOrDefault(x => x.IdXe == id);

            if (item == null)
            {
                gio.Add(new GioHang
                {
                    IdXe = xe.IdXe,
                    TenXe = xe.TieuDe,
                    HinhAnh = hinh,
                    SoLuong = 1,
                    Gia = xe.Gia
                });
            }
            else
            {
                item.SoLuong++;
            }

            return Json(new { success = true });
        }

        // ============================
        // CẬP NHẬT SỐ LƯỢNG
        // ============================
        [HttpPost]
        public ActionResult CapNhatSoLuong(string id, string type)
        {
            var gio = LayGioHang();
            var item = gio.FirstOrDefault(x => x.IdXe == id);

            if (item == null)
                return Json(new { success = false });

            if (type == "+")
                item.SoLuong++;
            else if (type == "-" && item.SoLuong > 1)
                item.SoLuong--;

            return Json(new { success = true, qty = item.SoLuong });
        }

        // ============================
        // XÓA KHỎI GIỎ
        // ============================
        public ActionResult Xoa(string id)
        {
            var gio = LayGioHang();
            var item = gio.FirstOrDefault(x => x.IdXe == id);

            if (item != null)
                gio.Remove(item);

            int newCount = gio.Sum(x => x.SoLuong);

            return Json(new { success = true, count = newCount }, JsonRequestBehavior.AllowGet);
        }

        // ============================
        // LẤY SỐ LƯỢNG GIỎ HÀNG
        // ============================
        public ActionResult SoLuong()
        {
            var gio = LayGioHang();
            int count = gio.Sum(x => x.SoLuong);

            return Json(new { count }, JsonRequestBehavior.AllowGet);
        }

        // ============================
        // TRANG GIỎ HÀNG
        // ============================
        public ActionResult Index()
        {
            return View(LayGioHang());
        }

        // ============================
        // POPUP GIỎ HÀNG
        // ============================
        public ActionResult Popup()
        {
            return PartialView("_PopupGioHang", LayGioHang());
        }
    }
}
