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
    public class EmployeeContractsController : Controller
    {
        private readonly IEmployeeContract _context;

        public EmployeeContractsController(IEmployeeContract context)
        {
            _context = context;
        }

        // GET: EmployeeContracts
        public async Task<IActionResult> Index()
        {            
            return View(await _context.GetAll());
        }

        // GET: EmployeeContracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var employeeContract = await _context
                .GetById(id);
            if (employeeContract == null)
            {
                return NotFound();
            }

            return View(employeeContract);
        }

        // GET: EmployeeContracts/Create
        public IActionResult Create()
        {
            //ViewData["EmployeeId"] = new SelectList(_context.GetAll(), "EmployeeId", "EmployeeId");
            return View();
        }

        // POST: EmployeeContracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeContractId,EmployeeContractPath,StartDate,EndDate,EmployeeId")] EmployeeContract employeeContract)
        {
            if (ModelState.IsValid)
            {
                _context.Insert(employeeContract);
                _context.Save();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId", employeeContract.EmployeeId);
            return View(employeeContract);
        }

        // GET: EmployeeContracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var employeeContract = await _context.GetById(id);
            if (employeeContract == null)
            {
                return NotFound();
            }
            //ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId", employeeContract.EmployeeId);
            return View(employeeContract);
        }

        // POST: EmployeeContracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeContractId,EmployeeContractPath,StartDate,EndDate,EmployeeId")] EmployeeContract employeeContract)
        {
            if (id != employeeContract.EmployeeContractId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeContract);
                    _context.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeContractExists(employeeContract.EmployeeContractId))
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
            //ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId", employeeContract.EmployeeId);
            return View(employeeContract);
        }

        // GET: EmployeeContracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var employeeContract = await _context.GetById(id);
            if (employeeContract == null)
            {
                return NotFound();
            }

            return View(employeeContract);
        }

        // POST: EmployeeContracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EmployeeContract'  is null.");
            }
            var employeeContract = await _context.GetById(id);
            if (employeeContract != null)
            {
                _context.Remove(employeeContract);
            }            
            _context.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeContractExists(int id)
        {
          return _context.Exist(id);
        }
    }
}
