using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TKW.Models;

namespace TKW.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        QLMohoDBEntities2 db = new QLMohoDBEntities2();

        // GET: /SanPham/
        public ActionResult Index()
        {
            var sanPhams = db.SanPhams.ToList();
            return View(sanPhams);
        }

        // GET: /SanPham/DanhSach
        public ActionResult DanhSach(string idDanhMuc)
        {
            var sanPhams = db.SanPhams.AsQueryable();

            if (!string.IsNullOrEmpty(idDanhMuc))
            {
                sanPhams = sanPhams.Where(s => s.IdDanhMuc == idDanhMuc);
            }

            return View(sanPhams.ToList());
        }

        // GET: /SanPham/ChiTiet/5

        public ActionResult ChiTietSanPham(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Lấy sản phẩm theo VARCHAR ID
            var sp = db.SanPhams.FirstOrDefault(s => s.IdSanPham == id);
            if (sp == null)
                return HttpNotFound();

            // Tạo model chi tiết sản phẩm
            var model = new ChiTietSanPham
            {
                SanPham = sp,
                DanhMuc = sp.DanhMuc,
                SanPhamLienQuan = db.SanPhams
                        .Where(x => x.IdDanhMuc == sp.IdDanhMuc && x.IdSanPham != sp.IdSanPham)
                        .Take(4)
                        .ToList()
            };

            return View(model);
        }


        // GET: /SanPham/Them
        public ActionResult Them()
        {
            ViewBag.DanhMucId = new SelectList(db.DanhMucs, "IdDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: /SanPham/Them
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Them(SanPham sp)
        {
            if (ModelState.IsValid)
            {
                sp.NgayThem = DateTime.Now;
                db.SanPhams.Add(sp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DanhMucId = new SelectList(db.DanhMucs, "IdDanhMuc", "TenDanhMuc", sp.IdDanhMuc);
            return View(sp);
        }

        // GET: /SanPham/Sua/5
        public ActionResult Sua(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sp = db.SanPhams.FirstOrDefault(s => s.IdSanPham == id);
            if (sp == null)
                return HttpNotFound();

            ViewBag.DanhMucId = new SelectList(db.DanhMucs, "IdDanhMuc", "TenDanhMuc", sp.IdDanhMuc);
            return View(sp);
        }


        // POST: /SanPham/Sua
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sua(SanPham sp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DanhMucId = new SelectList(db.DanhMucs, "IdDanhMuc", "TenDanhMuc", sp.IdDanhMuc);
            return View(sp);
        }

        // GET: /SanPham/Xoa/5
        public ActionResult Xoa(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sp = db.SanPhams.FirstOrDefault(s => s.IdSanPham == id);
            if (sp == null)
                return HttpNotFound();

            return View(sp);
        }


        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaConfirmed(string id)
        {
            var sp = db.SanPhams.FirstOrDefault(s => s.IdSanPham == id);
            if (sp != null)
            {
                db.SanPhams.Remove(sp);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // Giải phóng tài nguyên
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}