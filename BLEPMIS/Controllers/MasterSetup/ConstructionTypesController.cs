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
    public class ConstructionTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConstructionTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ConstructionTypes
        public async Task<IActionResult> Index()
        {
              return View(await _context.ConstructionType.ToListAsync());
        }

        // GET: ConstructionTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ConstructionType == null)
            {
                return NotFound();
            }

            var ConstructionType = await _context.ConstructionType
                .FirstOrDefaultAsync(m => m.ConstructionTypeId == id);
            if (ConstructionType == null)
            {
                return NotFound();
            }

            return View(ConstructionType);
        }

        // GET: ConstructionTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConstructionTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConstructionTypeId,Name")] ConstructionType ConstructionType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ConstructionType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ConstructionType);
        }

        // GET: ConstructionTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ConstructionType == null)
            {
                return NotFound();
            }

            var ConstructionType = await _context.ConstructionType.FindAsync(id);
            if (ConstructionType == null)
            {
                return NotFound();
            }
            return View(ConstructionType);
        }

        // POST: ConstructionTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConstructionTypeId,Name")] ConstructionType ConstructionType)
        {
            if (id != ConstructionType.ConstructionTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ConstructionType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConstructionTypeExists(ConstructionType.ConstructionTypeId))
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
            return View(ConstructionType);
        }

        // GET: ConstructionTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ConstructionType == null)
            {
                return NotFound();
            }

            var ConstructionType = await _context.ConstructionType
                .FirstOrDefaultAsync(m => m.ConstructionTypeId == id);
            if (ConstructionType == null)
            {
                return NotFound();
            }

            return View(ConstructionType);
        }

        // POST: ConstructionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ConstructionType == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ConstructionType'  is null.");
            }
            var ConstructionType = await _context.ConstructionType.FindAsync(id);
            if (ConstructionType != null)
            {
                _context.ConstructionType.Remove(ConstructionType);
                await _context.SaveChangesAsync();
            }
                        
            return RedirectToAction(nameof(Index));
        }

        private bool ConstructionTypeExists(int id)
        {
          return _context.ConstructionType.Any(e => e.ConstructionTypeId == id);
        }
    }
}
