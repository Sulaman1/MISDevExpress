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
    public class ToolModulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToolModulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ToolModules
        public async Task<IActionResult> Index()
        {
              return View(await _context.ToolModule.ToListAsync());
        }

        // GET: ToolModules/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ToolModule == null)
            {
                return NotFound();
            }

            var toolModule = await _context.ToolModule
                .FirstOrDefaultAsync(m => m.ToolModuleName == id);
            if (toolModule == null)
            {
                return NotFound();
            }

            return View(toolModule);
        }

        // GET: ToolModules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToolModules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ToolModuleName")] ToolModule toolModule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toolModule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toolModule);
        }

        // GET: ToolModules/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ToolModule == null)
            {
                return NotFound();
            }

            var toolModule = await _context.ToolModule.FindAsync(id);
            if (toolModule == null)
            {
                return NotFound();
            }
            return View(toolModule);
        }

        // POST: ToolModules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ToolModuleName")] ToolModule toolModule)
        {
            if (id != toolModule.ToolModuleName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toolModule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToolModuleExists(toolModule.ToolModuleName))
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
            return View(toolModule);
        }

        // GET: ToolModules/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ToolModule == null)
            {
                return NotFound();
            }

            var toolModule = await _context.ToolModule
                .FirstOrDefaultAsync(m => m.ToolModuleName == id);
            if (toolModule == null)
            {
                return NotFound();
            }

            return View(toolModule);
        }

        // POST: ToolModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ToolModule == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ToolModule'  is null.");
            }
            var toolModule = await _context.ToolModule.FindAsync(id);
            if (toolModule != null)
            {
                _context.ToolModule.Remove(toolModule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToolModuleExists(string id)
        {
          return _context.ToolModule.Any(e => e.ToolModuleName == id);
        }
    }
}
