using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TKW.Models;
using System.IO;

namespace TKW.Controllers
{
    public class XeController : Controller
    {
        WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // =============================
        // DANH SÁCH XE
        // =============================
        public ActionResult Index()
        {
            var xe = db.Xes
                       .OrderBy(x => x.IdXe)
                       .ToList();

            return View(xe);
        }

        // =============================
        // DANH SÁCH THEO DANH MỤC
        // =============================
        public ActionResult DanhSach(string idDanhMuc)
        {
            var xe = db.Xes.AsQueryable();

            if (!string.IsNullOrEmpty(idDanhMuc))
                xe = xe.Where(x => x.IdDanhMuc == idDanhMuc);

            return View(xe.OrderBy(x => x.IdXe).ToList());
        }

        // =============================
        // CHI TIẾT XE
        // =============================
        public ActionResult ChiTietXe(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var xe = db.Xes
                       .Include("DanhMucXe")
                       .Include("HangXe")
                       .Include("DongXe")
                       .Include("NguoiDung")
                       .FirstOrDefault(x => x.IdXe == id);

            if (xe == null) return HttpNotFound();

            var hinh = db.XeHinhAnhs.Where(h => h.IdXe == id).ToList();
            var nguoiBan = db.NguoiDungs.FirstOrDefault(n => n.IdNguoiDung == xe.IdNguoiBan);

            var danhGia = db.DanhGias
                            .Where(d => d.IdXe == id)
                            .OrderByDescending(d => d.NgayDanhGia)
                            .ToList();

            var tuongTu = db.Xes
                            .Where(x => x.IdDanhMuc == xe.IdDanhMuc && x.IdXe != xe.IdXe)
                            .Take(4)
                            .ToList();

            XeDetailViewModel model = new XeDetailViewModel
            {
                Xe = xe,
                HinhAnh = hinh,
                NguoiBan = nguoiBan,

                DanhMuc = xe.DanhMucXe,
                HangXe = xe.HangXe,
                DongXe = xe.DongXe,

                DanhSachDanhGia = danhGia,
                XeTuongTu = tuongTu
            };

            return View(model);
        }



        // =============================
        // GỬI ĐÁNH GIÁ XE
        // =============================
        [HttpPost]
        public ActionResult GuiDanhGia(string idXe, int soSao, string noiDung)
        {
            try
            {
                var user = Session["User"] as NguoiDung;
                if (user == null)
                {
                    return Json(new { success = false, message = "Bạn phải đăng nhập để đánh giá!" });
                }

                DanhGia dg = new DanhGia();
                dg.IdXe = idXe;
                dg.IdNguoiDung = user.IdNguoiDung;
                dg.SoSao = soSao;
                dg.NoiDung = noiDung ?? "";
                dg.NgayDanhGia = DateTime.Now;

                db.DanhGias.Add(dg);
                db.SaveChanges();

                return Json(new { success = true, message = "Đánh giá thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }


        // =============================
        // CRUD XE CHO ADMIN
        // =============================
        public ActionResult Them()
        {
            ViewBag.HangXe = new SelectList(db.HangXes, "IdHangXe", "TenHang");
            ViewBag.DanhMuc = new SelectList(db.DanhMucXes, "IdDanhMuc", "TenDanhMuc");
            ViewBag.DongXe = new SelectList(db.DongXes, "IdDongXe", "TenDong");
            return View();
        }

        [HttpPost]
        public ActionResult Them(Xe xe)
        {
            if (ModelState.IsValid)
            {
                xe.NgayDang = DateTime.Now;
                db.Xes.Add(xe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(xe);
        }

        public ActionResult Sua(string id)
        {
            var xe = db.Xes.FirstOrDefault(x => x.IdXe == id);
            if (xe == null) return HttpNotFound();

            ViewBag.HangXe = new SelectList(db.HangXes, "IdHangXe", "TenHang", xe.IdHangXe);
            ViewBag.DanhMuc = new SelectList(db.DanhMucXes, "IdDanhMuc", "TenDanhMuc", xe.IdDanhMuc);
            ViewBag.DongXe = new SelectList(db.DongXes, "IdDongXe", "TenDong", xe.IdDongXe);

            return View(xe);
        }

        [HttpPost]
        public ActionResult Sua(Xe xe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(xe).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(xe);
        }

        public ActionResult Xoa(string id)
        {
            var xe = db.Xes.FirstOrDefault(x => x.IdXe == id);
            if (xe == null) return HttpNotFound();
            return View(xe);
        }

        [HttpPost, ActionName("Xoa")]
        public ActionResult XoaConfirmed(string id)
        {
            var xe = db.Xes.FirstOrDefault(x => x.IdXe == id);
            if (xe != null)
            {
                db.Xes.Remove(xe);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
