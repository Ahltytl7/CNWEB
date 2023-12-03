using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CNWEB.Models;
using System.Drawing.Printing;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;
using System.Net.WebSockets;
using System;

namespace CNWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/group")]
    [HasCredential(RoleCode = "admin")]
    public class GroupController : BaseController
    {
        public GroupController(WebContext context) : base(context)
        {
        }
        public int MemberCount { get; set; }
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            var roles = _context.Roles.OrderBy(r=>r.GroupName).ToList();
            ViewBag.Roles = roles;
            var groups = _context.Groups.Include(g => g.Members).ToList();
        

            // Update MemberCount for each group
            foreach (var group in groups)
            {
                MemberCount = _context.Members.Where(m => m.GroupId == group.Id).Count();
            }
         /*   var totalProducts = groups;
            ViewBag.MemberCount = MemberCount;
            ViewBag.totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            var pagedList = new X.PagedList.PagedList<TradeMark>(lsTradeMark.OrderBy(x => x.Id), pageNumber, pageSize);*/
            return View(groups);
        }
        [Route("")]
        [Route("getName")]

        [HttpGet]
        public IActionResult getName(string idCheck)
        {
            var name = _context.Roles.Where(c => c.Id == idCheck).FirstOrDefault().Name;
            var group1 = _context.Roles.Where(c => c.Id == idCheck).FirstOrDefault().GroupName;
            var idXem = "";
            if(name != "Xem")
            {
                idXem = _context.Roles.Where(c => c.Name == "Xem" && c.GroupName == group1).FirstOrDefault().Id;
            }
            return Json(idXem);
        }
        [Route("")]
        [Route("getGroupName")]

        [HttpGet]
        public IActionResult getGroupName(string groupId)
        {
            var items = _context.Credentials.Where(c => c.GroupId == groupId).Select(c => c.RoleId).ToList();
            var groupNames = _context.Roles.Where(x=>items.Contains(x.Id)).Select(x=>x.GroupName).ToList();
            var namesId = _context.Roles.Where(y => groupNames.Contains(y.GroupName) && y.Name == "Xem").Select(y => y.Id).ToList();
            return Json(namesId);
        }
        [Route("")]
        [Route("getCredential")]
      
        [HttpGet] 
        public IActionResult getCredential(string groupId)
        {
            var items = _context.Credentials.Where(c => c.GroupId == groupId).Select(c => c.RoleId).ToList();

            return Json(items);
        }
        [Route("")]
        [Route("saveCredential1")]
        /*    [Route("Edit")]*/
        [HttpPost]
        public IActionResult saveCredential1(string groupId, string roleId)
        {
            try
            {

                var item1 = _context.Credentials.Where(c => c.GroupId == groupId && c.RoleId == roleId).FirstOrDefault();
                if (item1 == null)
                {
                    var randomBytes = new byte[16];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(randomBytes);
                    }
                    var randomId = BitConverter.ToString(randomBytes).Replace("-", "");
                    _context.Credentials.Add(new Models.Credential
                    {
                        Id = randomId,
                        GroupId = groupId,
                        RoleId = roleId
                    });

                }
                
                    _context.SaveChanges();
                    return Json(new { success = true });
               
             
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine(ex.Message);
                return Json(new { success = false, errorMessage = "Internal Server Error" });
            }
        }
        [Route("")]
        [Route("saveCredential")]
        /*    [Route("Edit")]*/
        [HttpPost]
        public IActionResult saveCredential(string groupId,string roleId)
        {
            try {
            var item = _context.Credentials.Where(c => c.GroupId == groupId && c.RoleId == roleId).FirstOrDefault();
         /*   var item1 = _context.Credentials.Where(c=>c.GroupId == groupId && c.RoleId == role1).FirstOrDefault();*/
            if (item != null )
            {
                _context.Credentials.Remove(item);
            }
            else
            {
                var randomBytes = new byte[16];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomBytes);
                }
                var randomId = BitConverter.ToString(randomBytes).Replace("-", "");
                _context.Credentials.Add(new Models.Credential {
                    Id = randomId,
                    GroupId = groupId,
                    RoleId = roleId });

            }
            _context.SaveChanges();
            return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine(ex.Message);
                return Json(new { success = false, errorMessage = "Internal Server Error" });
            }
        }
        [Route("")]
        [Route("deleteCredential")]
        /*    [Route("Edit")]*/
        [HttpPost]
        public IActionResult deleteCredential(string groupId, string roleId)
        {
            try
            {
                var item = _context.Credentials.Where(c => c.GroupId == groupId && c.RoleId == roleId).FirstOrDefault();
                /*   var item1 = _context.Credentials.Where(c=>c.GroupId == groupId && c.RoleId == role1).FirstOrDefault();*/
                if (item != null)
                {
                    _context.Credentials.Remove(item);
                }
              
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine(ex.Message);
                return Json(new { success = false, errorMessage = "Internal Server Error" });
            }
        }
        [Route("")]
        [Route("GroupMembers")]
        public IActionResult GroupMembers(string groupId)
        {
            var members = _context.Members.Where(m => m.GroupId == groupId).ToList();
            return View(members);
        }

        [Route("")]
        [Route("SaveMember")]
        [HttpPost]
        public IActionResult SaveMember( string memberName, string memberLoginName, string memberPassword, DateTime memberCreatedDate, int memberStatus, string groupId, string memberImage, string memberPhone, string memberAddress)
        {
            var memberName1 = memberName;
            var memberLoginName1 = memberLoginName;
            var memberPassword1 = memberPassword;
            var memberCreatedDate1 = memberCreatedDate;
            var memberStatus1 = memberStatus;
            var groupId1 = groupId;
            var memberImage1 = memberImage;
            var memberPhone1 = memberPhone;
            var memberAddress1 = memberAddress;
            try
            {
                // Tạo một đối tượng Member với thông tin từ request
                Member newMember = new Member
                {
                    
                    Name = memberName,
                    LoginName = memberLoginName,
                    Password = memberPassword,
                    CreatedDate = memberCreatedDate,
                    Status = memberStatus,
                    GroupId = groupId,
                    Image = memberImage,
                    Phone = memberPhone,
                    Address = memberAddress
                    // Thêm các trường khác tương ứng
                };

                // Thêm đối tượng Member vào cơ sở dữ liệu sử dụng Entity Framework
                _context.Members.Add(newMember);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine(ex.Message);
                return Json(new { success = false, errorMessage = "Lỗi khi lưu thông tin thành viên" });
            }
        }
        [Route("")]
        [Route("Create")]
        [HttpGet("Create")]
       
        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {

          
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Group group)
        {


            if (ModelState.IsValid)
            {
                // Xử lý giá trị trường EnteredDate
              
                _context.Add(group);
                _context.SaveChanges();
                return RedirectToAction("Index", "Group");
            }

            
            return View(group);
        }
        [Route("")]
        [Route("CheckRole")]
        [HttpGet("CheckRole")]

        // GET: Admin/AdminProducts/Create
        public IActionResult CheckRole(string groupName)
        {
            var role = _context.Roles.FirstOrDefault(x => x.GroupName == groupName && x.Name == "Xem").Id;
            return Json(role);
           /* var groupName = role.GroupName;*/
/*            var list = _context.Roles.Where(t=>t.GroupName==groupName).ToList();    */
        /*    var roleId1 = _context.Roles.FirstOrDefault(u=>u.Id==roleId && u.GroupName==groupName).Id;*/
           
        }
        [Route("")]
        [Route("Edit")]
        [HttpGet("Edit")]
   
     
        public IActionResult Edit(string id)
        {

            var group = _context.Groups.Find(id);

         

            return View(group);
        }



        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Group group)
        {
            if (id != group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   
               
                    _context.Update(group);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                   
                        return NotFound();
                  
                 
                }
                return RedirectToAction("Index");
            }
   
            return View(group);
        }
        [Route("")]
        [Route("Delete")]
        [HttpGet("Delete")]
      
        public IActionResult Delete(string id)
        {
       
  
            var tradeMark = _context.Groups.Find(id);

            if (tradeMark != null)
            {
                var productImages = _context.Groups.Where(x => x.Id == id).ToList();

                if (productImages.Any()) _context.RemoveRange(productImages);
                _context.Remove(tradeMark);
                _context.SaveChanges();
                TempData["Message"] = "Nhóm quyền đã được xoá";
            }
            else
            {
                TempData["Message"] = "Không tìm thấy nhóm quyền để xoá";
            }

            return RedirectToAction("Index");
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Groups == null)
            {
                return Problem("Entity set 'WebContext.Products'  is null.");
            }
            var group = await _context.Groups.FindAsync(id);
            if (group != null)
            {
                _context.Groups.Remove(group);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /*     [Route("")]
             [Route("getGroups")]
             public IActionResult getGroups()
             {
                 var groups = _context.Groups.Include(g => g.Members).ToList();

                 // Update MemberCount for each group
                 foreach (var group in groups)
                 {
                     group.MemberCount = group.Members.Count();
                 }

                 return View(groups);
             }*/
        /*
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var lenght = Request.Form["lenght"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = lenght != null ? Convert.ToInt32(lenght) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var items = _context.Groups.ToList().Select(g => new
            {
                g.Id,
                g.Name,
                CountofMember = _context.Members.Where(m => m.GroupId == g.Id).Count()
            });
            if (!string.IsNullOrEmpty(searchValue))
            {
                items = items.Where(n => n.Name.ToLower().Contains(searchValue.ToLower())).ToList();
            }
            recordsTotal = items.Count();
            items = items.OrderByDescending(n=>n.Name).Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = items });*/
    }
}
