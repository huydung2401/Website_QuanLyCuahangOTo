using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using TKW.Models;   // ⭐ MODEL MỚI

namespace TKW.Controllers
{
    public class HomeController : Controller
    {
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // ==========================================
        // TRANG CHỦ + BỘ LỌC TÌM KIẾM
        // ==========================================
        public ActionResult Index(
            string tuKhoa,
            List<string> hangXe,
            List<string> diaDiem,
            List<string> mucGia,
            int? namMin, int? namMax,
            List<string> soKm,
            List<string> kieuDang,
            List<string> nhienLieu,
            List<string> hopSo,
            List<string> mauSac,
            List<string> dangBoi
        )
        {
            // ====================================
            // 1. TRUY VẤN XE
            // ====================================
            var query = db.Xes
                          .Include("XeHinhAnhs")
                          .Include("HangXe")
                          .Include("DongXe")
                          .Include("DanhMucXe")
                          .Include("NguoiDung")
                          .Where(x => x.TrangThaiTin == "Đã duyệt")
                          .AsQueryable();

            // 1.1 Từ khoá
            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query = query.Where(x =>
                    x.TieuDe.Contains(tuKhoa) ||
                    x.HangXe.TenHang.Contains(tuKhoa) ||
                    x.DongXe.TenDong.Contains(tuKhoa)
                );
            }

            // 1.2 Lọc Hãng xe
            if (hangXe != null && hangXe.Any())
            {
                query = query.Where(x => hangXe.Contains(x.IdHangXe));
            }

            // 1.3 Địa điểm
            if (diaDiem != null && diaDiem.Any())
            {
                query = query.Where(x => diaDiem.Contains(x.DiaDiem));
            }

            // 1.4 Giá
            if (mucGia != null && mucGia.Any())
            {
                decimal minPrice = decimal.MaxValue;
                decimal maxPrice = decimal.MinValue;

                foreach (var item in mucGia)
                {
                    if (item == "0-300") { minPrice = Math.Min(minPrice, 0); maxPrice = Math.Max(maxPrice, 300_000_000); }
                    if (item == "300-500") { minPrice = Math.Min(minPrice, 300_000_000); maxPrice = Math.Max(maxPrice, 500_000_000); }
                    if (item == "500-800") { minPrice = Math.Min(minPrice, 500_000_000); maxPrice = Math.Max(maxPrice, 800_000_000); }
                    if (item == "800-1200") { minPrice = Math.Min(minPrice, 800_000_000); maxPrice = Math.Max(maxPrice, 1_200_000_000); }
                    if (item == "1200-max") { minPrice = Math.Min(minPrice, 1_200_000_000); maxPrice = 99_999_999_999; }
                }

                if (minPrice != decimal.MaxValue && maxPrice != decimal.MinValue)
                {
                    query = query.Where(x => x.Gia >= minPrice && x.Gia <= maxPrice);
                }
            }

            // 1.5 Năm sản xuất
            if (namMin.HasValue) query = query.Where(x => x.NamSX >= namMin.Value);
            if (namMax.HasValue) query = query.Where(x => x.NamSX <= namMax.Value);

            // 1.6 Số KM
            if (soKm != null && soKm.Any())
            {
                int minK = int.MaxValue;
                int maxK = int.MinValue;

                foreach (var item in soKm)
                {
                    if (item == "0-5000") { minK = Math.Min(minK, 0); maxK = Math.Max(maxK, 5000); }
                    if (item == "5000-20000") { minK = Math.Min(minK, 5000); maxK = Math.Max(maxK, 20000); }
                    if (item == "20000-50000") { minK = Math.Min(minK, 20000); maxK = Math.Max(maxK, 50000); }
                    if (item == "50000-100000") { minK = Math.Min(minK, 50000); maxK = Math.Max(maxK, 100000); }
                    if (item == "100000-max") { minK = Math.Min(minK, 100000); maxK = int.MaxValue; }
                }

                if (minK != int.MaxValue && maxK != int.MinValue)
                {
                    query = query.Where(x => x.SoKM >= minK && x.SoKM <= maxK);
                }
            }

