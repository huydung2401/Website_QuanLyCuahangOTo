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
        private List<SanPham> LayGioHang()
        {
            List<SanPham> gioHang = Session["GioHang"] as List<SanPham>;
            if (gioHang == null)
            {
                gioHang = new List<SanPham>();
                Session["GioHang"] = gioHang;
            }
            return gioHang;
        }

        
        public ActionResult Index()
        {
            var gioHang = LayGioHang();
            return View(gioHang);
        }

        //// -----------------------------
        //// THÊM SẢN PHẨM VÀO GIỎ
        //// -----------------------------
        //public ActionResult ThemVaoGio(int id)
        //{
        //    var gioHang = LayGioHang();

        //    // Lấy sản phẩm từ DB
        //    var sp = db.SanPhams.FirstOrDefault(s => s.IdSanPham == id);
        //    if (sp == null)
        //    {
        //        TempData["Message"] = "Sản phẩm không tồn tại!";
        //        return RedirectToAction("Index", "SanPham");
        //    }

        //    // Nếu sản phẩm đã có thì tăng số lượng
        //    var item = gioHang.FirstOrDefault(s => s.IdSanPham == id);
        //    if (item != null)
        //    {
        //        item.SoLuongLon++;
        //    }
        //    else
        //    {
        //        // Tạo bản sao sản phẩm cho giỏ hàng
        //        var spGio = new SanPham
        //        {
        //            IdSanPham = sp.IdSanPham,
        //            TenSanPham = sp.TenSanPham,
        //            Gia = sp.Gia,
        //            SoLuongLon = 1,
        //            HinhAnh = sp.HinhAnh
        //        };
        //        gioHang.Add(spGio);
        //    }

        //    Session["GioHang"] = gioHang;
        //    TempData["Message"] = "Đã thêm sản phẩm vào giỏ!";
        //    return RedirectToAction("Index");
        //}

        //// -----------------------------
        //// XÓA SẢN PHẨM KHỎI GIỎ
        //// -----------------------------
        //public ActionResult Xoa(int id)
        //{
        //    var gioHang = LayGioHang();
        //    var item = gioHang.FirstOrDefault(s => s.IdSanPham == id);
        //    if (item != null)
        //    {
        //        gioHang.Remove(item);
        //    }
        //    Session["GioHang"] = gioHang;
        //    TempData["Message"] = "Đã xóa sản phẩm khỏi giỏ!";
        //    return RedirectToAction("Index");
        //}

        //// -----------------------------
        //// CẬP NHẬT SỐ LƯỢNG
        //// -----------------------------
        //[HttpPost]
        //public ActionResult CapNhat(int id, int soLuong)
        //{
        //    var gioHang = LayGioHang();
        //    var item = gioHang.FirstOrDefault(s => s.IdSanPham == id);
        //    if (item != null)
        //    {
        //        item.SoLuongLon = soLuong;
        //    }
        //    Session["GioHang"] = gioHang;
        //    TempData["Message"] = "Đã cập nhật số lượng!";
        //    return RedirectToAction("Index");
        //}

        //// -----------------------------
        //// THANH TOÁN - LƯU HÓA ĐƠN
        //// -----------------------------
        //public ActionResult ThanhToan()
        //{
        //    var gioHang = LayGioHang();
        //    if (gioHang.Count == 0)
        //    {
        //        TempData["Message"] = "Giỏ hàng trống!";
        //        return RedirectToAction("Index");
        //    }

        //    // Giả sử bạn có Session["NguoiDungId"] khi đăng nhập
        //    int nguoiDungId = (int)(Session["NguoiDungId"] ?? 1);

        //    // Tính tổng tiền
        //    decimal tongTien = gioHang.Sum(sp => sp.Gia * sp.SoLuongLon);

        //    // Tạo hóa đơn mới
        //    HoaDon hd = new HoaDon
        //    {
        //        NguoiDungId = nguoiDungId,
        //        NgayDat = DateTime.Now,
        //        TongTien = tongTien,
        //        TrangThai = "Đang xử lý"
        //    };
        //    db.HoaDons.Add(hd);
        //    db.SaveChanges(); // Lưu để có IdHoaDon

        //    // Lưu chi tiết hóa đơn
        //    foreach (var sp in gioHang)
        //    {
        //        ChiTietHoaDon cthd = new ChiTietHoaDon
        //        {
        //            IdHoaDon = hd.IdHoaDon,
        //            IdSanPham = sp.IdSanPham,
        //            SoLuong = sp.SoLuongLon,
        //            DonGia = sp.Gia
        //        };
        //        db.ChiTietHoaDons.Add(cthd);
        //    }

        //    db.SaveChanges();

        //    // Xóa giỏ hàng sau khi thanh toán
        //    Session["GioHang"] = null;
        //    TempData["Message"] = "Thanh toán thành công!";
        //    return RedirectToAction("Index");
        //}
    }
}