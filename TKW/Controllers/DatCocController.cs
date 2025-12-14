using System;
using System.Linq;
using System.Web.Mvc;
using TKW.Models;      // ⭐ Namespace Models của dự án bạn
using System.Data.Entity;

namespace TKW.Controllers
{
    public class DatCocController : Controller
    {
        // ⭐ KẾT NỐI DATABASE MỚI
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // ===============================
        // 1. HIỂN THỊ FORM ĐẶT CỌC
        // ===============================
        public ActionResult TaoYeuCau(string idXe)
        {
            // Kiểm tra đăng nhập
            if (Session["User"] == null)
            {
                Session["ReturnUrl"] = "/DatCoc/TaoYeuCau?idXe=" + idXe;
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            if (string.IsNullOrEmpty(idXe))
                return RedirectToAction("Index", "Home");

            // Load thông tin xe
            var xe = db.Xes
                       .Include(x => x.XeHinhAnhs)
                       .FirstOrDefault(x => x.IdXe == idXe);

            if (xe == null)
                return HttpNotFound();

            return View(xe);
        }

        // ===============================
        // 2. XỬ LÝ LƯU ĐƠN CỌC
        // ===============================
        [HttpPost]
        public ActionResult XacNhanDatCoc(string idXe, decimal soTien, string phuongThuc, string ghiChu)
        {
            var user = Session["User"] as NguoiDung;

            if (user == null)
                return RedirectToAction("DangNhap", "TaiKhoan");

            try
            {
                // Tạo đơn đặt cọc mới
                DatCoc dc = new DatCoc
                {
                    IdXe = idXe,
                    IdNguoiDung = user.IdNguoiDung,
                    SoTienCoc = soTien,
                    PhuongThucTT = phuongThuc,   // ChuyenKhoan / TienMat
                    GhiChu = ghiChu,
                    TrangThai = "Chờ thanh toán",
                    NgayDat = DateTime.Now
                };

                db.DatCocs.Add(dc);
                db.SaveChanges();

                TempData["Success"] = "Gửi yêu cầu đặt cọc thành công!";
                return RedirectToAction("DonHangCuaToi", "TaiKhoan");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
                return RedirectToAction("TaoYeuCau", new { idXe = idXe });
            }
        }

        // 3. XEM CHI TIẾT
        //public ActionResult ChiTietDatCoc(int id)
        //{
        //    if (Session["User"] == null) return RedirectToAction("DangNhap", "TaiKhoan");

        //    var datCoc = db.DatCocs.Include("Xe").Include("Xe.XeHinhAnhs")
        //                           .FirstOrDefault(d => d.IdDatCoc == id);

        //    if (datCoc == null) return HttpNotFound();

        //    return View(datCoc);
        //}

        public ActionResult ChiTietDatCoc(int id)
        {
            if (Session["User"] == null)
                return RedirectToAction("DangNhap", "TaiKhoan");

            var datCoc = db.DatCocs
                .Include(d => d.Xe)
                .Include(d => d.Xe.XeHinhAnhs)
                .Include(d => d.NguoiDung)   // ⭐ BẮT BUỘC
                .FirstOrDefault(d => d.IdDatCoc == id);

            if (datCoc == null)
                return HttpNotFound();

            return View(datCoc);
        }


        // 4. HỦY CỌC
        // KHÁCH HÀNG GỬI YÊU CẦU HỦY
        [HttpPost]
        public ActionResult HuyDatCoc(int idDatCoc, string lyDoHuy)
        {
            var user = Session["User"] as TKW.Models.NguoiDung;
            if (user == null) return RedirectToAction("DangNhap", "TaiKhoan");

            var datCoc = db.DatCocs.Find(idDatCoc);
            if (datCoc == null) return HttpNotFound();

            if (datCoc.TrangThai == "Chờ thanh toán")
            {

                datCoc.TrangThai = "Đang yêu cầu hủy";
                datCoc.LyDoHuy = lyDoHuy;
                datCoc.NgayHuy = DateTime.Now;

                db.SaveChanges();
                TempData["Success"] = "Đã gửi yêu cầu hủy! Vui lòng chờ Admin xác nhận.";
            }
            else
            {
                TempData["Error"] = "Đơn hàng không hợp lệ để gửi yêu cầu hủy.";
            }

            return RedirectToAction("ChiTietDatCoc", new { id = idDatCoc });
        }
        // Index default
        public ActionResult Index()
        {
            return View();
        }
    }
}
