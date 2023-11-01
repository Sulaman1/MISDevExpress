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
    public class BSFGovtPackagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BSFGovtPackagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BSFGovtPackages
        public async Task<IActionResult> Index()
        {
              return View(await _context.BSFGovtPackage.ToListAsync());
        }

        // GET: BSFGovtPackages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BSFGovtPackage == null)
            {
                return NotFound();
            }

            var BSFGovtPackage = await _context.BSFGovtPackage
                .FirstOrDefaultAsync(m => m.BSFGovtPackageId == id);
            if (BSFGovtPackage == null)
            {
                return NotFound();
            }

            return View(BSFGovtPackage);
        }

        // GET: BSFGovtPackages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BSFGovtPackages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BSFGovtPackageId,PackageName,Description")] BSFGovtPackage BSFGovtPackage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(BSFGovtPackage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(BSFGovtPackage);
        }

        // GET: BSFGovtPackages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BSFGovtPackage == null)
            {
                return NotFound();
            }

            var BSFGovtPackage = await _context.BSFGovtPackage.FindAsync(id);
            if (BSFGovtPackage == null)
            {
                return NotFound();
            }
            return View(BSFGovtPackage);
        }

        // POST: BSFGovtPackages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BSFGovtPackageId,PackageName,Description")] BSFGovtPackage BSFGovtPackage)
        {
            if (id != BSFGovtPackage.BSFGovtPackageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(BSFGovtPackage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BSFGovtPackageExists(BSFGovtPackage.BSFGovtPackageId))
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
            return View(BSFGovtPackage);
        }

        // GET: BSFGovtPackages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BSFGovtPackage == null)
            {
                return NotFound();
            }

            var BSFGovtPackage = await _context.BSFGovtPackage
                .FirstOrDefaultAsync(m => m.BSFGovtPackageId == id);
            if (BSFGovtPackage == null)
            {
                return NotFound();
            }

            return View(BSFGovtPackage);
        }

        // POST: BSFGovtPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BSFGovtPackage == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BSFGovtPackage'  is null.");
            }
            var BSFGovtPackage = await _context.BSFGovtPackage.FindAsync(id);
            if (BSFGovtPackage != null)
            {
                _context.BSFGovtPackage.Remove(BSFGovtPackage);
                await _context.SaveChangesAsync();
            }
                        
            return RedirectToAction(nameof(Index));
        }

        private bool BSFGovtPackageExists(int id)
        {
          return _context.BSFGovtPackage.Any(e => e.BSFGovtPackageId == id);
        }
    }
}
