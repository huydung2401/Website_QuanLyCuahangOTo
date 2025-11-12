using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class DanhSachSanPham
    {
        // Kết nối đến DB thông qua Entity Framework Model
        QLMohoDBEntities2 db = new QLMohoDBEntities2();

        public DanhSachSanPham() { }

        // 🔹 Lấy toàn bộ danh sách sản phẩm
        public List<SanPham> LayTatCa()
        {
            return db.SanPhams.ToList();
        }

        // 🔹 Lấy sản phẩm theo Id
        public SanPham LayTheoId(int id)
        {
            return db.SanPhams.FirstOrDefault(sp => sp.IdSanPham == id);
        }

        // 🔹 Thêm mới sản phẩm
        public void ThemSanPham(SanPham sp)
        {
            db.SanPhams.Add(sp);
            db.SaveChanges();
        }

        // 🔹 Sửa sản phẩm
        public void SuaSanPham(SanPham sp)
        {
            var spCu = db.SanPhams.FirstOrDefault(x => x.IdSanPham == sp.IdSanPham);
            if (spCu != null)
            {
                spCu.TenSanPham = sp.TenSanPham;
                spCu.Gia = sp.Gia;
                spCu.GiaKhuyenMai = sp.GiaKhuyenMai;
                spCu.MoTaNgan = sp.MoTaNgan;
                spCu.MoTaChiTiet = sp.MoTaChiTiet;
                spCu.HinhAnh = sp.HinhAnh;
                spCu.DanhMucId = sp.DanhMucId;
                spCu.ChatLieu = sp.ChatLieu;
                spCu.MauSac = sp.MauSac;
                spCu.SoLuongTon = sp.SoLuongTon;
                spCu.NgayThem = sp.NgayThem;
                spCu.TrangThai = sp.TrangThai;
                db.SaveChanges();
            }
        }

        // 🔹 Xóa sản phẩm
        public void XoaSanPham(int id)
        {
            var sp = db.SanPhams.FirstOrDefault(x => x.IdSanPham == id);
            if (sp != null)
            {
                db.SanPhams.Remove(sp);
                db.SaveChanges();
            }
        }
    }
}