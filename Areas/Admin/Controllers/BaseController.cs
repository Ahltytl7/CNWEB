using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using CNWEB.Models;

namespace CNWEB.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
       
            protected readonly WebContext _context;

            public BaseController(WebContext context)
            {
                _context = context;
            }

            public Member member;

            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (HttpContext.Session.GetString("Member") == null && filterContext.RouteData.Values["controller"].ToString() != "Member")
                {
                    filterContext.Result = new RedirectResult("/Admin/Member/Login");
                }
                else
                {
                string memberJson = HttpContext.Session.GetString("Member");

                if (!string.IsNullOrEmpty(memberJson))
                {
                    member = JsonConvert.DeserializeObject<Member>(memberJson);
                }
            }
                base.OnActionExecuting(filterContext);
            }
            public IActionResult Index()
            {
                return View();
            }
            /* public override void OnActionExecuting(ActionExecutingContext context)
     {
         if(context.HttpContext.Session.GetString("LoginName") == null)
         {
             context.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    {"Area","Admin" },
                     { "Controller", "Member" },
                     { "Action", "Login" }
                }
            );
             return;
         }
         return;
     }*/
        
    }
}
