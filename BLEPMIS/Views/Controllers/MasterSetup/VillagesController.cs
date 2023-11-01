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
    public class VillagesController : Controller
    {
        private readonly IVillage _context;

        public VillagesController(IVillage context)
        {
            _context = context;
        }

        // GET: Villages
        public async Task<IActionResult> Index()
        {                                    
            return View(await _context.GetAll());
        }       
        // GET: Villages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var village = await _context.GetById(id);
            if (village == null)
            {
                return NotFound();
            }

            return View(village);
        }

        // GET: Villages/Create
        public async Task<IActionResult> Create()
        {                       

            ViewData["TehsilId"] = new SelectList(await _context.GetAllTehsil(User), "TehsilId", "TehsilName");
            return View();
        }

        // POST: Villages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Village village, int TehsilId)
        {
            if (ModelState.IsValid)
            {                
                var result = _context.Count(village.Name);                
                if (result > 0)
                {
                    ModelState.AddModelError(nameof(village.Name), "Village already exist!");
                    return View(village);
                }
                _context.Insert(village);                
                return RedirectToAction(nameof(Index));
            }
            ViewData["TehsilId"] = new SelectList(await _context.GetAllTehsil(User), "TehsilId", "TehsilName", TehsilId);
            return View(village);
        }

        // GET: Villages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var village = await _context.GetById(id);
            if (village == null)
            {
                return NotFound();
            }
            ViewData["UnionCouncilId"] = new SelectList(await _context.GetAllTehsil(User), "UnionCouncilId", "UnionCouncilName", village.UnionCouncilId);
            return View(village);
        }

        // POST: Villages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Village village, int TehsilId)
        {
            if (id != village.VillageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = _context.Count(village.Name);
                if (result > 0)
                {
                    ModelState.AddModelError(nameof(village.Name), "Village already exist!");
                    return View(village);
                }
                try
                {
                    _context.Update(village);                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VillageExists(village.VillageId))
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
            ViewData["TehsilId"] = new SelectList(await _context.GetAllTehsil(User), "TehsilId", "TehsilName", TehsilId);
            return View(village);
        }

        // GET: Villages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var village = await _context.GetById(id);
            if (village == null)
            {
                return NotFound();
            }

            return View(village);
        }

        // POST: Villages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var village = await _context.GetById(id);
            _context.Remove(village);                      
            return RedirectToAction(nameof(Index));
        }

        private bool VillageExists(int id)
        {
            return _context.Exist(id);
        }
    }
}
