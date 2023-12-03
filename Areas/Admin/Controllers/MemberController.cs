using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CNWEB.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CNWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/Member")]
    public class MemberController : BaseController
    {
        public MemberController(WebContext context) : base(context)
        {
        }
      
  /*      public IActionResult Index()
        {
            return View();
        }*/
        [Route("")]
        [Route("Login")]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CheckMember(IFormCollection form)
        {
            string LoginName = form["LoginName"], Password = form["Password"];
            var item = _context.Members.Where(m => m.LoginName == LoginName && m.Password == Password).FirstOrDefault();
            if (item == null)
            {
                return RedirectToAction("Login", "Member");
            }
            var RoleIdofMember = _context.Credentials.Where(c=>c.GroupId==item.GroupId).Select(c=>c.RoleId).ToList();
            var RoleCodeList = _context.Roles.Where(r => RoleIdofMember.Contains(r.Id)).Select(r => r.Code).ToList();
            HttpContext.Session.SetString("Credential", JsonConvert.SerializeObject(RoleCodeList));
            HttpContext.Session.SetString("Member", JsonConvert.SerializeObject(item));
            HttpContext.Response.Cookies.Append("Credential", JsonConvert.SerializeObject(RoleCodeList), new CookieOptions
            {
                // Các tùy chọn khác nếu cần
                Expires = DateTime.UtcNow.AddMinutes(60), // Thời gian sống của cookie
                HttpOnly = true, // Không cho phép truy cập từ JavaScript
            });
            return RedirectToAction("Index", "Home", new {area="Admin"});
        }
        [HttpGet("GetMD5Hash")]
        public string GetMD5Hash(string input)
        {
            using(MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder= new StringBuilder();
                for(int i=0;i<(data.Length);i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
        public void SetLoginCookie(Member member)
        {
            // Tạo một đối tượng CookieOptions để thiết lập các tùy chọn cookie
            var cookieOptions = new CookieOptions
            {
                // Đặt thời gian sống của cookie, ở đây là 7 ngày
                Expires = DateTime.Now.AddDays(7),
                // Đặt trạng thái cookie chỉ có thể được truy cập thông qua HTTP và không bị xss
                HttpOnly = true,
                // Đặt Secure=true nếu bạn chỉ muốn gửi cookie qua HTTPS
                Secure = true,
                // Đặt SameSite=Strict để bảo vệ khỏi tấn công Cross-Site Request Forgery (CSRF)
                SameSite = SameSiteMode.Strict
            };
          /*  SetLoginCookie(item);*/
            // Chuyển đối tượng Member thành chuỗi JSON và lưu vào cookie
            Response.Cookies.Append("UserCookie", JsonConvert.SerializeObject(member), cookieOptions);
        }
        [HttpGet]
        public IActionResult logout()
        {
            HttpContext.Session.Remove("Member");
            Response.Cookies.Delete("UserCookie");
            return RedirectToAction("Login", "Member");
        }
        [Route("")]
        [Route("Create")]
        [HttpGet("Create")]
        [HasCredential(RoleCode = "add-product")]
        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Member member, IFormFile fileupload)
        {


            if (ModelState.IsValid)
            {
                // Xử lý giá trị trường EnteredDate
                member.CreatedDate = DateTime.Now; // hoặc bạn có thể sử dụng giá trị ngày từ biểu mẫu
                var fileName = Path.GetFileName(fileupload.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    fileupload.CopyTo(stream);
                }
               member.Image = fileName; // Đường dẫn dựa trên cách bạn lưu hình ảnh trong thư mục wwwroot/uploads
                // Xử lý giá trị trường Status
                if (member.Status == null)
                {
                    member.Status = 1; // giá trị mặc định
                }
                _context.Add(member);
                _context.SaveChanges();
                return RedirectToAction("Index", "Group");
            }

            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");
           
            return View(member);
        }
    }
}
