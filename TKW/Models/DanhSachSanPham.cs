using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class DanhSachSanPham
    {
        // KẾT NỐI ĐÚNG DATABASE WEBSITE MUA BÁN Ô TÔ
        WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        public DanhSachSanPham() { }

        // 🔹 Lấy toàn bộ danh sách sản phẩm (Xe)
        public List<Xe> LayTatCa()
        {
            return db.Xes.ToList();
        }

        // 🔹 Lấy sản phẩm (Xe) theo Id
        public Xe LayTheoId(string id)
        {
            return db.Xes.FirstOrDefault(sp => sp.IdXe == id);
        }

        // 🔹 Thêm mới sản phẩm (Xe)
        public void ThemSanPham(Xe sp)
        {
            db.Xes.Add(sp);
            db.SaveChanges();
        }

        // 🔹 Sửa sản phẩm (Xe)
        public void SuaSanPham(Xe sp)
        {
            var spCu = db.Xes.FirstOrDefault(x => x.IdXe == sp.IdXe);
            if (spCu != null)
            {
                spCu.TieuDe = sp.TieuDe;
                spCu.Gia = sp.Gia;
                spCu.NamSX = sp.NamSX;
                spCu.SoKM = sp.SoKM;

                spCu.HopSo = sp.HopSo;
                spCu.NhienLieu = sp.NhienLieu;
                spCu.MauSac = sp.MauSac;

                spCu.DongCo = sp.DongCo;
                spCu.CongSuat = sp.CongSuat;
                spCu.KichThuoc = sp.KichThuoc;
                spCu.XuatXu = sp.XuatXu;

                spCu.MoTaNgan = sp.MoTaNgan;
                spCu.MoTaChiTiet = sp.MoTaChiTiet;
                spCu.DiaDiem = sp.DiaDiem;

                spCu.TrangThaiTin = sp.TrangThaiTin;

                spCu.IdNguoiBan = sp.IdNguoiBan;
                spCu.IdDanhMuc = sp.IdDanhMuc;
                spCu.IdHangXe = sp.IdHangXe;
                spCu.IdDongXe = sp.IdDongXe;

                db.SaveChanges();
            }
        }

        // 🔹 Xóa sản phẩm (Xe)
        public void XoaSanPham(string id)
        {
            var sp = db.Xes.FirstOrDefault(x => x.IdXe == id);
            if (sp != null)
            {
                db.Xes.Remove(sp);
                db.SaveChanges();
            }
        }
    }
}