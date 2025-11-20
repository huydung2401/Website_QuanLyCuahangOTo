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
    public class SanPhamController : Controller
    {
        // GET: SanPham
        QLMohoDBEntities2 db = new QLMohoDBEntities2();

        // GET: /SanPham/
        public ActionResult Index()
        {
            var sanPhams = db.SanPhams
                 .OrderBy(sp => sp.IdSanPham)   
                 .ToList();

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
            return View(sanPhams.OrderBy(s => s.IdSanPham).ToList());
        }

        // GET: /SanPham/ChiTiet/5 (ĐÃ ĐỒNG BỘ LOGIC ĐÁNH GIÁ + BIẾN THỂ + ẢNH)
        public ActionResult ChiTietSanPham(string id)
        {
            if (string.IsNullOrEmpty(id)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sp = db.SanPhams.FirstOrDefault(s => s.IdSanPham == id);
            if (sp == null) return HttpNotFound();

            // 1. Lấy Biến thể (Logic cũ cho Bếp/Giường)
            List<BienTheSanPham> bienThes = new List<BienTheSanPham>();
            if (sp.IdDanhMuc == "DM04" || sp.IdDanhMuc == "DM05")
            {
                bienThes = db.BienTheSanPhams
                             .Where(bt => bt.IdSanPham == sp.IdSanPham)
                             .OrderBy(bt => bt.Gia)
                             .ToList();
            }

            // 2. Lấy Gallery ảnh (Logic cũ)
            var listAnh = db.SanPhamHinhAnhs.Where(a => a.IdSanPham == id).ToList();

            // 3. Lấy Danh sách Đánh giá (Logic Mới)
            var listDanhGia = db.DanhGias
                                .Where(d => d.IdSanPham == id)
                                .OrderByDescending(d => d.NgayDanhGia)
                                .ToList();

            // 4. Gộp vào Model
            var model = new ChiTietSanPham
            {
                SanPham = sp,
                DanhMuc = sp.DanhMuc,
                BienThes = bienThes,
                HinhAnhChiTiet = listAnh,
                DanhSachDanhGia = listDanhGia, // Đổ dữ liệu đánh giá vào đây

                SanPhamLienQuan = db.SanPhams
                    .Where(x => x.IdDanhMuc == sp.IdDanhMuc && x.IdSanPham != sp.IdSanPham)
                    .Take(4)
                    .ToList()
            };

            return View(model);
        }

        // POST: Xử lý Gửi Đánh Giá (Logic Mới - Upload ảnh & Lưu DB)
        [HttpPost]
        public ActionResult GuiDanhGia(string idSanPham, int soSao, string noiDung, string tenNguoiDung)
        {
            try
            {
                string tenFileAnh = null;

                // 1. Xử lý upload ảnh (Giữ nguyên code cũ)
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        string extension = System.IO.Path.GetExtension(file.FileName);
                        tenFileAnh = fileName + "_" + DateTime.Now.Ticks + extension;
                        string folderPath = Server.MapPath("~/Images/Reviews/");
                        if (!System.IO.Directory.Exists(folderPath))
                        {
                            System.IO.Directory.CreateDirectory(folderPath);
                        }
                        string path = System.IO.Path.Combine(folderPath, tenFileAnh);
                        file.SaveAs(path);
                    }
                }

                // 2. Lưu đánh giá mới vào bảng DanhGia
                var user = Session["User"] as NguoiDung;

                DanhGia dg = new DanhGia();
                dg.IdSanPham = idSanPham;
                dg.IdNguoiDung = user?.IdNguoiDung;
                dg.TenNguoiDung = !string.IsNullOrEmpty(tenNguoiDung) ? tenNguoiDung : (user?.HoTen ?? "Khách ẩn danh");
                dg.SoSao = soSao;
                dg.NoiDung = noiDung ?? "";
                dg.HinhAnh = tenFileAnh;
                dg.NgayDanhGia = DateTime.Now;

                db.DanhGias.Add(dg);
                db.SaveChanges(); // Lưu xong đánh giá

                // 3. [QUAN TRỌNG] TÍNH LẠI SAO TRUNG BÌNH & ĐỒNG BỘ VÀO BẢNG SẢN PHẨM
                var sp = db.SanPhams.FirstOrDefault(s => s.IdSanPham == idSanPham);
                if (sp != null)
                {
                    // Lấy tất cả đánh giá của sản phẩm này
                    var listReviews = db.DanhGias.Where(x => x.IdSanPham == idSanPham).ToList();

                    if (listReviews.Count > 0)
                    {
                        // Tính trung bình cộng
                        double diemTrungBinh = listReviews.Average(x => x.SoSao);
                        // Làm tròn (Ví dụ 4.5 -> 5) và lưu vào cột DanhGia của bảng SanPham
                        sp.DanhGia = (int)Math.Round(diemTrungBinh);
                    }
                    else
                    {
                        sp.DanhGia = soSao; // Nếu là đánh giá đầu tiên
                    }

                    db.SaveChanges(); // Cập nhật lại bảng Sản Phẩm
                }

                return Json(new { success = true, message = "Đánh giá thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        // ==========================================================
        // PHẦN 2: QUẢN LÝ SẢN PHẨM (ADMIN - CRUD - CODE CŨ)
        // ==========================================================

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
            if (string.IsNullOrEmpty(id)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sp = db.SanPhams.FirstOrDefault(s => s.IdSanPham == id);
            if (sp == null) return HttpNotFound();

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
            if (string.IsNullOrEmpty(id)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sp = db.SanPhams.FirstOrDefault(s => s.IdSanPham == id);
            if (sp == null) return HttpNotFound();

            return View(sp);
        }

        // POST: /SanPham/Xoa (Xác nhận)
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

        // GET: Specifications (Partial View cũ)
        public ActionResult Specifications(string ID)
        {
            var sp = db.SanPhams.FirstOrDefault(s => s.IdSanPham == ID);
            if (sp == null) return HttpNotFound();
            return PartialView("_Specifications", sp);
        }

        // Giải phóng tài nguyên
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}