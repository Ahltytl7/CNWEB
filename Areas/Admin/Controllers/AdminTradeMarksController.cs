using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNWEB.Models;
using X.PagedList;
namespace CNWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/AdminTradeMarks")]
    public class AdminTradeMarksController : Controller
    {
        private readonly WebContext _context;

        public AdminTradeMarksController(WebContext context)
        {
            _context = context;
        }
        [Route("")]
        [Route("Index")]
        [HttpGet("Index")]
        // GET: Admin/AdminTradeMarks
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
            var pageSize = 3;

            var lsTradeMark = from x in _context.TradeMarks select x;

            if (!String.IsNullOrEmpty(CatID))
            {

                lsTradeMark = lsTradeMark.Where(x =>
         x.Id.ToLower().Contains(CatID) || x.Names.ToLower().Contains(CatID));

            }


            var totalProducts = lsTradeMark.Count();
            ViewBag.totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            var pagedList = new X.PagedList.PagedList<TradeMark>(lsTradeMark.OrderBy(x => x.Id), pageNumber, pageSize);

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
        [Route("Details")]
        [HttpGet("Details")]
        // GET: Admin/AdminTradeMarks/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.TradeMarks == null)
            {
                return NotFound();
            }

            var tradeMark = await _context.TradeMarks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tradeMark == null)
            {
                return NotFound();
            }

            return View(tradeMark);
        }
        [Route("")]
        [Route("Create")]
        [HttpGet("Create")]
        // GET: Admin/AdminTradeMarks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminTradeMarks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TradeMark tradeMark, IFormFile fileupload)
        {
            if (ModelState.IsValid)
            {
                // Xử lý giá trị trường EnteredDate
                /*  tradeMark.EnteredDate = DateTime.Now; */// hoặc bạn có thể sử dụng giá trị ngày từ biểu mẫu
                var fileName = Path.GetFileName(fileupload.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    fileupload.CopyTo(stream);
                }
                tradeMark.Logo = fileName; // Đường dẫn dựa trên cách bạn lưu hình ảnh trong thư mục wwwroot/uploads
                /*    // Xử lý giá trị trường Status
                    if (trade.Status == null)
                    {
                        product.Status = false; // giá trị mặc định
                    }*/
                _context.Add(tradeMark);
                _context.SaveChanges();
                return RedirectToAction("Index", "AdminTradeMarks");
            }

            /* ViewData["IdCategories"] = new SelectList(_context.Categories, "Id", "Names");
             ViewData["IdTradeMark"] = new SelectList(_context.TradeMarks, "Id", "Names");*/
            return View(tradeMark);
        }
        [Route("")]
        [Route("Edit")]
        [HttpGet("Edit")]
        // GET: Admin/AdminTradeMarks/Edit/5
        public IActionResult Edit(string id)
        {
            var tradeMark = _context.TradeMarks.Find(id);
            /*ViewData["IdCategories"] = new SelectList(_context.Categories, "Id", "Names", product.IdCategories);
            ViewData["IdTradeMark"] = new SelectList(_context.TradeMarks, "Id", "Names", product.IdTradeMark);
           */
            /*    if (product.IdCategories != null)
                {
                    ViewData["IdCategories"] = new SelectList(_context.Categories, "Id", "Names", product.IdCategories);
                }

                if (product.IdTradeMark != null)
                {
                    ViewData["IdTradeMark"] = new SelectList(_context.TradeMarks, "Id", "Names", product.IdTradeMark);
                }*/


            return View(tradeMark);
        }

        // POST: Admin/AdminTradeMarks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, TradeMark tradeMark, IFormFile fileupload)
        {
            if (id != tradeMark.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        fileupload.CopyTo(stream);
                    }
                    tradeMark.Logo = fileName;
                    // Xử lý giá trị trường Status

                    _context.Update(tradeMark);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TradeMarkExists(tradeMark.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
                return RedirectToAction("Index");
            }
            [Route("")]
            [Route("Delete")]
            [HttpGet("Delete")]
            // GET: Admin/AdminTradeMarks/Delete/5
            public IActionResult Delete(string id)
            {
                TempData["Message"] = "";
            var tradeMark = _context.TradeMarks.Find(id);

            if (tradeMark != null)
            {
                var productImages = _context.TradeMarks.Where(x => x.Id == id).ToList();

                if (productImages.Any()) _context.RemoveRange(productImages);
                _context.Remove(tradeMark);
                _context.SaveChanges();
                TempData["Message"] = "Sản phẩm đã được xoá";
            }
            else
            {
                TempData["Message"] = "Không tìm thấy sản phẩm để xoá";
            }

            return RedirectToAction("Index");
            /* var productImages = _context.TradeMarks.Where(x => x.Id == id).ToList();

             if (productImages.Any()) _context.RemoveRange(productImages);
             _context.Remove(_context.TradeMarks.Find(id));
             _context.SaveChanges();
             TempData["Message"] = "Sản phẩm đã được xoá";
             return RedirectToAction("Index");*/
        }

            // POST: Admin/AdminTradeMarks/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(string id)
            {
                if (_context.TradeMarks == null)
                {
                    return Problem("Entity set 'WebContext.TradeMarks'  is null.");
                }
                var tradeMark = await _context.TradeMarks.FindAsync(id);
                if (tradeMark != null)
                {
                    _context.TradeMarks.Remove(tradeMark);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool TradeMarkExists(string id)
            {
                return (_context.TradeMarks?.Any(e => e.Id == id)).GetValueOrDefault();
            }
        }
    }
