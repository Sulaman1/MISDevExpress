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

namespace BLEPMIS.Controllers.BSF
{
    public class BSFGovStagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BSFGovStagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BSFGovStages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BSFGovStage.Include(b => b.BSFGov);           
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> _Index(int id)
        {            
            var applicationDbContext = _context.BSFGovStage.Include(b => b.BSFGov).Where(a=>a.BSFGovId == id);
            ViewBag.IsCompleted = applicationDbContext.Select(a => a.BSFGov.IsCompleted).FirstOrDefault();
            return PartialView(await applicationDbContext.OrderBy(a=>a.BSFGovStageId).ToListAsync());
        }
        // GET: BSFGovStages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BSFGovStage == null)
            {
                return NotFound();
            }

            var bSFGovStage = await _context.BSFGovStage
                .Include(b => b.BSFGov)
                .FirstOrDefaultAsync(m => m.BSFGovStageId == id);
            if (bSFGovStage == null)
            {
                return NotFound();
            }

            return View(bSFGovStage);
        }

        // GET: BSFGovStages/Create
        public IActionResult Create(int id)
        {
            ViewData["BSFGovId"] = new SelectList(_context.BSFGov, "BSFGovId", "BusinessPlanAttachment");
            ViewData["Stage"] = new SelectList(_context.Stage, "Name", "Name");
            BSFGovStage obj = new BSFGovStage();
            obj.BSFGovId = id;
            obj.CreatedBy = User.Identity.Name;
            obj.UserId = "1";
            obj.IsCompleted = false;            
            obj.StageNumber = (_context.BSFGovStage.Where(a=>a.BSFGovId == id).Count() + 1).ToString();
            ViewBag.Id = id;
            ViewBag.IsCompleted = _context.BSFGov.Find(id).IsCompleted;
            obj.OnDate = DateTime.Today;            
            return View(obj);
        }

        // POST: BSFGovStages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BSFGovStage bSFGovStage, IFormFile StageAttachment, IFormFile BeforeAttachment, IFormFile AfterAttachment)
        {
            if (ModelState.IsValid)
            {
                if (StageAttachment != null && StageAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\Stage\\" + bSFGovStage.BSFGovId.ToString() + "\\");
                    string fileName = Path.GetFileName(StageAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "StageAttachment" + randomNumber.ToString() + Path.GetExtension(fileName);
                    bSFGovStage.StageAttachment = Path.Combine("/Documents/BSF/Gov/Stage/" + bSFGovStage.BSFGovId.ToString() + "/" + fileName);//Server Path
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
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\Before\\" + bSFGovStage.BSFGovId.ToString() + "\\");
                    string fileName = Path.GetFileName(BeforeAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "BeforeAttachment" + randomNumber.ToString() + Path.GetExtension(fileName);
                    bSFGovStage.BeforeAttachment = Path.Combine("/Documents/BSF/Gov/Before/" + bSFGovStage.BSFGovId.ToString() + "/" + fileName);//Server Path
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
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\After\\" + bSFGovStage.BSFGovId.ToString() + "\\");
                    string fileName = Path.GetFileName(AfterAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "AfterAttachment" + randomNumber.ToString() + Path.GetExtension(fileName);
                    bSFGovStage.AfterAttachment = Path.Combine("/Documents/BSF/Gov/After/" + bSFGovStage.BSFGovId.ToString() + "/" + fileName);//Server Path
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
                _context.Add(bSFGovStage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create), new {id = bSFGovStage.BSFGovId});
            }
            ViewData["BSFGovId"] = new SelectList(_context.BSFGov, "BSFGovId", "BusinessPlanAttachment", bSFGovStage.BSFGovId);
            ViewData["Stage"] = new SelectList(_context.Stage, "Name", "Name", bSFGovStage.StageName);
            return View(bSFGovStage);
        }

        // GET: BSFGovStages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BSFGovStage == null)
            {
                return NotFound();
            }

            var bSFGovStage = await _context.BSFGovStage.FindAsync(id);                        
            ViewBag.Id = id;                                               
            ViewData["BSFGovId"] = new SelectList(_context.BSFGov, "BSFGovId", "BusinessPlanAttachment", bSFGovStage.BSFGovId);
            ViewData["Stage"] = new SelectList(_context.Stage, "Name", "Name", bSFGovStage.StageName);
            return View(bSFGovStage);
        }

        // POST: BSFGovStages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BSFGovStage bSFGovStage, IFormFile StageAttachment, IFormFile BeforeAttachment, IFormFile AfterAttachment)
        {            

            if (ModelState.IsValid)
            {
                try
                {
                    if (StageAttachment != null && StageAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\Stage\\" + bSFGovStage.BSFGovId.ToString() + "\\");
                        string fileName = Path.GetFileName(StageAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "StageAttachment" + randomNumber.ToString() + Path.GetExtension(fileName);
                        bSFGovStage.StageAttachment = Path.Combine("/Documents/BSF/Gov/Stage/" + bSFGovStage.BSFGovId.ToString() + "/" + fileName);//Server Path
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
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\Before\\" + bSFGovStage.BSFGovId.ToString() + "\\");
                        string fileName = Path.GetFileName(BeforeAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "BeforeAttachment" + randomNumber.ToString() + Path.GetExtension(fileName);
                        bSFGovStage.BeforeAttachment = Path.Combine("/Documents/BSF/Gov/Before/" + bSFGovStage.BSFGovId.ToString() + "/" + fileName);//Server Path
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
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\BSF\\Gov\\After\\" + bSFGovStage.BSFGovId.ToString() + "\\");
                        string fileName = Path.GetFileName(AfterAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        fileName = "AfterAttachment" + randomNumber.ToString() + Path.GetExtension(fileName);
                        bSFGovStage.AfterAttachment = Path.Combine("/Documents/BSF/Gov/After/" + bSFGovStage.BSFGovId.ToString() + "/" + fileName);//Server Path
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
                    bSFGovStage.IsCompleted = true;
                    _context.Update(bSFGovStage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BSFGovStageExists(bSFGovStage.BSFGovStageId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Create), new {id = bSFGovStage.BSFGovId});
            }            
            ViewData["BSFGovId"] = new SelectList(_context.BSFGov, "BSFGovId", "BusinessPlanAttachment", bSFGovStage.BSFGovId);
            ViewData["Stage"] = new SelectList(_context.Stage, "Name", "Name", bSFGovStage.StageName);
            return View(bSFGovStage);
        }

        // GET: BSFGovStages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BSFGovStage == null)
            {
                return NotFound();
            }

            var bSFGovStage = await _context.BSFGovStage
                .Include(b => b.BSFGov)
                .FirstOrDefaultAsync(m => m.BSFGovStageId == id);
            if (bSFGovStage == null)
            {
                return NotFound();
            }

            return View(bSFGovStage);
        }

        // POST: BSFGovStages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BSFGovStage == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BSFGovStage'  is null.");
            }
            var bSFGovStage = await _context.BSFGovStage.FindAsync(id);
            if (bSFGovStage != null)
            {
                _context.BSFGovStage.Remove(bSFGovStage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BSFGovStageExists(int id)
        {
          return _context.BSFGovStage.Any(e => e.BSFGovStageId == id);
        }
    }
}
