using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TKW.Models;
using System.Data.Entity;
namespace TKW.Controllers
{
    public class DonHangController : Controller
    {
        QLMohoDBEntities2 db = new QLMohoDBEntities2();

        // =============================
        // GET: DonHang/Index
        // =============================
        public ActionResult Index()
        {
            // Lấy danh sách hóa đơn (đơn hàng)
            var donHangs = db.HoaDons
                             .OrderByDescending(h => h.NgayDat)
                             .ToList();

            return View(donHangs);
        }

        // =============================
        // GET: DonHang/ChiTiet/5
        // =============================
        public ActionResult ChiTiet(string id)
        {
            // Lấy chi tiết hóa đơn theo IdHoaDon
            var hoaDon = db.HoaDons
                           .Include("NguoiDung")
                           .Include("ChiTietHoaDons.SanPham")
                           .FirstOrDefault(h => h.IdHoaDon == id);

            if (hoaDon == null)
                return HttpNotFound();

            return View(hoaDon);
        }

        // =============================
        // POST: DonHang/CapNhatTrangThai
        // =============================
        [HttpPost]
        public ActionResult CapNhatTrangThai(string id, string trangThai)
        {
            var hoaDon = db.HoaDons.FirstOrDefault(h => h.IdHoaDon == id);
            if (hoaDon == null)
            {
                TempData["Message"] = "Không tìm thấy hóa đơn!";
                return RedirectToAction("Index");
            }

            try
            {
                hoaDon.TrangThai = trangThai;
                db.SaveChanges();
                TempData["Message"] = "Cập nhật trạng thái thành công!";
            }
            catch (Exception)
            {
                TempData["Message"] = "Cập nhật thất bại!";
            }

            return RedirectToAction("Index");
        }
        // ==========================================================
        // PHẦN 2: THANH TOÁN & ĐẶT HÀNG (CLIENT / KHÁCH HÀNG)
        // ==========================================================

        // GET: Trang Thanh Toán (Điền thông tin)
        [HttpGet]
        public ActionResult ThanhToan(string idSanPham, int soLuong = 1)
        {
            var model = new ThanhToanViewModel();
            model.SanPhamMua = new List<ChiTietDonHangItem>();

            // A. Tự điền thông tin khách nếu đã đăng nhập
            if (Session["User"] != null)
            {
                var user = Session["User"] as NguoiDung;
                model.HoTen = user.HoTen;
                model.Email = user.Email;
                model.DienThoai = user.DienThoai;
                model.DiaChi = user.DiaChi;
            }

            // B. Xử lý trường hợp "Mua Ngay"
            if (!string.IsNullOrEmpty(idSanPham))
            {
                var sp = db.SanPhams.FirstOrDefault(s => s.IdSanPham == idSanPham);
                if (sp != null)
                {
                    model.SanPhamMua.Add(new ChiTietDonHangItem
                    {
                        IdSanPham = sp.IdSanPham,
                        TenSanPham = sp.TenSanPham,
                        HinhAnh = sp.HinhAnh,
                        Gia = sp.GiaKhuyenMai > 0 ? sp.GiaKhuyenMai.Value : sp.Gia,
                        SoLuong = soLuong
                    });
                }
            }
            // C. Xử lý trường hợp "Thanh toán từ Giỏ Hàng"
            else
            {
                var gioHang = Session["GioHang"] as List<GioHang>; 

                if (gioHang != null && gioHang.Count > 0)
                {
                    foreach (var sp in gioHang)
                    {
                        model.SanPhamMua.Add(new ChiTietDonHangItem
                        {
                            IdSanPham = sp.IdSanPham,
                            TenSanPham = sp.TenSanPham,
                            HinhAnh = sp.HinhAnh,
                            Gia = sp.GiaKhuyenMai > 0 ? sp.GiaKhuyenMai.Value : sp.Gia,
                            SoLuong = sp.SoLuong // ✔ Lấy số lượng đúng từ giỏ hàng
                        });
                    }
                }
            }

            // D. Tính tổng + điều hướng
            if (model.SanPhamMua.Count == 0)
                return RedirectToAction("Index", "Home");

            model.TongTien = model.SanPhamMua.Sum(x => x.ThanhTien);

            return View(model);  // ✔ Điều hướng đúng tới View ThanhToan
        }


