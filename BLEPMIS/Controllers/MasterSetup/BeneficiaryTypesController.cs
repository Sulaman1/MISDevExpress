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
using BAL.IRepository.MasterSetup.CD;

namespace BLEPMIS.Controllers.MasterSetup
{
    public class BeneficiaryTypesController : Controller
    {
        private readonly IBeneficiaryType _context;

        public BeneficiaryTypesController(IBeneficiaryType context)
        {
            _context = context;
        }

        // GET: BeneficiaryTypes
        public async Task<IActionResult> Index()
        {
              return View(await _context.GetAll());
        }

        // GET: BeneficiaryTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var beneficiaryType = await _context.GetById(id);
            if (beneficiaryType == null)
            {
                return NotFound();
            }

            return View(beneficiaryType);
        }

        // GET: BeneficiaryTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BeneficiaryTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BeneficiaryTypeId,BeneficiaryTypeName")] BeneficiaryType beneficiaryType)
        {
            if (ModelState.IsValid)
            {
                _context.Insert(beneficiaryType);
                _context.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(beneficiaryType);
        }

        // GET: BeneficiaryTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var beneficiaryType = await _context.GetById(id);
            if (beneficiaryType == null)
            {
                return NotFound();
            }
            return View(beneficiaryType);
        }

        // POST: BeneficiaryTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BeneficiaryTypeId,BeneficiaryTypeName")] BeneficiaryType beneficiaryType)
        {
            if (id != beneficiaryType.BeneficiaryTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(beneficiaryType);
                    _context.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BeneficiaryTypeExists(beneficiaryType.BeneficiaryTypeId))
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
            return View(beneficiaryType);
        }

        // GET: BeneficiaryTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var beneficiaryType = await _context.GetById(id);
            if (beneficiaryType == null)
            {
                return NotFound();
            }

            return View(beneficiaryType);
        }

        // POST: BeneficiaryTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BeneficiaryType'  is null.");
            }
            var beneficiaryType = await _context.GetById(id);
            if (beneficiaryType != null)
            {
                _context.Remove(beneficiaryType);
            }            
            _context.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool BeneficiaryTypeExists(int id)
        {
          return _context.Exist(id);
        }
    }
}
