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

namespace BLEPMIS.Controllers.Training
{
    public class LIPTrainingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LIPTrainingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: LIPTrainings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LIPTraining.Include(m => m.Employee).Include(m => m.TrainingType);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> IndexIDO()
        {
            //var applicationDbContext = await _context.LIPAssetTransfer.Include(l => l.LIPPackage).Include(a=>a.Member).Include(l => l.Village.UnionCouncil).Include(a=>a.IdentifiedBy).Where(a=>a.IsSubmitted == false).ToListAsync();
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.IdentifiedBy).Select(a => new LIPAssetTransfer { LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, Member = new Member { MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC }, IsSubmitted = a.IsSubmitted, DistrictId = a.DistrictId }).Where(a => a.IsSubmitted == false).ToListAsync();
            //LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, 
            //IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, 
            //Member = new Member { FatherName = a.Member.FatherName, MemberName = a.Member.MemberName, CNIC = a.Member.CNIC }                 
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(applicationDbContext);
        }

        // GET: LIPTrainings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LIPTraining == null)
            {
                return NotFound();
            }

            var LIPTraining = await _context.LIPTraining
                .Include(m => m.Employee)
                .Include(a => a.Village.UnionCouncil.Tehsil.District)
                .Include(m => m.TrainingType.TrainingHead)
                .FirstOrDefaultAsync(m => m.LIPTrainingId == id);
            if (LIPTraining == null)
            {
                return NotFound();
            }
            return View(LIPTraining);
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
        // GET: LIPTrainings/Create
        public async Task<IActionResult> Create()
        {
            ViewData["TrainingById"] = new SelectList(_context.Section.Where(a => a.SectionId == 2), "SectionId", "Name");
            var THead = _context.TrainingHead;
            ViewData["TrainingHeadId"] = new SelectList(THead, "TrainingHeadId", "TrainingHeadName");
            ViewData["EmployeeId"] = new SelectList(_context.Employee.Where(a => a.SectionId == 2), "EmployeeId", "EmployeeName");
            var districtAccess = _context.District.Where(a => a.DistrictId > 1);
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser.DistrictId > 1)
            {
                districtAccess = districtAccess.Where(a => a.DistrictId == currentUser.DistrictId);
            }
            ViewData["DistrictId"] = new SelectList(districtAccess, "DistrictId", "Name");
            //-----------------------------------------------            
            LIPTraining obj = new LIPTraining();
            obj.TrainingOn = DateTime.Today.Date;
            return View(obj);
        }

        // POST: LIPTrainings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LIPTraining LIPTraining, int TrainingHeadId, IFormFile AttendanceAttachment, IFormFile ReportAttachment, IFormFile SessionPlanAttachment, IFormFile PictureAttachment1, IFormFile PictureAttachment2, IFormFile PictureAttachment3, IFormFile PictureAttachment4)
        {
            if (ModelState.IsValid)
            {
                if (AttendanceAttachment != null && AttendanceAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + LIPTraining.TrainingTypeId + "\\AttendanceSheet\\");
                    string fileName = Path.GetFileName(AttendanceAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(999, 100000);
                    fileName = "AttendanceSheet" + randomNumber.ToString() + Path.GetExtension(fileName);
                    LIPTraining.AttendanceAttachment = Path.Combine("/Documents/Training" + LIPTraining.TrainingTypeId + "/AttendanceSheet/" + fileName);//Server Path
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
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + LIPTraining.TrainingTypeId + "\\Report\\");
                    string fileName = Path.GetFileName(ReportAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(999, 100000);
                    fileName = "Report" + randomNumber.ToString() + Path.GetExtension(fileName);
                    LIPTraining.ReportAttachment = Path.Combine("/Documents/Training" + LIPTraining.TrainingTypeId + "/Report/" + fileName);//Server Path
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
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + LIPTraining.TrainingTypeId + "\\SessionPlan\\");
                    string fileName = Path.GetFileName(SessionPlanAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(999, 100000);
                    fileName = "SessionPlan" + randomNumber.ToString() + Path.GetExtension(fileName);
                    LIPTraining.SessionPlanAttachment = Path.Combine("/Documents/Training" + LIPTraining.TrainingTypeId + "/SessionPlan/" + fileName);//Server Path
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
                if (PictureAttachment1 != null && PictureAttachment1.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + LIPTraining.TrainingTypeId + "\\Pictures\\");
                    string fileName = Path.GetFileName(PictureAttachment1.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(999, 100000);
                    fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                    LIPTraining.PictureAttachment1 = Path.Combine("/Documents/Training" + LIPTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
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
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + LIPTraining.TrainingTypeId + "\\Pictures\\");
                    string fileName = Path.GetFileName(PictureAttachment2.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(999, 100000);
                    fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                    LIPTraining.PictureAttachment2 = Path.Combine("/Documents/Training" + LIPTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
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
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + LIPTraining.TrainingTypeId + "\\Pictures\\");
                    string fileName = Path.GetFileName(PictureAttachment3.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(999, 100000);
                    fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                    LIPTraining.PictureAttachment3 = Path.Combine("/Documents/Training" + LIPTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
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
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + LIPTraining.TrainingTypeId + "\\Pictures\\");
                    string fileName = Path.GetFileName(PictureAttachment4.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(999, 100000);
                    fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                    LIPTraining.PictureAttachment4 = Path.Combine("/Documents/Training" + LIPTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
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
                _context.Add(LIPTraining);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainingById"] = new SelectList(_context.Section.Where(a => a.SectionId == 2), "SectionId", "Name", LIPTraining.EmployeeId);
            ViewData["TrainingHeadId"] = new SelectList(_context.TrainingHead, "TrainingHeadId", "TrainingHeadName", TrainingHeadId);
            ViewData["TrainingTypeId"] = new SelectList(_context.TrainingType.Where(a => a.TrainingHeadId == TrainingHeadId), "TrainingTypeId", "TrainingTypeName", TrainingHeadId);
            return View(LIPTraining);
        }

        // GET: LIPTrainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LIPTraining == null)
            {
                return NotFound();
            }

            var LIPTraining = await _context.LIPTraining.Include(a => a.Village.UnionCouncil).Where(a => a.LIPTrainingId == id).FirstOrDefaultAsync();
            if (LIPTraining == null)
            {
                return NotFound();
            }
            int TrainingHeadId = _context.TrainingType.Find(LIPTraining.TrainingTypeId).TrainingHeadId;
            ViewData["TrainingById"] = new SelectList(_context.Section.Where(a => a.SectionId == 2), "SectionId", "Name", LIPTraining.EmployeeId);
            ViewData["EmployeeId"] = new SelectList(_context.Employee.Where(a => a.SectionId == 2), "EmployeeId", "EmployeeName", LIPTraining.EmployeeId);
            ViewData["TrainingHeadId"] = new SelectList(_context.TrainingHead, "TrainingHeadId", "TrainingHeadName", TrainingHeadId);
            ViewData["TrainingTypeId"] = new SelectList(_context.TrainingType.Where(a => a.TrainingHeadId == TrainingHeadId), "TrainingTypeId", "TrainingTypeName", TrainingHeadId);
            var districtAccess = _context.District.Where(a => a.DistrictId == LIPTraining.DistrictId);
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewData["DistrictId"] = new SelectList(districtAccess, "DistrictId", "Name");
            ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == LIPTraining.DistrictId), "TehsilId", "TehsilName", LIPTraining.Village.UnionCouncil.TehsilId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == LIPTraining.Village.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", LIPTraining.Village.UnionCouncilId);
            ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == LIPTraining.Village.UnionCouncilId), "VillageId", "Name", LIPTraining.VillageId);
            return View(LIPTraining);
        }

        // POST: LIPTrainings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LIPTraining LIPTraining, int TrainingHeadId, IFormFile AttendanceAttachment, IFormFile ReportAttachment, IFormFile SessionPlanAttachment, IFormFile PictureAttachment1, IFormFile PictureAttachment2, IFormFile PictureAttachment3, IFormFile PictureAttachment4)
        {
            if (id != LIPTraining.LIPTrainingId)
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
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + LIPTraining.TrainingTypeId + "\\AttendanceSheet\\");
                        string fileName = Path.GetFileName(AttendanceAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "AttendanceSheet" + randomNumber.ToString() + Path.GetExtension(fileName);
                        LIPTraining.AttendanceAttachment = Path.Combine("/Documents/Training" + LIPTraining.TrainingTypeId + "/AttendanceSheet/" + fileName);//Server Path
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
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + LIPTraining.TrainingTypeId + "\\Report\\");
                        string fileName = Path.GetFileName(ReportAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "Report" + randomNumber.ToString() + Path.GetExtension(fileName);
                        LIPTraining.ReportAttachment = Path.Combine("/Documents/Training" + LIPTraining.TrainingTypeId + "/Report/" + fileName);//Server Path
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
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + LIPTraining.TrainingTypeId + "\\SessionPlan\\");
                        string fileName = Path.GetFileName(SessionPlanAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "SessionPlan" + randomNumber.ToString() + Path.GetExtension(fileName);
                        LIPTraining.SessionPlanAttachment = Path.Combine("/Documents/Training" + LIPTraining.TrainingTypeId + "/SessionPlan/" + fileName);//Server Path
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
                    if (PictureAttachment1 != null && PictureAttachment1.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + LIPTraining.TrainingTypeId + "\\Pictures\\");
                        string fileName = Path.GetFileName(PictureAttachment1.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                        LIPTraining.PictureAttachment1 = Path.Combine("/Documents/Training" + LIPTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
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
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + LIPTraining.TrainingTypeId + "\\Pictures\\");
                        string fileName = Path.GetFileName(PictureAttachment2.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                        LIPTraining.PictureAttachment2 = Path.Combine("/Documents/Training" + LIPTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
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
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + LIPTraining.TrainingTypeId + "\\Pictures\\");
                        string fileName = Path.GetFileName(PictureAttachment3.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                        LIPTraining.PictureAttachment3 = Path.Combine("/Documents/Training" + LIPTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
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
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\Training" + LIPTraining.TrainingTypeId + "\\Pictures\\");
                        string fileName = Path.GetFileName(PictureAttachment4.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "Picture" + randomNumber.ToString() + Path.GetExtension(fileName);
                        LIPTraining.PictureAttachment4 = Path.Combine("/Documents/Training" + LIPTraining.TrainingTypeId + "/Pictures/" + fileName);//Server Path
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
                    _context.Update(LIPTraining);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LIPTrainingExists(LIPTraining.LIPTrainingId))
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
            ViewData["TrainingById"] = new SelectList(_context.Section.Where(a => a.SectionId == 2), "SectionId", "Name");
            ViewData["EmployeeId"] = new SelectList(_context.Employee.Where(a => a.SectionId == 2), "EmployeeId", "EmployeeName", LIPTraining.EmployeeId);
            ViewData["TrainingHeadId"] = new SelectList(_context.TrainingHead, "TrainingHeadId", "TrainingHeadName", TrainingHeadId);
            ViewData["TrainingTypeId"] = new SelectList(_context.TrainingType.Where(a => a.TrainingHeadId == TrainingHeadId), "TrainingTypeId", "TrainingTypeName", TrainingHeadId);
            return View(LIPTraining);
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
        // GET: LIPTrainings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LIPTraining == null)
            {
                return NotFound();
            }

            var LIPTraining = await _context.LIPTraining
                .Include(m => m.Employee)
                .Include(m => m.TrainingType)
                .FirstOrDefaultAsync(m => m.LIPTrainingId == id);
            if (LIPTraining == null)
            {
                return NotFound();
            }

            return View(LIPTraining);
        }

        // POST: LIPTrainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LIPTraining == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LIPTraining'  is null.");
            }
            var LIPTraining = await _context.LIPTraining.FindAsync(id);
            if (LIPTraining != null)
            {
                _context.LIPTraining.Remove(LIPTraining);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool LIPTrainingExists(int id)
        {
            return _context.LIPTraining.Any(e => e.LIPTrainingId == id);
        }
    }
}
