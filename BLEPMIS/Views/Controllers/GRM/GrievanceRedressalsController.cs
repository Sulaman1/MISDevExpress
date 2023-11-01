using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.GRM;
using DBContext.Data;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BLEPMIS.Controllers.GRM
{
    public class GrievanceRedressalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GrievanceRedressalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GrievanceRedressals
        public IActionResult Index()
        {
            ViewBag.TotalComplaint = _context.GrievanceRedressal.Count();
            ViewBag.TotalComplaintResolved = _context.GrievanceRedressal.Count(a=>a.Status == "Resolved");
            ViewBag.TotalComplaintInProcess = _context.GrievanceRedressal.Count(a=>a.Status == "In Process");            
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name");
            return View();
        }
        public async Task<IActionResult> _Index(string DName, int s = 0)
        {
            List<GrievanceRedressal> list = null;
            if(DName == null)
            {
                if(s == 0)
                {
                    list = await _context.GrievanceRedressal.ToListAsync();
                }
                else
                {
                    list = await _context.GrievanceRedressal.Where(a=>a.Status == (s == 1 ? "In Process" : "Resolved")).ToListAsync();
                }               
            }
            else
            {
                if (s == 0)
                {
                    list = await _context.GrievanceRedressal.Where(a=>a.DistrictName == DName).ToListAsync();
                }
                else
                {
                    list = await _context.GrievanceRedressal.Where(a => a.Status == (s == 1 ? "In Process" : "Resolved") && a.DistrictName == DName).ToListAsync();
                }
            }
            return PartialView(list);
        }
        // GET: GrievanceRedressals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GrievanceRedressal == null)
            {
                return NotFound();
            }

            var grievanceRedressal = await _context.GrievanceRedressal
                .FirstOrDefaultAsync(m => m.GRMId == id);
            if (grievanceRedressal == null)
            {
                return NotFound();
            }

            return View(grievanceRedressal);
        }

        // GET: GrievanceRedressals/Create
        public IActionResult Create()
        {
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name");
            return View();
        }

        // POST: GrievanceRedressals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GrievanceRedressal grievanceRedressal, IFormFile Attachment1, IFormFile Attachment2, IFormFile Attachment3)
        {
            var TehsilList = _context.Tehsil.Include(a => a.District).Where(a => a.District.Name == grievanceRedressal.DistrictName).ToList();
            if (ModelState.IsValid)
            {                
                if (grievanceRedressal.IsByEmail)
                {
                    if(grievanceRedressal.Email == null)
                    {
                        ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", grievanceRedressal.DistrictName);                        
                        ViewData["TehsilId"] = new SelectList(TehsilList, "TehsilName", "TehsilName", grievanceRedressal.TehsilName);
                        ModelState.AddModelError("CustomError", "Email required!");
                        return View(grievanceRedressal);
                    }
                }
                if (grievanceRedressal.IsByPhone)
                {
                    if (grievanceRedressal.ContactNumber == null)
                    {
                        ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", grievanceRedressal.DistrictName);                        
                        ViewData["TehsilId"] = new SelectList(TehsilList, "TehsilName", "TehsilName", grievanceRedressal.TehsilName);
                        ModelState.AddModelError("CustomError", "Phone# Required!");
                        return View(grievanceRedressal);
                    }
                }
                if (grievanceRedressal.IsPickUpResponses)
                {
                    if (grievanceRedressal.PickUpResponses == "N/A")
                    {
                        ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", grievanceRedressal.DistrictName);                        
                        ViewData["TehsilId"] = new SelectList(TehsilList, "TehsilName", "TehsilName", grievanceRedressal.TehsilName);
                        ModelState.AddModelError("CustomError", "Pickup Response required!");
                        return View(grievanceRedressal);
                    }
                }
                if (grievanceRedressal.IsByMail)
                {
                    if (grievanceRedressal.MailingAddress == null)
                    {
                        ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", grievanceRedressal.DistrictName);
                        ViewData["TehsilId"] = new SelectList(TehsilList, "TehsilName", "TehsilName", grievanceRedressal.TehsilName);
                        ModelState.AddModelError("CustomError", "Mailling Address required!");
                        return View(grievanceRedressal);
                    }
                }
                if (Attachment1 != null && Attachment1.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\GRM\\GRMA1\\" + grievanceRedressal.TehsilName + "\\");
                    string fileName = Path.GetFileName(Attachment1.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "GRMA1" + randomNumber.ToString() + Path.GetExtension(fileName);
                    grievanceRedressal.Attachment1 = Path.Combine("/Documents/GRM/GRMA1/" + grievanceRedressal.TehsilName + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await Attachment1.CopyToAsync(stream);
                    }
                }
                if (Attachment2 != null && Attachment2.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\GRM\\GRMA2\\" + grievanceRedressal.TehsilName + "\\");
                    string fileName = Path.GetFileName(Attachment2.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "GRMA2" + randomNumber.ToString() + Path.GetExtension(fileName);
                    grievanceRedressal.Attachment2 = Path.Combine("/Documents/GRM/GRMA2/" + grievanceRedressal.TehsilName + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await Attachment2.CopyToAsync(stream);
                    }
                }
                if (Attachment3 != null && Attachment3.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\GRM\\GRMA3\\" + grievanceRedressal.TehsilName + "\\");
                    string fileName = Path.GetFileName(Attachment3.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "GRMA3" + randomNumber.ToString() + Path.GetExtension(fileName);
                    grievanceRedressal.Attachment3 = Path.Combine("/Documents/GRM/GRMA3/" + grievanceRedressal.TehsilName + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await Attachment3.CopyToAsync(stream);
                    }
                }
                _context.Add(grievanceRedressal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", grievanceRedressal.DistrictName);            
            ViewData["TehsilId"] = new SelectList(TehsilList, "TehsilName", "TehsilName", grievanceRedressal.TehsilName);
            return View(grievanceRedressal);
        }

        // GET: GrievanceRedressals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GrievanceRedressal == null)
            {
                return NotFound();
            }

            var grievanceRedressal = await _context.GrievanceRedressal.FindAsync(id);
            if (grievanceRedressal == null)
            {
                return NotFound();
            }
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", grievanceRedressal.DistrictName);
            var TehsilList = _context.Tehsil.Include(a=>a.District).Where(a => a.District.Name == grievanceRedressal.DistrictName).ToList();
            ViewData["TehsilId"] = new SelectList(TehsilList, "TehsilName", "TehsilName", grievanceRedressal.TehsilName);
            return View(grievanceRedressal);
        }

        // POST: GrievanceRedressals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GrievanceRedressal grievanceRedressal, IFormFile Attachment1, IFormFile Attachment2, IFormFile Attachment3)
        {
            if (id != grievanceRedressal.GRMId)
            {
                return NotFound();
            }
            var TehsilList = _context.Tehsil.Include(a => a.District).Where(a => a.District.Name == grievanceRedressal.DistrictName).ToList();
            if (ModelState.IsValid)
            {
                if (grievanceRedressal.IsByEmail)
                {
                    if (grievanceRedressal.Email == null)
                    {
                        ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", grievanceRedressal.DistrictName);
                        ViewData["TehsilId"] = new SelectList(TehsilList, "TehsilName", "TehsilName", grievanceRedressal.TehsilName);
                        ModelState.AddModelError("CustomError", "Email required!");
                        return View(grievanceRedressal);
                    }
                }
                if (grievanceRedressal.IsByPhone)
                {
                    if (grievanceRedressal.ContactNumber == null)
                    {
                        ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", grievanceRedressal.DistrictName);
                        ViewData["TehsilId"] = new SelectList(TehsilList, "TehsilName", "TehsilName", grievanceRedressal.TehsilName);
                        ModelState.AddModelError("CustomError", "Phone# Required!");
                        return View(grievanceRedressal);
                    }
                }
                if (grievanceRedressal.IsPickUpResponses)
                {
                    if (grievanceRedressal.PickUpResponses == "N/A")
                    {
                        ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", grievanceRedressal.DistrictName);
                        ViewData["TehsilId"] = new SelectList(TehsilList, "TehsilName", "TehsilName", grievanceRedressal.TehsilName);
                        ModelState.AddModelError("CustomError", "Pickup Response required!");
                        return View(grievanceRedressal);
                    }
                }
                if (grievanceRedressal.IsByMail)
                {
                    if (grievanceRedressal.MailingAddress == null)
                    {
                        ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", grievanceRedressal.DistrictName);
                        ViewData["TehsilId"] = new SelectList(TehsilList, "TehsilName", "TehsilName", grievanceRedressal.TehsilName);
                        ModelState.AddModelError("CustomError", "Mailling Address required!");
                        return View(grievanceRedressal);
                    }
                }
                try
                {
                    if (Attachment1 != null && Attachment1.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\GRM\\GRMA1\\" + grievanceRedressal.TehsilName + "\\");
                        string fileName = Path.GetFileName(Attachment1.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "GRMA1" + randomNumber.ToString() + Path.GetExtension(fileName);
                        grievanceRedressal.Attachment1 = Path.Combine("/Documents/GRM/GRMA1/" + grievanceRedressal.TehsilName + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await Attachment1.CopyToAsync(stream);
                        }
                    }
                    if (Attachment2 != null && Attachment2.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\GRM\\GRMA2\\" + grievanceRedressal.TehsilName + "\\");
                        string fileName = Path.GetFileName(Attachment2.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "GRMA2" + randomNumber.ToString() + Path.GetExtension(fileName);
                        grievanceRedressal.Attachment2 = Path.Combine("/Documents/GRM/GRMA2/" + grievanceRedressal.TehsilName + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await Attachment2.CopyToAsync(stream);
                        }
                    }
                    if (Attachment3 != null && Attachment3.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\GRM\\GRMA3\\" + grievanceRedressal.TehsilName + "\\");
                        string fileName = Path.GetFileName(Attachment3.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "GRMA3" + randomNumber.ToString() + Path.GetExtension(fileName);
                        grievanceRedressal.Attachment3 = Path.Combine("/Documents/GRM/GRMA3/" + grievanceRedressal.TehsilName + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await Attachment3.CopyToAsync(stream);
                        }
                    }                    
                    _context.Update(grievanceRedressal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrievanceRedressalExists(grievanceRedressal.GRMId))
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
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", grievanceRedressal.DistrictName);            
            ViewData["TehsilId"] = new SelectList(TehsilList, "TehsilName", "TehsilName", grievanceRedressal.TehsilName);
            return View(grievanceRedressal);
        }
        public async Task<IActionResult> Resolved(int? id)
        {
            if (id == null || _context.GrievanceRedressal == null)
            {
                return NotFound();
            }

            var grievanceRedressal = await _context.GrievanceRedressal.FindAsync(id);
            if (grievanceRedressal == null)
            {
                return NotFound();
            }
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", grievanceRedressal.DistrictName);
            var TehsilList = _context.Tehsil.Include(a => a.District).Where(a => a.District.Name == grievanceRedressal.DistrictName).ToList();
            ViewData["TehsilId"] = new SelectList(TehsilList, "TehsilName", "TehsilName", grievanceRedressal.TehsilName);
            return View(grievanceRedressal);
        }

        // POST: GrievanceRedressals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resolved(int id, GrievanceRedressal grievanceRedressal, IFormFile Attachment4)
        {
            if (id != grievanceRedressal.GRMId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {                   
                    if (Attachment4 != null && Attachment4.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\GRM\\GRMA4\\" + grievanceRedressal.TehsilName + "\\");
                        string fileName = Path.GetFileName(Attachment4.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "GRMA4" + randomNumber.ToString() + Path.GetExtension(fileName);
                        grievanceRedressal.Attachment4 = Path.Combine("/Documents/GRM/GRMA4/" + grievanceRedressal.TehsilName + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await Attachment4.CopyToAsync(stream);
                        }
                    }
                    _context.Update(grievanceRedressal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrievanceRedressalExists(grievanceRedressal.GRMId))
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
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", grievanceRedressal.DistrictName);
            var TehsilList = _context.Tehsil.Include(a => a.District).Where(a => a.District.Name == grievanceRedressal.DistrictName).ToList();
            ViewData["TehsilId"] = new SelectList(TehsilList, "TehsilName", "TehsilName", grievanceRedressal.TehsilName);
            return View(grievanceRedressal);
        }
        // GET: GrievanceRedressals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GrievanceRedressal == null)
            {
                return NotFound();
            }

            var grievanceRedressal = await _context.GrievanceRedressal
                .FirstOrDefaultAsync(m => m.GRMId == id);
            if (grievanceRedressal == null)
            {
                return NotFound();
            }

            return View(grievanceRedressal);
        }

        // POST: GrievanceRedressals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GrievanceRedressal == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GrievanceRedressal'  is null.");
            }
            var grievanceRedressal = await _context.GrievanceRedressal.FindAsync(id);
            if (grievanceRedressal != null)
            {
                _context.GrievanceRedressal.Remove(grievanceRedressal);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GrievanceRedressalExists(int id)
        {
          return _context.GrievanceRedressal.Any(e => e.GRMId == id);
        }
    }
}
