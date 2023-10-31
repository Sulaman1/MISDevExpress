using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.BSF;
using DBContext.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Identity;
using DAL.Models;
using DAL.Models.Domain.MasterSetup;

namespace BLEPMIS.Controllers.BSF
{
    public class BSFGovsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public BSFGovsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BSFGovs
        public async Task<IActionResult> Index()
        {
              return View(await _context.BSFGov.ToListAsync());
        }

        // GET: BSFGovs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var bsfGov = _context.BSFGov.Find(id);
            ViewBag.DepartmentName = bsfGov.DepartmentName;
            ViewBag.Id = id;
            var applicationDbContext = _context.BSFGovStage.Include(b => b.BSFGov).Where(a => a.BSFGovId == id);
            ViewBag.TotalGrant = applicationDbContext.Select(a => a.BSFGov.TotalGrant).FirstOrDefault();
            ViewBag.ReceivedGrant = applicationDbContext.Sum(a => a.AmountRelease);
            ViewBag.TotalStage = 12;
            ViewBag.MaxRelease = bsfGov.TotalGrant - applicationDbContext.Sum(a => a.AmountRelease);
            ViewBag.ComletedStage = applicationDbContext.Count(a => a.IsCompleted == true);
            return View(bsfGov);
        }

        // GET: BSFGovs/Create
        public async Task<IActionResult> Create()
        {
            BSFGov bSFGov = new BSFGov();
            bSFGov.CreatedBy = User.Identity.Name;
            bSFGov.CreatedOn = DateTime.Today;            
            var currentuser = await _userManager.GetUserAsync(User);
            bSFGov.UserId = currentuser.Id;
            bSFGov.BSFName = "Test";
            ViewData["BSFDepartmentId"] = new SelectList(_context.BSFDepartment, "Name", "Name");            
            ViewData["BSFGovtPackage"] = new SelectList(_context.BSFGovtPackage, "PackageName", "PackageName");            
            ViewData["ConstructionType"] = new SelectList(_context.ConstructionType, "Name", "Name");                                   
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a=>a.DistrictId > 1), "Name", "Name");            
            return View(bSFGov);
        }

        // POST: BSFGovs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BSFGov bSFGov, IFormFile WorkScopeAttachment)
        {
            if (ModelState.IsValid)
            {
                if (WorkScopeAttachment != null && WorkScopeAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\WorkScope\\" + bSFGov.DistrictName + "\\");
                    string fileName = Path.GetFileName(WorkScopeAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "WorkScope" + randomNumber.ToString() + Path.GetExtension(fileName);
                    bSFGov.WorkScopeAttachment = Path.Combine("/Documents/BSF/Gov/WorkScope/" + bSFGov.DistrictName + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await WorkScopeAttachment.CopyToAsync(stream);
                    }
                }
                /*if (FisibilityReportAttachment != null && FisibilityReportAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\FReport\\" + bSFGov.DistrictName + "\\");
                    string fileName = Path.GetFileName(FisibilityReportAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "FReport" + randomNumber.ToString() + Path.GetExtension(fileName);
                    bSFGov.FisibilityReportAttachment = Path.Combine("/Documents/BSF/Gov/FReport/" + bSFGov.DistrictName + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await FisibilityReportAttachment.CopyToAsync(stream);
                    }
                }*/
                //-------------------------------------------------
                int BSFCount = 1;
                string BSFCode = "BSF-Gov";
                string val = (BSFCount).ToString("D2");
                bSFGov.BSFName = (BSFCode + "-" + val);
                while (_context.BSFGov.Count(a => a.BSFName == bSFGov.BSFName) > 0)
                {
                    val = (++BSFCount).ToString("D2");
                    bSFGov.BSFName = (BSFCode + "-" + val);
                }
                //-------------------------------------------------
                _context.Add(bSFGov);
                await _context.SaveChangesAsync();
                /*int MaxId = _context.BSFGov.Max(a => a.BSFGovId);
                
                BSFGovStage bSFGovStage = new BSFGovStage();
                if(bSFGov.IsNew == "New")
                {
                    bSFGovStage.BSFGovStageId = 0;
                    bSFGovStage.BSFGovId = MaxId;
                    bSFGovStage.StageName = "Designing/BOQ";
                    bSFGovStage.StageNumber = "1";
                    bSFGovStage.IsNew = "New";
                    bSFGovStage.CreatedBy = User.Identity.Name;
                    bSFGovStage.UserId = bSFGov.UserId;
                    _context.Add(bSFGovStage);
                    _context.SaveChanges();

                    bSFGovStage.BSFGovStageId = 0;
                    bSFGovStage.BSFGovId = MaxId;
                    bSFGovStage.IsNew = "New";
                    bSFGovStage.StageName = "Contract Awarded";
                    bSFGovStage.StageNumber = "2";
                    bSFGovStage.UserId = bSFGov.UserId;
                    bSFGovStage.CreatedBy = User.Identity.Name;
                    _context.Add(bSFGovStage);
                    _context.SaveChanges();

                    bSFGovStage.BSFGovStageId = 0;
                    bSFGovStage.BSFGovId = MaxId;
                    bSFGovStage.IsNew = "New";
                    bSFGovStage.StageName = "(a) Foundation";
                    bSFGovStage.StageNumber = "3 Implementation";
                    bSFGovStage.UserId = bSFGov.UserId;
                    bSFGovStage.CreatedBy = User.Identity.Name;
                    _context.Add(bSFGovStage);
                    _context.SaveChanges();

                    bSFGovStage.BSFGovStageId = 0;
                    bSFGovStage.BSFGovStageId = 0;
                    bSFGovStage.BSFGovId = MaxId;
                    bSFGovStage.IsNew = "New";
                    bSFGovStage.StageName = "(b) Complete Gray Structure";
                    bSFGovStage.StageNumber = "3 Implementation";
                    bSFGovStage.UserId = bSFGov.UserId;
                    bSFGovStage.CreatedBy = User.Identity.Name;
                    _context.Add(bSFGovStage);
                    _context.SaveChanges();

                    bSFGovStage.BSFGovStageId = 0;
                    bSFGovStage.BSFGovId = MaxId;
                    bSFGovStage.StageName = "(c) Completion (FCR)";
                    bSFGovStage.StageNumber = "3 Implementation";
                    bSFGovStage.IsNew = "New";
                    bSFGovStage.UserId = bSFGov.UserId;
                    bSFGovStage.CreatedBy = User.Identity.Name;
                    _context.Add(bSFGovStage);
                    _context.SaveChanges();

                    bSFGovStage.BSFGovStageId = 0;
                    bSFGovStage.BSFGovId = MaxId;
                    bSFGovStage.IsNew = "New";
                    bSFGovStage.StageName = "Handing Over";
                    bSFGovStage.StageNumber = "4";
                    bSFGovStage.UserId = bSFGov.UserId;
                    bSFGovStage.CreatedBy = User.Identity.Name;
                    _context.Add(bSFGovStage);
                    _context.SaveChanges();
                }
                else
                {
                    bSFGovStage.BSFGovStageId = 0;
                    bSFGovStage.BSFGovId = MaxId;
                    bSFGovStage.StageName = "Designing/BOQ";
                    bSFGovStage.StageNumber = "1";
                    bSFGovStage.IsNew = "Rehabilitation";
                    bSFGovStage.UserId = bSFGov.UserId;
                    bSFGovStage.CreatedBy = User.Identity.Name;
                    _context.Add(bSFGovStage);
                    _context.SaveChanges();

                    bSFGovStage.BSFGovStageId = 0;
                    bSFGovStage.BSFGovId = MaxId;
                    bSFGovStage.StageName = "Contract Awarded";
                    bSFGovStage.StageNumber = "2";
                    bSFGovStage.IsNew = "Rehabilitation";
                    bSFGovStage.UserId = bSFGov.UserId;
                    bSFGovStage.CreatedBy = User.Identity.Name;
                    _context.Add(bSFGovStage);
                    _context.SaveChanges();

                    bSFGovStage.BSFGovStageId = 0;
                    bSFGovStage.BSFGovId = MaxId;
                    bSFGovStage.IsNew = "Rehabilitation";
                    bSFGovStage.StageName = "(a) Before";
                    bSFGovStage.StageNumber = "3 Implementation";
                    bSFGovStage.UserId = bSFGov.UserId;
                    bSFGovStage.CreatedBy = User.Identity.Name;
                    _context.Add(bSFGovStage);
                    _context.SaveChanges();

                    bSFGovStage.BSFGovStageId = 0;
                    bSFGovStage.BSFGovId = MaxId;
                    bSFGovStage.StageName = "(b) After";
                    bSFGovStage.StageNumber = "3 Implementation";
                    bSFGovStage.IsNew = "Rehabilitation";
                    bSFGovStage.UserId = bSFGov.UserId;
                    bSFGovStage.CreatedBy = User.Identity.Name;
                    _context.Add(bSFGovStage);
                    _context.SaveChanges();

                    bSFGovStage.BSFGovStageId = 0;
                    bSFGovStage.BSFGovId = MaxId;
                    bSFGovStage.IsNew = "Rehabilitation";
                    bSFGovStage.StageName = "Handing Over";
                    bSFGovStage.StageNumber = "4";
                    bSFGovStage.UserId = bSFGov.UserId;
                    bSFGovStage.CreatedBy = User.Identity.Name;
                    _context.Add(bSFGovStage);
                    _context.SaveChanges();                  
                }*/
                return RedirectToAction(nameof(Index));
            }
            ViewData["BSFDepartmentId"] = new SelectList(_context.BSFDepartment, "Name", "Name", bSFGov.DepartmentName);
            return View(bSFGov);
        }

        // GET: BSFGovs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BSFGov == null)
            {
                return NotFound();
            }

            var bSFGov = await _context.BSFGov.FindAsync(id);
            if (bSFGov == null)
            {
                return NotFound();
            }
            ViewData["BSFDepartmentId"] = new SelectList(_context.BSFDepartment, "Name", "Name", bSFGov.DepartmentName);
            ViewData["BSFGovtPackage"] = new SelectList(_context.BSFGovtPackage, "PackageName", "PackageName", bSFGov.BSFGovtPackage);
            ViewData["ConstructionType"] = new SelectList(_context.ConstructionType, "Name", "Name", bSFGov.ConstructionType);
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", bSFGov.DistrictName);
            return View(bSFGov);
        }

        // POST: BSFGovs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BSFGov bSFGov, IFormFile WorkScopeAttachment, IFormFile FisibilityReportAttachment)
        {
            if (id != bSFGov.BSFGovId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (WorkScopeAttachment != null && WorkScopeAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\WorkScope\\" + bSFGov.DistrictName + "\\");
                        string fileName = Path.GetFileName(WorkScopeAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "WorkScope" + randomNumber.ToString() + Path.GetExtension(fileName);
                        bSFGov.WorkScopeAttachment = Path.Combine("/Documents/BSF/Gov/WorkScope/" + bSFGov.DistrictName + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await WorkScopeAttachment.CopyToAsync(stream);
                        }
                    }
                    /*if (FisibilityReportAttachment != null && FisibilityReportAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\FReport\\" + bSFGov.DistrictName + "\\");
                        string fileName = Path.GetFileName(FisibilityReportAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "FReport" + randomNumber.ToString() + Path.GetExtension(fileName);
                        bSFGov.FisibilityReportAttachment = Path.Combine("/Documents/BSF/Gov/FReport/" + bSFGov.DistrictName + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await FisibilityReportAttachment.CopyToAsync(stream);
                        }
                    }*/
                    bSFGov.CreatedBy = User.Identity.Name;
                    bSFGov.CreatedOn = DateTime.Today;
                    _context.Update(bSFGov);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BSFGovExists(bSFGov.BSFGovId))
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
            ViewData["BSFGovtPackage"] = new SelectList(_context.BSFGovtPackage, "PackageName", "PackageName", bSFGov.BSFGovtPackage);
            ViewData["ConstructionType"] = new SelectList(_context.ConstructionType, "Name", "Name", bSFGov.ConstructionType);
            ViewData["BSFDepartmentId"] = new SelectList(_context.BSFDepartment, "Name", "Name", bSFGov.DepartmentName);
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", bSFGov.DistrictName);
            return View(bSFGov);
        }

        // GET: BSFGovs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BSFGov == null)
            {
                return NotFound();
            }

            var bSFGov = await _context.BSFGov
                .FirstOrDefaultAsync(m => m.BSFGovId == id);
            if (bSFGov == null)
            {
                return NotFound();
            }

            return View(bSFGov);
        }

        // POST: BSFGovs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BSFGov == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BSFGov'  is null.");
            }
            var bSFGov = await _context.BSFGov.FindAsync(id);
            if (bSFGov != null)
            {
                _context.BSFGov.Remove(bSFGov);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BSFGovExists(int id)
        {
          return _context.BSFGov.Any(e => e.BSFGovId == id);
        }
    }
}
