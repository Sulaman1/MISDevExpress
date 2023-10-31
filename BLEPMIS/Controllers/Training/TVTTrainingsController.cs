using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBContext.Data;
using DAL.Models.Domain.Training;
using DAL.Models.Domain.MasterSetup;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Identity;
using DAL.Models;
using System.Diagnostics.Metrics;
using static Constant.Constants.Permissions;

namespace BLEPMIS.Controllers.Training
{
    public class TVTTrainingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TVTTrainingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TVTTrainings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.TVTTraining.Include(m => m.TrainingType.TrainingHead).ToListAsync();
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentUser.DistrictId).ToList();
            }
            return View(applicationDbContext);
        }

        // GET: TVTTrainings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TVTTraining == null)
            {
                return NotFound();
            }

            var tVTTraining = await _context.TVTTraining                 
                .Include(a=>a.Village.UnionCouncil.Tehsil.District)                
                .Include(m => m.TrainingType.TrainingHead)
                .FirstOrDefaultAsync(m => m.TVTTrainingId == id);
            if (tVTTraining == null)
            {
                return NotFound();
            }            
            return View(tVTTraining);
        }
        public async Task<JsonResult> GetTrainingTypes(int trainingHeadId)
        {
            List<TrainingType> trainings = await _context.TrainingType.Where(a => a.TrainingHeadId == trainingHeadId).ToListAsync();
            var trainingList = trainings.Select(m => new SelectListItem()
            {
                Text = m.TrainingTypeName.ToString(),
                Value = m.TrainingTypeId.ToString(),
            });
            return Json(trainingList);
        }
        public async Task<JsonResult> GetEmployees(int sectionId)
        {
            List<Employee> emplyees = await _context.Employee.Where(a => a.SectionId == sectionId).ToListAsync();
            var employeeList = emplyees.Select(m => new SelectListItem()
            {
                Text = m.EmployeeName.ToString(),
                Value = m.EmployeeId.ToString(),
            });
            return Json(employeeList);
        }
        // GET: TVTTrainings/Create
        public async Task<IActionResult> Create()
        {
            ViewData["TVTTrainedById"] = new SelectList(_context.TVTTrainedBy, "Name", "Name");
            var THead = _context.TrainingHead;
            ViewData["TrainingHeadId"] = new SelectList(THead.Where(a=>a.TrainingHeadId == 3), "TrainingHeadId", "TrainingHeadName");            
            var districtAccess = _context.District.Where(a => a.DistrictId > 1);
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser.DistrictId > 1)
            {
                districtAccess = districtAccess.Where(a => a.DistrictId == currentUser.DistrictId);
            }
            ViewData["DistrictId"] = new SelectList(districtAccess, "DistrictId", "Name");
            //-----------------------------------------------            
            TVTTraining obj = new TVTTraining();            
            obj.CreatedOn = DateTime.Today.Date;            
            return View(obj);
        }

        // POST: TVTTrainings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TVTTraining tVTTraining, int TrainingHeadId, IFormFile AttendanceAttachment, IFormFile ReportAttachment, IFormFile SessionPlanAttachment, IFormFile PictureAttachment1, IFormFile PictureAttachment2, IFormFile PictureAttachment3, IFormFile PictureAttachment4)
        {
            if (ModelState.IsValid)
            {
                var TrainingCount = _context.TVTTraining.Count(a => a.TrainingTypeId == tVTTraining.TrainingTypeId) + 1;
                var TrainingTypeInfo = _context.TrainingType.Find(tVTTraining.TrainingTypeId);
                var DistrictCode = _context.District.Find(tVTTraining.DistrictId).Code;
                string TrainingCode = DistrictCode + "-" + _context.TrainingHead.Find(TrainingTypeInfo.TrainingHeadId).TrainingHeadCode + "-" + TrainingTypeInfo.TrainingTypeCode;
                string val = (TrainingCount).ToString("D3");
                tVTTraining.TrainingCode = (TrainingCode + "-" + val);
                while (_context.TVTTraining.Count(a => a.TrainingCode == tVTTraining.TrainingCode) > 0)
                {
                    val = (++TrainingCount).ToString("D3");
                    tVTTraining.TrainingCode = (TrainingCode + "-" + val);
                }
                if (AttendanceAttachment != null && AttendanceAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training"+ tVTTraining.TrainingTypeId +"\\AttendanceSheet\\");
                    string fileName = Path.GetFileName(AttendanceAttachment.FileName);                    
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 999999);
                    fileName = "AttendanceSheet" + randomNumber.ToString() + Path.GetExtension(fileName);
                    tVTTraining.AttendanceAttachment = Path.Combine("/Documents/Training" + tVTTraining.TrainingTypeId + "/AttendanceSheet/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await AttendanceAttachment.CopyToAsync(stream);
                    }                                                                                  
                }
                if (ReportAttachment != null && ReportAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + tVTTraining.TrainingTypeId + "\\Report\\");
                    string fileName = Path.GetFileName(ReportAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 999999);
                    fileName = "Report" + randomNumber.ToString() + Path.GetExtension(fileName);
                    tVTTraining.ReportAttachment = Path.Combine("/Documents/Training" + tVTTraining.TrainingTypeId + "/Report/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await ReportAttachment.CopyToAsync(stream);
                    }
                }
                if (SessionPlanAttachment != null && SessionPlanAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + tVTTraining.TrainingTypeId + "\\SessionPlan\\");
                    string fileName = Path.GetFileName(SessionPlanAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 999999);
                    fileName = "SessionPlan" + randomNumber.ToString() + Path.GetExtension(fileName);
                    tVTTraining.SessionPlanAttachment = Path.Combine("/Documents/Training" + tVTTraining.TrainingTypeId + "/SessionPlan/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await SessionPlanAttachment.CopyToAsync(stream);
                    }
                }
                if (PictureAttachment1 != null && PictureAttachment1.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + tVTTraining.TrainingTypeId + "\\Pictures\\");
                    string fileName = Path.GetFileName(PictureAttachment1.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 999999);
                    fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                    tVTTraining.PictureAttachment1 = Path.Combine("/Documents/Training" + tVTTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await PictureAttachment1.CopyToAsync(stream);
                    }
                }
                if (PictureAttachment2 != null && PictureAttachment2.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + tVTTraining.TrainingTypeId + "\\Pictures\\");
                    string fileName = Path.GetFileName(PictureAttachment2.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 999999);
                    fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                    tVTTraining.PictureAttachment2 = Path.Combine("/Documents/Training" + tVTTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await PictureAttachment2.CopyToAsync(stream);
                    }
                }
                if (PictureAttachment3 != null && PictureAttachment3.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + tVTTraining.TrainingTypeId + "\\Pictures\\");
                    string fileName = Path.GetFileName(PictureAttachment3.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 999999);
                    fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                    tVTTraining.PictureAttachment3 = Path.Combine("/Documents/Training" + tVTTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await PictureAttachment3.CopyToAsync(stream);
                    }
                }
                if (PictureAttachment4 != null && PictureAttachment4.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + tVTTraining.TrainingTypeId + "\\Pictures\\");
                    string fileName = Path.GetFileName(PictureAttachment4.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 999999);
                    fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                    tVTTraining.PictureAttachment4 = Path.Combine("/Documents/Training" + tVTTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await PictureAttachment4.CopyToAsync(stream);
                    }
                }
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);                
                tVTTraining.District = _context.District.Find(tVTTraining.DistrictId).Name;
                _context.Add(tVTTraining);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TVTTrainedById"] = new SelectList(_context.TVTTrainedBy, "Name", "Name", tVTTraining.TVTTrainer);
            ViewData["TrainingHeadId"] = new SelectList(_context.TrainingHead.Where(a => a.TrainingHeadId == 3), "TrainingHeadId", "TrainingHeadName", TrainingHeadId);
            ViewData["TrainingTypeId"] = new SelectList(_context.TrainingType.Where(a=>a.TrainingHeadId == TrainingHeadId), "TrainingTypeId", "TrainingTypeName", TrainingHeadId);
            return View(tVTTraining);
        }

        // GET: TVTTrainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TVTTraining == null)
            {
                return NotFound();
            }

            var tVTTraining = await _context.TVTTraining.Include(a=>a.Village.UnionCouncil.Tehsil).Where(a=>a.TVTTrainingId == id).FirstOrDefaultAsync();
            if (tVTTraining == null)
            {
                return NotFound();
            }
            int TrainingHeadId = _context.TrainingType.Find(tVTTraining.TrainingTypeId).TrainingHeadId;
            ViewData["TVTTrainedById"] = new SelectList(_context.TVTTrainedBy, "Name", "Name", tVTTraining.TVTTrainer);            
            ViewData["TrainingHeadId"] = new SelectList(_context.TrainingHead.Where(a => a.TrainingHeadId == 3), "TrainingHeadId", "TrainingHeadName", TrainingHeadId);
            ViewData["TrainingTypeId"] = new SelectList(_context.TrainingType.Where(a => a.TrainingHeadId == TrainingHeadId), "TrainingTypeId", "TrainingTypeName", TrainingHeadId);
            var districtAccess = _context.District.Where(a => a.DistrictId == tVTTraining.DistrictId);
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);           
            ViewData["DistrictId"] = new SelectList(districtAccess, "DistrictId", "Name");
            ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == tVTTraining.DistrictId), "TehsilId", "TehsilName", tVTTraining.Village.UnionCouncil.TehsilId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == tVTTraining.Village.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", tVTTraining.Village.UnionCouncilId);
            ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == tVTTraining.Village.UnionCouncilId), "VillageId", "Name", tVTTraining.VillageId);
            ViewBag.IsAllow = true;            
            return View(tVTTraining);
        }

        // POST: TVTTrainings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TVTTraining tVTTraining, int TrainingHeadId, IFormFile? AttendanceAttachment, IFormFile? ReportAttachment, IFormFile? SessionPlanAttachment, IFormFile? PictureAttachment1, IFormFile? PictureAttachment2, IFormFile? PictureAttachment3, IFormFile? PictureAttachment4)
        {
            if (id != tVTTraining.TVTTrainingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (AttendanceAttachment != null && AttendanceAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + tVTTraining.TrainingTypeId + "\\AttendanceSheet\\");
                        string fileName = Path.GetFileName(AttendanceAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 999999);
                        fileName = "AttendanceSheet" + randomNumber.ToString() + Path.GetExtension(fileName);
                        tVTTraining.AttendanceAttachment = Path.Combine("/Documents/Training" + tVTTraining.TrainingTypeId + "/AttendanceSheet/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await AttendanceAttachment.CopyToAsync(stream);
                        }
                    }
                    if (ReportAttachment != null && ReportAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + tVTTraining.TrainingTypeId + "\\Report\\");
                        string fileName = Path.GetFileName(ReportAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 999999);
                        fileName = "Report" + randomNumber.ToString() + Path.GetExtension(fileName);
                        tVTTraining.ReportAttachment = Path.Combine("/Documents/Training" + tVTTraining.TrainingTypeId + "/Report/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await ReportAttachment.CopyToAsync(stream);
                        }
                    }
                    if (SessionPlanAttachment != null && SessionPlanAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + tVTTraining.TrainingTypeId + "\\SessionPlan\\");
                        string fileName = Path.GetFileName(SessionPlanAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 999999);
                        fileName = "SessionPlan" + randomNumber.ToString() + Path.GetExtension(fileName);
                        tVTTraining.SessionPlanAttachment = Path.Combine("/Documents/Training" + tVTTraining.TrainingTypeId + "/SessionPlan/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await SessionPlanAttachment.CopyToAsync(stream);
                        }
                    }
                    if (PictureAttachment1 != null && PictureAttachment1.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + tVTTraining.TrainingTypeId + "\\Pictures\\");
                        string fileName = Path.GetFileName(PictureAttachment1.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 999999);
                        fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                        tVTTraining.PictureAttachment1 = Path.Combine("/Documents/Training" + tVTTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await PictureAttachment1.CopyToAsync(stream);
                        }
                    }
                    if (PictureAttachment2 != null && PictureAttachment2.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + tVTTraining.TrainingTypeId + "\\Pictures\\");
                        string fileName = Path.GetFileName(PictureAttachment2.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 999999);
                        fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                        tVTTraining.PictureAttachment2 = Path.Combine("/Documents/Training" + tVTTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await PictureAttachment2.CopyToAsync(stream);
                        }
                    }
                    if (PictureAttachment3 != null && PictureAttachment3.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + tVTTraining.TrainingTypeId + "\\Pictures\\");
                        string fileName = Path.GetFileName(PictureAttachment3.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 999999);
                        fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                        tVTTraining.PictureAttachment3 = Path.Combine("/Documents/Training" + tVTTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await PictureAttachment3.CopyToAsync(stream);
                        }
                    }
                    if (PictureAttachment4 != null && PictureAttachment4.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + tVTTraining.TrainingTypeId + "\\Pictures\\");
                        string fileName = Path.GetFileName(PictureAttachment4.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 999999);
                        fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                        tVTTraining.PictureAttachment4 = Path.Combine("/Documents/Training" + tVTTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await PictureAttachment4.CopyToAsync(stream);
                        }
                    }
                    tVTTraining.District = _context.District.Find(tVTTraining.DistrictId).Name;
                    _context.Update(tVTTraining);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TVTTrainingExists(tVTTraining.TVTTrainingId))
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
            ViewData["TVTTrainedById"] = new SelectList(_context.TVTTrainedBy, "Name", "Name", tVTTraining.TVTTrainer);
            ViewData["TrainingHeadId"] = new SelectList(_context.TrainingHead.Where(a => a.TrainingHeadId == 3), "TrainingHeadId", "TrainingHeadName", TrainingHeadId);
            ViewData["TrainingTypeId"] = new SelectList(_context.TrainingType.Where(a => a.TrainingHeadId == TrainingHeadId), "TrainingTypeId", "TrainingTypeName", TrainingHeadId);
            ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == tVTTraining.Village.UnionCouncilId), "VillageId", "Name", tVTTraining.VillageId);
            return View(tVTTraining);
        }
        public async Task<JsonResult> GetTehsils(int districtId)
        {
            List<Tehsil> tehsils = await _context.Tehsil.Where(a => a.DistrictId == districtId).ToListAsync();
            var tehsilList = tehsils.Select(m => new SelectListItem()
            {
                Text = m.TehsilName.ToString(),
                Value = m.TehsilId.ToString(),
            });
            return Json(tehsilList);
        }
        public async Task<JsonResult> GetUCs(int tehsilId)
        {
            List<UnionCouncil> unionCouncils = await _context.UnionCouncil.Where(a => a.TehsilId == tehsilId).ToListAsync();
            var UCList = unionCouncils.Select(m => new SelectListItem()
            {
                Text = m.UnionCouncilName.ToString(),
                Value = m.UnionCouncilId.ToString(),
            });
            return Json(UCList);
        }
        // GET: TVTTrainings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TVTTraining == null)
            {
                return NotFound();
            }

            var tVTTraining = await _context.TVTTraining                
                .Include(m => m.TrainingType)
                .FirstOrDefaultAsync(m => m.TVTTrainingId == id);
            if (tVTTraining == null)
            {
                return NotFound();
            }

            return View(tVTTraining);
        }

        // POST: TVTTrainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TVTTraining == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TVTTraining'  is null.");
            }
            var tVTTraining = await _context.TVTTraining.FindAsync(id);
            if (tVTTraining != null)
            {
                _context.TVTTraining.Remove(tVTTraining);
                await _context.SaveChangesAsync();
            }
                        
            return RedirectToAction(nameof(Index));
        }

        private bool TVTTrainingExists(int id)
        {
          return _context.TVTTraining.Any(e => e.TVTTrainingId == id);
        }
    }
}