        // POST: Xử lý Lưu Đơn Hàng vào Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DatHang(ThanhToanViewModel form)
        {
            try
            {
                // A. Tạo Hóa Đơn
                string maHD = "HD" + DateTime.Now.ToString("ddHHmmss"); // Mã tự sinh theo thời gian
                string idNguoiDung = "KHACHLE";

                if (Session["User"] != null)
                {
                    idNguoiDung = ((NguoiDung)Session["User"]).IdNguoiDung;
                }

                HoaDon hd = new HoaDon();
                hd.IdHoaDon = maHD;
                hd.IdNguoiDung = idNguoiDung;
                hd.NgayDat = DateTime.Now;
                hd.TongTien = form.TongTien;
                hd.TrangThai = "Chờ xử lý";

                // LƯU THÔNG TIN GIAO HÀNG
                hd.HoTenNguoiNhan = form.HoTen;
                hd.DienThoaiNguoiNhan = form.DienThoai;
                hd.DiaChiGiaoHang = form.DiaChi;
                hd.GhiChu = form.GhiChu;
                hd.PhuongThucThanhToan = form.PhuongThucThanhToan;

                db.HoaDons.Add(hd);
                db.SaveChanges(); // Lưu hóa đơn trước để có ID

                // B. Lưu Chi Tiết Hóa Đơn & CẬP NHẬT SỐ LƯỢNG ĐÃ BÁN
                if (form.SanPhamMua != null)
                {
                    foreach (var item in form.SanPhamMua)
                    {
                        ChiTietHoaDon cthd = new ChiTietHoaDon();
                        cthd.IdChiTietHoaDon = "CT" + Guid.NewGuid().ToString().Substring(0, 8); // Mã chi tiết ngẫu nhiên
                        cthd.IdHoaDon = maHD;
                        cthd.IdSanPham = item.IdSanPham;
                        cthd.SoLuong = item.SoLuong;
                        cthd.DonGia = item.Gia;

                        db.ChiTietHoaDons.Add(cthd);

                        // --- CẬP NHẬT SỐ LƯỢNG ĐÃ BÁN VÀ TỒN KHO ---
                        var sp = db.SanPhams.FirstOrDefault(s => s.IdSanPham == item.IdSanPham);
                        if (sp != null)
                        {
                            // Cộng dồn số lượng đã bán
                            sp.DaBan = (sp.DaBan ?? 0) + item.SoLuong;

                            // Trừ số lượng tồn kho
                            sp.SoLuongTon = (sp.SoLuongTon ?? 0) - item.SoLuong;
                            if (sp.SoLuongTon < 0) sp.SoLuongTon = 0;
                        }
                    }
                    db.SaveChanges(); // Lưu chi tiết và cập nhật sản phẩm
                }

                // C. Xóa giỏ hàng sau khi đặt thành công
                Session["GioHang"] = null;

                return RedirectToAction("DatHangThanhCong", new { id = maHD });
            }
            catch (Exception ex)
            {
                // Ghi log lỗi và hiển thị thông báo thân thiện
                System.Diagnostics.Debug.WriteLine("Lỗi đặt hàng: " + ex.Message);
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi đặt hàng. Vui lòng thử lại!";
                return RedirectToAction("ThanhToan");
            }
        }

        // GET: Trang thông báo thành công
        public ActionResult DatHangThanhCong(string id)
        {
            var hoaDon = db.HoaDons
                           .Include("ChiTietHoaDons.SanPham") // Load chi tiết để hiển thị nếu cần
                           .FirstOrDefault(h => h.IdHoaDon == id);

            if (hoaDon == null) return RedirectToAction("Index", "Home");

            return View(hoaDon);
        }
    }
}