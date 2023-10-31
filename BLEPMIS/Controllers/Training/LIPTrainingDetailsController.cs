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
using DAL.Models.Domain.MasterSetup;

namespace BLEPMIS.Controllers.Training
{
    public class LIPTrainingDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public LIPTrainingDetailsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: LIPTrainingDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LIPTrainingDetail.Include(a=>a.Member).Include(m => m.LIPTraining);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> _Index(int id)
        {
            var applicationDbContext = _context.LIPTrainingDetail.Include(m => m.Member).Include(m => m.LIPTraining).Include(a => a.Member).Where(a => a.LIPTrainingId == id);
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
            var Info = _context.Member.Where(a => a.CNIC == id).FirstOrDefault();
            
            if (Info == null)
            {
                return Json(new { isValid = false });
            }
            var AnyOtherTraining = _context.LIPTrainingDetail.Include(a=>a.LIPTraining.Village.UnionCouncil.Tehsil).Include(a => a.LIPTraining.TrainingType.TrainingHead).Where(a => a.MemberId == Info.MemberId).ToList();
            string abounttrainings = "";
            if (AnyOtherTraining.Count() > 0)
            {
                abounttrainings += "Selected member has been already took (" + AnyOtherTraining.Count().ToString() + ") training(s).";
                int counter = 1;
                foreach (var a in AnyOtherTraining)
                {
                    abounttrainings += "(" + counter++ + ") Tehsil: " + a.LIPTraining.Village.UnionCouncil.Tehsil.TehsilName + ", UC: " + a.LIPTraining.Village.UnionCouncil.UnionCouncilName + ", Training Head: " + a.LIPTraining.TrainingType.TrainingHead.TrainingHeadName + ", Training Type: " + a.LIPTraining.TrainingType.TrainingTypeName + ", Training Title: " + a.LIPTraining.TrainingName + ". ";
                }
            }
            return Json(new { isValid = true, Info, count = AnyOtherTraining.Count(), message = abounttrainings });
        }
        public async Task<JsonResult> AddBeneficiaryInTraining(int MTId, int MId, int PSCRanking)
        {
            var result = _context.LIPTrainingDetail.Count(a => a.MemberId == MId);
            if (MTId == 0 || MId == 0)
            {
                return Json(new { isValid = false, message = "Failed to Add Member!" });
            }
            else if (result > 0)
            {
                return Json(new { isValid = false, message = "Selected member is already added!" });
            }
            else
            {
                LIPTrainingDetail obj = new LIPTrainingDetail();
                obj.MemberId = MId;
                int CurrentMemberCount = _context.LIPTrainingDetail.Count(a => a.LIPTrainingId == MTId) + 1;
                obj.LIPNumber = "(LIPT)-" + CurrentMemberCount;                 
                obj.PSCRanking = PSCRanking;
                obj.LIPTrainingId = MTId;
                obj.CreatedOn = DateTime.Today.Date;
                _context.Add(obj);
                //var memberTrainingObj = _context.LIPTraining.Find(MTId);
                //memberTrainingObj.TotalMember += 1;
                //_context.Update(memberTrainingObj);   
                await _context.SaveChangesAsync();
            }
            return Json(new { isValid = true, message = "Member has been added successfully." });
        }
        // GET: LIPTrainingDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LIPTrainingDetail == null)
            {
                return NotFound();
            }

            var LIPTrainingDetail = await _context.LIPTrainingDetail
                .Include(m => m.Member)
                .Include(m => m.LIPTraining)
                .FirstOrDefaultAsync(m => m.LIPTrainingDetailId == id);
            if (LIPTrainingDetail == null)
            {
                return NotFound();
            }
            return View(LIPTrainingDetail);
        }

        // GET: LIPTrainingDetails/Create
        public IActionResult Create()
        {
            ViewData["CommunityInstituteMemberId"] = new SelectList(_context.CommunityInstituteMember, "CommunityInstituteMemberId", "CNIC");
            ViewData["LIPTrainingId"] = new SelectList(_context.LIPTraining, "LIPTrainingId", "TrainingName");
            return View();
        }

        // POST: LIPTrainingDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LIPTrainingDetail LIPTrainingDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(LIPTrainingDetail);   
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CommunityInstituteMemberId"] = new SelectList(_context.CommunityInstituteMember, "CommunityInstituteMemberId", "CNIC", LIPTrainingDetail.CommunityInstituteMemberId);            
            ViewData["LIPTrainingId"] = new SelectList(_context.LIPTraining, "LIPTrainingId", "TrainingName", LIPTrainingDetail.LIPTrainingId);
            return View(LIPTrainingDetail);
        }

        // GET: LIPTrainingDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LIPTrainingDetail == null)
            {
                return NotFound();
            }

            var LIPTrainingDetail = await _context.LIPTrainingDetail.FindAsync(id);
            if (LIPTrainingDetail == null)
            {
                return NotFound();
            }
            //ViewData["CommunityInstituteMemberId"] = new SelectList(_context.CommunityInstituteMember, "CommunityInstituteMemberId", "CNIC", LIPTrainingDetail.CommunityInstituteMemberId);
            ViewData["LIPTrainingId"] = new SelectList(_context.LIPTraining, "LIPTrainingId", "TrainingName", LIPTrainingDetail.LIPTrainingId);
            return View(LIPTrainingDetail);
        }

        // POST: LIPTrainingDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LIPTrainingDetailId,MemberTrainingId,CommunityInstituteMemberId,CreatedOn")] LIPTrainingDetail LIPTrainingDetail)
        {
            if (id != LIPTrainingDetail.LIPTrainingDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(LIPTrainingDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LIPTrainingDetailExists(LIPTrainingDetail.LIPTrainingDetailId))
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
            //ViewData["CommunityInstituteMemberId"] = new SelectList(_context.CommunityInstituteMember, "CommunityInstituteMemberId", "CNIC", LIPTrainingDetail.CommunityInstituteMemberId);
            ViewData["LIPTrainingId"] = new SelectList(_context.LIPTraining, "LIPTrainingId", "TrainingName", LIPTrainingDetail.LIPTrainingId);
            return View(LIPTrainingDetail);
        }

        // GET: LIPTrainingDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LIPTrainingDetail == null)
            {
                return NotFound();
            }

            var LIPTrainingDetail = await _context.LIPTrainingDetail
                .Include(m => m.Member)
                .Include(m => m.LIPTraining)
                .FirstOrDefaultAsync(m => m.LIPTrainingDetailId == id);
            if (LIPTrainingDetail == null)
            {
                return NotFound();
            }

            return View(LIPTrainingDetail);
        }

        // POST: LIPTrainingDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LIPTrainingDetail == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LIPTrainingDetail'  is null.");
            }
            var LIPTrainingDetail = await _context.LIPTrainingDetail.FindAsync(id);
            if (LIPTrainingDetail != null)
            {
                _context.LIPTrainingDetail.Remove(LIPTrainingDetail);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Details), "MemberTrainings", new { LIPTrainingDetail.LIPTrainingId });
        }

        private bool LIPTrainingDetailExists(int id)
        {
            return _context.LIPTrainingDetail.Any(e => e.LIPTrainingDetailId == id);
        }
    }
}
