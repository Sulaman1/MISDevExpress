using DAL.Models.Domain.MasterSetup;
using DAL.Models.Domain.Training;
using DAL.Models.ViewModels;
using DAL.Models;
using DBContext.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using BLEPMIS.Areas.IP.Models.ViewModels;

namespace BLEPMIS.Areas.IP.Controllers.Training
{
    public class LIPTrainings22Controller : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public LIPTrainings22Controller(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: LIPAssetTransfers
        public async Task<IActionResult> Index()
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
        public async Task<IActionResult> SubmittedForReviewIndex()
        {
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.IdentifiedBy).Select(a => new LIPAssetTransfer { LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, Member = new Member { MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC }, IsSubmitted = a.IsSubmitted, IsReviewed = a.IsReviewed, DistrictId = a.DistrictId }).Where(a => a.IsSubmitted == true && a.IsReviewed == false).ToListAsync();
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
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.IdentifiedBy).Select(a => new LIPAssetTransfer { LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, Member = new Member { MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC, Village = new Village { Name = a.Village.Name, UnionCouncilId = a.Village.UnionCouncilId, UnionCouncil = new UnionCouncil { UnionCouncilName = a.Village.UnionCouncil.UnionCouncilName } } }, DistrictId = a.DistrictId, IsSubmitted = a.IsSubmitted, IsReviewed = a.IsReviewed, IsAssetTransfer = a.IsAssetTransfer }).Where(a => a.IsReviewed == true && a.IsAssetTransfer == false).ToListAsync();
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(applicationDbContext);
        }
        public async Task<IActionResult> ATIndex()
        {
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.Village.UnionCouncil.Tehsil).Include(a => a.IdentifiedBy).Select(a =>
                new LIPAssetTransfer
                {
                    LIPCode = a.LIPCode,
                    LIPAssetTransferId = a.LIPAssetTransferId,
                    LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName },
                    IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name },
                    Member = new Member { MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC },
                    IsSubmitted = a.IsSubmitted,
                    IsAssetTransfer = a.IsAssetTransfer,
                    Village = new Village { Name = a.Village.Name, UnionCouncil = new UnionCouncil { UnionCouncilName = a.Village.UnionCouncil.UnionCouncilName, Tehsil = new Tehsil { TehsilName = a.Village.UnionCouncil.Tehsil.TehsilName } } }
                }).Where(a => a.IsAssetTransfer == true).ToListAsync();
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(applicationDbContext);
        }
        public async Task<IActionResult> VerifiedIndex2()
        {
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.IdentifiedBy).Select(a => new LIPAssetTransfer { LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, Member = new Member { MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC }, DistrictId = a.DistrictId, IsSubmitted = a.IsSubmitted, IsReviewed = a.IsReviewed, IsInternalReviewed = a.IsInternalReviewed }).Where(a => a.IsReviewed == true && a.IsInternalReviewed == false).ToListAsync();
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.DistrictId > 1)
            {
                applicationDbContext = applicationDbContext.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            return View(applicationDbContext);
        }
        public async Task<IActionResult> VerifiedIndex3()
        {
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.IdentifiedBy).Select(a => new LIPAssetTransfer { LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, Member = new Member { MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CNIC = a.Member.CNIC }, DistrictId = a.DistrictId, IsSubmitted = a.IsSubmitted, IsInternalReviewed = a.IsInternalReviewed }).Where(a => a.IsInternalReviewed == true).ToListAsync();
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
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
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
            var Info = _context.LIPAssetTransfer.Include(a => a.Member.Village.UnionCouncil.Tehsil.District).Include(a => a.IdentifiedBy).Include(a => a.LIPPackage).Where(a => a.Member.CNIC == id && a.DistrictId == currentdistrictId).FirstOrDefault();
            if (Info == null)
            {
                return Json(new { isValid = false, isDuplicate = false });
            }
            else
            {
                var IsExist = _context.LIPAssetTransfer.Include(a => a.Member).Count(a => a.Member.CNIC == id && a.IsReviewed == true);
                if (IsExist > 0)
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
        public async Task<JsonResult> ApprovalRequest(int id, int val, string description)
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
            member.BeneficiaryTypeId = 2;
            member.IsRefugee = false;
            member.BeneficiaryTypeId = 2;
            member.BeneficiaryTypeId = 2;
            member.BeneficiaryTypeId = 2;
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
            LIPMemberAfgan Obj = new LIPMemberAfgan();
            Obj.Member = member;
            Obj.LIPAssetTransfer = lIPAssetTransfer;
            return View(Obj);
        }

        // POST: LIPAssetTransfers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LIPMemberAfgan tuple, int DistrictId, IFormFile LIPFormAttachment, IFormFile GRNAttachment, IFormFile CNICAttachment, IFormFile PictureAttachment1, IFormFile ProfilePicture)
        {
            if (ModelState.IsValid)
            {
                int MemberId = 0;
                var currentuser = await _userManager.GetUserAsync(User);
                var result = _context.Member.Where(a => a.CNIC == tuple.Member.CNIC).FirstOrDefault();
                if (result != null)
                {
                    MemberId = _context.Member.Where(a => a.CNIC == tuple.Member.CNIC).Select(a => a.MemberId).FirstOrDefault();
                    if (tuple.Member.VillageId != result.VillageId)
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
                var LIPCount = _context.LIPAssetTransfer.Count(a => a.DistrictId == DistrictId) + 1;
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
            ViewBag.UserId = currentuser.Id;
            var lIPAssetTransfer = await _context.LIPAssetTransfer.Include(a => a.Member).Where(a => a.LIPAssetTransferId == id).FirstOrDefaultAsync();
            if (lIPAssetTransfer == null)
            {
                return NotFound();
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
                    lIPAssetTransfer.IdentifiedBy = null;
                    var MemberObj = new Member();
                    MemberObj = lIPAssetTransfer.Member;
                    //lIPAssetTransfer.Member = null;
                    lIPAssetTransfer.LIPPackage = null;
                    var currentuser = await _userManager.GetUserAsync(User);
                    lIPAssetTransfer.UserId = currentuser.Id;
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
        public async Task<IActionResult> Edit2(int id, LIPAssetTransfer lIPAssetTransfer, int DistrictId, int TehsilId, IFormFile LIPFormAttachment, IFormFile GRNAttachment, IFormFile CNICAttachment, IFormFile PictureAttachment1)
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
        // GET: LIPAssetTransfers/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
    }
}
