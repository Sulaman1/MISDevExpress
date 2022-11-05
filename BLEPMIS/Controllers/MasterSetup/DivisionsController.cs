using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using DAL.Models.Domain.MasterSetup;
using DBContext.Data;
using BAL.IRepository.MasterSetup;

namespace BLEPMIS.Controllers.MasterSetup
{    
    public class DivisionsController : Controller
    {
        private readonly IDivision _context;

        public DivisionsController(IDivision context)
        {
            _context = context;
        }

        // GET: Divisions
        public async Task<IActionResult> Index()
        {            
            return View(await _context.GetAll());
        }

        // GET: Divisions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.GetById(id);
            if (division == null)
            {
                return NotFound();
            }

            return View(division);
        }

        // GET: Divisions/Create        
        public IActionResult Create()
        {
            ViewData["ProvienceId"] = new SelectList(_context.GetAllProvience(), "ProvienceId", "Name");
            return View();
        }

        // POST: Divisions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DivisionId,Name,Code,Description,ProvienceId")] Division division)
        {
            if (ModelState.IsValid)
            {
                var IsExist = _context.Count(division.Name);
                if (IsExist > 0)
                {
                    ModelState.AddModelError(nameof(division.Name), "Already exist with same name!");
                    return View(division);
                }
                _context.Insert(division);
                _context.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProvienceId"] = new SelectList(_context.GetAllProvience(), "ProvienceId", "Name", division.ProvienceId);
            return View(division);
        }

        // GET: Divisions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.GetById(id);
            if (division == null)
            {
                return NotFound();
            }
            ViewData["ProvienceId"] = new SelectList(_context.GetAllProvience(), "ProvienceId", "Name", division.ProvienceId);
            return View(division);
        }

        // POST: Divisions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DivisionId,Name,Code,Description,ProvienceId")] Division division)
        {
            if (id != division.DivisionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var IsExist = _context.Count(division.Name);
                    if (IsExist > 0)
                    {
                        ModelState.AddModelError(nameof(division.Name), "Already exist with same name!");
                        return BadRequest(ModelState);
                    }
                    _context.Update(division);
                    _context.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DivisionExists(division.DivisionId))
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
            ViewData["ProvienceId"] = new SelectList(_context.GetAllProvience(), "ProvienceId", "Name", division.ProvienceId);
            return View(division);
        }

        // GET: Divisions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.GetById(id);
            if (division == null)
            {
                return NotFound();
            }

            return View(division);
        }

        // POST: Divisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var division = await _context.GetById(id);
            _context.Remove(division);
            _context.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool DivisionExists(int id)
        {
            return _context.Exist(id);
        }
    }
}
