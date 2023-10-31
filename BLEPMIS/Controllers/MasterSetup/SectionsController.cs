using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBContext.Data;
using DAL.Models.Domain.MasterSetup;
using BAL.IRepository.MasterSetup.HR;

namespace BLEPMIS.Controllers.MasterSetup
{
    public class SectionsController : Controller
    {
        private readonly ISection _context;

        public SectionsController(ISection context)
        {
            _context = context;
        }

        // GET: Sections
        public async Task<IActionResult> Index()
        {
              return View(await _context.GetAll());
        }

        // GET: Sections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var section = await _context.GetById(id);
            if (section == null)
            {
                return NotFound();
            }

            return View(section);
        }

        // GET: Sections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SectionId,Name")] Section section)
        {
            if (ModelState.IsValid)
            {
                var result = _context.Count(section.Name);
                if (result > 0)
                {
                    ModelState.AddModelError(nameof(section.Name), "Name already exist!");
                    return BadRequest(ModelState);
                }
                _context.Insert(section);                
                return RedirectToAction(nameof(Index));
            }
            return View(section);
        }

        // GET: Sections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var section = await _context.GetById(id);
            if (section == null)
            {
                return NotFound();
            }
            return View(section);
        }

        // POST: Sections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SectionId,Name")] Section section)
        {
            if (id != section.SectionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = _context.Count(section.Name);
                    if (result > 0)
                    {
                        ModelState.AddModelError(nameof(section.Name), "Name already exist!");
                        return BadRequest(ModelState);
                    }
                    _context.Update(section);                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectionExists(section.SectionId))
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
            return View(section);
        }

        // GET: Sections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var section = await _context.GetById(id);
            if (section == null)
            {
                return NotFound();
            }

            return View(section);
        }

        // POST: Sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Section'  is null.");
            }
            var section = await _context.GetById(id);
            if (section != null)
            {
                _context.Remove(section);
            }
                        
            return RedirectToAction(nameof(Index));
        }

        private bool SectionExists(int id)
        {
          return _context.Exist(id);
        }
    }
}
