using CNWEB.App_Start;
using CNWEB.Models;
using CNWEB.Models.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CNWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/home")]
    public class HomeController : Controller
    {
        private readonly WebContext _context;

        public HomeController(WebContext context)
        {
            _context = context;
        }
        [Route("")]
        [Route("Index")]
        [Authentication(Context = "_context")] // Truyền thể hiện của WebContext tới Authentication
        public IActionResult Index()
        {
           /* var user = SessionConfig.GetUser();
            if(user == null)
            {
                return Redirect("/Admin/AdminUser/Login");
            }*/
            var username = HttpContext.Session.GetString("Username");
            var fullname = HttpContext.Session.GetString("Name");
            var id = HttpContext.Session.GetString("ID");
            var image = HttpContext.Session.GetString("Image");
            var phone = HttpContext.Session.GetString("Phone");
            // Đặt thông tin người dùng vào ViewBag
            ViewBag.Username = username;
            ViewBag.fullname = fullname;
            return View();
        }

  /*      public IActionResult SomeAction()
        {
            // Lấy thông tin người dùng từ cơ sở dữ liệu hoặc từ nơi khác
            ViewBag.Username = "John Doe"; // Thay bằng thông tin thực

            return View();
        }*/
    }
}
