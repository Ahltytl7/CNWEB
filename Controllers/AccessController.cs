using Microsoft.AspNetCore.Mvc;
using CNWEB.Models;
namespace CNWEB.Controllers
{
    public class AccessController : Controller
    {
        private readonly WebContext _context;

        public AccessController(WebContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                var u = _context.Users
                    .Where(x => string.Equals(x.Username, user.Username) && string.Equals(x.Password, user.Password))
                    .FirstOrDefault();

                if (u != null)
                {
                    if (u.Username != null)
                    {
                      /*  ViewBag.Username = user.Username;
                        ViewBag.image = user.Image;*/
                        HttpContext.Session.SetString("Username", u.Username.ToString());
                        HttpContext.Session.SetString("Name", u.Name); // Giả sử bạn có thông tin họ tên
                        HttpContext.Session.SetString("Image", u.Image); // Giả sử bạn có thông tin hình ảnh
                        HttpContext.Session.SetString("Id", u.Id);
                        HttpContext.Session.SetString("Phone", u.Phone);
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                }

            }

            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("Name");
            HttpContext.Session.Remove("Image");
            HttpContext.Session.Remove("Id");
            HttpContext.Session.Remove("Phone");
            return RedirectToAction("Login", "Access");

        }
    }
}

