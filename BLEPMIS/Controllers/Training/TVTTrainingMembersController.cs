using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBContext.Data;
using DAL.Models.Domain.Training;
using Microsoft.AspNetCore.Identity;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BLEPMIS.Controllers.Training
{
    public class TVTTrainingMembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TVTTrainingMembersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TVTTrainingMembers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TVTTrainingMember.Include(m => m.Member).Include(m => m.TVTTraining);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> _Index(int id)
        {
            var applicationDbContext = await _context.TVTTrainingMember.Include(m => m.Member).Include(m => m.TVTTraining).Where(a=>a.TVTTrainingId == id).ToListAsync();
            return PartialView(applicationDbContext);
        }
        public async Task<IActionResult> TrackMember(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> AjaxMemberInformation(string id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var Info = _context.Member.Where(a=>a.CNIC == id).FirstOrDefault();
                        
            if (Info != null)
            {
                return Json(new { isValid = true, Info, message = "Fetch data successfully!" });
            }
            
            return Json(new { isValid = false, Info, message = "Beneficiary data not found!" });
        }
        public async Task<JsonResult> AddBeneficiaryInTraining(int MId, int TId)
        {
            var result = _context.TVTTrainingMember.Count(a => a.Member.MemberId == MId /*&& a.TVTTrainingId == TId*/);
            if (MId == 0 || TId == 0)
            {
                return Json(new { isValid = false, message = "Failed to Add Member!" });
            }
            else if (result > 0)
            {
                return Json(new { isValid = false, message = "Selected member is already added!" });
            }
            else
            {                
                TVTTrainingMember obj = new TVTTrainingMember();                
                obj.TVTTrainingId = TId;
                //obj.Age = age;
                obj.MemberId = MId;
                obj.Designation = "";
                obj.IdentifiedBy = "";
                obj.PWD = "";
                obj.RPL = "";
                obj.PreferredSkill1 = "";
                obj.PreferredSkill2 = "";
                obj.PreferredSkill3 = "";
                obj.PreferredSkill4 = "";
                obj.SelfEmployed = "";                
                obj.CreatedOn = DateTime.Today.Date;
                //---------
                var TCode = _context.TVTTraining.Find(TId).TrainingCode;
                var TrainingMemberCount = _context.TVTTrainingMember.Count(a => a.TVTTrainingId == TId) + 1;
                string val = (TrainingMemberCount).ToString("D3");
                obj.BeneficiaryMISCode = (TCode + "-" + val);
                while (_context.TVTTrainingMember.Count(a => a.BeneficiaryMISCode == obj.BeneficiaryMISCode) > 0)
                {
                    val = (++TrainingMemberCount).ToString("D3");
                    obj.BeneficiaryMISCode = (TCode + "-" + val);
                }
                //---------
                _context.Add(obj);              
                await _context.SaveChangesAsync();
            }
            return Json(new { isValid = true, message = "Member has been added successfully." });
        }
        // GET: TVTTrainingMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TVTTrainingMember == null)
            {
                return NotFound();
            }

            var tVTTrainingMember = await _context.TVTTrainingMember
                .Include(m => m.Member)
                .Include(m => m.TVTTraining)
                .FirstOrDefaultAsync(m => m.TVTTrainingMemberId == id);
            if (tVTTrainingMember == null)
            {
                return NotFound();
            }            
            return View(tVTTrainingMember);
        }

        // GET: TVTTrainingMembers/Create
        public IActionResult Create()
        {
            ViewData["CommunityInstituteMemberId"] = new SelectList(_context.CommunityInstituteMember, "CommunityInstituteMemberId", "CNIC");
            ViewData["MemberTrainingId"] = new SelectList(_context.MemberTraining, "MemberTrainingId", "MemberTrainingId");
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TVTTrainingMember tVTTrainingMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tVTTrainingMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CommunityInstituteMemberId"] = new SelectList(_context.TVTTrainingMember, "TVTTrainingMemberId", "CNIC", tVTTrainingMember.CommunityInstituteMemberId);
            //ViewData["MemberTrainingId"] = new SelectList(_context.MemberTraining, "MemberTrainingId", "MemberTrainingId", tVTTrainingMember.MemberTrainingId);
            return View(tVTTrainingMember);
        }        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TVTTrainingMember == null)
            {
                return NotFound();
            }

            var tVTTrainingMember = await _context.TVTTrainingMember.FindAsync(id);
            if (tVTTrainingMember == null)
            {
                return NotFound();
            }
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "Name", "Name");
            ViewData["DesignationId"] = new SelectList(_context.Designation, "DesignationName", "DesignationName");
            //ViewData["MemberTrainingId"] = new SelectList(_context.MemberTraining, "MemberTrainingId", "MemberTrainingId", tVTTrainingMember.MemberTrainingId);
            return View(tVTTrainingMember);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TVTTrainingMember tVTTrainingMember, IFormFile EducationDocAttachment, IFormFile CNICAttachment, IFormFile AdmissionFormAttachment)
        {
            if (id != tVTTrainingMember.TVTTrainingMemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (EducationDocAttachment != null && EducationDocAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\TVTTrainingMember" + tVTTrainingMember.MemberId.ToString() + "\\EduDoc\\");
                        string fileName = Path.GetFileName(EducationDocAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "EduDoc" + randomNumber.ToString() + Path.GetExtension(fileName);
                        tVTTrainingMember.EducationDocAttachment = Path.Combine("/Documents/TVTTrainingMember" + tVTTrainingMember.MemberId.ToString() + "/EduDoc/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await EducationDocAttachment.CopyToAsync(stream);
                        }
                    }
                    if (CNICAttachment != null && CNICAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\TVTTrainingMember" + tVTTrainingMember.MemberId.ToString() + "\\CNIC\\");
                        string fileName = Path.GetFileName(CNICAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "CNIC" + randomNumber.ToString() + Path.GetExtension(fileName);
                        tVTTrainingMember.CNICAttachment = Path.Combine("/Documents/TVTTrainingMember" + tVTTrainingMember.MemberId.ToString() + "/CNIC/" + fileName);//Server Path
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
                    if (AdmissionFormAttachment != null && AdmissionFormAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\TVTTrainingMember" + tVTTrainingMember.MemberId.ToString() + "\\AForm\\");
                        string fileName = Path.GetFileName(AdmissionFormAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "AForm" + randomNumber.ToString() + Path.GetExtension(fileName);
                        tVTTrainingMember.AdmissionFormAttachment = Path.Combine("/Documents/TVTTrainingMember" + tVTTrainingMember.MemberId.ToString() + "/AForm/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await AdmissionFormAttachment.CopyToAsync(stream);
                        }
                    }
                    _context.Update(tVTTrainingMember);

                    var member = _context.Member.Find(tVTTrainingMember.MemberId);
                    member.Age = tVTTrainingMember.Age;
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TVTTrainingMemberExists(tVTTrainingMember.TVTTrainingMemberId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(TrackMember), "TVTTrainingMembers", new {id = tVTTrainingMember.TVTTrainingId});
            }
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "Name", "Name", tVTTrainingMember.IdentifiedBy);
            ViewData["DesignationId"] = new SelectList(_context.Designation, "DesignationName", "DesignationName", tVTTrainingMember.Designation);
            return View(tVTTrainingMember);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TVTTrainingMember == null)
            {
                return NotFound();
            }

            var tVTTrainingMember = await _context.TVTTrainingMember
                .Include(m => m.Member)
                .Include(m => m.TVTTrainingId)
                .FirstOrDefaultAsync(m => m.TVTTrainingMemberId == id);
            if (tVTTrainingMember == null)
            {
                return NotFound();
            }

            return View(tVTTrainingMember);
        }

        // POST: TVTTrainingMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TVTTrainingMember == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TVTTrainingMember'  is null.");
            }
            var tVTTrainingMember = await _context.TVTTrainingMember.FindAsync(id);
            if (tVTTrainingMember != null)
            {
                _context.TVTTrainingMember.Remove(tVTTrainingMember);
                await _context.SaveChangesAsync();
            }
                        
            return RedirectToAction(nameof(Details), "TVTTrainings", new {tVTTrainingMember.TVTTrainingId});
        }

        private bool TVTTrainingMemberExists(int id)
        {
          return _context.TVTTrainingMember.Any(e => e.TVTTrainingMemberId == id);
        }
    }
}
