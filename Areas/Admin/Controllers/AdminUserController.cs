using Microsoft.AspNetCore.Mvc;
using CNWEB.Models;
using CNWEB.App_Start;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CNWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/AdminUser")]
    public class AdminUserController : Controller
    {
        private readonly WebContext _context;
        /* private readonly IHostEnvironment _hostEnvironment;*/

        public AdminUserController(WebContext context)
        {
            _context = context;
            /* _webHostEnvironment = hostEnvironment; */
        }

        [Route("")]
        [Route("Login")]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string name, string pass)
        {
            SessionConfig s = new SessionConfig();
            mapTaiKhoan map = new mapTaiKhoan();
            var user = map.TimKiem(name,pass);
            if (user != null)
            {
                s.SetUser(user);
                s.GetUser();
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            ViewBag.error = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View();
        }
        [Route("")]
        [Route("Index")]
        [HttpGet("Index")]
        public IActionResult Index(int? page, string CatID)
        {
            var username = HttpContext.Session.GetString("Username");
            var fullname = HttpContext.Session.GetString("Name");
            var id = HttpContext.Session.GetString("ID");
            var image = HttpContext.Session.GetString("Image");
            var phone = HttpContext.Session.GetString("Phone");
            // Đặt thông tin người dùng vào ViewBag
            ViewBag.Username = username;
            ViewBag.fullname = fullname;
            var pageNumber = page ?? 1;
            var pageSize = 5;

            var lsProducts = from x in _context.Users select x;

            if (!String.IsNullOrEmpty(CatID))
            {

                lsProducts = lsProducts.Where(x =>
                x.Id.ToLower().Contains(CatID) || x.Name.ToLower().Contains(CatID) ||
           x.Username.ToLower().Contains(CatID) || x.Password.ToLower().Contains(CatID) || x.Phone.ToLower().Contains(CatID));

            }


            var totalProducts = lsProducts.Count();
            ViewBag.totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            var pagedList = new X.PagedList.PagedList<User>(lsProducts.OrderBy(x => x.Id), pageNumber, pageSize);

            var startRange = (pageNumber - 1) * pageSize + 1;
            var endRange = Math.Min(pageNumber * pageSize, totalProducts);

            ViewBag.CatIDValue = CatID;
       
            ViewBag.TotalRecords = totalProducts;

            ViewBag.CurrentPage = pageNumber;
            ViewBag.StartRange = startRange;
            ViewBag.EndRange = endRange;


            return View(pagedList); // Trả về danh sách đã phân trang cho view

        }
        [Route("")]
        [Route("LoiPhanQuyen")]
        [HttpGet("LoiPhanQuyen")]
        public IActionResult LoiPhanQuyen()
        {
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
    }
}