            // 1.7 Kiểu dáng
            if (kieuDang != null && kieuDang.Any())
            {
                query = query.Where(x => kieuDang.Contains(x.DanhMucXe.TenDanhMuc));
            }

            // 1.8 Nhiên liệu
            if (nhienLieu != null && nhienLieu.Any())
                query = query.Where(x => nhienLieu.Contains(x.NhienLieu));

            // 1.9 Hộp số
            if (hopSo != null && hopSo.Any())
                query = query.Where(x => hopSo.Contains(x.HopSo));

            // 1.10 Màu sắc
            if (mauSac != null && mauSac.Any())
            {
                var dbColors = new List<string>();

                if (mauSac.Contains("Trang")) dbColors.Add("Trắng");
                if (mauSac.Contains("Den")) dbColors.Add("Đen");
                if (mauSac.Contains("Do")) dbColors.Add("Đỏ");
                if (mauSac.Contains("Bac")) dbColors.Add("Bạc");
                if (mauSac.Contains("Xanh")) dbColors.Add("Xanh");

                if (dbColors.Any())
                    query = query.Where(x => dbColors.Contains(x.MauSac));
            }

            // 1.11 Đăng bởi
            if (dangBoi != null && dangBoi.Any())
            {
                var roles = new List<string>();
                if (dangBoi.Contains("CaNhan")) roles.Add("User");
                if (dangBoi.Contains("Salon")) roles.Add("Seller");

                if (roles.Any())
                    query = query.Where(x => roles.Contains(x.NguoiDung.VaiTro));
            }

            // ====================================
            // 2. XE MỚI – XE GIÁ TỐT – SEDAN
            // ====================================
            ViewBag.XeMoi = db.Xes.Include("XeHinhAnhs")
                                  .Where(x => x.TrangThaiTin == "Đã duyệt")
                                  .OrderByDescending(x => x.NgayDang)
                                  .Take(6)
                                  .ToList();

            ViewBag.GiaTot = db.Xes.Include("XeHinhAnhs")
                                   .Where(x => x.Gia < 1_000_000_000)
                                   .OrderBy(x => x.Gia)
                                   .Take(6)
                                   .ToList();

           ViewBag.Sedan = db.Xes.Include("XeHinhAnhs")
                      .Where(x => x.IdDanhMuc == "DM01")
                      .OrderBy(x => x.IdXe)   // Sắp từ Xe001 → Xe999
                      .Take(16)
                      .ToList();

            // ====================================
            // 3. GIÁ TRỊ MẶC ĐỊNH CỦA SLIDER
            // ====================================
            ViewBag.CurrentNamMin = namMin ?? 2000;
            ViewBag.CurrentNamMax = namMax ?? 2025;
            ViewBag.CurrentTuKhoa = tuKhoa;

            // Danh sách Hãng xe cho filter sidebar
            ViewBag.ListHangXe = db.HangXes
                                   .OrderBy(h => h.TenHang)
                                   .ToList();

            // ====================================
            // 4. TRẢ DỮ LIỆU VỀ VIEW
            // ====================================
            return View(query.OrderByDescending(x => x.NgayDang).ToList());
        }

        // ======================
        // CHUYỂN SANG TRANG CHI TIẾT XE
        // ======================
        public ActionResult ChiTietXe(string id)
        {
            return RedirectToAction("ChiTietXe", "Xe", new { id = id });
        }

        public ActionResult About() => View();
        public ActionResult Contact() => View();
        public ActionResult GioiThieu() => View();

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }

        // ======================================
        // MENU MUA XE (PARTIAL) – dùng trong _Layout
        // ======================================
        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var model = new MenuView
            {
                HangXePhoBien = db.HangXes
                                  .OrderBy(x => x.TenHang)
                                  .Take(10)
                                  .ToList(),

                DongXePhoBien = db.DongXes
                                  .OrderBy(x => x.TenDong)
                                  .Take(10)
                                  .ToList()
            };

            return PartialView("_MainMenu", model);
        }
    }
}
