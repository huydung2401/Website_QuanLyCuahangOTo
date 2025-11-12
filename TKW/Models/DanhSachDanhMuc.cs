using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class DanhSachDanhMuc
    {
        QLMohoDBEntities2 db = new QLMohoDBEntities2();

        public DanhSachDanhMuc() 
        { 

        }

        public List<DanhMuc> GetDanhSachDanhMuc(int? id = null)
        {
            if (id.HasValue)
            {
                return db.DanhMucs
                         .Where(dm => dm.IdDanhMuc == id.Value)
                         .ToList();
            }
            else
            {
                return db.DanhMucs.ToList();
            }
        }

        // ===============================
        // 🔹 Thêm danh mục
        // ===============================
        public bool AddDanhMuc(string ten, string mota)
        {
            try
            {
                DanhMuc dm = new DanhMuc
                {
                    TenDanhMuc = ten,
                    MoTa = mota ?? ""
                };

                db.DanhMucs.Add(dm);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi thêm danh mục: " + ex.Message);
                return false;
            }
        }

        // ===============================
        // 🔹 Cập nhật danh mục
        // ===============================
        public bool UpdateDanhMuc(int id, string ten, string mota)
        {
            try
            {
                var dm = db.DanhMucs.FirstOrDefault(d => d.IdDanhMuc == id);
                if (dm == null) return false;

                dm.TenDanhMuc = ten;
                dm.MoTa = mota ?? "";

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi cập nhật danh mục: " + ex.Message);
                return false;
            }
        }

        // ===============================
        // 🔹 Xóa danh mục
        // ===============================
        public bool DeleteDanhMuc(int id)
        {
            try
            {
                var dm = db.DanhMucs.FirstOrDefault(d => d.IdDanhMuc == id);
                if (dm == null) return false;

                db.DanhMucs.Remove(dm);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xóa danh mục: " + ex.Message);
                return false;
            }
        }
    }

}
