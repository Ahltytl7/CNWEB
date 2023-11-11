using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNWEB.Models;

namespace CNWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/AdminRoles")]
    public class AdminRolesController : Controller
    {
        private readonly WebContext _context;

        public AdminRolesController(WebContext context)
        {
            _context = context;
        }
        [Route("")]
        [Route("Index")]
        // GET: Admin/AdminRoles
        public async Task<IActionResult> Index()
        {
              return _context.AppRoles != null ? 
                          View(await _context.AppRoles.ToListAsync()) :
                          Problem("Entity set 'WebContext.AppRoles'  is null.");
        }

        // GET: Admin/AdminRoles/Details/5
        [Route("")]
        [Route("Details")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.AppRoles == null)
            {
                return NotFound();
            }

            var appRole = await _context.AppRoles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appRole == null)
            {
                return NotFound();
            }

            return View(appRole);
        }

        // GET: Admin/AdminRoles/Create
        [Route("")]
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] AppRole appRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appRole);
        }

        // GET: Admin/AdminRoles/Edit/5
        [Route("")]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.AppRoles == null)
            {
                return NotFound();
            }

            var appRole = await _context.AppRoles.FindAsync(id);
            if (appRole == null)
            {
                return NotFound();
            }
            return View(appRole);
        }

        // POST: Admin/AdminRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] AppRole appRole)
        {
            if (id != appRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppRoleExists(appRole.Id))
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
            return View(appRole);
        }

        // GET: Admin/AdminRoles/Delete/5
        [Route("")]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.AppRoles == null)
            {
                return NotFound();
            }

            var appRole = await _context.AppRoles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appRole == null)
            {
                return NotFound();
            }

            return View(appRole);
        }

        // POST: Admin/AdminRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.AppRoles == null)
            {
                return Problem("Entity set 'WebContext.AppRoles'  is null.");
            }
            var appRole = await _context.AppRoles.FindAsync(id);
            if (appRole != null)
            {
                _context.AppRoles.Remove(appRole);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppRoleExists(string id)
        {
          return (_context.AppRoles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
