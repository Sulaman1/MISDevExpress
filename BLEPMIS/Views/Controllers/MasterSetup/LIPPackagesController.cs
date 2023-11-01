using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.MasterSetup;
using DBContext.Data;

namespace BLEPMIS.Controllers.MasterSetup
{
    public class LIPPackagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LIPPackagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LIPPackages
        public async Task<IActionResult> Index()
        {
              return View(await _context.LIPPackage.ToListAsync());
        }

        // GET: LIPPackages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LIPPackage == null)
            {
                return NotFound();
            }

            var lIPPackage = await _context.LIPPackage
                .FirstOrDefaultAsync(m => m.LIPPackageId == id);
            if (lIPPackage == null)
            {
                return NotFound();
            }

            return View(lIPPackage);
        }

        // GET: LIPPackages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LIPPackages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LIPPackageId,PackageName,PackagePrice,Description")] LIPPackage lIPPackage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lIPPackage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lIPPackage);
        }

        // GET: LIPPackages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LIPPackage == null)
            {
                return NotFound();
            }

            var lIPPackage = await _context.LIPPackage.FindAsync(id);
            if (lIPPackage == null)
            {
                return NotFound();
            }
            return View(lIPPackage);
        }

        // POST: LIPPackages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LIPPackageId,PackageName,PackagePrice,Description")] LIPPackage lIPPackage)
        {
            if (id != lIPPackage.LIPPackageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lIPPackage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LIPPackageExists(lIPPackage.LIPPackageId))
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
            return View(lIPPackage);
        }

        // GET: LIPPackages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LIPPackage == null)
            {
                return NotFound();
            }

            var lIPPackage = await _context.LIPPackage
                .FirstOrDefaultAsync(m => m.LIPPackageId == id);
            if (lIPPackage == null)
            {
                return NotFound();
            }

            return View(lIPPackage);
        }

        // POST: LIPPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LIPPackage == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LIPPackage'  is null.");
            }
            var lIPPackage = await _context.LIPPackage.FindAsync(id);
            if (lIPPackage != null)
            {
                _context.LIPPackage.Remove(lIPPackage);
                await _context.SaveChangesAsync();
            }
                        
            return RedirectToAction(nameof(Index));
        }

        private bool LIPPackageExists(int id)
        {
          return _context.LIPPackage.Any(e => e.LIPPackageId == id);
        }
    }
}
