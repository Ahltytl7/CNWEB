using Microsoft.AspNetCore.Mvc;
using CNWEB.Models;
using CNWEB.App_Start;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNWEB.Models.Authentication;

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
        [Authentication(MaChucNang = "02")]
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
        [Route("")]
        [Route("PhanQuyenChucNang")]
        [HttpGet("PhanQuyenChucNang")]
        public IActionResult PhanQuyenChucNang(string idTaiKhoan)
        {
            var username = HttpContext.Session.GetString("Username");
            var fullname = HttpContext.Session.GetString("Name");
            var id = HttpContext.Session.GetString("ID");
            var image = HttpContext.Session.GetString("Image");
            var phone = HttpContext.Session.GetString("Phone");
            // Đặt thông tin người dùng vào ViewBag
            ViewBag.Username = username;
            ViewBag.fullname = fullname;
            /* var username = HttpContext.Session.GetString("Username");
             var fullname = HttpContext.Session.GetString("Name");
             var id = HttpContext.Session.GetString("ID");
             var image = HttpContext.Session.GetString("Image");
             var phone = HttpContext.Session.GetString("Phone");

             // Đặt thông tin người dùng vào ViewBag
             ViewBag.Username = username;
             ViewBag.fullname = fullname;*/

            /*    
                if (!String.IsNullOrEmpty(idTaiKhoan))
                {*/
            /*var user = _context.Users.SingleOrDefault(x => x.Id == idTaiKhoan);
            if (user != null)
            {
                return View(user);
            }
            return NotFound(); // Trả về trang lỗi 404*/
            if (idTaiKhoan == null || _context.Users == null)
            {
                return NotFound();
            }

            var order =  _context.Users
                .FirstOrDefault(m => m.Id == idTaiKhoan);
            if (order == null)
            {
                return NotFound();
            }
            /* var chitietdonhang = _context.OrderDetails.Include(x => x.Product).AsNoTracking().Where(x => x.OrderId == order.Id).OrderBy(x => x.Id).ToList();
             ViewBag.chitiet = chitietdonhang;*/
            return View(order);

        }
        [Route("")]
        [Route("LuuPhanQuyen")]
        [HttpGet("LuuPhanQuyen")]
        public IActionResult LuuPhanQuyen(string idTaiKhoan,string chucNang)
        {
            var username = HttpContext.Session.GetString("Username");
            var fullname = HttpContext.Session.GetString("Name");
            var id = HttpContext.Session.GetString("ID");
            var image = HttpContext.Session.GetString("Image");
            var phone = HttpContext.Session.GetString("Phone");
            // Đặt thông tin người dùng vào ViewBag
            ViewBag.Username = username;
            ViewBag.fullname = fullname;
            new mapPhanQuyen().LuuPhanQuyen(idTaiKhoan,chucNang);
            return RedirectToAction("PhanQuyenChucNang", new { idTaiKhoan = idTaiKhoan });
        }
    }
}
