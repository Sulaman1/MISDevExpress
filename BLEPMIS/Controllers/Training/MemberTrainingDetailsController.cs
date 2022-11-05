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

namespace BLEPMIS.Controllers.Training
{
    public class MemberTrainingDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public MemberTrainingDetailsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MemberTrainingDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MemberTrainingDetail.Include(m => m.CommunityInstituteMember).Include(m => m.MemberTraining);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> _Index(int id)
        {
            var applicationDbContext = _context.MemberTrainingDetail.Include(m => m.CommunityInstituteMember.Member).Include(m => m.MemberTraining).Include(a=>a.CommunityInstituteMember.Designation).Where(a=>a.MemberTrainingId == id);
            return PartialView(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> TrackMember(int id)
        {
            ViewBag.MTId = id;            
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> AjaxMemberInformation(string id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var Info = _context.CommunityInstituteMember.Include(a=>a.CommunityInstitution).Include(a=>a.Member).Where(a=>a.Member.CNIC == id && a.Member.BeneficiaryTypeId == 1 && a.CommunityInstitution.IsVerified == true).FirstOrDefault();           
            if(currentUser.DistrictId > 1)
            {
                Info = Info.CommunityInstitution.DistrictId == currentUser.DistrictId ? Info : null;
            }
            if (Info == null)
            {
                return Json(new { isValid = false});
            }
            var AnyOtherTraining = _context.MemberTrainingDetail.Include(a=>a.CommunityInstituteMember.CommunityInstitution.UnionCouncil.Tehsil).Include(a => a.MemberTraining.TrainingType.TrainingHead).Where(a => a.CommunityInstituteMember.MemberId == Info.MemberId).ToList();
            string abounttrainings = "";
            if(AnyOtherTraining.Count() > 0)
            {
                abounttrainings += "Selected member has been already took (" + AnyOtherTraining.Count().ToString() + ") training(s).";
                int counter = 1;
                foreach (var a in AnyOtherTraining)
                {
                    abounttrainings += "("+ counter++ +") Tehsil: " + a.CommunityInstituteMember.CommunityInstitution.UnionCouncil.Tehsil.TehsilName + ", UC: " + a.CommunityInstituteMember.CommunityInstitution.UnionCouncil.UnionCouncilName + ", Training Head: " + a.MemberTraining.TrainingType.TrainingHead.TrainingHeadName + ", Training Type: " + a.MemberTraining.TrainingType.TrainingTypeName + ", Training Title: " + a.MemberTraining.TrainingName + ". ";
                }
            }
            return Json(new { isValid = true, Info, count = AnyOtherTraining.Count(), message = abounttrainings});
        }
        public async Task<JsonResult> AddBeneficiaryInTraining(int CDMId, int MTId)
        {
            var result = _context.MemberTrainingDetail.Count(a => a.CommunityInstituteMemberId == CDMId && a.MemberTrainingId == MTId);
            if (CDMId == 0 || MTId == 0)
            {
                return Json(new { isValid = false, message = "Failed to Add Member!" });
            }
            else if (result > 0)
            {
                return Json(new { isValid = false, message = "Selected member is already added!" });
            }
            else
            {                
                MemberTrainingDetail obj = new MemberTrainingDetail();
                obj.CommunityInstituteMemberId = CDMId;
                obj.MemberTrainingId = MTId;
                obj.CreatedOn = DateTime.Today.Date;
                _context.Add(obj);
               /* var memberTrainingObj = _context.MemberTraining.Find(MTId);
                memberTrainingObj.TotalMember += 1;
                _context.Update(memberTrainingObj);*/
                await _context.SaveChangesAsync();                
            }
            return Json(new { isValid = true, message = "Member has been added successfully." });
        }
        // GET: MemberTrainingDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MemberTrainingDetail == null)
            {
                return NotFound();
            }

            var memberTrainingDetail = await _context.MemberTrainingDetail
                .Include(m => m.CommunityInstituteMember)
                .Include(m => m.MemberTraining)
                .FirstOrDefaultAsync(m => m.MemberTrainingDetailId == id);
            if (memberTrainingDetail == null)
            {
                return NotFound();
            }            
            return View(memberTrainingDetail);
        }

        // GET: MemberTrainingDetails/Create
        public IActionResult Create()
        {
            ViewData["CommunityInstituteMemberId"] = new SelectList(_context.CommunityInstituteMember, "CommunityInstituteMemberId", "CNIC");
            ViewData["MemberTrainingId"] = new SelectList(_context.MemberTraining, "MemberTrainingId", "MemberTrainingId");
            return View();
        }

        // POST: MemberTrainingDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberTrainingDetailId,MemberTrainingId,CommunityInstituteMemberId,CreatedOn")] MemberTrainingDetail memberTrainingDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memberTrainingDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CommunityInstituteMemberId"] = new SelectList(_context.CommunityInstituteMember, "CommunityInstituteMemberId", "CNIC", memberTrainingDetail.CommunityInstituteMemberId);
            ViewData["MemberTrainingId"] = new SelectList(_context.MemberTraining, "MemberTrainingId", "MemberTrainingId", memberTrainingDetail.MemberTrainingId);
            return View(memberTrainingDetail);
        }

