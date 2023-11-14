using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using CNWEB.Models; // Giả sử WebContext nằm trong namespace này
namespace CNWEB.Models.Authentication
{
    /*public class Authentication:ActionFilterAttribute 
    {
        private readonly WebContext _context; // Thêm WebContext ở đây

        public Authentication(WebContext context)
        {
            _context = context;
        }
        public string MaChucNang { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
          
            if (context.HttpContext.Session.GetString("Username") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {{"Controller","Access"},
                        {"Action","Login" }

                    }
                    );
                return;
            }
            if (string.IsNullOrEmpty(MaChucNang) == false)
            {
                var check = new mapPhanQuyen(_context).KiemTra(HttpContext.Session.GetString("Id"), MaChucNang);
            }
            return;
        }
    }*/
    public class Authentication : ActionFilterAttribute
    {
        public string MaChucNang { get; set; }
        public string Context { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("Username") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "Controller", "Access" },
                        { "Action", "Login" }
                    }
                );
                return;
            }

            if (!string.IsNullOrEmpty(MaChucNang))
            {
                var check = new mapPhanQuyen((WebContext)context.HttpContext.RequestServices.GetService(typeof(WebContext))).KiemTra(context.HttpContext.Session.GetString("Id"), MaChucNang);
                if (check == false)
                {
                    context.Result = new RedirectToRouteResult(
                  new RouteValueDictionary
                  {
                         {"Area","Admin"},
                        { "Controller", "AdminUser" },
                        { "Action", "LoiPhanQuyen" },
                  }
                    );
                    
                }
                else
                {
                    return;
                }

            }

            return;
        }
    }
}
