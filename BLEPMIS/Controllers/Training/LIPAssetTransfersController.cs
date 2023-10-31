using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.Training;
using DBContext.Data;
using BAL.IRepository.MasterSetup.UserManagement;
using Microsoft.AspNetCore.Identity;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using DAL.Models.Domain.MasterSetup;
using BAL.IRepository.MasterSetup.CD;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics.Metrics;
using DAL.Models.ViewModels;

namespace BLEPMIS.Controllers.Training
{
    public class LIPAssetTransfersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public LIPAssetTransfersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: LIPAssetTransfers
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = await _context.LIPAssetTransfer.Include(l => l.LIPPackage).Include(a=>a.Member).Include(l => l.Village.UnionCouncil).Include(a=>a.IdentifiedBy).Where(a=>a.IsSubmitted == false).ToListAsync();
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a=>a.LIPPackage).Include(a=>a.Member).Include(a => a.IdentifiedBy).Select(a=> new LIPAssetTransfer { LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName}, IdentifiedBy = new IdentifiedBy{ Name = a.IdentifiedBy.Name}, Member = new Member {MemberName = a.Member.MemberName, BeneficiaryTypeId = a.Member.BeneficiaryTypeId, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC,},IsSubmitted = a.IsSubmitted, DistrictId  =a.DistrictId}).Where(a=>a.IsSubmitted == false && a.Member.BeneficiaryTypeId == 1).ToListAsync();
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
        public async Task<IActionResult> SubmittedForReviewIndex()
        {
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.IdentifiedBy).Select(a => new LIPAssetTransfer { LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, Member = new Member { MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC, BeneficiaryTypeId = a.Member.BeneficiaryTypeId }, IsSubmitted = a.IsSubmitted, IsReviewed = a.IsReviewed, DistrictId = a.DistrictId}).Where(a => a.IsSubmitted == true && a.IsReviewed == false && a.Member.BeneficiaryTypeId == 1).ToListAsync();
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(applicationDbContext);
        }
        public async Task<IActionResult> VerifiedFilterView()
        {
            var currentuser = await _userManager.GetUserAsync(User);
            var applicationDbContext = _context.District.ToList();
            ViewBag.DId = 1;
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
                ViewBag.DId = currentuser.DistrictId;
            }
            ViewData["DistrictId"] = new SelectList(applicationDbContext, "DistrictId", "Name");
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName");
            return View();
        }
        public IActionResult ReloadLIPAssetTransferReport(int DId, int TId, int UCId, int PId)
        {
            return ViewComponent("LIPAssetTransferReport", new { DId, TId, UCId, PId });
        }       
        public async Task<IActionResult> VerifiedIndex()
        {
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.IdentifiedBy).Select(a => new LIPAssetTransfer { LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, Member = new Member { MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, BeneficiaryTypeId = a.Member.BeneficiaryTypeId, CNIC = a.Member.CNIC, Village = new Village { Name = a.Village.Name, UnionCouncilId = a.Village.UnionCouncilId, UnionCouncil = new UnionCouncil { UnionCouncilName = a.Village.UnionCouncil.UnionCouncilName } } }, DistrictId = a.DistrictId, IsSubmitted = a.IsSubmitted, IsReviewed = a.IsReviewed, IsAssetTransfer = a.IsAssetTransfer }).Where(a => a.IsReviewed == true && a.IsAssetTransfer == false && a.Member.BeneficiaryTypeId == 1).ToListAsync();
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(applicationDbContext);
        }
        public async Task<IActionResult> ATIndex()
        {
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a=>a.Village.UnionCouncil.Tehsil).Include(a => a.IdentifiedBy).Select(a => 
                new LIPAssetTransfer
                {
                    LIPCode = a.LIPCode,
                    LIPAssetTransferId = a.LIPAssetTransferId,
                    LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName },
                    IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name },
                    Member = new Member { MemberName = a.Member.MemberName, BeneficiaryTypeId = a.Member.BeneficiaryTypeId, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC, CellNo = a.Member.CellNo },
                    IsSubmitted = a.IsSubmitted,
                    DistrictId = a.DistrictId,
                    IsAssetTransfer = a.IsAssetTransfer,
                    Village = new Village { Name = a.Village.Name, UnionCouncil = new UnionCouncil { UnionCouncilName = a.Village.UnionCouncil.UnionCouncilName, Tehsil = new Tehsil { TehsilName = a.Village.UnionCouncil.Tehsil.TehsilName } } }
                }).Where(a => a.IsAssetTransfer == true && a.Member.BeneficiaryTypeId == 1).ToListAsync();
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(applicationDbContext);
        }
        public async Task<IActionResult> ATIndexIDO()
        {
            /*var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.IdentifiedBy).Select(a => new LIPAssetTransfer { LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName, Description = a.LIPPackage.Description }, IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, Member = new Member { CellNo = a.Member.CellNo, BeneficiaryTypeId = a.Member.BeneficiaryTypeId, MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC, Village = new Village { Name = a.Village.Name, UnionCouncilId = a.Village.UnionCouncilId, UnionCouncil = new UnionCouncil { UnionCouncilName = a.Village.UnionCouncil.UnionCouncilName, Tehsil = new Tehsil { TehsilName = a.Village.UnionCouncil.Tehsil.TehsilName } } } }, DistrictId = a.DistrictId, IsSubmitted = a.IsSubmitted, IsReviewed = a.IsReviewed, IsAssetTransfer = a.IsAssetTransfer }).Where(a => a.IsAssetTransfer == true).ToListAsync();
            applicationDbContext = applicationDbContext.Where(a => a.Member.BeneficiaryTypeId == 3).ToList();
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(applicationDbContext);*/
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.Village.UnionCouncil.Tehsil).Include(a => a.IdentifiedBy).Select(a =>
                new LIPAssetTransfer
                {
                    LIPCode = a.LIPCode,
                    LIPAssetTransferId = a.LIPAssetTransferId,
                    LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName },
                    IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name },
                    Member = new Member { MemberName = a.Member.MemberName, BeneficiaryTypeId = a.Member.BeneficiaryTypeId, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC, CellNo = a.Member.CellNo },
                    IsSubmitted = a.IsSubmitted,
                    DistrictId = a.DistrictId,
                    IsAssetTransfer = a.IsAssetTransfer,
                    Village = new Village { Name = a.Village.Name, UnionCouncil = new UnionCouncil { UnionCouncilName = a.Village.UnionCouncil.UnionCouncilName, Tehsil = new Tehsil { TehsilName = a.Village.UnionCouncil.Tehsil.TehsilName } } }
                }).Where(a => a.IsAssetTransfer == true && a.Member.BeneficiaryTypeId == 3).ToListAsync();
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(applicationDbContext);
        }
        public async Task<IActionResult> VerifiedIndex2()
        {
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.IdentifiedBy).Select(a => new LIPAssetTransfer { LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, Member = new Member { MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC },DistrictId = a.DistrictId, IsSubmitted = a.IsSubmitted, IsReviewed = a.IsReviewed, IsInternalReviewed = a.IsInternalReviewed, IsAssetTransfer = a.IsAssetTransfer }).Where(a => a.IsReviewed == true && a.IsAssetTransfer == true && a.IsInternalReviewed == false && a.Member.BeneficiaryTypeId == 1).ToListAsync();
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(applicationDbContext);
        }
        public async Task<IActionResult> VerifiedIndex3()
        {
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.IdentifiedBy).Select(a => new LIPAssetTransfer { LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, Member = new Member { MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC },DistrictId = a.DistrictId, IsSubmitted = a.IsSubmitted, IsInternalReviewed = a.IsInternalReviewed }).Where(a => a.IsInternalReviewed == true && a.Member.BeneficiaryTypeId == 1).ToListAsync();
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(applicationDbContext);
        }
        public async Task<IActionResult> TrackMember()
        {           
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a=>a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            ViewBag.CurrentDistrict = currentuser.DistrictId;
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> AjaxMemberInformation(string id, int currentdistrictId)
        {
            var Info = _context.LIPAssetTransfer.Include(a => a.Member.Village.UnionCouncil.Tehsil.District).Include(a=>a.IdentifiedBy).Include(a=>a.LIPPackage).Where(a => a.Member.CNIC == id && a.DistrictId == currentdistrictId).FirstOrDefault();            
            if (Info == null)
            {
                return Json(new { isValid = false, isDuplicate = false });
            }
            else
            {
                var IsExist = _context.LIPAssetTransfer.Include(a => a.Member).Count(a => a.Member.CNIC == id && a.IsReviewed == true);
                if(IsExist > 0)
                {
                    return Json(new { isValid = true, Info });
                }
            }
            return Json(new { isValid = false, isDuplicate = false });
        }
        public async Task<JsonResult> SubmitForReviewRequest(int Id)
        {
            var status = true;
            var applicantInfo = await _context.LIPAssetTransfer.FindAsync(Id);
            if (applicantInfo == null)
            {
                status = false;
            }
            applicantInfo.IsSubmitted = true;//KDA Hard
            applicantInfo.SubmittedOnDate = DateTime.Now;
            applicantInfo.SubmittedBy = User.Identity.Name;            
            _context.Update(applicantInfo);
            await _context.SaveChangesAsync();
            if (status)
            {
                return Json(new { isValid = true, message = "Profile has been submitted successfully for review." });
            }
            else
            {
                return Json(new { isValid = false, message = "Failed to Submit Profile!" });
            }
        }
        public async Task<JsonResult> ApprovalRequest(int id,int val, string description)
        {
            var status = true;
            var applicantInfo = await _context.LIPAssetTransfer.FindAsync(id);
            if (applicantInfo == null)
            {
                status = false;
            }
               
            string message = "";
            if (status)
            {
                if (val == 0)
                {
                    applicantInfo.IsReviewed = false;//KDA Hard
                    applicantInfo.IsSubmitted = false;//KDA Hard
                    applicantInfo.IsRejected = true;
                    applicantInfo.RejectedComments = description;
                }
                else
                {
                    applicantInfo.IsReviewed = true;
                    applicantInfo.ReviewedOn = DateTime.Today.Date;
                    applicantInfo.ReviewedBy = User.Identity.Name;
                }
                _context.Update(applicantInfo);
                await _context.SaveChangesAsync();
                message = "LIP has been approved and forward successfully.";
                return Json(new { isValid = status, message = message });
            }
            else
            {
                return Json(new { isValid = status, message = "Failed to Submit LIP!" });
            }
        }        
        // GET: LIPAssetTransfers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LIPAssetTransfer == null)
            {
                return NotFound();
            }

            var lIPAssetTransfer = await _context.LIPAssetTransfer
                .Include(l => l.LIPPackage)
                .Include(l => l.Village.UnionCouncil.Tehsil.District)
                .Include(l => l.IdentifiedBy)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.LIPAssetTransferId == id);
            if (lIPAssetTransfer == null)
            {
                return NotFound();
            }

            return View(lIPAssetTransfer);
        }

        // GET: LIPAssetTransfers/Create
        public async Task<IActionResult> Create()
        {
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName");
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name");            
            Member member = new Member();
            member.BeneficiaryTypeId = 1;
            member.IsRefugee = false;       
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name");
            LIPAssetTransfer lIPAssetTransfer = new LIPAssetTransfer();
            lIPAssetTransfer.IsInternalReviewed = false;
            lIPAssetTransfer.IsReviewed = false;
            lIPAssetTransfer.IsSubmitted = false;
            lIPAssetTransfer.IsRejected = false;
            lIPAssetTransfer.UserId = currentuser.Id;
            LIPMember Obj = new LIPMember();
            Obj.Member = member;            
            Obj.LIPAssetTransfer = lIPAssetTransfer;
            return View(Obj);
        }

        // POST: LIPAssetTransfers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LIPMember tuple, int DistrictId, IFormFile LIPFormAttachment, IFormFile GRNAttachment, IFormFile CNICAttachment, IFormFile PictureAttachment1, IFormFile ProfilePicture)
        {           
            if (ModelState.IsValid)
            {
                int MemberId = 0;
                var currentuser = await _userManager.GetUserAsync(User);
                if (tuple.Member.CNIC.IndexOf('_') > 0)
                {
                    ModelState.AddModelError(nameof(tuple.Member.CNIC), "Invalid CNIC!");
                    ModelState.AddModelError("CustomError", "Invalid CNIC!");
                    var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
                    if (currentuser.DistrictId > 1)
                    {
                        districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
                    }
                    ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name", DistrictId);
                    ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", tuple.LIPAssetTransfer.LIPPackageId);
                    ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", tuple.LIPAssetTransfer.IdentifiedById);
                    return View(tuple);
                }
                var result = _context.Member.Where(a=> a.CNIC == tuple.Member.CNIC).FirstOrDefault();                
                if (result != null)
                {                  
                    MemberId = _context.Member.Where(a => a.CNIC == tuple.Member.CNIC).Select(a=>a.MemberId).FirstOrDefault();
                    if(result.VillageId == 0)
                    {
                        result.VillageId = tuple.Member.VillageId;
                        var memberdata = _context.Member.Find(MemberId);
                        memberdata.VillageId = result.VillageId;
                        _context.Update(memberdata);
                        await _context.SaveChangesAsync();
                    }
                    if(tuple.Member.VillageId != result.VillageId)                    
                    {
                        ModelState.AddModelError("CustomError", "Member already exist in MIS but village information mismatch!");
                        var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
                        if (currentuser.DistrictId > 1)
                        {
                            districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
                        }
                        ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name", DistrictId);
                        ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", tuple.LIPAssetTransfer.LIPPackageId);
                        ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", tuple.LIPAssetTransfer.IdentifiedById);
                        return View(tuple);
                    }
                }
                else
                {                    
                    _context.Add(tuple.Member);
                    _context.SaveChanges();
                    MemberId = _context.Member.Max(a => a.MemberId);
                }                
                var IsExist = _context.LIPAssetTransfer.Count(a => a.MemberId == MemberId);
                if (IsExist > 0)
                {
                    ModelState.AddModelError("CustomError", "Member already exist in LIP pool!");                    
                    var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
                    if (currentuser.DistrictId > 1)
                    {
                        districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
                    }
                    ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name", DistrictId);
                    ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", tuple.LIPAssetTransfer.LIPPackageId);
                    ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", tuple.LIPAssetTransfer.IdentifiedById);
                    return View(tuple);                    
                }
               /* IsExist = _context.LIPAssetTransfer.Include(a=>a.Member).Count(a => a.Member.CellNo == tuple.Member.CellNo);
                if (IsExist > 0)
                {
                    ModelState.AddModelError("CustomError", "Member mobile number already exist in LIP pool!");
                    var currentuser = await _userManager.GetUserAsync(User);
                    var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
                    if (currentuser.DistrictId > 1)
                    {
                        districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
                    }
                    ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name", DistrictId);
                    ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", tuple.LIPAssetTransfer.LIPPackageId);
                    ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", tuple.LIPAssetTransfer.IdentifiedById);
                    return View(tuple);
                }*/
                if (ProfilePicture != null && ProfilePicture.Length > 0)
                {
                    await using var memoryStream = new MemoryStream();
                    await ProfilePicture.CopyToAsync(memoryStream);
                    tuple.Member.ProfilePicture = memoryStream.ToArray();
                }                                              
                tuple.LIPAssetTransfer.MemberId = MemberId;
                tuple.LIPAssetTransfer.VillageId = tuple.Member.VillageId;
                tuple.LIPAssetTransfer.DistrictId = DistrictId;
                var LIPCount = _context.LIPAssetTransfer.Count(a=>a.DistrictId == DistrictId) + 1;
                string DistrictCode = _context.District.Find(DistrictId).Code;
                string val = (LIPCount).ToString("D3");
                tuple.LIPAssetTransfer.LIPCode = (DistrictCode + "-" + val);
                while (_context.LIPAssetTransfer.Count(a => a.LIPCode == tuple.LIPAssetTransfer.LIPCode) > 0)
                {
                    val = (++LIPCount).ToString("D3");
                    tuple.LIPAssetTransfer.LIPCode = (DistrictCode + "-" + val);
                }
                if (LIPFormAttachment != null && LIPFormAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\LIPForm"+ MemberId.ToString() +"\\");
                    string fileName = Path.GetFileName(LIPFormAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "LIPForm" + randomNumber.ToString() + Path.GetExtension(fileName);
                    tuple.LIPAssetTransfer.LIPFormAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/LIPForm" + MemberId.ToString() + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await LIPFormAttachment.CopyToAsync(stream);
                    }
                }
                /*if (GRNAttachment != null && GRNAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\GRN\\");
                    string fileName = Path.GetFileName(GRNAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(999, 100000);
                    fileName = "GRN" + randomNumber.ToString() + Path.GetExtension(fileName);
                    lIPAssetTransfer.GRNAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/GRN/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await GRNAttachment.CopyToAsync(stream);
                    }
                }*/
                if (CNICAttachment != null && CNICAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\CNIC"+ MemberId.ToString() +"\\");
                    string fileName = Path.GetFileName(CNICAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(999, 100000);
                    fileName = "CNIC" + randomNumber.ToString() + Path.GetExtension(fileName);
                    tuple.LIPAssetTransfer.CNICAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/CNIC" + MemberId.ToString() +"/" + fileName);//Server Path
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
                /*if (PictureAttachment1 != null && PictureAttachment1.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\Picture1\\");
                    string fileName = Path.GetFileName(PictureAttachment1.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(999, 100000);
                    fileName = "Picture1" + randomNumber.ToString() + Path.GetExtension(fileName);
                    lIPAssetTransfer.PictureAttachment1 = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/Picture1/" + fileName);//Server Path
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
                }*/                
                tuple.LIPAssetTransfer.UserId = currentuser.Id;
                tuple.LIPAssetTransfer.District = _context.District.Find(DistrictId).Name;
                _context.Add(tuple.LIPAssetTransfer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", tuple.LIPAssetTransfer.LIPPackageId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil, "UnionCouncilId", "UnionCouncilName", tuple.LIPAssetTransfer.Village.UnionCouncilId);
            return View(tuple);
        }

        public async Task<IActionResult> Edit0(int? id)
        {
            if (id == null || _context.LIPAssetTransfer == null)
            {
                return NotFound();
            }
            var currentuser = await _userManager.GetUserAsync(User);            
            var lIPAssetTransfer = await _context.LIPAssetTransfer.Include(a => a.Member).Where(a => a.LIPAssetTransferId == id).FirstOrDefaultAsync();
            if (lIPAssetTransfer == null)
            {
                return NotFound();
            }
            ViewBag.IsAllow = true;
            if(lIPAssetTransfer.UserId != "123")
            {
                if (currentuser.Id != lIPAssetTransfer.UserId)
                {
                    ViewBag.IsAllow = false;
                    ViewBag.CreatedBy = _context.Users.Find(lIPAssetTransfer.UserId).UserName;
                }
            }            
            var linker = _context.Village.Include(a => a.UnionCouncil.Tehsil).Where(a => a.VillageId == lIPAssetTransfer.VillageId).FirstOrDefault();
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a=>a.DistrictId == lIPAssetTransfer.DistrictId), "DistrictId", "Name", lIPAssetTransfer.DistrictId);
            ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a=>a.DistrictId == lIPAssetTransfer.DistrictId), "TehsilId", "TehsilName", linker.UnionCouncil.TehsilId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a=>a.TehsilId == linker.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", linker.UnionCouncilId);
            ViewData["VillageId"] = new SelectList(_context.Village.Where(a=>a.UnionCouncilId == linker.UnionCouncilId), "VillageId", "Name", linker.VillageId);
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", lIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", lIPAssetTransfer.IdentifiedById);
            return View(lIPAssetTransfer);
        }

        // POST: LIPAssetTransfers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit0(int id, LIPAssetTransfer lIPAssetTransfer, int DistrictId, int TehsilId, IFormFile LIPFormAttachment, IFormFile GRNAttachment, IFormFile CNICAttachment, IFormFile PictureAttachment1)
        {
            if (id != lIPAssetTransfer.LIPAssetTransferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (LIPFormAttachment != null && LIPFormAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\LIPForm"+ lIPAssetTransfer.MemberId.ToString() +"\\");
                        string fileName = Path.GetFileName(LIPFormAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "LIPForm" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.LIPFormAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/LIPForm" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await LIPFormAttachment.CopyToAsync(stream);
                        }
                    }
                    if (CNICAttachment != null && CNICAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\CNIC"+ lIPAssetTransfer.MemberId.ToString() +"\\");
                        string fileName = Path.GetFileName(CNICAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "CNIC" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.CNICAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/CNIC"+ lIPAssetTransfer.MemberId.ToString() +"/" + fileName);//Server Path
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
                    lIPAssetTransfer.IdentifiedBy = null;
                    var MemberObj = new Member();
                    MemberObj = lIPAssetTransfer.Member;                    
                    //lIPAssetTransfer.Member = null;
                    lIPAssetTransfer.LIPPackage = null;
                    var currentuser = await _userManager.GetUserAsync(User);
                    lIPAssetTransfer.UserId = currentuser.Id;
                    lIPAssetTransfer.District = _context.District.Find(DistrictId).Name;
                    _context.LIPAssetTransfer.Update(lIPAssetTransfer);
                    _context.Member.Update(MemberObj);                   
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LIPAssetTransferExists(lIPAssetTransfer.LIPAssetTransferId))
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
            var linker = _context.Village.Include(a => a.UnionCouncil.Tehsil).Where(a => a.VillageId == lIPAssetTransfer.VillageId).FirstOrDefault();
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId == lIPAssetTransfer.DistrictId), "DistrictId", "Name", lIPAssetTransfer.DistrictId);
            ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == lIPAssetTransfer.DistrictId), "TehsilId", "TehsilName", linker.UnionCouncil.TehsilId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == linker.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", linker.UnionCouncilId);
            ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == linker.UnionCouncilId), "VillageId", "Name", linker.VillageId);
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", lIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", lIPAssetTransfer.IdentifiedById);
            return View(lIPAssetTransfer);
        }
        // GET: LIPAssetTransfers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LIPAssetTransfer == null)
            {
                return NotFound();
            }

            var lIPAssetTransfer = await _context.LIPAssetTransfer.Include(a=>a.LIPPackage).Include(a=>a.IdentifiedBy).Include(a=>a.Member).Include(a=>a.Village.UnionCouncil.Tehsil.District).Where(a=>a.LIPAssetTransferId == id).FirstOrDefaultAsync();
            if (lIPAssetTransfer == null)
            {
                return NotFound();
            }            
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }            
            return View(lIPAssetTransfer);
        }

        // POST: LIPAssetTransfers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LIPAssetTransfer lIPAssetTransfer, int DistrictId, int TehsilId, IFormFile LIPFormAttachment, IFormFile GRNAttachment, IFormFile CNICAttachment, IFormFile PictureAttachment1)
        {
            if (id != lIPAssetTransfer.LIPAssetTransferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {                    
                    //if (LIPFormAttachment != null && LIPFormAttachment.Length > 0)
                    //{
                    //    var rootPath = Path.Combine(
                    //    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\LIPForm\\");
                    //    string fileName = Path.GetFileName(LIPFormAttachment.FileName);
                    //    Random random = new Random();
                    //    int randomNumber = random.Next(999, 100000);
                    //    fileName = "LIPForm" + randomNumber.ToString() + Path.GetExtension(fileName);
                    //    lIPAssetTransfer.LIPFormAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/LIPForm/" + fileName);//Server Path
                    //    string sPath = Path.Combine(rootPath);
                    //    if (!System.IO.Directory.Exists(sPath))
                    //    {
                    //        System.IO.Directory.CreateDirectory(sPath);
                    //    }
                    //    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    //    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    //    {
                    //        await LIPFormAttachment.CopyToAsync(stream);
                    //    }
                    //}
                    if (GRNAttachment != null && GRNAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\GRN"+ lIPAssetTransfer.MemberId.ToString() +"\\");
                        string fileName = Path.GetFileName(GRNAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "GRN" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.GRNAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/GRN"+ lIPAssetTransfer.MemberId.ToString() +"/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await GRNAttachment.CopyToAsync(stream);
                        }
                    }
                    if (CNICAttachment != null && CNICAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\CNIC"+ lIPAssetTransfer.MemberId.ToString() +"\\");
                        string fileName = Path.GetFileName(CNICAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "CNIC" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.CNICAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/CNIC"+ lIPAssetTransfer.MemberId.ToString() +"/" + fileName);//Server Path
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
                    if (PictureAttachment1 != null && PictureAttachment1.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\Picture1"+ lIPAssetTransfer.MemberId.ToString() +"\\");
                        string fileName = Path.GetFileName(PictureAttachment1.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "Picture1" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.PictureAttachment1 = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/Picture1"+ lIPAssetTransfer.MemberId.ToString() +"/" + fileName);//Server Path
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
                    lIPAssetTransfer.IsAssetTransfer = true;
                    lIPAssetTransfer.AssetTransferBy = User.Identity.Name;
                    lIPAssetTransfer.IdentifiedBy = null;
                    lIPAssetTransfer.Member = null;
                    lIPAssetTransfer.LIPPackage = null;                                        
                    lIPAssetTransfer.Village = null;                                        
                    _context.LIPAssetTransfer.Update(lIPAssetTransfer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LIPAssetTransferExists(lIPAssetTransfer.LIPAssetTransferId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(VerifiedIndex));
            }
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name", lIPAssetTransfer.DistrictId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil, "UnionCouncilId", "UnionCouncilName", lIPAssetTransfer.Village.UnionCouncilId);
            ViewData["TehsilId"] = new SelectList(_context.UnionCouncil, "UnionCouncilId", "UnionCouncilName", TehsilId);
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", lIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", lIPAssetTransfer.IdentifiedById);
            return View(lIPAssetTransfer);
        }
        public async Task<IActionResult> SpecialEdit(int? id)
        {
            if (id == null || _context.LIPAssetTransfer == null)
            {
                return NotFound();
            }

            var lIPAssetTransfer = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.IdentifiedBy).Include(a => a.Member).Include(a => a.Village.UnionCouncil.Tehsil.District).Where(a => a.LIPAssetTransferId == id).FirstOrDefaultAsync();
            if (lIPAssetTransfer == null)
            {
                return NotFound();
            }
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(lIPAssetTransfer);
        }

        // POST: LIPAssetTransfers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SpecialEdit(int id, LIPAssetTransfer lIPAssetTransfer, int DistrictId, int TehsilId, IFormFile LIPFormAttachment)
        {
            if (id != lIPAssetTransfer.LIPAssetTransferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (LIPFormAttachment != null && LIPFormAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\LIPForm\\");
                        string fileName = Path.GetFileName(LIPFormAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "LIPForm" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.LIPFormAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/LIPForm/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await LIPFormAttachment.CopyToAsync(stream);
                        }
                    }    
                    lIPAssetTransfer.IdentifiedBy = null;
                    lIPAssetTransfer.Member = null;
                    lIPAssetTransfer.LIPPackage = null;
                    lIPAssetTransfer.Village = null;
                    _context.LIPAssetTransfer.Update(lIPAssetTransfer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LIPAssetTransferExists(lIPAssetTransfer.LIPAssetTransferId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ATIndex));
            }
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name", lIPAssetTransfer.DistrictId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil, "UnionCouncilId", "UnionCouncilName", lIPAssetTransfer.Village.UnionCouncilId);
            ViewData["TehsilId"] = new SelectList(_context.UnionCouncil, "UnionCouncilId", "UnionCouncilName", TehsilId);
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", lIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", lIPAssetTransfer.IdentifiedById);
            return View(lIPAssetTransfer);
        }

        public async Task<IActionResult> Edit2(int? id)
        {
            if (id == null || _context.LIPAssetTransfer == null)
            {
                return NotFound();
            }

            var lIPAssetTransfer = await _context.LIPAssetTransfer.Include(a => a.Member).Include(a => a.Village.UnionCouncil).Where(a => a.LIPAssetTransferId == id).FirstOrDefaultAsync();
            if (lIPAssetTransfer == null)
            {
                return NotFound();
            }
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", lIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", lIPAssetTransfer.IdentifiedById);
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name", lIPAssetTransfer.DistrictId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a=>a.TehsilId == lIPAssetTransfer.Village.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", lIPAssetTransfer.Village.UnionCouncilId);
            ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a=>a.DistrictId == lIPAssetTransfer.DistrictId), "TehsilId", "TehsilName", lIPAssetTransfer.Village.UnionCouncil.TehsilId);
            ViewData["VillageId"] = new SelectList(_context.Village.Where(a=>a.UnionCouncilId == lIPAssetTransfer.Village.UnionCouncilId), "VillageId", "Name", lIPAssetTransfer.VillageId);
            return View(lIPAssetTransfer);
        }

        // POST: LIPAssetTransfers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2(int id, LIPAssetTransfer lIPAssetTransfer, int DistrictId, int TehsilId, IFormFile PictureAttachment4)
        {
            if (id != lIPAssetTransfer.LIPAssetTransferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {           
                    if (PictureAttachment4 != null && PictureAttachment4.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\InternalReview" + lIPAssetTransfer.Member.ToString() +"\\");
                        string fileName = Path.GetFileName(PictureAttachment4.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "InternalReview" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.PictureAttachment4 = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/InternalReview" + lIPAssetTransfer.MemberId.ToString()+"/" + fileName);//Server Path
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
                    lIPAssetTransfer.Village = null;
                    lIPAssetTransfer.Member = null;
                    lIPAssetTransfer.IsInternalReviewed= true;
                    _context.Update(lIPAssetTransfer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LIPAssetTransferExists(lIPAssetTransfer.LIPAssetTransferId))
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
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name", lIPAssetTransfer.DistrictId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil, "UnionCouncilId", "UnionCouncilName", lIPAssetTransfer.Village.UnionCouncilId);
            ViewData["TehsilId"] = new SelectList(_context.UnionCouncil, "UnionCouncilId", "UnionCouncilName", TehsilId);
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", lIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", lIPAssetTransfer.IdentifiedById);
            return View(lIPAssetTransfer);
        }
        // GET: LIPAssetTransfers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LIPAssetTransfer == null)
            {
                return NotFound();
            }

            var lIPAssetTransfer = await _context.LIPAssetTransfer
                .Include(l => l.LIPPackage)
                //.Include(l => l.Village.UnionCouncil)
                .FirstOrDefaultAsync(m => m.LIPAssetTransferId == id);
            if (lIPAssetTransfer == null)
            {
                return NotFound();
            }

            return View(lIPAssetTransfer);
        }

        // POST: LIPAssetTransfers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LIPAssetTransfer == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LIPAssetTransfer'  is null.");
            }
            var lIPAssetTransfer = await _context.LIPAssetTransfer.FindAsync(id);
            if (lIPAssetTransfer != null)
            {
                _context.LIPAssetTransfer.Remove(lIPAssetTransfer);
                await _context.SaveChangesAsync();
            }
                        
            return RedirectToAction(nameof(Index));
        }

        private bool LIPAssetTransferExists(int id)
        {
          return _context.LIPAssetTransfer.Any(e => e.LIPAssetTransferId == id);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////
        ///

        public async Task<IActionResult> VerifiedFilterViewIDO()
        {
            var currentuser = await _userManager.GetUserAsync(User);
            var applicationDbContext = _context.District.ToList();
            ViewBag.DId = 1;
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
                ViewBag.DId = currentuser.DistrictId;
            }
            ViewData["DistrictId"] = new SelectList(applicationDbContext, "DistrictId", "Name");
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage.Where(a => a.LIPPackageId > 8), "LIPPackageId", "PackageName");
            return View();
        }

        public IActionResult ReloadLIPAssetTransferReportIDO(int DId, int TId, int UCId, int PId)
        {
            bool IsRefugee = true;
            return ViewComponent("LIPAssetTransferReport", new { DId, TId, UCId, PId, IsRefugee });
        }
        // GET: LIPAssetTransfers/Create
        public async Task<IActionResult> CreateIDO()
        {
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage.Where(a => a.LIPPackageId > 8), "LIPPackageId", "PackageName");
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy.Where(a => a.IdentifiedById == 4), "IdentifiedById", "Name");
            Member member = new Member();
            member.BeneficiaryTypeId = 3;
            member.IsRefugee = true;
            member.BeneficiaryTypeId = 3;
            member.BeneficiaryTypeId = 3;
            member.BeneficiaryTypeId = 3;

            MemberAfghanDetail memberAfghan = new MemberAfghanDetail();
            //memberAfghan.Member = member;
            //memberAfghan.AssistanceOther = "N/A";
            //memberAfghan.TrainingOther = "N/A";
            // memberAfghan.TrainingInsterest = "";

            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name");
            LIPAssetTransfer lIPAssetTransfer = new LIPAssetTransfer();
            lIPAssetTransfer.IsInternalReviewed = false;
            lIPAssetTransfer.IsReviewed = false;
            lIPAssetTransfer.IsSubmitted = false;
            lIPAssetTransfer.IsRejected = false;
            lIPAssetTransfer.UserId = currentuser.Id;
            LIPMemberAfghan Obj = new LIPMemberAfghan();
            Obj.Member = member;
            Obj.LIPAssetTransfer = lIPAssetTransfer;
            Obj.MemberAfghanDetail = memberAfghan;
            return View(Obj);
        }

        // POST: LIPAssetTransfers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateIDO(LIPMemberAfghan tuple, int DistrictId, IFormFile LIPFormAttachment, IFormFile GRNAttachment, IFormFile CNICAttachment, IFormFile PictureAttachment1, IFormFile ProfilePicture)
        {
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            if (ModelState.IsValid)
            {
                int MemberId = 0;
                var result = _context.Member.Where(a => a.CNIC == tuple.Member.CNIC).FirstOrDefault();
                if (result != null)
                {
                    MemberId = _context.Member.Where(a => a.CNIC == tuple.Member.CNIC).Select(a => a.MemberId).FirstOrDefault();
                    if (tuple.Member.VillageId != result.VillageId)
                    {
                        ModelState.AddModelError("CustomError", "Member already exist in MIS but village information mismatch!");

                        ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name", DistrictId);
                        ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage.Where(a => a.LIPPackageId > 8), "LIPPackageId", "PackageName");
                        ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy.Where(a => a.IdentifiedById == 4), "IdentifiedById", "Name");
                        return View(tuple);
                    }
                }
                else
                {
                    if (tuple.MemberAfghanDetail.HHGender == "Female")
                    {
                        tuple.MemberAfghanDetail.IsHHFemale = true;
                    }
                    tuple.Member.BeneficiaryTypeId = 3;
                    tuple.Member.IsRefugee = true;
                    _context.Add(tuple.Member);
                    _context.SaveChanges();
                    MemberId = _context.Member.Max(a => a.MemberId);

                    tuple.MemberAfghanDetail.MemberId = MemberId;
                    _context.Add(tuple.MemberAfghanDetail);
                    _context.SaveChanges();
                }
                var IsExist = _context.LIPAssetTransfer.Count(a => a.MemberId == MemberId);
                if (IsExist > 0)
                {
                    ModelState.AddModelError("CustomError", "Member already exist in LIP pool!");
                    districts = _context.District.Where(a => a.DistrictId > 1).ToList();
                    if (currentuser.DistrictId > 1)
                    {
                        districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
                    }
                    ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name", DistrictId);
                    ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", tuple.LIPAssetTransfer.LIPPackageId);
                    ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", tuple.LIPAssetTransfer.IdentifiedById);
                    return View(tuple);
                }

                if (ProfilePicture != null && ProfilePicture.Length > 0)
                {
                    await using var memoryStream = new MemoryStream();
                    await ProfilePicture.CopyToAsync(memoryStream);
                    tuple.Member.ProfilePicture = memoryStream.ToArray();
                }
                tuple.LIPAssetTransfer.MemberId = MemberId;
                tuple.LIPAssetTransfer.VillageId = tuple.Member.VillageId;
                tuple.LIPAssetTransfer.DistrictId = DistrictId;
                var LIPCount = _context.LIPAssetTransfer.Count(a => a.DistrictId == DistrictId) + 1;
                string DistrictCode = _context.District.Find(DistrictId).Code;
                string val = (LIPCount).ToString("D3");
                tuple.LIPAssetTransfer.LIPCode = ("RF-" + DistrictCode + "-" + val);
                while (_context.LIPAssetTransfer.Count(a => a.LIPCode == tuple.LIPAssetTransfer.LIPCode) > 0)
                {
                    val = (++LIPCount).ToString("D3");
                    tuple.LIPAssetTransfer.LIPCode = ("RF-" + DistrictCode + "-" + val);
                }
                if (LIPFormAttachment != null && LIPFormAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\LIPForm" + MemberId.ToString() + "\\");
                    string fileName = Path.GetFileName(LIPFormAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    fileName = "LIPForm" + randomNumber.ToString() + Path.GetExtension(fileName);
                    tuple.LIPAssetTransfer.LIPFormAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/LIPForm" + MemberId.ToString() + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await LIPFormAttachment.CopyToAsync(stream);
                    }
                }
                /*if (GRNAttachment != null && GRNAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\GRN\\");
                    string fileName = Path.GetFileName(GRNAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(999, 100000);
                    fileName = "GRN" + randomNumber.ToString() + Path.GetExtension(fileName);
                    lIPAssetTransfer.GRNAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/GRN/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await GRNAttachment.CopyToAsync(stream);
                    }
                }*/
                if (CNICAttachment != null && CNICAttachment.Length > 0)
                {
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\CNIC" + MemberId.ToString() + "\\");
                    string fileName = Path.GetFileName(CNICAttachment.FileName);
                    Random random = new Random();
                    int randomNumber = random.Next(999, 100000);
                    fileName = "CNIC" + randomNumber.ToString() + Path.GetExtension(fileName);
                    tuple.LIPAssetTransfer.CNICAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/CNIC" + MemberId.ToString() + "/" + fileName);//Server Path
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

                tuple.LIPAssetTransfer.UserId = currentuser.Id;
                _context.Add(tuple.LIPAssetTransfer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", tuple.LIPAssetTransfer.LIPPackageId);
            // ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil, "UnionCouncilId", "UnionCouncilName", tuple.LIPAssetTransfer.Village.UnionCouncilId);
            ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name", DistrictId);
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", tuple.LIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", tuple.LIPAssetTransfer.IdentifiedById);

            return View(tuple);
        }

        // GET: LIPAssetTransfers/Delete/5
        public async Task<IActionResult> DeleteIDO(int? id)
        {
            if (id == null || _context.LIPAssetTransfer == null)
            {
                return NotFound();
            }

            var lIPAssetTransfer = await _context.LIPAssetTransfer
                .Include(l => l.LIPPackage)
                .Include(l => l.Village.UnionCouncil)
                .FirstOrDefaultAsync(m => m.LIPAssetTransferId == id);
            if (lIPAssetTransfer == null)
            {
                return NotFound();
            }

            return View(lIPAssetTransfer);
        }

        // POST: LIPAssetTransfers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedIDO(int id)
        {
            if (_context.LIPAssetTransfer == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LIPAssetTransfer'  is null.");
            }
            var lIPAssetTransfer = await _context.LIPAssetTransfer.FindAsync(id);
            if (lIPAssetTransfer != null)
            {
                _context.LIPAssetTransfer.Remove(lIPAssetTransfer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            LIPMemberAfghan Obj = new LIPMemberAfghan();
            Obj.LIPAssetTransfer = await _context.LIPAssetTransfer.Include(a => a.Member).Where(a => a.LIPAssetTransferId == id).FirstOrDefaultAsync();
            Obj.Member = await _context.Member.FindAsync(Obj.LIPAssetTransfer.MemberId);
            Obj.MemberAfghanDetail = await _context.MemberAfghanDetail.FindAsync(Obj.LIPAssetTransfer.MemberId);



            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage.Where(a => a.LIPPackageId > 8), "LIPPackageId", "PackageName");
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy.Where(a => a.IdentifiedById == 4), "IdentifiedById", "Name");
            //memberAfghan.Member = member;
            //memberAfghan.AssistanceOther = "N/A";
            //memberAfghan.TrainingOther = "N/A";


            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            var linker = _context.Village.Include(a => a.UnionCouncil.Tehsil).Where(a => a.VillageId == Obj.LIPAssetTransfer.VillageId).FirstOrDefault();

            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId == Obj.LIPAssetTransfer.DistrictId), "DistrictId", "Name", Obj.LIPAssetTransfer.DistrictId);
            ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == Obj.LIPAssetTransfer.DistrictId), "TehsilId", "TehsilName", linker.UnionCouncil.TehsilId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == linker.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", linker.UnionCouncilId);
            ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == linker.UnionCouncilId), "VillageId", "Name", linker.VillageId);
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage.Where(a => a.LIPPackageId > 8), "LIPPackageId", "PackageName", Obj.LIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy.Where(a => a.IdentifiedById == 4), "IdentifiedById", "Name", Obj.LIPAssetTransfer.IdentifiedById);

            return View(Obj);
        }

        // POST: LIPAssetTransfers/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(LIPMemberAfghan tuple, int DistrictId, IFormFile LIPFormAttachment, IFormFile GRNAttachment, IFormFile CNICAttachment, IFormFile PictureAttachment1, IFormFile ProfilePicture)
        {
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            var lIPAssetTransfer = tuple.LIPAssetTransfer;
            var linker = _context.Village.Include(a => a.UnionCouncil.Tehsil).Where(a => a.VillageId == tuple.LIPAssetTransfer.VillageId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var result = _context.Member.Where(a => a.CNIC == tuple.Member.CNIC).Select(a => a.MemberId).FirstOrDefault();
                if (result > 0)
                {
                    //   int MemberId = _context.Member.Where(a => a.CNIC == tuple.Member.CNIC ).Select(a => a.MemberId).FirstOrDefault();
                    if (tuple.Member.MemberId != result)
                    {
                        ModelState.AddModelError("CustomError", "Member already exist in MIS!");
                        //  var linker = _context.Village.Include(a => a.UnionCouncil.Tehsil).Where(a => a.VillageId == Obj.LIPAssetTransfer.VillageId).FirstOrDefault();

                        ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId == tuple.LIPAssetTransfer.DistrictId), "DistrictId", "Name", tuple.LIPAssetTransfer.DistrictId);
                        ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == tuple.LIPAssetTransfer.DistrictId), "TehsilId", "TehsilName", linker.UnionCouncil.TehsilId);
                        ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == linker.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", linker.UnionCouncilId);
                        ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == linker.UnionCouncilId), "VillageId", "Name", linker.VillageId);
                        ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage.Where(a => a.LIPPackageId > 8), "LIPPackageId", "PackageName", tuple.LIPAssetTransfer.LIPPackageId);
                        ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy.Where(a => a.IdentifiedById == 4), "IdentifiedById", "Name", tuple.LIPAssetTransfer.IdentifiedById);
                        return View(tuple);
                    }
                }
                try
                {
                    if (LIPFormAttachment != null && LIPFormAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\LIPForm" + lIPAssetTransfer.MemberId.ToString() + "\\");
                        string fileName = Path.GetFileName(LIPFormAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "LIPForm" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.LIPFormAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/LIPForm" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await LIPFormAttachment.CopyToAsync(stream);
                        }
                    }
                    if (CNICAttachment != null && CNICAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\CNIC" + lIPAssetTransfer.MemberId.ToString() + "\\");
                        string fileName = Path.GetFileName(CNICAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "CNIC" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.CNICAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/CNIC" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
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
                    //lIPAssetTransfer.IdentifiedBy = null;
                    //var MemberObj = new Member();
                    // MemberObj = lIPAssetTransfer.Member;
                    //lIPAssetTransfer.Member = null;
                    //lIPAssetTransfer.LIPPackage = null;
                    // var currentuser = await _userManager.GetUserAsync(User);
                    lIPAssetTransfer.UserId = currentuser.Id;
                    _context.LIPAssetTransfer.Update(lIPAssetTransfer);
                    _context.Member.Update(tuple.Member);
                    _context.MemberAfghanDetail.Update(tuple.MemberAfghanDetail);
                    // _context.Member.Update(MemberObj);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LIPAssetTransferExists(lIPAssetTransfer.LIPAssetTransferId))
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

            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId == tuple.LIPAssetTransfer.DistrictId), "DistrictId", "Name", tuple.LIPAssetTransfer.DistrictId);
            ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == tuple.LIPAssetTransfer.DistrictId), "TehsilId", "TehsilName", linker.UnionCouncil.TehsilId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == linker.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", linker.UnionCouncilId);
            ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == linker.UnionCouncilId), "VillageId", "Name", linker.VillageId);
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage.Where(a => a.LIPPackageId > 8), "LIPPackageId", "PackageName", tuple.LIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy.Where(a => a.IdentifiedById == 4), "IdentifiedById", "Name", tuple.LIPAssetTransfer.IdentifiedById);

            return View(tuple);
        }


        public async Task<IActionResult> EditIDO(int? id)
        {
            LIPMemberAfghan Obj = new LIPMemberAfghan();
            Obj.LIPAssetTransfer = await _context.LIPAssetTransfer.Include(a => a.Member).Where(a => a.LIPAssetTransferId == id).FirstOrDefaultAsync();
            Obj.Member = await _context.Member.FindAsync(Obj.LIPAssetTransfer.MemberId);
            Obj.MemberAfghanDetail = await _context.MemberAfghanDetail.FindAsync(Obj.LIPAssetTransfer.MemberId);

            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage.Where(a => a.LIPPackageId > 8), "LIPPackageId", "PackageName");
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy.Where(a => a.IdentifiedById == 4), "IdentifiedById", "Name");
            //memberAfghan.Member = member;
            //memberAfghan.AssistanceOther = "N/A";
            //memberAfghan.TrainingOther = "N/A";


            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            var linker = _context.Village.Include(a => a.UnionCouncil.Tehsil).Where(a => a.VillageId == Obj.LIPAssetTransfer.VillageId).FirstOrDefault();

            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId == Obj.LIPAssetTransfer.DistrictId), "DistrictId", "Name", Obj.LIPAssetTransfer.DistrictId);
            ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == Obj.LIPAssetTransfer.DistrictId), "TehsilId", "TehsilName", linker.UnionCouncil.TehsilId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == linker.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", linker.UnionCouncilId);
            ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == linker.UnionCouncilId), "VillageId", "Name", linker.VillageId);
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage.Where(a => a.LIPPackageId > 8), "LIPPackageId", "PackageName", Obj.LIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy.Where(a => a.IdentifiedById == 4), "IdentifiedById", "Name", Obj.LIPAssetTransfer.IdentifiedById);

            return View(Obj);
        }

        // POST: LIPAssetTransfers/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditIDO(int id, LIPMemberAfghan tuple, int DistrictId, IFormFile LIPFormAttachment, IFormFile GRNAttachment, IFormFile CNICAttachment, IFormFile PictureAttachment1)
        {
            // var lIPAssetTransfer = tuple.LIPAssetTransfer;
            var lIPAssetTransfer = await _context.LIPAssetTransfer.FindAsync(id);

            if (lIPAssetTransfer != null)
            {
                try
                {
                    if (GRNAttachment != null && GRNAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\GRN" + lIPAssetTransfer.MemberId.ToString() + "\\");
                        string fileName = Path.GetFileName(GRNAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "GRN" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.GRNAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/GRN" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await GRNAttachment.CopyToAsync(stream);
                        }
                    }
                    if (CNICAttachment != null && CNICAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\CNIC" + lIPAssetTransfer.MemberId.ToString() + "\\");
                        string fileName = Path.GetFileName(CNICAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "CNIC" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.CNICAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/CNIC" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
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
                    if (PictureAttachment1 != null && PictureAttachment1.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\Picture1" + lIPAssetTransfer.MemberId.ToString() + "\\");
                        string fileName = Path.GetFileName(PictureAttachment1.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "Picture1" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.PictureAttachment1 = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/Picture1" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
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
                    lIPAssetTransfer.IsAssetTransfer = true;
                    lIPAssetTransfer.AssetTransferBy = User.Identity.Name;
                    lIPAssetTransfer.AssetTransferOn = tuple.LIPAssetTransfer.AssetTransferOn;
                    lIPAssetTransfer.IdentifiedBy = null;
                    lIPAssetTransfer.Member = null;
                    lIPAssetTransfer.LIPPackage = null;

                    _context.LIPAssetTransfer.Update(lIPAssetTransfer);
                    await _context.SaveChangesAsync();
                    ////////////
                    //////////
                    /////////
                    ///////

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LIPAssetTransferExists(lIPAssetTransfer.LIPAssetTransferId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(VerifiedIndex));
            }
            var linker = _context.Village.Include(a => a.UnionCouncil.Tehsil).Where(a => a.VillageId == tuple.LIPAssetTransfer.VillageId).FirstOrDefault();

            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId == tuple.LIPAssetTransfer.DistrictId), "DistrictId", "Name", tuple.LIPAssetTransfer.DistrictId);
            ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == tuple.LIPAssetTransfer.DistrictId), "TehsilId", "TehsilName", linker.UnionCouncil.TehsilId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == linker.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", linker.UnionCouncilId);
            ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == linker.UnionCouncilId), "VillageId", "Name", linker.VillageId);
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage.Where(a => a.LIPPackageId > 8), "LIPPackageId", "PackageName", tuple.LIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy.Where(a => a.IdentifiedById == 4), "IdentifiedById", "Name", tuple.LIPAssetTransfer.IdentifiedById);

            return View(tuple);
        }


        // GET: LIPAssetTransfers
        public async Task<IActionResult> IndexIDO()
        {
            //var applicationDbContext = await _context.LIPAssetTransfer.Include(l => l.LIPPackage).Include(a=>a.Member).Include(l => l.Village.UnionCouncil).Include(a=>a.IdentifiedBy).Where(a=>a.IsSubmitted == false).ToListAsync();
            var applicationDbContext = await _context.LIPAssetTransfer
                .Include(a => a.LIPPackage)
                .Include(a => a.Member)
                .Include(a => a.IdentifiedBy)
                .Select(a => new LIPAssetTransfer
                {
                    LIPCode = a.LIPCode,
                    LIPAssetTransferId = a.LIPAssetTransferId,
                    IsRejected = a.IsRejected,
                    RejectedComments = a.RejectedComments,
                    LIPPackage = new LIPPackage
                    { PackageName = a.LIPPackage.PackageName },
                    IdentifiedBy = new IdentifiedBy
                    { Name = a.IdentifiedBy.Name },
                    Member = new Member
                    {
                        MemberName = a.Member.MemberName,
                        FatherName = a.Member.FatherName,
                        BeneficiaryTypeId = a.Member.BeneficiaryTypeId,
                        CNIC = a.Member.CNIC
                    },
                    IsSubmitted = a.IsSubmitted,
                    DistrictId = a.DistrictId
                })
                .Where(a => a.IsSubmitted == false).ToListAsync();
            //LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, 
            //IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, 
            //Member = new Member { FatherName = a.Member.FatherName, MemberName = a.Member.MemberName, CNIC = a.Member.CNIC }                 
            applicationDbContext = applicationDbContext.Where(a => a.Member.BeneficiaryTypeId == 3).ToList();
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(applicationDbContext);
        }

        // GET: LIPAssetTransfers/Details/5
        public async Task<IActionResult> DetailsIDO(int? id)
        {
            if (id == null || _context.LIPAssetTransfer == null)
            {
                return NotFound();
            }

            LIPMemberAfghan Obj = new LIPMemberAfghan();
            Obj.LIPAssetTransfer = await _context.LIPAssetTransfer
                .Include(l => l.LIPPackage)
                .Include(l => l.Village.UnionCouncil.Tehsil.District)
                .Include(l => l.IdentifiedBy)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.LIPAssetTransferId == id);
            Obj.Member = await _context.Member.FindAsync(Obj.LIPAssetTransfer.MemberId);
            Obj.MemberAfghanDetail = await _context.MemberAfghanDetail.FindAsync(Obj.LIPAssetTransfer.MemberId);


            var lIPAssetTransfer = await _context.LIPAssetTransfer
                .Include(l => l.LIPPackage)
                .Include(l => l.Village.UnionCouncil.Tehsil.District)
                .Include(l => l.IdentifiedBy)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.LIPAssetTransferId == id);
            if (lIPAssetTransfer == null)
            {
                return NotFound();
            }

            return View(Obj);
        }


        public async Task<IActionResult> EditX(int? id)
        {
            //if (id == null || _context.LIPAssetTransfer == null)
            //{
            //    return NotFound();
            //}
            var currentuser = await _userManager.GetUserAsync(User);
            ViewBag.UserId = currentuser.Id;
            var lIPAssetTransfer = await _context.LIPAssetTransfer.Include(a => a.Member).Where(a => a.LIPAssetTransferId == id).FirstOrDefaultAsync();
            //var member = await _context.Member.FindAsync(lIPAssetTransfer.MemberId);
            // var memberAfghanDetail = await _context.MemberAfghanDetail.FindAsync(lIPAssetTransfer.MemberId);

            LIPMemberAfghan Obj = new LIPMemberAfghan();
            Obj.LIPAssetTransfer = await _context.LIPAssetTransfer.Include(a => a.Member).Where(a => a.LIPAssetTransferId == id).FirstOrDefaultAsync();
            Obj.Member = await _context.Member.FindAsync(Obj.LIPAssetTransfer.MemberId);
            Obj.MemberAfghanDetail = await _context.MemberAfghanDetail.FindAsync(lIPAssetTransfer.MemberId);
            //if (lIPAssetTransfer == null)
            //{
            //    return NotFound();
            //}
            var linker = _context.Village.Include(a => a.UnionCouncil.Tehsil).Where(a => a.VillageId == lIPAssetTransfer.VillageId).FirstOrDefault();
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId == lIPAssetTransfer.DistrictId), "DistrictId", "Name", lIPAssetTransfer.DistrictId);
            ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == lIPAssetTransfer.DistrictId), "TehsilId", "TehsilName", linker.UnionCouncil.TehsilId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == linker.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", linker.UnionCouncilId);
            ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == linker.UnionCouncilId), "VillageId", "Name", linker.VillageId);
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", lIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", lIPAssetTransfer.IdentifiedById);


            //LIPMemberAfghan lIPMemberAfghan = new LIPMemberAfghan
            //{
            //    LIPAssetTransfer=lIPAssetTransfer,
            //    Member = member,
            //    MemberAfghanDetail=memberAfghanDetail
            //};

            return View(Obj);
            //return View(lIPMemberAfghan);
        }

        // POST: LIPAssetTransfers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditX(int id, LIPMemberAfghan lIPMemberAfghan, int DistrictId, int TehsilId, IFormFile LIPFormAttachment, IFormFile GRNAttachment, IFormFile CNICAttachment, IFormFile PictureAttachment1)
        {
            var lIPAssetTransfer = lIPMemberAfghan.LIPAssetTransfer;
            if (id != lIPAssetTransfer.LIPAssetTransferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (LIPFormAttachment != null && LIPFormAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\LIPForm" + lIPAssetTransfer.MemberId.ToString() + "\\");
                        string fileName = Path.GetFileName(LIPFormAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "LIPForm" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.LIPFormAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/LIPForm" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await LIPFormAttachment.CopyToAsync(stream);
                        }
                    }
                    if (CNICAttachment != null && CNICAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\CNIC" + lIPAssetTransfer.MemberId.ToString() + "\\");
                        string fileName = Path.GetFileName(CNICAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "CNIC" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.CNICAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/CNIC" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
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
                    //lIPAssetTransfer.IdentifiedBy = null;
                    //var MemberObj = new Member();
                    // MemberObj = lIPAssetTransfer.Member;
                    //lIPAssetTransfer.Member = null;
                    //lIPAssetTransfer.LIPPackage = null;
                    var currentuser = await _userManager.GetUserAsync(User);
                    lIPAssetTransfer.UserId = currentuser.Id;
                    _context.LIPAssetTransfer.Update(lIPAssetTransfer);
                    _context.Member.Update(lIPMemberAfghan.Member);
                    _context.MemberAfghanDetail.Update(lIPMemberAfghan.MemberAfghanDetail);
                    // _context.Member.Update(MemberObj);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LIPAssetTransferExists(lIPAssetTransfer.LIPAssetTransferId))
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
            var linker = _context.Village.Include(a => a.UnionCouncil.Tehsil).Where(a => a.VillageId == lIPAssetTransfer.VillageId).FirstOrDefault();
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId == lIPAssetTransfer.DistrictId), "DistrictId", "Name", lIPAssetTransfer.DistrictId);
            ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == lIPAssetTransfer.DistrictId), "TehsilId", "TehsilName", linker.UnionCouncil.TehsilId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == linker.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", linker.UnionCouncilId);
            ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == linker.UnionCouncilId), "VillageId", "Name", linker.VillageId);
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", lIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", lIPAssetTransfer.IdentifiedById);
            return View(lIPAssetTransfer);
        }

        // GET: LIPAssetTransfers/Edit/5
        public async Task<IActionResult> EditOldIDO(int? id)
        {
            if (id == null || _context.LIPAssetTransfer == null)
            {
                return NotFound();
            }

            var lIPAssetTransfer = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.IdentifiedBy).Include(a => a.Member).Include(a => a.Village.UnionCouncil.Tehsil.District).Where(a => a.LIPAssetTransferId == id).FirstOrDefaultAsync();
            if (lIPAssetTransfer == null)
            {
                return NotFound();
            }
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(lIPAssetTransfer);
        }

        // POST: LIPAssetTransfers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOldIDO(int id, LIPAssetTransfer lIPAssetTransfer, int DistrictId, int TehsilId, IFormFile LIPFormAttachment, IFormFile GRNAttachment, IFormFile CNICAttachment, IFormFile PictureAttachment1)
        {
            if (id != lIPAssetTransfer.LIPAssetTransferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //if (LIPFormAttachment != null && LIPFormAttachment.Length > 0)
                    //{
                    //    var rootPath = Path.Combine(
                    //    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\LIPForm\\");
                    //    string fileName = Path.GetFileName(LIPFormAttachment.FileName);
                    //    Random random = new Random();
                    //    int randomNumber = random.Next(999, 100000);
                    //    fileName = "LIPForm" + randomNumber.ToString() + Path.GetExtension(fileName);
                    //    lIPAssetTransfer.LIPFormAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/LIPForm/" + fileName);//Server Path
                    //    string sPath = Path.Combine(rootPath);
                    //    if (!System.IO.Directory.Exists(sPath))
                    //    {
                    //        System.IO.Directory.CreateDirectory(sPath);
                    //    }
                    //    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    //    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    //    {
                    //        await LIPFormAttachment.CopyToAsync(stream);
                    //    }
                    //}
                    if (GRNAttachment != null && GRNAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\GRN" + lIPAssetTransfer.MemberId.ToString() + "\\");
                        string fileName = Path.GetFileName(GRNAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "GRN" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.GRNAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/GRN" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await GRNAttachment.CopyToAsync(stream);
                        }
                    }
                    if (CNICAttachment != null && CNICAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\CNIC" + lIPAssetTransfer.MemberId.ToString() + "\\");
                        string fileName = Path.GetFileName(CNICAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "CNIC" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.CNICAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/CNIC" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
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
                    if (PictureAttachment1 != null && PictureAttachment1.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\Picture1" + lIPAssetTransfer.MemberId.ToString() + "\\");
                        string fileName = Path.GetFileName(PictureAttachment1.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "Picture1" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.PictureAttachment1 = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/Picture1" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
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
                    lIPAssetTransfer.IsAssetTransfer = true;
                    lIPAssetTransfer.AssetTransferBy = User.Identity.Name;
                    lIPAssetTransfer.IdentifiedBy = null;
                    lIPAssetTransfer.Member = null;
                    lIPAssetTransfer.LIPPackage = null;
                    _context.LIPAssetTransfer.Update(lIPAssetTransfer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LIPAssetTransferExists(lIPAssetTransfer.LIPAssetTransferId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(VerifiedIndex));
            }
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name", lIPAssetTransfer.DistrictId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil, "UnionCouncilId", "UnionCouncilName", lIPAssetTransfer.Village.UnionCouncilId);
            ViewData["TehsilId"] = new SelectList(_context.UnionCouncil, "UnionCouncilId", "UnionCouncilName", TehsilId);
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", lIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", lIPAssetTransfer.IdentifiedById);
            return View(lIPAssetTransfer);
        }


        public async Task<IActionResult> Edit2IDO(int? id)
        {
            if (id == null || _context.LIPAssetTransfer == null)
            {
                return NotFound();
            }

            var lIPAssetTransfer = await _context.LIPAssetTransfer.Include(a => a.Member).Include(a => a.Village.UnionCouncil).Where(a => a.LIPAssetTransferId == id).FirstOrDefaultAsync();
            if (lIPAssetTransfer == null)
            {
                return NotFound();
            }
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", lIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", lIPAssetTransfer.IdentifiedById);
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name", lIPAssetTransfer.DistrictId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == lIPAssetTransfer.Village.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", lIPAssetTransfer.Village.UnionCouncilId);
            ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == lIPAssetTransfer.DistrictId), "TehsilId", "TehsilName", lIPAssetTransfer.Village.UnionCouncil.TehsilId);
            ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == lIPAssetTransfer.Village.UnionCouncilId), "VillageId", "Name", lIPAssetTransfer.VillageId);
            return View(lIPAssetTransfer);
        }

        // POST: LIPAssetTransfers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2IDO(int id, LIPAssetTransfer lIPAssetTransfer, int DistrictId, int TehsilId, IFormFile LIPFormAttachment, IFormFile GRNAttachment, IFormFile CNICAttachment, IFormFile PictureAttachment1)
        {
            if (id != lIPAssetTransfer.LIPAssetTransferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (LIPFormAttachment != null && LIPFormAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\LIPForm" + lIPAssetTransfer.MemberId.ToString() + "\\");
                        string fileName = Path.GetFileName(LIPFormAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "LIPForm" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.LIPFormAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/LIPForm" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await LIPFormAttachment.CopyToAsync(stream);
                        }
                    }
                    if (GRNAttachment != null && GRNAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\GRN" + lIPAssetTransfer.MemberId.ToString() + "\\");
                        string fileName = Path.GetFileName(GRNAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "GRN" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.GRNAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/GRN" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await GRNAttachment.CopyToAsync(stream);
                        }
                    }
                    if (CNICAttachment != null && CNICAttachment.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\CNIC" + lIPAssetTransfer.MemberId.ToString() + "\\");
                        string fileName = Path.GetFileName(CNICAttachment.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "CNIC" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.CNICAttachment = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/CNIC" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
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
                    if (PictureAttachment1 != null && PictureAttachment1.Length > 0)
                    {
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\LIP" + DistrictId.ToString() + "\\Picture1" + lIPAssetTransfer.Member.ToString() + "\\");
                        string fileName = Path.GetFileName(PictureAttachment1.FileName);
                        Random random = new Random();
                        int randomNumber = random.Next(999, 100000);
                        fileName = "Picture1" + randomNumber.ToString() + Path.GetExtension(fileName);
                        lIPAssetTransfer.PictureAttachment1 = Path.Combine("/Documents/LIP" + DistrictId.ToString() + "/Picture1" + lIPAssetTransfer.MemberId.ToString() + "/" + fileName);//Server Path
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
                    lIPAssetTransfer.Village = null;
                    lIPAssetTransfer.Member = null;
                    lIPAssetTransfer.IsInternalReviewed = true;
                    _context.Update(lIPAssetTransfer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LIPAssetTransferExists(lIPAssetTransfer.LIPAssetTransferId))
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
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name", lIPAssetTransfer.DistrictId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil, "UnionCouncilId", "UnionCouncilName", lIPAssetTransfer.Village.UnionCouncilId);
            ViewData["TehsilId"] = new SelectList(_context.UnionCouncil, "UnionCouncilId", "UnionCouncilName", TehsilId);
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "LIPPackageId", "PackageName", lIPAssetTransfer.LIPPackageId);
            ViewData["IdentifiedById"] = new SelectList(_context.IdentifiedBy, "IdentifiedById", "Name", lIPAssetTransfer.IdentifiedById);
            return View(lIPAssetTransfer);
        }

        public async Task<IActionResult> VerifiedIndex2IDO()
        {
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.IdentifiedBy).Select(a => new LIPAssetTransfer { LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, Member = new Member { MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC }, DistrictId = a.DistrictId, IsSubmitted = a.IsSubmitted, IsReviewed = a.IsReviewed, IsInternalReviewed = a.IsInternalReviewed }).Where(a => a.IsReviewed == true && a.IsInternalReviewed == false).ToListAsync();
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(applicationDbContext);
        }


        public async Task<IActionResult> SubmittedForReviewIndexIDO()
        {
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.IdentifiedBy).Select(a => new LIPAssetTransfer { LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, Member = new Member { BeneficiaryTypeId = a.Member.BeneficiaryTypeId, MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC }, IsSubmitted = a.IsSubmitted, IsReviewed = a.IsReviewed, DistrictId = a.DistrictId }).Where(a => a.IsSubmitted == true && a.IsReviewed == false).ToListAsync();
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            applicationDbContext = applicationDbContext.Where(a => a.Member.BeneficiaryTypeId == 3).ToList();

            return View(applicationDbContext);
        }

        public async Task<IActionResult> VerifiedIndexIDO()
        {
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.IdentifiedBy).Select(a => new LIPAssetTransfer { LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, Member = new Member { BeneficiaryTypeId = a.Member.BeneficiaryTypeId, MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC, Village = new Village { Name = a.Village.Name, UnionCouncilId = a.Village.UnionCouncilId, UnionCouncil = new UnionCouncil { UnionCouncilName = a.Village.UnionCouncil.UnionCouncilName } } }, DistrictId = a.DistrictId, IsSubmitted = a.IsSubmitted, IsReviewed = a.IsReviewed, IsAssetTransfer = a.IsAssetTransfer }).Where(a => a.IsReviewed == true && a.IsAssetTransfer == false).ToListAsync();
            applicationDbContext = applicationDbContext.Where(a => a.Member.BeneficiaryTypeId == 3).ToList();
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(applicationDbContext);
        }

        public async Task<IActionResult> SummaryViewIDO()
        {
            var currentuser = await _userManager.GetUserAsync(User);
            //   var applicationDbContext = _context.District.ToList();
            var applicationDbContext = _context.LIPAssetTransfer.Include(a => a.Village.UnionCouncil.Tehsil.District)
              .Select(a => new District { DistrictId = a.Village.UnionCouncil.Tehsil.District.DistrictId, Name = a.Village.UnionCouncil.Tehsil.District.Name }).Distinct().ToList();

            ViewData["DistrictId"] = new SelectList(applicationDbContext, "DistrictId", "Name");
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage.Where(a => a.LIPPackageId > 8), "LIPPackageId", "PackageName");
            return View();
        }


        public async Task<IActionResult> SummaryPartial(int? DId, int? TId, int? UCId, int? PId)
        {
            //var applicationDbContext = await _context.LIPAssetTransfer
            //    .Include(a => a.LIPPackage)
            //    .Include(a => a.Member)
            //    .Include(a => a.IdentifiedBy)

            //    .Select(a => new LIPAssetTransfer { DistrictId = a.DistrictId, LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, 
            //        LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, 
            //        IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, 
            //        Member = new Member { BeneficiaryTypeId = a.Member.BeneficiaryTypeId, MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CellNo = a.Member.CellNo, CNIC = a.Member.CNIC, Village = new Village { VillageId = a.VillageId, Name = a.Village.Name, UnionCouncilId = a.Village.UnionCouncilId, 
            //            UnionCouncil = new UnionCouncil { UnionCouncilName = a.Village.UnionCouncil.UnionCouncilName, TehsilId = a.Village.UnionCouncil.TehsilId, 
            //                Tehsil = new Tehsil { TehsilName = a.Village.UnionCouncil.Tehsil.TehsilName } } } }, 
            //        IsSubmitted = a.IsSubmitted, IsReviewed = a.IsReviewed, IsAssetTransfer = a.IsAssetTransfer }).Where(a => a.IsAssetTransfer == true & a.IdentifiedById==4).ToListAsync();


            //var applicationDbContext = await _context.LipIDO.ToListAsync();
            var applicationDbContext = await _context.LipIDO.FromSqlRaw("exec SPLipIDO ").ToListAsync();

            if (DId != null)
            {
                if (TId != null)
                {
                    if (UCId != null)
                    {
                        applicationDbContext = applicationDbContext.Where(a => a.UnionCouncilId == UCId).ToList();
                    }
                    else
                    {
                        applicationDbContext = applicationDbContext.Where(a => a.TehsilId == TId).ToList();
                    }
                }
                else
                {
                    applicationDbContext = applicationDbContext.Where(a => a.DistrictId == DId).ToList();
                }
            }
            if (PId != null)
            {
                applicationDbContext = applicationDbContext.Where(a => a.LIPPackageId == PId).ToList();
            }
            ViewBag.TotalLIP = applicationDbContext.Count();
            return PartialView(applicationDbContext.ToList());

        }


        public async Task<JsonResult> GetTehsils(int districtId)
        {
            //var qry = await _context.LIPAssetTransfer.Include(a=>a.Village.UnionCouncil.Tehsil.d).Where(a=>a.Village.UnionCouncil.Tehsil.DistrictId==districtId).ToListAsync();
            // List<Tehsil> tehsils = await _context.GetAllTehsil(districtId);
            List<Tehsil> tehsils = await _context.LIPAssetTransfer.Include(a => a.Village.UnionCouncil.Tehsil.District)
            .Select(a => new Tehsil { TehsilId = a.Village.UnionCouncil.Tehsil.TehsilId, TehsilName = a.Village.UnionCouncil.Tehsil.TehsilName, DistrictId = a.Village.UnionCouncil.Tehsil.DistrictId }).Distinct().ToListAsync();
            tehsils = tehsils.Where(a => a.DistrictId == districtId).ToList();
            var tehsilList = tehsils.Select(m => new SelectListItem()
            {
                Text = m.TehsilName.ToString(),
                Value = m.TehsilId.ToString(),
            });
            return Json(tehsilList);
        }
        public async Task<JsonResult> GetUCs(int tehsilId)
        {
            //List<UnionCouncil> unionCouncils = await _context.GetAllUC(tehsilId);
            List<UnionCouncil> unionCouncils = await _context.LIPAssetTransfer.Include(a => a.Village.UnionCouncil.Tehsil.District)
              .Select(a => new UnionCouncil { UnionCouncilId = a.Village.UnionCouncil.UnionCouncilId, UnionCouncilName = a.Village.UnionCouncil.UnionCouncilName, TehsilId = a.Village.UnionCouncil.Tehsil.TehsilId }).Distinct().ToListAsync();
            unionCouncils = unionCouncils.Where(a => a.TehsilId == tehsilId).ToList();
            var UCList = unionCouncils.Select(m => new SelectListItem()
            {
                Text = m.UnionCouncilName.ToString(),
                Value = m.UnionCouncilId.ToString(),
            });
            return Json(UCList);
        }
        public async Task<JsonResult> GetVillages(int ucId)
        {
            //  List<Village> villages = await _context.GetAllVillage(ucId);
            List<Village> villages = await _context.LIPAssetTransfer.Include(a => a.Village.Name)
              .Select(a => new Village { VillageId = a.Village.VillageId, Name = a.Village.Name, UnionCouncilId = a.Village.UnionCouncilId }).Distinct().ToListAsync();
            villages = villages.Where(a => a.UnionCouncilId == ucId).ToList();
            var VillageList = villages.Select(m => new SelectListItem()
            {
                Text = m.Name.ToString(),
                Value = m.VillageId.ToString(),
            });
            return Json(VillageList);
        }
        public IActionResult ReloadLIPAssetTransferSummary(int DId, int TId, int UCId, int PId)
        {
            bool IsRefugee = true;
            return ViewComponent("LIPAssetTransferSummary", new { DId, TId, UCId, PId, IsRefugee });
        }
      
        // GET: LIPAssetTransfers/Print/5
        public async Task<IActionResult> Print(int? id)
        {
            if (id == null || _context.LIPAssetTransfer == null)
            {
                return NotFound();
            }

            LIPMemberAfghan Obj = new LIPMemberAfghan();
            Obj.LIPAssetTransfer = await _context.LIPAssetTransfer
                .Include(l => l.LIPPackage)
                .Include(l => l.Village.UnionCouncil.Tehsil.District)
                .Include(l => l.IdentifiedBy)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.LIPAssetTransferId == id);
            Obj.Member = await _context.Member.FindAsync(Obj.LIPAssetTransfer.MemberId);
            Obj.MemberAfghanDetail = await _context.MemberAfghanDetail.FindAsync(Obj.LIPAssetTransfer.MemberId);


            var lIPAssetTransfer = await _context.LIPAssetTransfer
                .Include(l => l.LIPPackage)
                .Include(l => l.Village.UnionCouncil.Tehsil.District)
                .Include(l => l.IdentifiedBy)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.LIPAssetTransferId == id);
            if (lIPAssetTransfer == null)
            {
                return NotFound();
            }

            return View(Obj);
        }


    }
}
