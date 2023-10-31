using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.ToolApp;
using DBContext.Data;

namespace BLEPMIS.Controllers.ToolApp
{
    public class ToolControlsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToolControlsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ToolControls
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ToolControl.Include(t => t.Control).Include(t => t.Tool);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ToolControls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ToolControl == null)
            {
                return NotFound();
            }

            var toolControl = await _context.ToolControl
                .Include(t => t.Control)
                .Include(t => t.Tool)
                .FirstOrDefaultAsync(m => m.ToolControlId == id);
            if (toolControl == null)
            {
                return NotFound();
            }

            return View(toolControl);
        }

        // GET: ToolControls/Create
        public IActionResult Create()
        {
            ViewData["ControlId"] = new SelectList(_context.Control, "ControlId", "Name");
            ViewData["ToolId"] = new SelectList(_context.Tool, "ToolId", "Description");
            return View();
        }

        // POST: ToolControls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ToolControlId,ToolId,ControlId,Name,IsActive,OrderNo,Value")] ToolControl toolControl)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toolControl);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ControlId"] = new SelectList(_context.Control, "ControlId", "Name", toolControl.ControlId);
            ViewData["ToolId"] = new SelectList(_context.Tool, "ToolId", "Description", toolControl.ToolId);
            return View(toolControl);
        }

        // GET: ToolControls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ToolControl == null)
            {
                return NotFound();
            }

            var toolControl = await _context.ToolControl.FindAsync(id);
            if (toolControl == null)
            {
                return NotFound();
            }
            ViewData["ControlId"] = new SelectList(_context.Control, "ControlId", "Name", toolControl.ControlId);
            ViewData["ToolId"] = new SelectList(_context.Tool, "ToolId", "Description", toolControl.ToolId);
            return View(toolControl);
        }

        // POST: ToolControls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ToolControlId,ToolId,ControlId,Name,IsActive,OrderNo,Value")] ToolControl toolControl)
        {
            if (id != toolControl.ToolControlId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toolControl);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToolControlExists(toolControl.ToolControlId))
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
            ViewData["ControlId"] = new SelectList(_context.Control, "ControlId", "Name", toolControl.ControlId);
            ViewData["ToolId"] = new SelectList(_context.Tool, "ToolId", "Description", toolControl.ToolId);
            return View(toolControl);
        }

        // GET: ToolControls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ToolControl == null)
            {
                return NotFound();
            }

            var toolControl = await _context.ToolControl
                .Include(t => t.Control)
                .Include(t => t.Tool)
                .FirstOrDefaultAsync(m => m.ToolControlId == id);
            if (toolControl == null)
            {
                return NotFound();
            }

            return View(toolControl);
        }

        // POST: ToolControls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ToolControl == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ToolControl'  is null.");
            }
            var toolControl = await _context.ToolControl.FindAsync(id);
            if (toolControl != null)
            {
                _context.ToolControl.Remove(toolControl);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("AddControl", "Tools", new {id = toolControl.ToolId});
        }

        private bool ToolControlExists(int id)
        {
          return _context.ToolControl.Any(e => e.ToolControlId == id);
        }
    }
}
