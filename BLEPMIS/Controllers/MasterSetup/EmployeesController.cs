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
    public class EmployeesController : Controller
    {
        private readonly IEmployee _context;

        public EmployeesController(IEmployee context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {            
            return View(await _context.GetAll());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var employee = await _context.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["SectionId"] = new SelectList(_context.GetAllSection(), "SectionId", "Name");
            Employee obj = new Employee();
            obj.IsActive = true;            
            return View(obj);
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,EmployeeName,FatherName,CNIC,ContactNumber,Address,Designation,IsActive,SectionId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Insert(employee);
                _context.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SectionId"] = new SelectList(_context.GetAllSection(), "SectionId", "Name", employee.SectionId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var employee = await _context.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["SectionId"] = new SelectList(_context.GetAllSection(), "SectionId", "Name", employee.SectionId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,EmployeeName,FatherName,CNIC,ContactNumber,Address,Designation,IsActive,SectionId")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    _context.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            ViewData["SectionId"] = new SelectList(_context.GetAllSection(), "SectionId", "Name", employee.SectionId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var employee = await _context.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employee'  is null.");
            }
            var employee = await _context.GetById(id);
            if (employee != null)
            {
                _context.Remove(employee);
            }
            
            _context.Save();
            return RedirectToAction(nameof(Index));
        }
        private bool EmployeeExists(int id)
        {
          return _context.Exist(id);
        }
    }
}
