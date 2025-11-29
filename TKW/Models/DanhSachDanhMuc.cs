using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class DanhSachDanhMucXe
    {
        WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        public DanhSachDanhMucXe()
        {

        }

        // =============================
        // 🔹 Lấy danh sách danh mục xe
        // =============================
        public List<DanhMucXe> GetDanhSachDanhMuc(string id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return db.DanhMucXes
                         .Where(dm => dm.IdDanhMuc == id)
                         .ToList();
            }

            return db.DanhMucXes.ToList();
        }

        // =============================
        // 🔹 Tạo mã danh mục tự động DM01 → DM99
        // =============================
        private string TaoMaDanhMuc()
        {
            int count = db.DanhMucXes.Count() + 1;
            return "DM" + count.ToString("00");
        }

        // =============================
        // 🔹 Thêm danh mục xe
        // =============================
        public bool AddDanhMuc(string ten, string mota)
        {
            try
            {
                DanhMucXe dm = new DanhMucXe
                {
                    IdDanhMuc = TaoMaDanhMuc(),
                    TenDanhMuc = ten,
                    MoTa = mota ?? ""
                };

                db.DanhMucXes.Add(dm);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi thêm danh mục: " + ex.Message);
                return false;
            }
        }

        // =============================
        // 🔹 Cập nhật danh mục xe
        // =============================
        public bool UpdateDanhMuc(string id, string ten, string mota)
        {
            try
            {
                var dm = db.DanhMucXes.FirstOrDefault(d => d.IdDanhMuc == id);
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

        // =============================
        // 🔹 Xóa danh mục xe
        // =============================
        public bool DeleteDanhMuc(string id)
        {
            try
            {
                var dm = db.DanhMucXes.FirstOrDefault(d => d.IdDanhMuc == id);
                if (dm == null) return false;

                // Kiểm tra danh mục có xe không
                bool tonTaiXe = db.Xes.Any(x => x.IdDanhMuc == id);
                if (tonTaiXe)
                    return false; // Không xóa được vì đang có xe thuộc danh mục

                db.DanhMucXes.Remove(dm);
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
