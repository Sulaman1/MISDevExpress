using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.MasterSetup;
using BAL.IRepository.MasterSetup;

namespace BLEPMIS.Controllers.MasterSetup
{
    public class UnionCouncilsController : Controller
    {
        private readonly IUnionCouncil _context;

        public UnionCouncilsController(IUnionCouncil context)
        {
            _context = context;
        }

        // GET: UnionCouncils
        public async Task<IActionResult> Index()
        {                                    
            return View(await _context.GetAll());
        }       
        // GET: UnionCouncils/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unionCouncil = await _context.GetById(id);
            if (unionCouncil == null)
            {
                return NotFound();
            }

            return View(unionCouncil);
        }

        // GET: UnionCouncils/Create
        public IActionResult Create()
        {                       
            ViewData["DistrictId"] = new SelectList(_context.GetAllDistrict().Result.Where(a=>a.DistrictId > 1), "DistrictId", "Name");
            return View();
        }

        // POST: UnionCouncils/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UnionCouncilId,UnionCouncilName,TehsilId")] UnionCouncil unionCouncil, int DistrictId)
        {
            if (ModelState.IsValid)
            {                
                var result = _context.Count(unionCouncil.UnionCouncilName);                
                if (result > 0)
                {
                    ModelState.AddModelError(nameof(unionCouncil.UnionCouncilName), "UC already exist!");
                    return View(unionCouncil);
                }
                _context.Insert(unionCouncil);                
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_context.GetAllDistrict().Result.Where(a=>a.DistrictId > 1), "DistrictId", "Name", DistrictId);
            return View(unionCouncil);
        }

        // GET: UnionCouncils/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unionCouncil = await _context.GetById(id);
            if (unionCouncil == null)
            {
                return NotFound();
            }
            ViewData["DistrictId"] = new SelectList(_context.GetAllDistrict().Result.Where(a=>a.DistrictId > 1), "DistrictId", "Name", unionCouncil.TehsilId);
            return View(unionCouncil);
        }

        // POST: UnionCouncils/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UnionCouncilId,UnionCouncilName,TehsilId")] UnionCouncil unionCouncil, int DistrictId)
        {
            if (id != unionCouncil.UnionCouncilId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = _context.Count(unionCouncil.UnionCouncilName);
                if (result > 0)
                {
                    ModelState.AddModelError(nameof(unionCouncil.UnionCouncilName), "UC already exist!");
                    return View(unionCouncil);
                }
                try
                {
                    _context.Update(unionCouncil);                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnionCouncilExists(unionCouncil.UnionCouncilId))
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
            ViewData["DistrictId"] = new SelectList(_context.GetAllDistrict().Result.Where(a=>a.DistrictId > 1), "DistrictId", "Name", DistrictId);
            return View(unionCouncil);
        }

        // GET: UnionCouncils/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unionCouncil = await _context.GetById(id);
            if (unionCouncil == null)
            {
                return NotFound();
            }

            return View(unionCouncil);
        }

        // POST: UnionCouncils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unionCouncil = await _context.GetById(id);
            _context.Remove(unionCouncil);            
            return RedirectToAction(nameof(Index));
        }

        private bool UnionCouncilExists(int id)
        {
            return _context.Exist(id);
        }
    }
}
