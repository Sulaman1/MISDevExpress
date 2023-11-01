using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.BSF;
using DBContext.Data;
using Microsoft.AspNetCore.Identity;
using DAL.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using DAL.Models.Domain.HTSModule;
using static Constant.Constants.Permissions;
using GemBox.Document;

namespace CCView.Controllers.HTSModule
{
    public class HTSStagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HTSStagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: HTSStages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HTSStage.Include(b => b.HTS);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> _Index(int id)
        {
            var applicationDbContext = _context.HTSStage.Include(b => b.HTS).Where(a => a.HTSId == id);
            return PartialView(await applicationDbContext.ToListAsync());
        }

        // GET: HTSStages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HTSStage == null)
            {
                return NotFound();
            }

            var HTSStage = await _context.HTSStage
                .Include(b => b.HTS)
                .FirstOrDefaultAsync(m => m.HTSStageId == id);
            if (HTSStage == null)
            {
                return NotFound();
            }

            return View(HTSStage);
        }

        // GET: HTSStages/Create
        public async Task<IActionResult> Create(int id)
        {
            HTSStage hTS = new HTSStage();
            hTS.HTSId = id;
            hTS.CreatedBy = User.Identity.Name;
            var currentuser = await _userManager.GetUserAsync(User);
            hTS.UserId = currentuser.Id;
            var HTS = await _context.HTS.Where(a => a.HTSId == id).FirstOrDefaultAsync();            
            var applicationDbContext = _context.HTSStage.Include(b => b.HTS).Where(a => a.HTSId == id).ToList();
            ViewBag.TotalGrant = _context.HTS.Find(id).TotalGrant;
            ViewBag.ReceivedGrant = applicationDbContext.Sum(a => a.AmountPaid);
            hTS.InstallmentNo = applicationDbContext.Count();
            ViewBag.ComletedStage = hTS.InstallmentNo;
            hTS.InstallmentNo++;
            ViewBag.MaxRelease = HTS.TotalGrant - applicationDbContext.Sum(a => a.AmountPaid);
            ViewBag.IsCompleted = HTS.IsCompleted;            
            ViewBag.BName = _context.Member.Find(HTS.MemberId).MemberName;
            return View(hTS);
        }

        // POST: HTSStages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HTSStage HTSStage, IFormFile StageAttachment, IFormFile Picture1, IFormFile Picture2, IFormFile Picture3, IFormFile Picture4)
        {
            if (ModelState.IsValid)
            {
                if (StageAttachment != null && StageAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HTS\\Stage\\" + HTSStage.HTSId.ToString() + "\\");
                    string fileName = Path.GetFileName(StageAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "StageAttachment" + randomNumber.ToString() + Path.GetExtension(fileName);
                    HTSStage.StageAttachment = Path.Combine("/Documents/HTS/Stage/" + HTSStage.HTSId.ToString() + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await StageAttachment.CopyToAsync(stream);
                    }
                }
                if (Picture1 != null && Picture1.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\Pic1\\" + HTSStage.HTSId.ToString() + "\\");
                    string fileName = Path.GetFileName(Picture1.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "Pic1" + randomNumber.ToString() + Path.GetExtension(fileName);
                    HTSStage.Picture1 = Path.Combine("/Documents/BSF/Gov/Pic1/" + HTSStage.HTSId.ToString() + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await Picture1.CopyToAsync(stream);
                    }
                }
                if (Picture2 != null && Picture2.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\Pic2\\" + HTSStage.HTSId.ToString() + "\\");
                    string fileName = Path.GetFileName(Picture2.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "Pic2" + randomNumber.ToString() + Path.GetExtension(fileName);
                    HTSStage.Picture2 = Path.Combine("/Documents/BSF/Gov/Pic2/" + HTSStage.HTSId.ToString() + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await Picture2.CopyToAsync(stream);
                    }
                }
                if (Picture3 != null && Picture3.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\Pic3\\" + HTSStage.HTSId.ToString() + "\\");
                    string fileName = Path.GetFileName(Picture3.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "Pic3" + randomNumber.ToString() + Path.GetExtension(fileName);
                    HTSStage.Picture3 = Path.Combine("/Documents/BSF/Gov/Pic3/" + HTSStage.HTSId.ToString() + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await Picture3.CopyToAsync(stream);
                    }
                }
                if (Picture4 != null && Picture4.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\Pic4\\" + HTSStage.HTSId.ToString() + "\\");
                    string fileName = Path.GetFileName(Picture4.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "Pic4" + randomNumber.ToString() + Path.GetExtension(fileName);
                    HTSStage.Picture4 = Path.Combine("/Documents/BSF/Gov/Pic4/" + HTSStage.HTSId.ToString() + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await Picture4.CopyToAsync(stream);
                    }
                }
                _context.Add(HTSStage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create), new { id = HTSStage.HTSId});
            }
            ViewData["HTSId"] = new SelectList(_context.HTS, "HTSId", "AccountTitle", HTSStage.HTSId);
            return View(HTSStage);
        }

        // GET: HTSStages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HTSStage == null)
            {
                return NotFound();
            }

            var HTSStage = await _context.HTSStage.FindAsync(id);
            if (HTSStage == null)
            {
                return NotFound();
            }
            var HTS = await _context.HTS.Where(a => a.HTSId == HTSStage.HTSId).FirstOrDefaultAsync();
            var applicationDbContext = _context.HTSStage.Include(b => b.HTS).Where(a => a.HTSId == id).ToList();
            ViewBag.TotalGrant = _context.HTS.Find(HTS.HTSId).TotalGrant;
            ViewBag.ReceivedGrant = applicationDbContext.Sum(a => a.AmountPaid);            
            ViewBag.ComletedStage = HTSStage.InstallmentNo;            
            ViewBag.MaxRelease = HTS.TotalGrant - applicationDbContext.Sum(a => a.AmountPaid);
            ViewBag.IsCompleted = HTS.IsCompleted;
            ViewBag.BName = _context.Member.Find(HTS.MemberId).MemberName;
            return View(HTSStage);
        }

        // POST: HTSStages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HTSStage HTSStage,IFormFile StageAttachment, IFormFile Picture1, IFormFile Picture2, IFormFile Picture3, IFormFile Picture4)
        {
            if (id != HTSStage.HTSStageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (StageAttachment != null && StageAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HTS\\Stage\\" + HTSStage.HTSId.ToString() + "\\");
                        string fileName = Path.GetFileName(StageAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "StageAttachment" + randomNumber.ToString() + Path.GetExtension(fileName);
                        HTSStage.StageAttachment = Path.Combine("/Documents/HTS/Stage/" + HTSStage.HTSId.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await StageAttachment.CopyToAsync(stream);
                        }
                    }
                    if (Picture1 != null && Picture1.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\Pic1\\" + HTSStage.HTSId.ToString() + "\\");
                        string fileName = Path.GetFileName(Picture1.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "Pic1" + randomNumber.ToString() + Path.GetExtension(fileName);
                        HTSStage.Picture1 = Path.Combine("/Documents/BSF/Gov/Pic1/" + HTSStage.HTSId.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await Picture1.CopyToAsync(stream);
                        }
                    }
                    if (Picture2 != null && Picture2.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\Pic2\\" + HTSStage.HTSId.ToString() + "\\");
                        string fileName = Path.GetFileName(Picture2.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "Pic2" + randomNumber.ToString() + Path.GetExtension(fileName);
                        HTSStage.Picture2 = Path.Combine("/Documents/BSF/Gov/Pic2/" + HTSStage.HTSId.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await Picture2.CopyToAsync(stream);
                        }
                    }
                    if (Picture3 != null && Picture3.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\Pic3\\" + HTSStage.HTSId.ToString() + "\\");
                        string fileName = Path.GetFileName(Picture3.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "Pic3" + randomNumber.ToString() + Path.GetExtension(fileName);
                        HTSStage.Picture3 = Path.Combine("/Documents/BSF/Gov/Pic3/" + HTSStage.HTSId.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await Picture3.CopyToAsync(stream);
                        }
                    }
                    if (Picture4 != null && Picture4.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\Pic4\\" + HTSStage.HTSId.ToString() + "\\");
                        string fileName = Path.GetFileName(Picture4.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "Pic4" + randomNumber.ToString() + Path.GetExtension(fileName);
                        HTSStage.Picture4 = Path.Combine("/Documents/BSF/Gov/Pic4/" + HTSStage.HTSId.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await Picture4.CopyToAsync(stream);
                        }
                    }
                    _context.Update(HTSStage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HTSStageExists(HTSStage.HTSStageId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Create), new { id = HTSStage.HTSId});
            }
            ViewData["HTSId"] = new SelectList(_context.HTS, "HTSId", "AccountTitle", HTSStage.HTSId);
            return View(HTSStage);
        }

        // GET: HTSStages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HTSStage == null)
            {
                return NotFound();
            }

            var HTSStage = await _context.HTSStage
                .Include(b => b.HTS)
                .FirstOrDefaultAsync(m => m.HTSStageId == id);
            if (HTSStage == null)
            {
                return NotFound();
            }

            return View(HTSStage);
        }

        // POST: HTSStages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HTSStage == null)
            {
                return Problem("Entity set 'ApplicationDbContext.HTSStage'  is null.");
            }
            var HTSStage = await _context.HTSStage.FindAsync(id);
            if (HTSStage != null)
            {
                _context.HTSStage.Remove(HTSStage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HTSStageExists(int id)
        {
          return _context.HTSStage.Any(e => e.HTSStageId == id);
        }
    }
}
