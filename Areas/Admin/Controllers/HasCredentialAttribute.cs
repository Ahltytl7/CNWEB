using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace CNWEB.Areas.Admin.Controllers
{
    public class HasCredentialAttribute : ActionFilterAttribute
    {
        /*public string RoleCode { get; set; }*/
       
      /*  public void OnAuthorization(AuthorizationFilterContext context)
        {
            var credentialCookie = context.HttpContext.Request.Cookies["Credential"];
            List<string> credential = JsonConvert.DeserializeObject<List<string>>(credentialCookie);

            if (credential == null || !credential.Contains(this.RoleCode))
            {
                context.Result = new RedirectResult("/Admin/Home/NotCredential");
            }*/
          /*  else
            {
                context.Result = new RedirectResult("/Admin/Home/NotCredential");
            }*/
    /*    }
        private List<string> GetCredentialByLoggedInMember(HttpContext httpContext)
        {
            return httpContext.Session.Get<List<string>>("Credential");
        }*/
        public string RoleCode { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("Member") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "Controller", "Member" },
                        { "Action", "Login" }
                    }
                );
                return;
            }

            /* if (!string.IsNullOrEmpty(MaChucNang))
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

             }*/
            var credentialCookie = context.HttpContext.Request.Cookies["Credential"];
            List<string> credential = JsonConvert.DeserializeObject<List<string>>(credentialCookie);

            if (credential == null || !credential.Contains(this.RoleCode))
            {
                /*context.Result = new RedirectResult("/Admin/Home/NotCredential");*/
                context.Result = new RedirectToRouteResult(
                   new RouteValueDictionary
                   {
                          {"Area","Admin"},
                         { "Controller", "Home" },
                         { "Action", "NotCredential" },
                   }
                     );

            }

            return;
        }
        /*      public void OnAuthorization(AuthorizationFilterContext context)
              {
                  // Kiểm tra quyền hạn dựa trên RoleCode
                  var hasPermission = CheckPermission(context.HttpContext, RoleCode);

                  if (!hasPermission)
                  {
                      // Chuyển hướng hoặc xử lý khi không có quyền
                      context.Result = new ForbidResult();
                  }
              }

              private bool CheckPermission(HttpContext httpContext, string roleCode)
              {
                  // Lấy thông tin quyền hạn từ nguồn dữ liệu (có thể là cơ sở dữ liệu, session, cache, ...)
                  var credentialCookie = httpContext.Request.Cookies["Credential"];
                  var credential = JsonConvert.DeserializeObject<List<string>>(credentialCookie);

                  // Kiểm tra xem RoleCode có trong danh sách quyền hạn không
                  return credential != null && credential.Contains(roleCode);
              }*/

        /*   public void OnAuthorization(AuthorizationFilterContext context)
           {
               // Kiểm tra xem người dùng đã đăng nhập chưa
               if (!context.HttpContext.User.Identity.IsAuthenticated)
               {
                   // Người dùng chưa đăng nhập, bạn có thể xử lý ở đây nếu cần
                   context.Result = new RedirectResult("/Admin/Member/Login");
                   return;
               }

               // Lấy danh sách quyền từ Session
               List<string> userRoles = GetCredentialByLoggedInMember(context.HttpContext);

               // Kiểm tra quyền của người dùng
               if (!userRoles.Contains(this.RoleCode))
               {
                   // Người dùng không có quyền, chuyển hướng đến trang không có quyền
                   context.Result = new RedirectResult("/Admin/Home/NotCredential");
               }
           }*/


        /* public string RoleCode { get; set; }
         protected override bool AuthorizeCore(HttpContextBase httpContext)
         {
             List<string> credential = this.getCredentialByLoogedInMember();
             if(credential != null && credential.Contains(this.RoleCode))
             {
                 return true;
             }
             return false;
         }
         protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
         {
             filterContext.Result = new RedirectResult("/Home/NotCredential");
         }
          private List<string> getCredentialByLoogedInMember()
         {
             return (List<string>)HttpContext.Current.Session["Credential"];
         }*/
    }
}
