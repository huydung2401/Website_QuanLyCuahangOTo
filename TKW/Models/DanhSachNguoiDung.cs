using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class DanhSachNguoiDung
    {
        private QLMohoDBEntities2 db = new QLMohoDBEntities2();

        public DanhSachNguoiDung() { }

        // 🔹 Lấy danh sách người dùng
        public List<NguoiDung> LayTatCa()
        {
            return db.NguoiDungs.ToList();
        }

        // 🔹 Tìm theo Email
        public NguoiDung TimTheoEmail(string email)
        {
            return db.NguoiDungs.FirstOrDefault(nd => nd.Email == email);
        }

        // 🔹 Tìm theo ID
        public NguoiDung TimTheoId(string idNguoiDung)
        {
            return db.NguoiDungs.FirstOrDefault(nd => nd.IdNguoiDung == idNguoiDung);
        }

        // 🔹 Thêm người dùng mới
        public bool ThemNguoiDung(NguoiDung nd)
        {
            try
            {
                db.NguoiDungs.Add(nd);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi thêm người dùng: " + ex.Message);
                return false;
            }
        }

        // 🔹 Cập nhật thông tin người dùng
        public bool CapNhatNguoiDung(NguoiDung nd)
        {
            try
            {
                var existing = db.NguoiDungs.FirstOrDefault(x => x.IdNguoiDung == nd.IdNguoiDung);
                if (existing != null)
                {
                    existing.HoTen = nd.HoTen;
                    existing.Email = nd.Email;
                    existing.MatKhau = nd.MatKhau;
                    existing.DiaChi = nd.DiaChi;
                    existing.DienThoai = nd.DienThoai;
                    existing.LaAdmin = nd.LaAdmin;

                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi cập nhật người dùng: " + ex.Message);
                return false;
            }
        }

        // 🔹 Xóa người dùng
        public bool XoaNguoiDung(string idNguoiDung)
        {
            try
            {
                var nd = db.NguoiDungs.FirstOrDefault(x => x.IdNguoiDung == idNguoiDung);
                if (nd != null)
                {
                    db.NguoiDungs.Remove(nd);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi xóa người dùng: " + ex.Message);
                return false;
            }
        }

        // 🔹 Kiểm tra đăng nhập (Email + Mật khẩu)
        public NguoiDung KiemTraDangNhap(string email, string matKhau)
        {
            return db.NguoiDungs.FirstOrDefault(nd => nd.Email == email && nd.MatKhau == matKhau);
        }
    }
}