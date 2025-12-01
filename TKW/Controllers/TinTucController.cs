using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TKW.Models;    // EDMX của dự án TKW

namespace TKW.Controllers
{
    public class TinTucController : Controller
    {
        private WebsiteMuaBanOtoDBEntities db = new WebsiteMuaBanOtoDBEntities();

        // ===============================
        // TRANG TIN TỨC
        // ===============================
        public ActionResult Index()
        {
            // Lấy top 5 xe giá cao nhất (giống bản gốc)
            ViewBag.TopXe = db.Xes
                              .OrderByDescending(x => x.Gia)
                              .Take(5)
                              .ToList();

            return View();
        }
    }
}
