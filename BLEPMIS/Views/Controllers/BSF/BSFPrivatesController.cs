using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.BSF;
using DBContext.Data;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using DAL.Models;

namespace BLEPMIS.Controllers.BSF
{
    public class BSFPrivatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public BSFPrivatesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BSFPrivates
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BSFPrivate.Include(b => b.GeneralBusinessIdea);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BSFPrivates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var bsfPrivate = await _context.BSFPrivate.Include(a => a.GeneralBusinessIdea).Where(a => a.BSFPrivateId == id).FirstOrDefaultAsync();
            ViewBag.DepartmentName = bsfPrivate.GeneralBusinessIdea.GeneralBusinessIdeaName;
            var applicationDbContext = _context.BSFPrivateStage.Include(b => b.BSFPrivate).Where(a => a.BSFPrivateId == id);
            ViewBag.TotalGrant = _context.BSFPrivate.Find(id).TotalGrant;
            ViewBag.CompletedStage = applicationDbContext.Count();
            ViewBag.ReceivedGrant = applicationDbContext.Sum(a => a.AmountRelease);
            ViewBag.Id = id;
            return View(bsfPrivate);
        }

        // GET: BSFPrivates/Create
        public IActionResult Create()
        {
            ViewData["GeneralBusinessIdeaId"] = new SelectList(_context.GeneralBusinessIdea, "GeneralBusinessIdeaId", "GeneralBusinessIdeaName");
            ViewData["District"] = new SelectList(_context.District.Where(a=>a.DistrictId > 1), "Name", "Name");
            return View();
        }

        // POST: BSFPrivates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BSFPrivate bSFPrivate, IFormFile CNICAttachment, IFormFile NTNAttachment, IFormFile BusinessPlanAttachment, IFormFile FisibilityReportAttachment, IFormFile ContractAwardAttachment)
        {
            if (ModelState.IsValid)
            {
                //if (CNICAttachment != null && CNICAttachment.Length > 0)
                //{
                //    var rootPath = Path.Combine(
                //    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Private\\CNIC\\" + bSFPrivate.DistrictName + "\\");
                //    string fileName = Path.GetFileName(CNICAttachment.FileName);
                //    Random random = new Random();
                //    int randomNumber = random.Next(9999, 100000);
                //    fileName = "CNIC" + randomNumber.ToString() + Path.GetExtension(fileName);
                //    bSFPrivate.CNICAttachment = Path.Combine("/Documents/BSF/Private/CNIC/" + bSFPrivate.DistrictName + "/" + fileName);//Server Path
                //    string sPath = Path.Combine(rootPath);
                //    if (!System.IO.Directory.Exists(sPath))
                //    {
                //        System.IO.Directory.CreateDirectory(sPath);
                //    }
                //    string FullPathWithFileName = Path.Combine(sPath, fileName);
                //    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                //    {
                //        await CNICAttachment.CopyToAsync(stream);
                //    }
                //}

                if (NTNAttachment != null && NTNAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Private\\NTN\\" + bSFPrivate.DistrictName + "\\");
                    string fileName = Path.GetFileName(NTNAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "NTN" + randomNumber.ToString() + Path.GetExtension(fileName);
                    bSFPrivate.NTNAttachment  = Path.Combine("/Documents/BSF/Private/NTN/" + bSFPrivate.DistrictName + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await NTNAttachment.CopyToAsync(stream);
                    }
                }

                //if (BusinessPlanAttachment != null && BusinessPlanAttachment.Length > 0)
                //{
                //    var rootPath = Path.Combine(
                //    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Private\\BPlan\\" + bSFPrivate.DistrictName + "\\");
                //    string fileName = Path.GetFileName(BusinessPlanAttachment.FileName);
                //    Random random = new Random();
                //    int randomNumber = random.Next(9999, 100000);
                //    fileName = "BPlan" + randomNumber.ToString() + Path.GetExtension(fileName);
                //    bSFPrivate.BusinessPlanAttachment = Path.Combine("/Documents/BSF/Private/BPlan/" + bSFPrivate.DistrictName + "/" + fileName);//Server Path
                //    string sPath = Path.Combine(rootPath);
                //    if (!System.IO.Directory.Exists(sPath))
                //    {
                //        System.IO.Directory.CreateDirectory(sPath);
                //    }
                //    string FullPathWithFileName = Path.Combine(sPath, fileName);
                //    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                //    {
                //        await BusinessPlanAttachment.CopyToAsync(stream);
                //    }
                //}

                //if (FisibilityReportAttachment != null && FisibilityReportAttachment.Length > 0)
                //{
                //    var rootPath = Path.Combine(
                //    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Private\\FReport\\" + bSFPrivate.DistrictName + "\\");
                //    string fileName = Path.GetFileName(FisibilityReportAttachment.FileName);
                //    Random random = new Random();
                //    int randomNumber = random.Next(9999, 100000);
                //    fileName = "FReport" + randomNumber.ToString() + Path.GetExtension(fileName);
                //    bSFPrivate.FisibilityReportAttachment = Path.Combine("/Documents/BSF/Private/FReport/" + bSFPrivate.DistrictName + "/" + fileName);//Server Path
                //    string sPath = Path.Combine(rootPath);
                //    if (!System.IO.Directory.Exists(sPath))
                //    {
                //        System.IO.Directory.CreateDirectory(sPath);
                //    }
                //    string FullPathWithFileName = Path.Combine(sPath, fileName);
                //    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                //    {
                //        await FisibilityReportAttachment.CopyToAsync(stream);
                //    }
                //}

                //if (ContractAwardAttachment != null && ContractAwardAttachment.Length > 0)
                //{
                //    var rootPath = Path.Combine(
                //    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Private\\Contract\\" + bSFPrivate.DistrictName + "\\");
                //    string fileName = Path.GetFileName(ContractAwardAttachment.FileName);
                //    Random random = new Random();
                //    int randomNumber = random.Next(9999, 100000);
                //    fileName = "Contract" + randomNumber.ToString() + Path.GetExtension(fileName);
                //    bSFPrivate.ContractAwardAttachment = Path.Combine("/Documents/BSF/Private/Contract/" + bSFPrivate.DistrictName + "/" + fileName);//Server Path
                //    string sPath = Path.Combine(rootPath);
                //    if (!System.IO.Directory.Exists(sPath))
                //    {
                //        System.IO.Directory.CreateDirectory(sPath);
                //    }
                //    string FullPathWithFileName = Path.Combine(sPath, fileName);
                //    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                //    {
                //        await ContractAwardAttachment.CopyToAsync(stream);
                //    }
                //}

                bSFPrivate.CreatedBy = User.Identity.Name;
                bSFPrivate.CreatedOn = DateTime.Today;
                var currentuser = await _userManager.GetUserAsync(User);
                bSFPrivate.UserId = currentuser.Id;
                _context.Add(bSFPrivate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GeneralBusinessIdeaId"] = new SelectList(_context.GeneralBusinessIdea, "GeneralBusinessIdeaId", "GeneralBusinessIdeaName", bSFPrivate.GeneralBusinessIdeaId);
            return View(bSFPrivate);
        }

        // GET: BSFPrivates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BSFPrivate == null)
            {
                return NotFound();
            }

            var bSFPrivate = await _context.BSFPrivate.FindAsync(id);
            if (bSFPrivate == null)
            {
                return NotFound();
            }
            ViewData["GeneralBusinessIdeaId"] = new SelectList(_context.GeneralBusinessIdea, "GeneralBusinessIdeaId", "GeneralBusinessIdeaName", bSFPrivate.GeneralBusinessIdeaId);
            ViewData["District"] = new SelectList(_context.District.Where(a => a.DistrictId > 1), "Name", "Name", bSFPrivate.DistrictName);
            return View(bSFPrivate);
        }

        // POST: BSFPrivates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BSFPrivate bSFPrivate, IFormFile CNICAttachment, IFormFile NTNAttachment, IFormFile BusinessPlanAttachment, IFormFile FisibilityReportAttachment, IFormFile ContractAwardAttachment)
        {
            if (id != bSFPrivate.BSFPrivateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //if (CNICAttachment != null && CNICAttachment.Length > 0)
                    //{
                    //    var rootPath = Path.Combine(
                    //    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Private\\CNIC\\" + bSFPrivate.DistrictName + "\\");
                    //    string fileName = Path.GetFileName(CNICAttachment.FileName);
                    //    Random random = new Random();
                    //    int randomNumber = random.Next(9999, 100000);
                    //    fileName = "CNIC" + randomNumber.ToString() + Path.GetExtension(fileName);
                    //    bSFPrivate.CNICAttachment = Path.Combine("/Documents/BSF/Private/CNIC/" + bSFPrivate.DistrictName + "/" + fileName);//Server Path
                    //    string sPath = Path.Combine(rootPath);
                    //    if (!System.IO.Directory.Exists(sPath))
                    //    {
                    //        System.IO.Directory.CreateDirectory(sPath);
                    //    }
                    //    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    //    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    //    {
                    //        await CNICAttachment.CopyToAsync(stream);
                    //    }
                    //}

                    if (NTNAttachment != null && NTNAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Private\\NTN\\" + bSFPrivate.DistrictName + "\\");
                        string fileName = Path.GetFileName(NTNAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "NTN" + randomNumber.ToString() + Path.GetExtension(fileName);
                        bSFPrivate.NTNAttachment = Path.Combine("/Documents/BSF/Private/NTN/" + bSFPrivate.DistrictName + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await NTNAttachment.CopyToAsync(stream);
                        }
                    }

                    //if (BusinessPlanAttachment != null && BusinessPlanAttachment.Length > 0)
                    //{
                    //    var rootPath = Path.Combine(
                    //    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Private\\BPlan\\" + bSFPrivate.DistrictName + "\\");
                    //    string fileName = Path.GetFileName(BusinessPlanAttachment.FileName);
                    //    Random random = new Random();
                    //    int randomNumber = random.Next(9999, 100000);
                    //    fileName = "BPlan" + randomNumber.ToString() + Path.GetExtension(fileName);
                    //    bSFPrivate.BusinessPlanAttachment = Path.Combine("/Documents/BSF/Private/BPlan/" + bSFPrivate.DistrictName + "/" + fileName);//Server Path
                    //    string sPath = Path.Combine(rootPath);
                    //    if (!System.IO.Directory.Exists(sPath))
                    //    {
                    //        System.IO.Directory.CreateDirectory(sPath);
                    //    }
                    //    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    //    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    //    {
                    //        await BusinessPlanAttachment.CopyToAsync(stream);
                    //    }
                    //}

                    //if (FisibilityReportAttachment != null && FisibilityReportAttachment.Length > 0)
                    //{
                    //    var rootPath = Path.Combine(
                    //    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Private\\FReport\\" + bSFPrivate.DistrictName + "\\");
                    //    string fileName = Path.GetFileName(FisibilityReportAttachment.FileName);
                    //    Random random = new Random();
                    //    int randomNumber = random.Next(9999, 100000);
                    //    fileName = "FReport" + randomNumber.ToString() + Path.GetExtension(fileName);
                    //    bSFPrivate.FisibilityReportAttachment = Path.Combine("/Documents/BSF/Private/FReport/" + bSFPrivate.DistrictName + "/" + fileName);//Server Path
                    //    string sPath = Path.Combine(rootPath);
                    //    if (!System.IO.Directory.Exists(sPath))
                    //    {
                    //        System.IO.Directory.CreateDirectory(sPath);
                    //    }
                    //    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    //    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    //    {
                    //        await FisibilityReportAttachment.CopyToAsync(stream);
                    //    }
                    //}

                    //if (ContractAwardAttachment != null && ContractAwardAttachment.Length > 0)
                    //{
                    //    var rootPath = Path.Combine(
                    //    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Private\\Contract\\" + bSFPrivate.DistrictName + "\\");
                    //    string fileName = Path.GetFileName(ContractAwardAttachment.FileName);
                    //    Random random = new Random();
                    //    int randomNumber = random.Next(9999, 100000);
                    //    fileName = "Contract" + randomNumber.ToString() + Path.GetExtension(fileName);
                    //    bSFPrivate.ContractAwardAttachment = Path.Combine("/Documents/BSF/Private/Contract/" + bSFPrivate.DistrictName + "/" + fileName);//Server Path
                    //    string sPath = Path.Combine(rootPath);
                    //    if (!System.IO.Directory.Exists(sPath))
                    //    {
                    //        System.IO.Directory.CreateDirectory(sPath);
                    //    }
                    //    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    //    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    //    {
                    //        await ContractAwardAttachment.CopyToAsync(stream);
                    //    }
                    //}            
                    var currentuser = await _userManager.GetUserAsync(User);
                    bSFPrivate.UserId = currentuser.Id;
                    _context.Update(bSFPrivate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BSFPrivateExists(bSFPrivate.BSFPrivateId))
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
            ViewData["GeneralBusinessIdeaId"] = new SelectList(_context.GeneralBusinessIdea, "GeneralBusinessIdeaId", "GeneralBusinessIdeaName", bSFPrivate.GeneralBusinessIdeaId);
            return View(bSFPrivate);
        }

        // GET: BSFPrivates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BSFPrivate == null)
            {
                return NotFound();
            }

            var bSFPrivate = await _context.BSFPrivate
                .Include(b => b.GeneralBusinessIdea)
                .FirstOrDefaultAsync(m => m.BSFPrivateId == id);
            if (bSFPrivate == null)
            {
                return NotFound();
            }

            return View(bSFPrivate);
        }

        // POST: BSFPrivates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BSFPrivate == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BSFPrivate'  is null.");
            }
            var bSFPrivate = await _context.BSFPrivate.FindAsync(id);
            if (bSFPrivate != null)
            {
                _context.BSFPrivate.Remove(bSFPrivate);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BSFPrivateExists(int id)
        {
          return _context.BSFPrivate.Any(e => e.BSFPrivateId == id);
        }
    }
}
