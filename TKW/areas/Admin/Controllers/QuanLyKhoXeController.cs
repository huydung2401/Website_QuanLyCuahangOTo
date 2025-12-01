using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TKW.Areas.Admin.Models;         // ViewModel của Admin
using TKW.Models;                      // Models của TKW

namespace TKW.Areas.Admin.Controllers
{
    public class QuanLyKhoXeController : BaseAdminController
    {
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // ============================================================
        // 1. DANH SÁCH XE
        // ============================================================
        public ActionResult Index()
        {
            var listXe = db.Xes
                           .OrderByDescending(x => x.NgayDang)
                           .ToList();

            return View(listXe);
        }

        // ============================================================
        // 2. FORM THÊM XE
        // ============================================================
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        // ============================================================
        // 3. XỬ LÝ THÊM XE
        // ============================================================
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(XeInputViewModel model)
        {
            if (model.ImageFile == null)
                ModelState.Remove("ImageFile");

            if (ModelState.IsValid)
            {
                try
                {
                    string newIdXe = GenerateXeId();

                    Xe xe = new Xe
                    {
                        IdXe = newIdXe,
                        TieuDe = model.TieuDe,
                        Gia = model.Gia,
                        NamSX = model.NamSX,
                        HopSo = model.HopSo,
                        NhienLieu = model.NhienLieu,
                        MoTaChiTiet = model.Mota,
                        NgayDang = DateTime.Now,
                        TrangThaiTin = "Đã duyệt",

                        IdHangXe = model.IdHangXe,
                        IdDongXe = model.IdDongXe,
                        IdDanhMuc = model.IdDanhMuc
                    };

                    // Lấy người đăng
                    var user = (NguoiDung)Session["User"];
                    xe.IdNguoiBan = user != null ? user.IdNguoiDung : "ND001";

                    // Lưu ảnh
                    if (model.ImageFile != null)
                    {
                        string imgName = SaveImage(model.ImageFile, newIdXe);

                        db.XeHinhAnhs.Add(new XeHinhAnh
                        {
                            IdXe = newIdXe,
                            HinhAnh = imgName
                        });
                    }

                    db.Xes.Add(xe);
                    db.SaveChanges();

                    TempData["ThongBao"] = "Thêm xe mới thành công!";
                    TempData["LoaiThongBao"] = "alert-success";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi: " + ex.Message);
                }
            }

            SetViewBag(model.IdHangXe, model.IdDanhMuc, model.IdDongXe);
            return View(model);
        }

        // ============================================================
        // 4. FORM SỬA XE
        // ============================================================
        public ActionResult Edit(string id)
        {
            var xe = db.Xes.Find(id);
            if (xe == null) return HttpNotFound();

            var model = new XeInputViewModel
            {
                TieuDe = xe.TieuDe,
                Gia = xe.Gia,
                NamSX = xe.NamSX ?? 2020,
                HopSo = xe.HopSo,
                NhienLieu = xe.NhienLieu,
                Mota = xe.MoTaChiTiet,
                IdHangXe = xe.IdHangXe,
                IdDongXe = xe.IdDongXe,
                IdDanhMuc = xe.IdDanhMuc
            };

            SetViewBag(xe.IdHangXe, xe.IdDanhMuc, xe.IdDongXe);

            ViewBag.CurrentId = id;
            ViewBag.CurrentImage = db.XeHinhAnhs
                                      .Where(x => x.IdXe == id)
                                      .Select(x => x.HinhAnh)
                                      .FirstOrDefault();

            return View(model);
        }

