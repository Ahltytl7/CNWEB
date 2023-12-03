using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using X.PagedList.Mvc.Core;
using CNWEB.Models;
using X.PagedList;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;

namespace CNWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/AdminProducts")]
    [HasCredential(RoleCode = "product")]
    public class AdminProductsController : Controller
    {
        private readonly WebContext _context;
        /* private readonly IHostEnvironment _hostEnvironment;*/
      
        public AdminProductsController(WebContext context)
        {
            _context = context;
           /* _webHostEnvironment = hostEnvironment; */
        }

        // GET: Admin/AdminProducts
        [Route("")]
        [Route("Index")]
        [HttpGet("Index")]
        public IActionResult Index(int? page, string CatID)
        {
      
            var pageNumber = page ?? 1;
            var pageSize = 5;

            var lsProducts = from x in _context.Products select x;
         
            if (!String.IsNullOrEmpty(CatID))
            {

                lsProducts = lsProducts.Where(x =>
         x.Id.ToLower().Contains(CatID) || x.Name.ToLower().Contains(CatID) ||
           _context.Categories.Any(c => c.Id == x.IdCategories && c.Names.ToLower().Contains(CatID)));
  
                }
    
            var totalProducts = lsProducts.Count();
            ViewBag.totalProducts = totalProducts;
            ViewBag.totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            var pagedList = new X.PagedList.PagedList<Product>(lsProducts.OrderBy(x => x.Id), pageNumber, pageSize);
         
            var startRange = (pageNumber - 1) * pageSize + 1;
            var endRange = Math.Min(pageNumber * pageSize, totalProducts);

            ViewBag.CatIDValue = CatID;
            ViewBag.IdTradeMark = new SelectList(_context.TradeMarks.ToList(), "Id", "Names");
            ViewBag.IdCategories = new SelectList(_context.Categories.ToList(), "Id", "Names");
            ViewBag.TotalRecords = totalProducts;
        
            ViewBag.CurrentPage = pageNumber;
            ViewBag.StartRange = startRange;
            ViewBag.EndRange = endRange;
         

            return View(pagedList); // Trả về danh sách đã phân trang cho view

        }
     
     /*   public IActionResult SearchProduct (string txtCatID,string CatID)
        {

            var lsProducts = from x in _context.Products select x;
            if (!String.IsNullOrEmpty(txtCatID))
            {
                if (txtCatID == "1") // Tìm kiếm theo mã sản phẩm
                {
                    lsProducts = lsProducts.Where(x => x.Id.ToLower().Contains(CatID));
                }
                else if (txtCatID == "2") // Tìm kiếm theo tên sản phẩm
                {
                    lsProducts = lsProducts.Where(x => x.Name.ToLower().Contains(CatID));
                }
                else if (txtCatID == "3") // Tìm kiếm theo danh mục
                {
                    lsProducts = lsProducts.Where(x => x.IdCategories.ToLower().Contains(CatID));
                }
            }*/
        /*    if (!String.IsNullOrEmpty(CatID))
            {
                lsProducts = lsProducts.Where(x => x.Name.ToLower().Contains(CatID));

            }*/
     /*       return View("Index", lsProducts);
        }*/
        [Route("")]
        [Route("Details")]
        [HttpGet("Details")]
        [HasCredential(RoleCode = "view-product")]
        // GET: Admin/AdminProducts/Details
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
            
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["IdCategories"] = new SelectList(_context.Categories, "Id", "Names");
            ViewData["IdTradeMark"] = new SelectList(_context.TradeMarks, "Id", "Names");
        /*    var cate = _context.Categories.Where()*/
            return View(product);
        }
  
        [Route("")]
        [Route("Create")]
        [HttpGet("Create")]
        [HasCredential(RoleCode = "add-product")]
        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
     
            ViewData["IdCategories"] = new SelectList(_context.Categories, "Id", "Names");
            ViewData["IdTradeMark"] = new SelectList(_context.TradeMarks, "Id", "Names");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( Product product, IFormFile fileupload)
        {
    

            if (ModelState.IsValid)
            {
                // Xử lý giá trị trường EnteredDate
                product.EnteredDate = DateTime.Now; // hoặc bạn có thể sử dụng giá trị ngày từ biểu mẫu
                var fileName = Path.GetFileName(fileupload.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    fileupload.CopyTo(stream);
                }
                product.Image = fileName; // Đường dẫn dựa trên cách bạn lưu hình ảnh trong thư mục wwwroot/uploads
                // Xử lý giá trị trường Status
                if (product.Status == null)
                {
                    product.Status = false; // giá trị mặc định
                }
                _context.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index", "AdminProducts");
            }

            ViewData["IdCategories"] = new SelectList(_context.Categories, "Id", "Names");
            ViewData["IdTradeMark"] = new SelectList(_context.TradeMarks, "Id", "Names");
            return View(product);
        }
        [Route("")]
        [Route("Edit")]
        [HttpGet("Edit")]
        [HasCredential(RoleCode = "edit-product")]
        // GET: Admin/AdminProducts/Edit/5
        public IActionResult Edit(string id)
        {
         
            var product = _context.Products.Find(id);
       
            ViewBag.IdTradeMark = new SelectList(_context.TradeMarks.ToList(), "Id", "Names");
            ViewBag.IdCategories = new SelectList(_context.Categories.ToList(), "Id", "Names");

            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Product product, IFormFile fileupload)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý giá trị trường EnteredDate
                    product.EnteredDate = DateTime.Now; // hoặc bạn có thể sử dụng giá trị ngày từ biểu mẫu
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        fileupload.CopyTo(stream);
                    }
                    product.Image = fileName;
                    // Xử lý giá trị trường Status
                    if (product.Status == null)
                    {
                        product.Status = false; // giá trị mặc định
                    }
                    _context.Update(product);
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "Id", "Names", product.IdCategories);
            ViewData["IdTradeMark"] = new SelectList(_context.TradeMarks, "Id", "Id", product.IdTradeMark);
            return View(product);
        }
        [Route("")]
        [Route("Delete")]
        [HttpGet("Delete")]
        [HasCredential(RoleCode = "delete-product")]
        // GET: Admin/AdminProducts/Delete/5
        public IActionResult Delete(string id)
        {
            /*    if (id == null || _context.Products == null)
                {
                    return NotFound();
                }
    */
            var tradeMark = _context.Products.Find(id);

            if (tradeMark != null)
            {
                var productImages = _context.Products.Where(x => x.Id == id).ToList();

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
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'WebContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
