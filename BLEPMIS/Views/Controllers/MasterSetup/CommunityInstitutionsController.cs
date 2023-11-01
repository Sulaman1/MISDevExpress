using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.MasterSetup;
using Microsoft.AspNetCore.Http;
using System.IO;
using UnionCouncil = DAL.Models.Domain.MasterSetup.UnionCouncil;
using Tehsil = DAL.Models.Domain.MasterSetup.Tehsil;
using BAL.IRepository.MasterSetup.CD;
using DBContext.Data;

namespace BLEPMIS.Controllers.MasterSetup
{
    public class CommunityInstitutionsController : Controller
    {
        private readonly ICommunityInstitution _context;                     
        private readonly ApplicationDbContext _context0;                     
        
        public CommunityInstitutionsController(ICommunityInstitution context, ApplicationDbContext context1)
        {
            _context = context;            
            _context0 = context1;            
        }
        public async Task<IActionResult> CDSummaryView()
        {          
            ViewData["DistrictId"] = new SelectList(await _context.GetAllDistrict(), "DistrictId", "Name");           
            ViewData["CommunityTypeId"] = new SelectList(await _context.GetCommunityTypes(), "CommunityTypeId", "Name");           
            return View();
        }
        public IActionResult ReloadCDSummary(int DId, int TId, int UCId, int CTId)
        {
            return ViewComponent("CDSummary", new { DId, TId, UCId, CTId });
        }

