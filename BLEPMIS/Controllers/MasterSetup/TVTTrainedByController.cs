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
    public class TVTTrainedByController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TVTTrainedByController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TVTTrainedBys
        public async Task<IActionResult> Index()
        {
              return View(await _context.TVTTrainedBy.ToListAsync());
        }

        // GET: TVTTrainedBys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TVTTrainedBy == null)
            {
                return NotFound();
            }

            var tVTTrainedBy = await _context.TVTTrainedBy
                .FirstOrDefaultAsync(m => m.TVTTrainedById == id);
            if (tVTTrainedBy == null)
            {
                return NotFound();
            }

            return View(tVTTrainedBy);
        }

        // GET: TVTTrainedBys/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TVTTrainedBys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TVTTrainedBy tVTTrainedBy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tVTTrainedBy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tVTTrainedBy);
        }

        // GET: TVTTrainedBys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TVTTrainedBy == null)
            {
                return NotFound();
            }

            var tVTTrainedBy = await _context.TVTTrainedBy.FindAsync(id);
            if (tVTTrainedBy == null)
            {
                return NotFound();
            }
            return View(tVTTrainedBy);
        }

        // POST: TVTTrainedBys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,TVTTrainedBy tVTTrainedBy)
        {
            if (id != tVTTrainedBy.TVTTrainedById)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tVTTrainedBy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TVTTrainedByExists(tVTTrainedBy.TVTTrainedById))
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
            return View(tVTTrainedBy);
        }

        // GET: TVTTrainedBys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TVTTrainedBy == null)
            {
                return NotFound();
            }

            var tVTTrainedBy = await _context.TVTTrainedBy
                .FirstOrDefaultAsync(m => m.TVTTrainedById == id);
            if (tVTTrainedBy == null)
            {
                return NotFound();
            }

            return View(tVTTrainedBy);
        }

        // POST: TVTTrainedBys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TVTTrainedBy == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TVTTrainedBy'  is null.");
            }
            var tVTTrainedBy = await _context.TVTTrainedBy.FindAsync(id);
            if (tVTTrainedBy != null)
            {
                _context.TVTTrainedBy.Remove(tVTTrainedBy);
                await _context.SaveChangesAsync();
            }
                        
            return RedirectToAction(nameof(Index));
        }

        private bool TVTTrainedByExists(int id)
        {
          return _context.TVTTrainedBy.Any(e => e.TVTTrainedById == id);
        }
    }
}
