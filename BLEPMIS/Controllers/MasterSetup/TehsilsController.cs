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
    public class TehsilsController : Controller
    {
        private readonly ITehsil _context;

        public TehsilsController(ITehsil context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {                     
            return View(await _context.GetAll());
        }
        // GET: Tehsils           
        // GET: Tehsils/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tehsil = await _context.GetById(id);
            if (tehsil == null)
            {
                return NotFound();
            }

            return View(tehsil);
        }

        // GET: Tehsils/Create
        public IActionResult Create()
        {          
            ViewData["DistrictId"] = new SelectList(_context.GetAllDistrict().Result.Where(a=>a.DistrictId > 1), "DistrictId", "Name");
            return View();
        }

        // POST: Tehsils/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TehsilId,TehsilName,DistrictId")] Tehsil tehsil)
        {
            if (ModelState.IsValid)
            {
                var result = _context.Count(tehsil.TehsilName);
                if (result > 0)
                {
                    ModelState.AddModelError(nameof(tehsil.TehsilName), "Tehsil already exist!");
                    return View(tehsil);
                }               
                _context.Insert(tehsil);
                _context.Save();                                          
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_context.GetAllDistrict().Result.Where(a => a.DistrictId > 1), "DistrictId", "Name", tehsil.DistrictId);
            return View(tehsil);
        }

        // GET: Tehsils/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tehsil = await _context.GetById(id);
            if (tehsil == null)
            {
                return NotFound();
            }
            ViewData["DistrictId"] = new SelectList(_context.GetAllDistrict().Result.Where(a => a.DistrictId > 1), "DistrictId", "Name", tehsil.DistrictId);
            return View(tehsil);
        }

        // POST: Tehsils/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TehsilId,TehsilName,DistrictId")] Tehsil tehsil)
        {
            if (id != tehsil.TehsilId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = _context.Count(tehsil.TehsilName);
                if (result > 0)
                {
                    ModelState.AddModelError(nameof(tehsil.TehsilName), "Tehsil already exist!");
                    return View(tehsil);
                }
                try
                {
                    _context.Update(tehsil);
                    _context.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TehsilExists(tehsil.TehsilId))
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
            ViewData["DistrictId"] = new SelectList(_context.GetAllDistrict().Result.Where(a => a.DistrictId > 1), "DistrictId", "Name", tehsil.DistrictId);
            return View(tehsil);
        }

        // GET: Tehsils/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tehsil = await _context.GetById(id);
            if (tehsil == null)
            {
                return NotFound();
            }

            return View(tehsil);
        }

        // POST: Tehsils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tehsil = await _context.GetById(id);
            _context.Remove(tehsil);
            _context.Save();
            return RedirectToAction(nameof(Index));
        }
        private bool TehsilExists(int id)
        {
            return _context.Exist(id);
        }
    }
}
