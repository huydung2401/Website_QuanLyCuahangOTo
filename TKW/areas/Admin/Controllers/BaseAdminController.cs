using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TKW.Models;

namespace TKW.Areas.Admin.Controllers
{
    public class BaseAdminController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Lấy session đăng nhập
            var user = Session["user"] as NguoiDung;

            // Nếu chưa login hoặc vai trò không hợp lệ
            if (user == null || (user.VaiTro != "Admin" && user.VaiTro != "Seller"))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        area = "",
                        controller = "Login",
                        action = "Index"
                    })
                );

                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
