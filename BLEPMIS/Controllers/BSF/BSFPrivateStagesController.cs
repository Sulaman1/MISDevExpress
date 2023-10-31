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

namespace BLEPMIS.Controllers.BSF
{
    public class BSFPrivateStagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BSFPrivateStagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BSFPrivateStages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BSFPrivateStage.Include(b => b.BSFPrivate);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> _Index(int id)
        {
            var applicationDbContext = _context.BSFPrivateStage.Include(b => b.BSFPrivate).Where(a => a.BSFPrivateId == id);
            return PartialView(await applicationDbContext.ToListAsync());
        }

        // GET: BSFPrivateStages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BSFPrivateStage == null)
            {
                return NotFound();
            }

            var bSFPrivateStage = await _context.BSFPrivateStage
                .Include(b => b.BSFPrivate)
                .FirstOrDefaultAsync(m => m.BSFPrivateStageId == id);
            if (bSFPrivateStage == null)
            {
                return NotFound();
            }

            return View(bSFPrivateStage);
        }

        // GET: BSFPrivateStages/Create
        public async Task<IActionResult> Create(int id)
        {
            BSFPrivateStage bSF = new BSFPrivateStage();
            bSF.BSFPrivateId = id;
            bSF.CreatedBy = User.Identity.Name;
            var currentuser = await _userManager.GetUserAsync(User);
            bSF.UserId = currentuser.Id;
            var bsfPrivate = await _context.BSFPrivate.Include(a => a.GeneralBusinessIdea).Where(a => a.BSFPrivateId == id).FirstOrDefaultAsync();
            ViewBag.DepartmentName = bsfPrivate.GeneralBusinessIdea.GeneralBusinessIdeaName;
            var applicationDbContext = _context.BSFPrivateStage.Include(b => b.BSFPrivate).Where(a => a.BSFPrivateId == id);
            ViewBag.TotalGrant = _context.BSFPrivate.Find(id).TotalGrant;
            ViewBag.ReceivedGrant = applicationDbContext.Sum(a => a.AmountRelease);
            bSF.StageNumber = applicationDbContext.Count();
            ViewBag.ComletedStage = bSF.StageNumber;
            bSF.StageNumber++;
            ViewBag.MaxRelease = bsfPrivate.TotalGrant - applicationDbContext.Sum(a => a.AmountRelease);
            ViewBag.IsCompleted = bsfPrivate.IsCompleted;
            return View(bSF);
        }

        // POST: BSFPrivateStages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BSFPrivateStage bSFPrivateStage, IFormFile StageAttachment, IFormFile BeforeAttachment, IFormFile AfterAttachment)
        {
            if (ModelState.IsValid)
            {
                if (StageAttachment != null && StageAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Private\\Stage\\" + bSFPrivateStage.BSFPrivateId.ToString() + "\\");
                    string fileName = Path.GetFileName(StageAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "StageAttachment" + randomNumber.ToString() + Path.GetExtension(fileName);
                    bSFPrivateStage.StageAttachment = Path.Combine("/Documents/BSF/Private/Stage/" + bSFPrivateStage.BSFPrivateId.ToString() + "/" + fileName);//Server Path
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
                if (BeforeAttachment != null && BeforeAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\Before\\" + bSFPrivateStage.BSFPrivateId.ToString() + "\\");
                    string fileName = Path.GetFileName(BeforeAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "BeforeAttachment" + randomNumber.ToString() + Path.GetExtension(fileName);
                    bSFPrivateStage.BeforeAttachment = Path.Combine("/Documents/BSF/Gov/Before/" + bSFPrivateStage.BSFPrivateId.ToString() + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await BeforeAttachment.CopyToAsync(stream);
                    }
                }
                if (AfterAttachment != null && AfterAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Private\\After\\" + bSFPrivateStage.BSFPrivateId.ToString() + "\\");
                    string fileName = Path.GetFileName(AfterAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "AfterAttachment" + randomNumber.ToString() + Path.GetExtension(fileName);
                    bSFPrivateStage.AfterAttachment = Path.Combine("/Documents/BSF/Private/After/" + bSFPrivateStage.BSFPrivateId.ToString() + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await AfterAttachment.CopyToAsync(stream);
                    }
                }
                _context.Add(bSFPrivateStage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create), new { id = bSFPrivateStage.BSFPrivateId});
            }
            ViewData["BSFPrivateId"] = new SelectList(_context.BSFPrivate, "BSFPrivateId", "AccountTitle", bSFPrivateStage.BSFPrivateId);
            return View(bSFPrivateStage);
        }

        // GET: BSFPrivateStages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BSFPrivateStage == null)
            {
                return NotFound();
            }

            var bSFPrivateStage = await _context.BSFPrivateStage.FindAsync(id);
            if (bSFPrivateStage == null)
            {
                return NotFound();
            }
            ViewData["BSFPrivateId"] = new SelectList(_context.BSFPrivate, "BSFPrivateId", "AccountTitle", bSFPrivateStage.BSFPrivateId);
            return View(bSFPrivateStage);
        }

        // POST: BSFPrivateStages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BSFPrivateStageId,BSFPrivateId,StageNumber,StageName,StageAttachment,AmountRelease,OnDate,UserId,CreatedBy")] BSFPrivateStage bSFPrivateStage)
        {
            if (id != bSFPrivateStage.BSFPrivateStageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bSFPrivateStage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BSFPrivateStageExists(bSFPrivateStage.BSFPrivateStageId))
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
            ViewData["BSFPrivateId"] = new SelectList(_context.BSFPrivate, "BSFPrivateId", "AccountTitle", bSFPrivateStage.BSFPrivateId);
            return View(bSFPrivateStage);
        }

        // GET: BSFPrivateStages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BSFPrivateStage == null)
            {
                return NotFound();
            }

            var bSFPrivateStage = await _context.BSFPrivateStage
                .Include(b => b.BSFPrivate)
                .FirstOrDefaultAsync(m => m.BSFPrivateStageId == id);
            if (bSFPrivateStage == null)
            {
                return NotFound();
            }

            return View(bSFPrivateStage);
        }

        // POST: BSFPrivateStages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BSFPrivateStage == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BSFPrivateStage'  is null.");
            }
            var bSFPrivateStage = await _context.BSFPrivateStage.FindAsync(id);
            if (bSFPrivateStage != null)
            {
                _context.BSFPrivateStage.Remove(bSFPrivateStage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BSFPrivateStageExists(int id)
        {
          return _context.BSFPrivateStage.Any(e => e.BSFPrivateStageId == id);
        }
    }
}