        // GET: CommunityInstitutions
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.Heading = (id == 1) ? "Community Institutions" : "Common Interest Groups";
            ViewBag.Id = id;
            //-----------------------------------------------
            var applicationDbContext = await _context.GetAll(id, HttpContext.User);                                 
            return View(applicationDbContext);
        }
        public async Task<IActionResult> SubmittedForReviewIndex(int id)
        {
            ViewBag.Heading = (id == 1) ? "Community Institutions" : "Common Interest Groups";
            ViewBag.Id = id;
            return View(await _context.GetAllSubmittedForReview(id, HttpContext.User));
        }
        public async Task<IActionResult> SubmittedForVerifyIndex(int id)
        {
            ViewBag.Heading = (id == 1) ? "Community Institutions" : "Common Interest Groups";
            ViewBag.Id = id;                                  
            return View(await _context.GetAllSubmittedForVerify(id, HttpContext.User));
        }
        public async Task<IActionResult> VerifiedIndex(int id)
        {
            ViewBag.Heading = (id == 1) ? "Community Institutions" : "Common Interest Groups";
            ViewBag.Id = id;                                   
            return View(await _context.GetAllVerified(id, HttpContext.User));
        }
        // GET: CommunityInstitutions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var communityInstitution = await _context.GetById(id);
            if (communityInstitution == null)
            {
                return NotFound();
            }
            ViewBag.Id = communityInstitution.CommunityTypeId;
            return View(communityInstitution);
        }
        public async Task<IActionResult> SubmittedDetails(int? id, int c)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var communityInstitution = await _context.GetByIdSubmittedDetails(id);
            if (communityInstitution == null)
            {
                return RedirectToAction("SubmittedForReviewIndex", new { id = c});
            }

            return View(communityInstitution);
        }
        public async Task<IActionResult> VerifiedDetails(int? id, int c)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var communityInstitution = await _context.GetByIdVerifiedDetails(id);
            if (communityInstitution == null)
            {
                return RedirectToAction("SubmittedForVerifyIndex", new { id = c });
            }

            return View(communityInstitution);
        }
        public async Task<JsonResult> CISubmitForReviewRequest(int id)
        {
            var status = await _context.CISubmitForReviewRequest(id, User.Identity.Name);
            if (status)
            {
                return Json(new { isValid = true, message = "Profile has been submitted successfully for review." });
            }
            else
            {
                return Json(new { isValid = false, message = "Failed to Submit Profile!" });
            }            
        }
        public async Task<JsonResult> CIApprovalRequest(int id, int val, string description)
        {
            var status = await _context.CIApprovalRequest(id, val, User.Identity.Name, description);
            string message = "";
            if (status)
            {
                message = "Profile has been approved successfully.";
                return Json(new { isValid = status, message = message });
            }
            else
            {
                return Json(new { isValid = status, message = "Failed to Submit Profile!"});
            }            
        }
        public async Task<JsonResult> CISubmitForApprovalRequest(int id, int val, string description)
        {
            var status = await _context.CISubmitForApprovalRequest(id, val, User.Identity.Name, description);
            string message = "";
            if (status)
            {
                message = "Profile has been forwarded for approval successfully.";
                return Json(new { isValid = status, message = message });
            }
            else
            {
                return Json(new { isValid = status, message = "Profile has been submitted successfully for approval." });
            }
        }
        public async Task<JsonResult> GetTehsils(int districtId)
        {
            List<Tehsil>tehsils = await _context.GetAllTehsil(districtId);
            var tehsilList = tehsils.Select(m => new SelectListItem()
            {
                Text = m.TehsilName.ToString(),
                Value = m.TehsilId.ToString(),
            });
            return Json(tehsilList);            
        }
        public async Task<JsonResult> GetTehsilsName(string districtName)
        {
            List<Tehsil> tehsils = await _context.GetAllTehsilName(districtName);
            var tehsilList = tehsils.Select(m => new SelectListItem()
            {
                Text = m.TehsilName.ToString(),
                Value = m.TehsilName.ToString(),
            });
            return Json(tehsilList);
        }
        public async Task<JsonResult> GetUCs(int tehsilId)
        {
            List<UnionCouncil> unionCouncils = await _context.GetAllUC(tehsilId);
            var UCList = unionCouncils.Select(m => new SelectListItem()
            {
                Text = m.UnionCouncilName.ToString(),
                Value = m.UnionCouncilId.ToString(),
            });
            return Json(UCList);
        }
        public async Task<JsonResult> GetVillages(int ucId)
        {
            List<Village> villages = await _context.GetAllVillage(ucId);
            var VillageList = villages.Select(m => new SelectListItem()
            {
                Text = m.Name.ToString(),
                Value = m.VillageId.ToString(),
            });
            return Json(VillageList);
        }
        // GET: CommunityInstitutions/detail
        public async Task<IActionResult> Create(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            ViewBag.Heading = (id == 1) ? "Community Institutions" : "Common Interest Groups";           
            //-----------------------------------------------                        
            ViewData["DistrictId"] = new SelectList(await _context.GetDistricts(HttpContext.User), "DistrictId", "Name");
            //-----------------------------------------------            
            CommunityInstitution obj = new CommunityInstitution();
            obj.CommunityTypeId = id;
            obj.OnDate = DateTime.Today.Date;
            var currentUser = _context.GetCurrentUser(HttpContext.User);
            obj.OfficerName = currentUser.Result.FirstName + " " + currentUser.Result.LastName;
            return View(obj);
        }

        // POST: CommunityInstitutions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, CommunityInstitution communityInstitution, int DistrictId, int TehsilId, IFormFile SeletionFormAttachment, IFormFile VillageProfileAttachment, IFormFile TOPAttachment)
        {
            if (id == 0)
            {
                return NotFound();
            }
            ViewBag.Heading = (id == 1) ? "Community Institutions" : "Common Interest Groups";
            if (SeletionFormAttachment != null && SeletionFormAttachment.Length > 0)
            {
                if (Path.GetExtension(SeletionFormAttachment.FileName) != ".pdf")
                {
                    ModelState.AddModelError(nameof(communityInstitution.SeletionFormAttachment), "Please attach only Pdf file format.");
                }
            }
            if (VillageProfileAttachment != null && VillageProfileAttachment.Length > 0)
            {
                if (Path.GetExtension(VillageProfileAttachment.FileName) != ".pdf")
                {
                    ModelState.AddModelError(nameof(communityInstitution.VillageProfileAttachment), "Please attach only Pdf file format.");
                }
            }
            if (TOPAttachment != null && TOPAttachment.Length > 0)
            {
                if (Path.GetExtension(TOPAttachment.FileName) != ".pdf")
                {
                    ModelState.AddModelError(nameof(communityInstitution.TOPAttachment), "Please attach only Pdf file format.");
                }
            }
            if (ModelState.IsValid)
            {
                if (SeletionFormAttachment != null && SeletionFormAttachment.Length > 0)
                {
                    Random random = new Random();
                    int randomNumber1 = random.Next(999, 100000);
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\CD" + DistrictId.ToString() + "\\SeletionForm"+ randomNumber1.ToString() +"\\");
                    string fileName = Path.GetFileName(SeletionFormAttachment.FileName);
                    int randomNumber2 = random.Next(999, 100000);
                    fileName = "SeletionForm" + randomNumber2.ToString() + Path.GetExtension(fileName);
                    communityInstitution.SeletionFormAttachment = Path.Combine("/Documents/CD" + DistrictId.ToString() + "/SeletionForm" + randomNumber1.ToString() + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await SeletionFormAttachment.CopyToAsync(stream);
                    }
                }
                if (VillageProfileAttachment != null && VillageProfileAttachment.Length > 0)
                {
                    Random random = new Random();
                    int randomNumber1 = random.Next(999, 100000);
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\CD" + DistrictId.ToString() + "\\VillageProfile"+ randomNumber1.ToString() +"\\");
                    string fileName = Path.GetFileName(VillageProfileAttachment.FileName);                    
                    int randomNumber2 = random.Next(999, 100000);
                    fileName = "VillageProfile" + randomNumber2.ToString() + Path.GetExtension(fileName);
                    communityInstitution.VillageProfileAttachment = Path.Combine("/Documents/CD" + DistrictId.ToString() + "/VillageProfile"+ randomNumber1.ToString() + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await VillageProfileAttachment.CopyToAsync(stream);
                    }
                }
                if (TOPAttachment != null && TOPAttachment.Length > 0)
                {
                    Random random = new Random();
                    int randomNumber1 = random.Next(999, 100000);
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\CD" + DistrictId.ToString() + "\\TOP"+ randomNumber1.ToString() +"\\");
                    string fileName = Path.GetFileName(TOPAttachment.FileName);                    
                    int randomNumber2 = random.Next(999, 100000);
                    fileName = "TOP" + randomNumber2.ToString() + Path.GetExtension(fileName);
                    communityInstitution.TOPAttachment = Path.Combine("/Documents/CD" + DistrictId.ToString() + "/TOP"+ randomNumber1.ToString() +"/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await TOPAttachment.CopyToAsync(stream);
                    }
                }                
                _context.Insert(communityInstitution, DistrictId, TehsilId);                
                return RedirectToAction(nameof(Index), new {id = communityInstitution.CommunityTypeId});
            }
            //-----------------------------------------------
            var districtAccess = await _context.GetDistricts(HttpContext.User);          
            ViewData["DistrictId"] = new SelectList(districtAccess, "DistrictId", "Name", communityInstitution.DistrictId);
            //----------------------------------------------- 
            ViewData["TehsilId"] = new SelectList(await _context.GetAllTehsil(DistrictId), "TehsilId", "TehsilName", TehsilId);
            ViewData["UnionCouncilId"] = new SelectList(await _context.GetAllUC(TehsilId), "UnionCouncilId", "UnionCouncilName", communityInstitution.Village.UnionCouncilId);
            ViewData["VillageId"] = new SelectList(await _context.GetAllVillage(communityInstitution.Village.UnionCouncilId), "VillageId", "Name", communityInstitution.VillageId);
            return View(communityInstitution);
        }

        // GET: CommunityInstitutions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }
            var communityInstitution = await _context.GetById(id);
            if (communityInstitution == null)
            {
                return NotFound();
            }
            if (communityInstitution.CommunityTypeId == 1)
            {
                ViewBag.Heading = "Community Institutions";
            }
            else
            {
                ViewBag.Heading = "Common Interest Groups";
            }                        
            ViewData["DistrictId"] = new SelectList(await _context.GetDistricts(HttpContext.User), "DistrictId", "Name", communityInstitution.DistrictId);            
            ViewData["TehsilId"] = new SelectList(await _context.GetAllTehsil(communityInstitution.DistrictId), "TehsilId", "TehsilName", communityInstitution.Village.UnionCouncil.TehsilId);
            ViewData["UnionCouncilId"] = new SelectList(await _context.GetAllUC(communityInstitution.Village.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", communityInstitution.Village.UnionCouncilId);
            ViewData["VillageId"] = new SelectList(await _context.GetAllVillage(communityInstitution.Village.UnionCouncilId), "VillageId", "Name", communityInstitution.VillageId);
            return View(communityInstitution);
        }

        // POST: CommunityInstitutions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  CommunityInstitution communityInstitution, int DistrictId, IFormFile SeletionFormAttachment, IFormFile VillageProfileAttachment, IFormFile TOPAttachment, int IsUnverifed)
        {
            if (id != communityInstitution.CommunityInstitutionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {           
                    if(IsUnverifed == 1)
                    {
                        communityInstitution.IsVerified = false;
                    }else if(IsUnverifed == 2)
                    {
                        communityInstitution.IsReviewed = false;
                        communityInstitution.IsVerified = false;
                    }else if(IsUnverifed == 3)
                    {
                        communityInstitution.IsReviewed = false;
                        communityInstitution.IsVerified = false;
                        communityInstitution.IsSubmittedForReview = false;
                    }
                    if (SeletionFormAttachment != null && SeletionFormAttachment.Length > 0)
                    {
                        Random random = new Random();
                        int randomNumber1 = random.Next(999, 100000);
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\CD" + DistrictId.ToString() + "\\SeletionForm" + randomNumber1.ToString() + "\\");
                        string fileName = Path.GetFileName(SeletionFormAttachment.FileName);
                        int randomNumber2 = random.Next(999, 100000);
                        fileName = "SeletionForm" + randomNumber2.ToString() + Path.GetExtension(fileName);
                        communityInstitution.SeletionFormAttachment = Path.Combine("/Documents/CD" + DistrictId.ToString() + "/SeletionForm" + randomNumber1.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await SeletionFormAttachment.CopyToAsync(stream);
                        }
                    }
                    if (VillageProfileAttachment != null && VillageProfileAttachment.Length > 0)
                    {
                        Random random = new Random();
                        int randomNumber1 = random.Next(999, 100000);
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\CD" + DistrictId.ToString() + "\\VillageProfile" + randomNumber1.ToString() + "\\");
                        string fileName = Path.GetFileName(VillageProfileAttachment.FileName);
                        int randomNumber2 = random.Next(999, 100000);
                        fileName = "VillageProfile" + randomNumber2.ToString() + Path.GetExtension(fileName);
                        communityInstitution.VillageProfileAttachment = Path.Combine("/Documents/CD" + DistrictId.ToString() + "/VillageProfile" + randomNumber1.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await VillageProfileAttachment.CopyToAsync(stream);
                        }
                    }
                    if (TOPAttachment != null && TOPAttachment.Length > 0)
                    {
                        Random random = new Random();
                        int randomNumber1 = random.Next(999, 100000);
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\CD" + DistrictId.ToString() + "\\TOP" + randomNumber1.ToString() + "\\");
                        string fileName = Path.GetFileName(TOPAttachment.FileName);
                        int randomNumber2 = random.Next(999, 100000);
                        fileName = "TOP" + randomNumber2.ToString() + Path.GetExtension(fileName);
                        communityInstitution.TOPAttachment = Path.Combine("/Documents/CD" + DistrictId.ToString() + "/TOP" + randomNumber1.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await TOPAttachment.CopyToAsync(stream);
                        }
                    }
                    communityInstitution.District = _context0.District.Find(DistrictId).Name;
                    _context0.Update(communityInstitution);
                    _context0.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunityInstitutionExists(communityInstitution.CommunityInstitutionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new {id = communityInstitution.CommunityTypeId});
            }
            ViewBag.Heading = (id == 1) ? "Community Institutions" : "Common Interest Groups";           
            ViewData["DistrictId"] = new SelectList(await _context.GetDistricts(HttpContext.User), "DistrictId", "Name", communityInstitution.DistrictId);            
            ViewData["TehsilId"] = new SelectList(await _context.GetAllTehsil(communityInstitution.DistrictId), "TehsilId", "TehsilName", communityInstitution.Village.UnionCouncil.TehsilId);
            ViewData["UnionCouncilId"] = new SelectList(await _context.GetAllUC(communityInstitution.Village.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", communityInstitution.Village.UnionCouncilId);
            ViewData["VillageId"] = new SelectList(await _context.GetAllVillage(communityInstitution.Village.UnionCouncilId), "VillageId", "Name", communityInstitution.VillageId);
            return View(communityInstitution);
        }
        // GET: CommunityInstitutions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var communityInstitution = await _context.GetById(id);
            if (communityInstitution == null)
            {
                return NotFound();
            }

            return View(communityInstitution);
        }
        // POST: CommunityInstitutions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CommunityInstitution'  is null.");
            }
            var communityInstitution = await _context.GetById(id);
            if (communityInstitution != null)
            {
                _context.Remove(communityInstitution);
            }
                        
            return RedirectToAction(nameof(Index));
        }
        private bool CommunityInstitutionExists(int id)
        {
          return _context.Exist(id);
        }
    }
}