        // ============================================================
        // 5. LƯU SỬA XE
        // ============================================================
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(string id, XeInputViewModel model)
        {
            if (model.ImageFile == null)
                ModelState.Remove("ImageFile");

            if (ModelState.IsValid)
            {
                var xe = db.Xes.Find(id);
                if (xe == null) return HttpNotFound();

                try
                {
                    // Cập nhật thông tin
                    xe.TieuDe = model.TieuDe;
                    xe.Gia = model.Gia;
                    xe.NamSX = model.NamSX;
                    xe.HopSo = model.HopSo;
                    xe.NhienLieu = model.NhienLieu;
                    xe.MoTaChiTiet = model.Mota;
                    xe.IdHangXe = model.IdHangXe;
                    xe.IdDongXe = model.IdDongXe;
                    xe.IdDanhMuc = model.IdDanhMuc;
                    
                    xe.TrangThaiTin = model.TrangThaiTin;


                    // Thay ảnh nếu có upload mới
                    if (model.ImageFile != null)
                    {
                        var anhCu = db.XeHinhAnhs.FirstOrDefault(x => x.IdXe == id);
                        if (anhCu != null)
                        {
                            string oldPath = Server.MapPath("~/Content/images/" + anhCu.HinhAnh);
                            if (System.IO.File.Exists(oldPath))
                                System.IO.File.Delete(oldPath);

                            db.XeHinhAnhs.Remove(anhCu);
                        }

                        string imgName = SaveImage(model.ImageFile, id);

                        db.XeHinhAnhs.Add(new XeHinhAnh
                        {
                            IdXe = id,
                            HinhAnh = imgName
                        });
                    }

                    db.SaveChanges();
                    TempData["ThongBao"] = "Cập nhật thành công!";
                    TempData["LoaiThongBao"] = "alert-success";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi: " + ex.Message);
                }
            }

            SetViewBag(model.IdHangXe, model.IdDanhMuc, model.IdDongXe);
            return View(model);
        }

        // ============================================================
        // 6. XÓA XE
        // ============================================================
        public ActionResult Delete(string id)
        {
            try
            {
                var xe = db.Xes.Find(id);
                if (xe == null) return HttpNotFound();

                // Xóa ảnh
                var imgs = db.XeHinhAnhs.Where(x => x.IdXe == id).ToList();

                foreach (var img in imgs)
                {
                    string path = Server.MapPath("~/Content/images/" + img.HinhAnh);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    db.XeHinhAnhs.Remove(img);
                }

                // Xóa yêu thích
                var fav = db.YeuThiches.Where(y => y.IdXe == id).ToList();
                if (fav.Any())
                    db.YeuThiches.RemoveRange(fav);


                // Xóa xe
                db.Xes.Remove(xe);
                db.SaveChanges();

                TempData["ThongBao"] = "Đã xóa thành công!";
                TempData["LoaiThongBao"] = "alert-success";
            }
            catch
            {
                TempData["ThongBao"] = "Không thể xóa vì xe đã có dữ liệu liên quan!";
                TempData["LoaiThongBao"] = "alert-danger";
            }

            return RedirectToAction("Index");
        }

        // ============================================================
        // HÀM PHỤ
        // ============================================================
        private void SetViewBag(string hang = null, string danh = null, string dong = null)
        {
            ViewBag.HangXe = new SelectList(db.HangXes, "IdHangXe", "TenHang", hang);
            ViewBag.DanhMuc = new SelectList(db.DanhMucXes, "IdDanhMuc", "TenDanhMuc", danh);
            ViewBag.DongXe = new SelectList(db.DongXes, "IdDongXe", "TenDong", dong);
        }

        private string SaveImage(HttpPostedFileBase file, string idXe)
        {
            string ext = Path.GetExtension(file.FileName);
            string fileName = idXe + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;

            string path = Server.MapPath("~/Content/images/" + fileName);
            file.SaveAs(path);

            return fileName;
        }

        private string GenerateXeId()
        {
            var last = db.Xes.OrderByDescending(x => x.IdXe).FirstOrDefault();
            if (last == null) return "XE001";

            try
            {
                int num = int.Parse(last.IdXe.Substring(2));
                return "XE" + (num + 1).ToString("D3");
            }
            catch
            {
                return "XE" + DateTime.Now.Ticks.ToString().Substring(10);
            }
        }
    }
}
