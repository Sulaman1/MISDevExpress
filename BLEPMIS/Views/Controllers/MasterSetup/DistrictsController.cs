using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.MasterSetup;
using BAL.IRepository.MasterSetup;

namespace BLEPMIS.Controllers.MasterSetup
{
    public class DistrictsController : Controller
    {
        private readonly IDistrict _context;        

        public DistrictsController(IDistrict context)
        {
            _context = context;            
        }

        // GET: Districts
        public async Task<IActionResult> Index()
        {            
            return View(await _context.GetAll());
        }

        // GET: Districts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.GetById(id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // GET: Districts/Create
        public IActionResult Create()
        {
            ViewData["DivisionId"] = new SelectList(_context.GetAllDivision(), "DivisionId", "Name");
            return View();
        }

        // POST: Districts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DistrictId,Name,Code,Description,DivisionId")] District district)
        {
            if (ModelState.IsValid)
            {
                var IsExist = _context.Count(district.Name);
                if (IsExist > 0)
                {
                    ModelState.AddModelError(nameof(district.Name), "Already exist with same name!");
                    return View(district);
                }
                _context.Insert(district);                
                //await _context.SaveChangesAsync();                
                //await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);                
                return RedirectToAction(nameof(Index));
            }
            ViewData["DivisionId"] = new SelectList(_context.GetAllDivision(), "DivisionId", "Name", district.DivisionId);
            return View(district);
        }

        // GET: Districts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.GetById(id);
            if (district == null)
            {
                return NotFound();
            }
            ViewData["DivisionId"] = new SelectList(_context.GetAllDivision(), "DivisionId", "Name", district.DivisionId);
            return View(district);
        }

        // POST: Districts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DistrictId,Name,Code,Description,DivisionId")] District district)
        {
            if (id != district.DistrictId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var IsExist = _context.Count(district.Name);
                    if (IsExist > 0)
                    {
                        ModelState.AddModelError(nameof(district.Name), "Already exist with same name!");
                        return View(district);
                    }
                    //_context.Entry(district).CurrentValues.SetValues(district);
                    _context.Update(district);                    
                    //await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                    /*_context.Update(district);
                    await _context.SaveChangesAsync();*/
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistrictExists(district.DistrictId))
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
            ViewData["DivisionId"] = new SelectList(_context.GetAllDivision(), "DivisionId", "Name", district.DivisionId);
            return View(district);
        }

        // GET: Districts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.GetById(id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // POST: Districts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var district = await _context.GetById(id);
            _context.Remove(district);            
            //await _context.SaveChangesAsync();
            //await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            return RedirectToAction(nameof(Index));
        }

        private bool DistrictExists(int id)
        {
            return _context.Exist(id);
        }
    }
}
