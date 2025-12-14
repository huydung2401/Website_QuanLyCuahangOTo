using System;
using System.Linq;
using System.Web.Mvc;
using TKW.Areas.Admin.Models;         // ViewModel KhachHangChiTietVM
using TKW.Models;                      // Model TKW

namespace TKW.Areas.Admin.Controllers
{
    public class QuanLyKhachHangController : BaseAdminController
    {
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // ================================================================
        // 1. DANH SÁCH KHÁCH HÀNG (Chỉ lấy User)
        // ================================================================
        public ActionResult Index()
        {
            var users = db.NguoiDungs
                .Where(u => u.VaiTro == "User")
                .AsEnumerable() // ⚠ bắt buộc vì tính toán ngoài Entity
                .Select(u => new
                {
                    User = u,
                    SoTuongTac =
                        db.DanhGias.Count(d => d.IdNguoiDung == u.IdNguoiDung) +
                        db.DatCocs.Count(d => d.IdNguoiDung == u.IdNguoiDung) +
                        db.LaiThus.Count(l => l.IdNguoiDung == u.IdNguoiDung) 
                     
                })
                .OrderByDescending(x => x.SoTuongTac > 0) // có tương tác lên đầu
                .ThenByDescending(x => x.SoTuongTac)      // nhiều hơn lên trên
                .Select(x => x.User) // ⬅️ trả lại NguoiDung
                .ToList();

            return View(users);
        }

        // ================================================================
        // 2. XEM CHI TIẾT KHÁCH (ALL-IN-ONE)
        // ================================================================
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var user = db.NguoiDungs.Find(id);
            if (user == null)
                return HttpNotFound();

            var viewModel = new KhachHangChiTietVM
            {
                NguoiDung = user,

                // Xe khách này đã đăng bán
                TinDaDang = db.Xes
                              .Where(x => x.IdNguoiBan == id)
                              .OrderByDescending(x => x.NgayDang)
                              .ToList(),

                // Lịch sử đặt cọc
                LichSuDatCoc = db.DatCocs
                                 .Where(d => d.IdNguoiDung == id)
                                 .OrderByDescending(d => d.NgayDat)
                                 .ToList(),

                // Lịch sử lái thử
                LichSuLaiThu = db.LaiThus
                                 .Where(l => l.IdNguoiDung == id)
                                 .OrderByDescending(l => l.NgayTao)
                                 .ToList(),

                // Lịch sử đánh giá xe
                LichSuDanhGia = db.DanhGias
                                  .Where(d => d.IdNguoiDung == id)
                                  .OrderByDescending(d => d.NgayDanhGia)
                                  .ToList()
            };

            return View(viewModel);
        }

        // ================================================================
        // 3. XÓA BÌNH LUẬN KHÁCH
        // ================================================================
        [HttpPost]
        public ActionResult DeleteComment(int idDanhGia, string idUserRedirect)
        {
            var cmt = db.DanhGias.Find(idDanhGia);

            if (cmt != null)
            {
                db.DanhGias.Remove(cmt);
                db.SaveChanges();

                TempData["ThongBao"] = "Đã xóa bình luận vi phạm.";
                TempData["LoaiThongBao"] = "alert-success";
            }

            return RedirectToAction("Details", new { id = idUserRedirect });
        }

        // ================================================================
        // 4. KHÓA TÀI KHOẢN
        // ================================================================
        public ActionResult KhoaTaiKhoan(string id)
        {
            var user = db.NguoiDungs.Find(id);

            if (user != null)
            {
                user.TrangThai = false;
                db.SaveChanges();

                TempData["ThongBao"] = "Đã khóa tài khoản: " + user.HoTen;
                TempData["LoaiThongBao"] = "alert-warning";
            }

            return RedirectToAction("Index");
        }

        // ================================================================
        // 5. MỞ KHÓA TÀI KHOẢN
        // ================================================================
        public ActionResult MoKhoaTaiKhoan(string id)
        {
            var user = db.NguoiDungs.Find(id);

            if (user != null)
            {
                user.TrangThai = true;
                db.SaveChanges();

                TempData["ThongBao"] = "Đã mở khóa tài khoản: " + user.HoTen;
                TempData["LoaiThongBao"] = "alert-success";
            }

            return RedirectToAction("Index");
        }

       

    }
}
