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
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BLEPMIS.Controllers.MasterSetup
{
    public class EmployeeContractsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeContractsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeContracts
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.Id = id;            
            return View(await _context.EmployeeContract.Include(a=>a.HREmployee).Where(a=>a.HREmployeeId == id).ToListAsync());
        }

        // GET: EmployeeContracts/Details/5
        

        // GET: EmployeeContracts/Create
        public IActionResult Create(int id)
        {            
            EmployeeContract obj = new EmployeeContract();
            obj.HREmployeeId = id;
            obj.StartDate = DateTime.Today;
            obj.EndDate = DateTime.Today;
            return View(obj);
        }

        // POST: EmployeeContracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeContract employeeContract, IFormFile? EmployeeContractPath)
        {
            if (ModelState.IsValid)
            {
                if (EmployeeContractPath != null && EmployeeContractPath.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HR" + employeeContract.HREmployeeId + "\\");
                    string fileName = Path.GetFileName(EmployeeContractPath.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "Contract" + randomNumber.ToString() + Path.GetExtension(fileName);
                    employeeContract.EmployeeContractPath = Path.Combine("/Documents/HR" + employeeContract.HREmployeeId + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await EmployeeContractPath.CopyToAsync(stream);
                    }
                }
                _context.Add(employeeContract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {id = employeeContract.HREmployeeId});
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

            var employeeContract = await _context.EmployeeContract.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, EmployeeContract employeeContract)
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

            var employeeContract = await _context.EmployeeContract.FindAsync(id);
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
            var employeeContract = await _context.EmployeeContract.FindAsync(id);
            if (employeeContract != null)
            {
                _context.Remove(employeeContract);
            }                        
            return RedirectToAction(nameof(Index), new {id = employeeContract.HREmployeeId});
        }

        private bool EmployeeContractExists(int id)
        {
            return _context.EmployeeContract.Any(e => e.EmployeeContractId == id);
        }
    }
}
