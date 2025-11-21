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
        // GET: GioHang
        //private List<SanPham> LayGioHang()
        //{
        //    List<SanPham> gioHang = Session["GioHang"] as List<SanPham>;
        //    if (gioHang == null)
        //    {
        //        gioHang = new List<SanPham>();
        //        Session["GioHang"] = gioHang;
        //    }
        //    return gioHang;
        //}

        QLMohoDBEntities2 db = new QLMohoDBEntities2();

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

        [HttpPost]
        public ActionResult ThemGioHang(string id)
        {
            var sp = db.SanPhams.Find(id);
            if (sp == null) return Json(new { success = false });

            var gio = LayGioHang();
            var item = gio.FirstOrDefault(x => x.IdSanPham == id);

            if (item == null)
            {
                gio.Add(new GioHang
                {
                    IdSanPham = sp.IdSanPham,
                    TenSanPham = sp.TenSanPham,
                    HinhAnh = sp.HinhAnh,
                    SoLuong = 1,
                    Gia = sp.Gia,
                    GiaKhuyenMai = sp.GiaKhuyenMai
                });
            }
            else item.SoLuong++;

            return Json(new { success = true });
        }


        // Cập nhật số lượng
        [HttpPost]
        public ActionResult CapNhatSoLuong(string id, string type)
        {
            var gio = LayGioHang();
            var item = gio.FirstOrDefault(x => x.IdSanPham == id);

            if (item == null)
                return Json(new { success = false });

            if (type == "+") item.SoLuong++;
            if (type == "-" && item.SoLuong > 1) item.SoLuong--;

            return Json(new { success = true });
        }

        // Xóa
        public ActionResult Xoa(string id)
        {
            var gio = LayGioHang();
            var item = gio.FirstOrDefault(x => x.IdSanPham == id);
            if (item != null) gio.Remove(item);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        // Tổng số lượng (badge)
        public ActionResult SoLuong()
        {
            var gio = LayGioHang();
            int count = gio.Sum(x => x.SoLuong);
            return Json(new { count }, JsonRequestBehavior.AllowGet);
        }

        // Trang giỏ hàng
        public ActionResult Index()
        {
            var cart = Session["GioHang"] as List<GioHang>;

            if (cart == null)
                cart = new List<GioHang>();

            return View(cart);
        }

        // Popup
        public ActionResult Popup()
        {
            return PartialView("_PopupGioHang", LayGioHang());
        }



    }
}