using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TKW.Models;   // ⭐ đảm bảo namespace Models của bạn đúng
using System.Data.Entity;

namespace TKW.Controllers
{
    public class BanXeController : Controller
    {
        // ⭐ ĐÃ ĐỔI SANG DATABASE ĐÚNG
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // ==========================================
        // 1. TRANG QUẢN LÝ TIN ĐĂNG
        // ==========================================
        public ActionResult Index()
        {
            return RedirectToAction("QuanLyTinDang");
        }

        public ActionResult QuanLyTinDang()
        {
            var user = Session["User"] as NguoiDung;
            if (user == null) return RedirectToAction("DangNhap", "TaiKhoan");

            var myCars = db.Xes
                           .Include(x => x.XeHinhAnhs)
                           .Where(x => x.IdNguoiBan == user.IdNguoiDung)
                           .OrderByDescending(x => x.NgayDang)
                           .ToList();

            return View(myCars);
        }

        // ==========================================
        // 2. ĐĂNG TIN BÁN XE (GET)
        // ==========================================
        public ActionResult DangTin()
        {
            if (Session["User"] == null) return RedirectToAction("DangNhap", "TaiKhoan");

            ViewBag.HangXe = new SelectList(db.HangXes, "IdHangXe", "TenHang");
            ViewBag.DanhMuc = new SelectList(db.DanhMucXes, "IdDanhMuc", "TenDanhMuc");
            ViewBag.DongXe = new SelectList(db.DongXes.OrderBy(d => d.TenDong), "IdDongXe", "TenDong");

            return View();
        }

        // ==========================================
        // 3. ĐĂNG TIN (POST)
        // ==========================================
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DangTin(Xe model, IEnumerable<HttpPostedFileBase> files)
        {
            var user = Session["User"] as NguoiDung;
            if (user == null) return RedirectToAction("DangNhap", "TaiKhoan");

            ModelState.Remove("IdXe");
            ModelState.Remove("IdNguoiBan");

            if (ModelState.IsValid)
            {
                try
                {
                    // A. Tạo mã xe tự động
                    string newId = "XE001";
                    var last = db.Xes.OrderByDescending(x => x.IdXe).FirstOrDefault();

                    if (last != null)
                    {
                        string num = last.IdXe.Substring(2);
                        if (int.TryParse(num, out int n))
                        {
                            n++;
                            newId = "XE" + n.ToString("000");
                        }
                        else
                        {
                            newId = "XE" + DateTime.Now.Ticks.ToString().Substring(10);
                        }
                    }

                    // B. Gán thông tin
                    model.IdXe = newId;
                    model.IdNguoiBan = user.IdNguoiDung;
                    model.NgayDang = DateTime.Now;
                    model.TrangThaiTin = "Chờ duyệt";

                    db.Xes.Add(model);
                    db.SaveChanges();

                    // C. Upload ảnh
                    if (files != null)
                    {
                        int count = 1;
                        foreach (var f in files)
                        {
                            if (f != null && f.ContentLength > 0)
                            {
                                string ext = Path.GetExtension(f.FileName);
                                string fileName = $"{model.IdXe}_{count}{ext}";
                                string path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);

                                f.SaveAs(path);

                                db.XeHinhAnhs.Add(new XeHinhAnh
                                {
                                    IdXe = model.IdXe,
                                    HinhAnh = fileName
                                });
                                count++;
                            }
                        }
                        db.SaveChanges();
                    }

                    TempData["Success"] = "Đăng tin thành công! Vui lòng chờ duyệt.";
                    return RedirectToAction("QuanLyTinDang");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Lỗi: " + ex.Message;
                }
            }

            // Load lại Dropdown nếu lỗi
            ViewBag.HangXe = new SelectList(db.HangXes, "IdHangXe", "TenHang", model.IdHangXe);
            ViewBag.DanhMuc = new SelectList(db.DanhMucXes, "IdDanhMuc", "TenDanhMuc", model.IdDanhMuc);
            ViewBag.DongXe = new SelectList(db.DongXes.OrderBy(d => d.TenDong), "IdDongXe", "TenDong", model.IdDongXe);

            return View(model);
        }
    }
}
