using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNWEB.Models;
using X.PagedList;
using CNWEB.Models.Authentication;
namespace CNWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/AdminOrders")]
    [HasCredential(RoleCode = "order")]
    public class AdminOrdersController : Controller
    {
        private readonly WebContext _context;

        public AdminOrdersController(WebContext context)
        {
            _context = context;
        }
        [Route("")]
        [Route("Index")]
        [HttpGet("Index")]
        [HasCredential(RoleCode = "order")]
        // GET: Admin/AdminOrders
        public IActionResult Index(int? page, string CatID)
        {
            /*var webContext = _context.Orders.Include(o => o.TransactStatus).Include(o => o.User);
            return View(await webContext.ToListAsync());*/
          /*  var username = HttpContext.Session.GetString("Username");
            var fullname = HttpContext.Session.GetString("Name");
            var id = HttpContext.Session.GetString("ID");
            var image = HttpContext.Session.GetString("Image");
            var phone = HttpContext.Session.GetString("Phone");
            // Đặt thông tin người dùng vào ViewBag
            ViewBag.Username = username;
            ViewBag.fullname = fullname;*/
            var pageNumber = page ?? 1;
            var pageSize = 5;

            var lsProducts = from x in _context.Orders select x;

             ViewData["DanhMuc"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "Status", CatID);
            /*  var orders = _context.Orders.Where(x => x.TransactStatusId == CatID);
              return View(orders);*/
            if (!String.IsNullOrEmpty(CatID))
            {
                lsProducts = lsProducts.Where(x =>
         x.Id.ToLower().Contains(CatID) || (x.Address != null && x.Address.ToLower().Contains(CatID)) || 
           _context.Customers.Any(c => c.Id == x.UserId && c.Name.ToLower().Contains(CatID)) || _context.Customers.Any(c => c.Id == x.UserId && c.Email.ToLower().Contains(CatID)) ||
     (_context.TransactStatuses.Any(c => c.TransactStatusId == x.TransactStatusId && c.Status != null && c.Status.ToLower().Contains(CatID)))
     // Add other fields for search here
     );
            }

            var totalProducts = lsProducts.Count();
            ViewBag.totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            var pagedList = new X.PagedList.PagedList<Order>(lsProducts.OrderBy(x => x.Id), pageNumber, pageSize);
            /*  var Categories = _context.Categories.ToList();*/
            /*     Categories.Insert(0, new Category(_context) { Id = "00", Names = "-------Danh Mục------" });*/
            var startRange = (pageNumber - 1) * pageSize + 1;
            var endRange = Math.Min(pageNumber * pageSize, totalProducts);
            /*  ViewBag.CatID = new SelectList(Categories, "Id", "Names", CatID);*/
         /*   ViewBag.IdTradeMark = new SelectList(_context.TradeMarks.ToList(), "Id", "Names");
            ViewBag.IdCategories = new SelectList(_context.Categories.ToList(), "Id", "Names");*/
            ViewBag.TotalRecords = totalProducts;
            ViewBag.CatIDValue = CatID;
            /* ViewBag.CurrentCatID = CatID;*/
            ViewBag.CurrentPage = pageNumber;
            ViewBag.StartRange = startRange;
            ViewBag.EndRange = endRange;
            /*  ViewData["DanhMuc"] = new SelectList(_context.Categories, "Id", "Names");*/

            return View(pagedList); // Trả về danh sách đã phân trang cho view
        }
        [Route("")]
        [Route("Details")]
        [HttpGet("Details")]
        [HasCredential(RoleCode = "view-order")]
        // GET: Admin/AdminOrders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.TransactStatus)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            var chitietdonhang = _context.OrderDetails.Include(x=>x.Product).AsNoTracking().Where(x=>x.OrderId==order.Id).OrderBy(x=>x.Id).ToList();
            ViewBag.chitiet = chitietdonhang;
            return View(order);
        }
        [HttpGet]
     /*   public IActionResult Filtter(string CatID = null)
        {
            var url = $"/Admin/AdminOrders?CatID={CatID}";
            if (string.IsNullOrEmpty(CatID))
            {
                url = $"/Admin/AdminOrders";
            }
            return Json(new { status = "success", redirectUrl = url });
        }*/
        [Route("")]
        [Route("Create")]
        [HttpGet("Create")]
       
        // GET: Admin/AdminOrders/Create
        public IActionResult Create()
        {
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Admin/AdminOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Address,Amount,OrderDate,Phone,TransactStatusId,UserId,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId", order.TransactStatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

            [Route("")]
        [Route("Edit")]
        [HttpGet("Edit")]
        // GET: Admin/AdminOrders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId", order.TransactStatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

        // POST: Admin/AdminOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*     [Route("")]
             [Route("UpdateTT")]
             *//*    [Route("Edit")]*/
        [HttpGet]
        public IActionResult GetData()
        {
            // Lấy dữ liệu mới từ cơ sở dữ liệu hoặc bất kỳ nguồn nào khác
            var data = _context.Orders.ToList();

            // Trả về một phần view chứa dữ liệu mới
            return PartialView("_RecordsTablePartial", data); // Thay thế _RecordsTablePartial bằng tên của file view chứa bảng dữ liệu của bạn
        }
        [Route("")]
        [Route("UpdateTT")]
        /*    [Route("Edit")]*/
        [HttpPost]
        [HasCredential(RoleCode = "edit-order")]
        /*   [ValidateAntiForgeryToken]*/
        public IActionResult UpdateTT(string id, string trangthai)
        {

            var item = _context.Orders.Find(id);
            if (item != null)
            {
                _context.Orders.Attach(item);
                if (trangthai == "01")
                {
                    item.TransactStatusId = "01";
                }
                else if (trangthai == "02")
                {
                    item.TransactStatusId = "02";
                }
                else if (trangthai == "03")
                {
                    item.TransactStatusId = "03";
                }
                else if (trangthai == "04")
                {
                    item.TransactStatusId = "04";
                }

                _context.Entry(item).Property(x => x.TransactStatusId).IsModified = true;
                _context.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }

            return Json(new { Message = "Unsuccess", Success = false });
        }
        /*        public IActionResult UpdateTT(string id, string trangthai)
                {

                    var item = _context.Orders.Find(id);
                    if (item != null)
                    {
                        _context.Orders.Attach(item);
                        if (trangthai == "01")
                        {
                            item.TransactStatusId = "01";
                        }
                        else if (trangthai == "02")
                        {
                            item.TransactStatusId = "02";
                        }
                        else if (trangthai == "03")
                        {
                            item.TransactStatusId = "03";
                        }
                        else if (trangthai == "04")
                        {
                            item.TransactStatusId = "04";
                        }

                        _context.Entry(item).Property(x => x.TransactStatusId).IsModified = true;
                        _context.SaveChanges();

                    }
                    var ls = _context.Orders.ToList();
                    return PartialView("_RecordsTablePartial", ls);
                    return Json(new { Message = "Unsuccess", Success = false });
                }*/
        /* if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId", order.TransactStatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);*/
        [Route("")]
        [Route("Delete")]
        [HttpGet("Delete")]
        // GET: Admin/AdminOrders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.TransactStatus)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/AdminOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'WebContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(string id)
        {
          return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