        // GET: MemberTrainingDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MemberTrainingDetail == null)
            {
                return NotFound();
            }

            var memberTrainingDetail = await _context.MemberTrainingDetail.FindAsync(id);
            if (memberTrainingDetail == null)
            {
                return NotFound();
            }
            ViewData["CommunityInstituteMemberId"] = new SelectList(_context.CommunityInstituteMember, "CommunityInstituteMemberId", "CNIC", memberTrainingDetail.CommunityInstituteMemberId);
            ViewData["MemberTrainingId"] = new SelectList(_context.MemberTraining, "MemberTrainingId", "MemberTrainingId", memberTrainingDetail.MemberTrainingId);
            return View(memberTrainingDetail);
        }

        // POST: MemberTrainingDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberTrainingDetailId,MemberTrainingId,CommunityInstituteMemberId,CreatedOn")] MemberTrainingDetail memberTrainingDetail)
        {
            if (id != memberTrainingDetail.MemberTrainingDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberTrainingDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberTrainingDetailExists(memberTrainingDetail.MemberTrainingDetailId))
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
            ViewData["CommunityInstituteMemberId"] = new SelectList(_context.CommunityInstituteMember, "CommunityInstituteMemberId", "CNIC", memberTrainingDetail.CommunityInstituteMemberId);
            ViewData["MemberTrainingId"] = new SelectList(_context.MemberTraining, "MemberTrainingId", "MemberTrainingId", memberTrainingDetail.MemberTrainingId);
            return View(memberTrainingDetail);
        }

        // GET: MemberTrainingDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MemberTrainingDetail == null)
            {
                return NotFound();
            }

            var memberTrainingDetail = await _context.MemberTrainingDetail
                .Include(m => m.CommunityInstituteMember.Member)
                .Include(m => m.MemberTraining)
                .FirstOrDefaultAsync(m => m.MemberTrainingDetailId == id);
            if (memberTrainingDetail == null)
            {
                return NotFound();
            }

            return View(memberTrainingDetail);
        }

        // POST: MemberTrainingDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MemberTrainingDetail == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MemberTrainingDetail'  is null.");
            }
            var memberTrainingDetail = await _context.MemberTrainingDetail.FindAsync(id);
            if (memberTrainingDetail != null)
            {
                _context.MemberTrainingDetail.Remove(memberTrainingDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), "MemberTrainings", new {memberTrainingDetail.MemberTrainingId});
        }

        private bool MemberTrainingDetailExists(int id)
        {
          return _context.MemberTrainingDetail.Any(e => e.MemberTrainingDetailId == id);
        }
    }
}
