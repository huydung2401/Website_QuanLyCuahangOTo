using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class DanhSachHoaDon
    {
        QLMohoDBEntities2 db = new QLMohoDBEntities2();

        public DanhSachHoaDon()
        {

        }

        // 🔹 Lấy danh sách hóa đơn
        public List<HoaDon> GetDanhSachHoaDon(int? idHoaDon = null)
        {
            if (idHoaDon.HasValue)
                return db.HoaDons.Where(h => h.IdHoaDon == idHoaDon.Value).ToList();

            return db.HoaDons.ToList();
        }

        // 🔹 Lấy hóa đơn theo ID
        public HoaDon GetHoaDonById(int idHoaDon)
        {
            return db.HoaDons.FirstOrDefault(h => h.IdHoaDon == idHoaDon);
        }

        // 🔹 Thêm hóa đơn mới
        public bool AddHoaDon(HoaDon hd)
        {
            try
            {
                db.HoaDons.Add(hd);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi thêm hóa đơn: " + ex.Message);
                return false;
            }
        }

        // 🔹 Cập nhật hóa đơn
        public bool UpdateHoaDon(HoaDon hd)
        {
            try
            {
                var existing = db.HoaDons.FirstOrDefault(h => h.IdHoaDon == hd.IdHoaDon);
                if (existing != null)
                {
                    existing.NguoiDungId = hd.NguoiDungId;
                    existing.NgayDat = hd.NgayDat;
                    existing.TongTien = hd.TongTien;
                    existing.TrangThai = hd.TrangThai;

                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi cập nhật hóa đơn: " + ex.Message);
                return false;
            }
        }

        // 🔹 Xóa hóa đơn
        public bool DeleteHoaDon(int idHoaDon)
        {
            try
            {
                var hd = db.HoaDons.FirstOrDefault(h => h.IdHoaDon == idHoaDon);
                if (hd != null)
                {
                    db.HoaDons.Remove(hd);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi xóa hóa đơn: " + ex.Message);
                return false;
            }
        }

        // 🔹 Cập nhật trạng thái hóa đơn
        public bool UpdateTrangThai(int idHoaDon, string trangThai)
        {
            try
            {
                var hd = db.HoaDons.FirstOrDefault(h => h.IdHoaDon == idHoaDon);
                if (hd != null)
                {
                    hd.TrangThai = trangThai;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi cập nhật trạng thái: " + ex.Message);
                return false;
            }
        }
    }
}