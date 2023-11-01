using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBContext.Data;
using DAL.Models.Domain.MasterSetup;
using BAL.IRepository.MasterSetup;

namespace BLEPMIS.Controllers.MasterSetup
{
    public class GeneralBusinessIdeasController : Controller
    {
        private readonly IGeneralBusinessIdea _context;

        public GeneralBusinessIdeasController(IGeneralBusinessIdea context)
        {
            _context = context;
        }

        // GET: GeneralBusinessIdeas
        public async Task<IActionResult> Index()
        {
              return View(await _context.GetAll());
        }

        // GET: GeneralBusinessIdeas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var generalBusinessIdea = await _context.GetById(id);
            if (generalBusinessIdea == null)
            {
                return NotFound();
            }

            return View(generalBusinessIdea);
        }

        // GET: GeneralBusinessIdeas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GeneralBusinessIdeas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GeneralBusinessIdeaId,GeneralBusinessIdeaName")] GeneralBusinessIdea generalBusinessIdea)
        {
            if (ModelState.IsValid)
            {
                _context.Insert(generalBusinessIdea);                
                return RedirectToAction(nameof(Index));
            }
            return View(generalBusinessIdea);
        }

        // GET: GeneralBusinessIdeas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var generalBusinessIdea = await _context.GetById(id);
            if (generalBusinessIdea == null)
            {
                return NotFound();
            }
            return View(generalBusinessIdea);
        }

        // POST: GeneralBusinessIdeas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GeneralBusinessIdeaId,GeneralBusinessIdeaName")] GeneralBusinessIdea generalBusinessIdea)
        {
            if (id != generalBusinessIdea.GeneralBusinessIdeaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(generalBusinessIdea);                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeneralBusinessIdeaExists(generalBusinessIdea.GeneralBusinessIdeaId))
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
            return View(generalBusinessIdea);
        }

        // GET: GeneralBusinessIdeas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var generalBusinessIdea = await _context.GetById(id);
            if (generalBusinessIdea == null)
            {
                return NotFound();
            }

            return View(generalBusinessIdea);
        }

        // POST: GeneralBusinessIdeas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GeneralBusinessIdea'  is null.");
            }
            var generalBusinessIdea = await _context.GetById(id);
            if (generalBusinessIdea != null)
            {
                _context.Remove(generalBusinessIdea);
            }            
            return RedirectToAction(nameof(Index));
        }

        private bool GeneralBusinessIdeaExists(int id)
        {
          return _context.Exist(id);
        }
    }
}
