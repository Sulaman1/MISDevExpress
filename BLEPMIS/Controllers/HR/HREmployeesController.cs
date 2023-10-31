using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.MasterSetup;
using DBContext.Data;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BLEPMIS.Controllers.HR
{
    public class HREmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HREmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HREmployees
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HREmployee;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HREmployees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HREmployee == null)
            {
                return NotFound();
            }

            var hREmployee = await _context.HREmployee              
                .FirstOrDefaultAsync(m => m.HREmployeeId == id);
            if (hREmployee == null)
            {
                return NotFound();
            }
            hREmployee.Contracts = _context.EmployeeContract.Where(a => a.HREmployeeId == id).ToList();
            return View(hREmployee);
        }

        // GET: HREmployees/Create
        public IActionResult Create()
        {
            ViewData["SectionId"] = new SelectList(_context.HRSection, "Name", "Name");
            ViewData["HRQualificationLevelId"] = new SelectList(_context.HRQualificationLevel, "Name", "Name");
            ViewData["HRDesignationId"] = new SelectList(_context.HRDesignation, "DesignationName", "DesignationName");
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a=>a.DistrictId > 1), "Name", "Name");
            return View();
        }

        // POST: HREmployees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( HREmployee hREmployee, IFormFile CNICAttachment, IFormFile JoiningLetterAttachment, IFormFile CVAttachment)
        {
            if (ModelState.IsValid)
            {
                if (CNICAttachment != null && CNICAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HR\\"+ hREmployee.Section + "\\" + hREmployee.CNIC + "\\");
                    string fileName = Path.GetFileName(CNICAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "CNIC" + randomNumber.ToString() + Path.GetExtension(fileName);
                    hREmployee.CNICAttachment = Path.Combine("/Documents/HR/" + hREmployee.Section + "/" + hREmployee.CNIC + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await CNICAttachment.CopyToAsync(stream);
                    }
                }
                if (JoiningLetterAttachment != null && JoiningLetterAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HR\\" + hREmployee.Section + "\\" + hREmployee.CNIC + "\\");
                    string fileName = Path.GetFileName(JoiningLetterAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "JoiningLetter" + randomNumber.ToString() + Path.GetExtension(fileName);
                    hREmployee.JoiningLetterAttachment = Path.Combine("/Documents/HR/" + hREmployee.Section + "/" + hREmployee.CNIC + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await JoiningLetterAttachment.CopyToAsync(stream);
                    }
                }
                if (CVAttachment != null && CVAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HR\\" + hREmployee.Section + "\\" + hREmployee.CNIC + "\\");
                    string fileName = Path.GetFileName(CVAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "CV" + randomNumber.ToString() + Path.GetExtension(fileName);
                    hREmployee.CVAttachment = Path.Combine("/Documents/HR/" + hREmployee.Section + "/" + hREmployee.CNIC + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await CVAttachment.CopyToAsync(stream);
                    }
                }
                _context.Add(hREmployee);                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SectionId"] = new SelectList(_context.HRSection, "Name", "Name", hREmployee.Section);
            ViewData["HRQualificationLevelId"] = new SelectList(_context.HRQualificationLevel, "Name", "Name", hREmployee.LastEducationLevel);
            ViewData["HRDesignationId"] = new SelectList(_context.HRDesignation, "DesignationName", "DesignationName", hREmployee.Designation);
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", hREmployee.DistrictOfWork);
            return View(hREmployee);
        }

        // GET: HREmployees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HREmployee == null)
            {
                return NotFound();
            }

            var hREmployee = await _context.HREmployee.FindAsync(id);
            if (hREmployee == null)
            {
                return NotFound();
            }
            ViewData["SectionId"] = new SelectList(_context.HRSection, "Name", "Name", hREmployee.Section);            
            ViewData["HRQualificationLevelId"] = new SelectList(_context.HRQualificationLevel, "Name", "Name", hREmployee.LastEducationLevel);
            ViewData["HRDesignationId"] = new SelectList(_context.HRDesignation, "DesignationName", "DesignationName", hREmployee.Designation);
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", hREmployee.DistrictOfWork);
            return View(hREmployee);
        }

        // POST: HREmployees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  HREmployee hREmployee, IFormFile? CNICAttachment, IFormFile? JoiningLetterAttachment, IFormFile? CVAttachment)
        {
            if (id != hREmployee.HREmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (CNICAttachment != null && CNICAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HR\\" + hREmployee.Section + "\\" + hREmployee.CNIC + "\\");
                        string fileName = Path.GetFileName(CNICAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "CNIC" + randomNumber.ToString() + Path.GetExtension(fileName);
                        hREmployee.CNICAttachment = Path.Combine("/Documents/HR/" + hREmployee.Section + "/" + hREmployee.CNIC + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await CNICAttachment.CopyToAsync(stream);
                        }
                    }
                    if (JoiningLetterAttachment != null && JoiningLetterAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HR\\" + hREmployee.Section + "\\" + hREmployee.CNIC + "\\");
                        string fileName = Path.GetFileName(JoiningLetterAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "JoiningLetter" + randomNumber.ToString() + Path.GetExtension(fileName);
                        hREmployee.JoiningLetterAttachment = Path.Combine("/Documents/HR/" + hREmployee.Section + "/" + hREmployee.CNIC + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await JoiningLetterAttachment.CopyToAsync(stream);
                        }
                    }
                    if (CVAttachment != null && CVAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HR\\" + hREmployee.Section + "\\" + hREmployee.CNIC + "\\");
                        string fileName = Path.GetFileName(CVAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "CV" + randomNumber.ToString() + Path.GetExtension(fileName);
                        hREmployee.CVAttachment = Path.Combine("/Documents/HR/" + hREmployee.Section + "/" + hREmployee.CNIC + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await CVAttachment.CopyToAsync(stream);
                        }
                    }
                    _context.Update(hREmployee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HREmployeeExists(hREmployee.HREmployeeId))
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
            ViewData["SectionId"] = new SelectList(_context.HRSection, "Name", "Name", hREmployee.Section);
            ViewData["HRQualificationLevelId"] = new SelectList(_context.HRQualificationLevel, "Name", "Name", hREmployee.LastEducationLevel);
            ViewData["HRDesignationId"] = new SelectList(_context.HRDesignation, "DesignationName", "DesignationName", hREmployee.Designation);
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", hREmployee.DistrictOfWork);
            return View(hREmployee);
        }

        // GET: HREmployees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HREmployee == null)
            {
                return NotFound();
            }

            var hREmployee = await _context.HREmployee                
                .FirstOrDefaultAsync(m => m.HREmployeeId == id);
            if (hREmployee == null)
            {
                return NotFound();
            }

            return View(hREmployee);
        }

        // POST: HREmployees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HREmployee == null)
            {
                return Problem("Entity set 'ApplicationDbContext.HREmployee'  is null.");
            }
            var hREmployee = await _context.HREmployee.FindAsync(id);
            if (hREmployee != null)
            {
                _context.HREmployee.Remove(hREmployee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HREmployeeExists(int id)
        {
          return _context.HREmployee.Any(e => e.HREmployeeId == id);
        }
    }
}
