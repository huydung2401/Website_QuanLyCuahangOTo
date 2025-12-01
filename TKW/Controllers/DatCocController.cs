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

        // Index default
        public ActionResult Index()
        {
            return View();
        }
    }
}
