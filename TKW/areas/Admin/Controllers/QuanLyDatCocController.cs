using System;
using System.Linq;
using System.Web.Mvc;
using TKW.Models;   // Model TKW

namespace TKW.Areas.Admin.Controllers
{
    public class QuanLyDatCocController : BaseAdminController
    {
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // =============================================================
        // 1. DANH SÁCH ĐƠN CỌC
        // =============================================================
        public ActionResult Index()
        {
            // Sắp xếp: Ưu tiên đơn "Đang yêu cầu hủy" lên đầu để xử lý trước, sau đó đến ngày đặt mới nhất
            var listDatCoc = db.DatCocs.OrderByDescending(x => x.TrangThai == "Đang yêu cầu hủy")
                                       .ThenByDescending(x => x.NgayDat)
                                       .ToList();

            return View(listDatCoc);
        }

        // =============================================================
        // 2. XÁC NHẬN THANH TOÁN (ĐÃ THU TIỀN)
        // =============================================================
        public ActionResult XacNhanThanhToan(int id)
        {
            //var don = db.DatCocs.Find(id);

            //if (don != null)
            //{
            //    don.TrangThai = "Đã cọc";
            //    db.SaveChanges();

            //    TempData["ThongBao"] = "Xác nhận thu tiền thành công cho đơn: " + id;
            //    TempData["LoaiThongBao"] = "alert-success";
            //}
            var don = db.DatCocs.Find(id);
            if (don != null && don.TrangThai == "Chờ thanh toán")
            {
                don.TrangThai = "Đã cọc";
                db.SaveChanges();

                TempData["ThongBao"] = "Xác nhận thu tiền thành công cho đơn: " + id;
                TempData["LoaiThongBao"] = "alert-success";
            }

            return RedirectToAction("Index");
        }

        // 3. DUYỆT YÊU CẦU HỦY (Dành cho khách hàng gửi yêu cầu)
        public ActionResult DuyetHuyDon(int id)
        {
            var don = db.DatCocs.Find(id);
            // Chỉ duyệt khi trạng thái đúng là "Đang yêu cầu hủy"
            if (don != null && don.TrangThai == "Đang yêu cầu hủy")
            {
                don.TrangThai = "Đã hủy";
                // don.LyDoHuy giữ nguyên lý do khách ghi

                db.SaveChanges();
                TempData["ThongBao"] = "Đã chấp thuận hủy đơn cọc số: " + id;
                TempData["LoaiThongBao"] = "alert-success";
            }
            return RedirectToAction("Index");
        }

        // =============================================================
        // 3. HỦY ĐƠN ĐẶT CỌC
        // =============================================================
        public ActionResult HuyDon(int id)
        {
            var don = db.DatCocs.Find(id);

            if (don != null)
            {
                don.TrangThai = "Đã hủy";
                db.SaveChanges();

                TempData["ThongBao"] = "Đã hủy đơn đặt cọc số: " + id;
                TempData["LoaiThongBao"] = "alert-warning";
            }

            return RedirectToAction("Index");
        }

        // =============================================================
        // 4. IN HÓA ĐƠN / HỢP ĐỒNG ĐẶT CỌC
        // =============================================================
        public ActionResult InHoaDon(int id)
        {
            var don = db.DatCocs.Find(id);

            if (don == null)
                return HttpNotFound();

            return View(don);
        }

        // =============================================================
        // 5. XEM CHI TIẾT ĐƠN CỌC
        // =============================================================
        public ActionResult Details(int id)
        {
            var don = db.DatCocs.Find(id);

            if (don == null)
                return HttpNotFound();

            return View(don);
        }
    }
}
