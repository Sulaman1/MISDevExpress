using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.HTSModule;
using DBContext.Data;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using DAL.Models;
using DAL.Models.Domain.MasterSetup;
using DAL.Models.ViewModels;
using static Constant.Constants.Permissions;
using Microsoft.Extensions.Hosting;

namespace CCView.Controllers.HTSModule
{
    public class HTSController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public HTSController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: HTSs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HTS.Include(a=>a.Member.Village.UnionCouncil.Tehsil.District);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HTSs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var HTS = await _context.HTS.Include(a=>a.Member.Village.UnionCouncil.Tehsil.District).Where(a => a.HTSId == id).FirstOrDefaultAsync();
            var applicationDbContext = _context.HTSStage.Include(b => b.HTS).Where(a => a.HTSId == id);
            ViewBag.TotalGrant = _context.HTS.Find(id).TotalGrant;
            ViewBag.CompletedInstallment = applicationDbContext.Count();
            ViewBag.IsCompleted = HTS.IsCompleted;
            ViewBag.ReceivedGrant = applicationDbContext.Sum(a => a.AmountPaid);
            ViewBag.Id = id;
            ViewBag.BName = HTS.Member.MemberName;
            return View(HTS);
        }

        // GET: HTSs/Create
        public async Task<IActionResult> Create()
        {
            var currentuser = await _userManager.GetUserAsync(User);
            var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
            if (currentuser.DistrictId > 1)
            {
                districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
            }
            ViewData["DistrictId"] = new SelectList(districts, "DistrictId", "Name");
            Member member = new Member();
            member.BeneficiaryTypeId = 1;
            member.IsRefugee = false;
            DAL.Models.Domain.HTSModule.HTS hst = new DAL.Models.Domain.HTSModule.HTS();
            hst.IsCompleted = false;     
            hst.UserId = currentuser.Id;
            HTSMember Obj = new HTSMember();
            Obj.Member = member;
            Obj.HTS = hst;
            return View(Obj);
        }

        // POST: HTSs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HTSMember tuple, int DistrictId, IFormFile CNICAttachment, IFormFile AgricultureLandProofAttachment, IFormFile ApplicationFormAttachment, IFormFile TunnelSiteSuitabilityFormAttachment)
        {
            if (ModelState.IsValid)
            {
                var linker = _context.Village.Include(a => a.UnionCouncil.Tehsil).Where(a => a.VillageId == tuple.Member.VillageId).FirstOrDefault();
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
                    ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId == linker.UnionCouncil.Tehsil.DistrictId), "DistrictId", "Name", linker.UnionCouncil.Tehsil.DistrictId);
                    ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == linker.UnionCouncil.Tehsil.DistrictId), "TehsilId", "TehsilName", linker.UnionCouncil.TehsilId);
                    ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == linker.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", linker.UnionCouncilId);
                    ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == linker.UnionCouncilId), "VillageId", "Name", linker.VillageId);
                    return View(tuple);
                }
                var result = _context.Member.Where(a => a.CNIC == tuple.Member.CNIC).FirstOrDefault();
                if (result != null)
                {
                    MemberId = _context.Member.Where(a => a.CNIC == tuple.Member.CNIC).Select(a => a.MemberId).FirstOrDefault();
                    if (result.VillageId == 0)
                    {
                        result.VillageId = tuple.Member.VillageId;
                        var memberdata = _context.Member.Find(MemberId);
                        memberdata.VillageId = result.VillageId;
                        _context.Update(memberdata);
                        await _context.SaveChangesAsync();
                    }
                    if (tuple.Member.VillageId != result.VillageId)
                    {
                        ModelState.AddModelError("CustomError", "Member already exist in MIS but village information mismatch!");
                        var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
                        if (currentuser.DistrictId > 1)
                        {
                            districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
                        }
                        ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId == linker.UnionCouncil.Tehsil.DistrictId), "DistrictId", "Name", linker.UnionCouncil.Tehsil.DistrictId);
                        ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == linker.UnionCouncil.Tehsil.DistrictId), "TehsilId", "TehsilName", linker.UnionCouncil.TehsilId);
                        ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == linker.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", linker.UnionCouncilId);
                        ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == linker.UnionCouncilId), "VillageId", "Name", linker.VillageId);
                        return View(tuple);
                    }
                }
                else
                {
                    _context.Add(tuple.Member);
                    _context.SaveChanges();
                    MemberId = _context.Member.Max(a => a.MemberId);
                }
                //var IsExist = _context.LIPAssetTransfer.Count(a => a.MemberId == MemberId);
                //if (IsExist > 0)
                //{
                //    ModelState.AddModelError("CustomError", "Member already exist in LIP pool!");
                //    var districts = _context.District.Where(a => a.DistrictId > 1).ToList();
                //    if (currentuser.DistrictId > 1)
                //    {
                //        districts = districts.Where(a => a.DistrictId == currentuser.DistrictId).ToList();
                //    }
                //    ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId == linker.UnionCouncil.Tehsil.DistrictId), "DistrictId", "Name", linker.UnionCouncil.Tehsil.DistrictId);
                //    ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == linker.UnionCouncil.Tehsil.DistrictId), "TehsilId", "TehsilName", linker.UnionCouncil.TehsilId);
                //    ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == linker.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", linker.UnionCouncilId);
                //    ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == linker.UnionCouncilId), "VillageId", "Name", linker.VillageId);
                //    return View(tuple);
                //}              
               
                tuple.HTS.MemberId = MemberId;
                tuple.HTS.VillageId = tuple.Member.VillageId;
                if (CNICAttachment != null && CNICAttachment.Length > 0)
                {
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HTS\\CNIC\\" + randomNumber.ToString() + "\\");
                    string fileName = Path.GetFileName(CNICAttachment.FileName);                    
                    int randomNumber2 = random.Next(9999, 100000);
                    fileName = "CNIC" + randomNumber2.ToString() + Path.GetExtension(fileName);
                    tuple.HTS.CNICAttachment = Path.Combine("/Documents/HTS/CNIC/" + randomNumber.ToString() + "/" + fileName);//Server Path
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
                if (AgricultureLandProofAttachment != null && AgricultureLandProofAttachment.Length > 0)
                {
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HTS\\ALP\\" + randomNumber.ToString() + "\\");
                    string fileName = Path.GetFileName(AgricultureLandProofAttachment.FileName);
                    int randomNumber2 = random.Next(9999, 100000);
                    fileName = "CNIC" + randomNumber2.ToString() + Path.GetExtension(fileName);
                    tuple.HTS.AgricultureLandProofAttachment = Path.Combine("/Documents/HTS/ALP/" + randomNumber.ToString() + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await AgricultureLandProofAttachment.CopyToAsync(stream);
                    }
                }
                if (ApplicationFormAttachment != null && ApplicationFormAttachment.Length > 0)
                {
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HTS\\AF\\" + randomNumber.ToString() + "\\");
                    string fileName = Path.GetFileName(ApplicationFormAttachment.FileName);
                    int randomNumber2 = random.Next(9999, 100000);
                    fileName = "AF" + randomNumber2.ToString() + Path.GetExtension(fileName);
                    tuple.HTS.ApplicationFormAttachment = Path.Combine("/Documents/HTS/AF/" + randomNumber.ToString() + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await ApplicationFormAttachment.CopyToAsync(stream);
                    }
                }
                if (TunnelSiteSuitabilityFormAttachment != null && TunnelSiteSuitabilityFormAttachment.Length > 0)
                {
                    Random random = new Random();
                    int randomNumber = random.Next(9999, 100000);
                    var rootPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HTS\\TSS\\" + randomNumber.ToString() + "\\");
                    string fileName = Path.GetFileName(TunnelSiteSuitabilityFormAttachment.FileName);
                    int randomNumber2 = random.Next(9999, 100000);
                    fileName = "TSS" + randomNumber2.ToString() + Path.GetExtension(fileName);
                    tuple.HTS.TunnelSiteSuitabilityFormAttachment = Path.Combine("/Documents/HTS/TSS/" + randomNumber.ToString() + "/" + fileName);//Server Path
                    string sPath = Path.Combine(rootPath);
                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }
                    string FullPathWithFileName = Path.Combine(sPath, fileName);
                    using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                    {
                        await TunnelSiteSuitabilityFormAttachment.CopyToAsync(stream);
                    }
                }
                tuple.HTS.CreatedBy = User.Identity.Name;
                tuple.HTS.CreatedOn = DateTime.Today;                
                tuple.HTS.UserId = currentuser.Id;
                tuple.HTS.District = _context.District.Find(DistrictId).Name;
                //----------------------
                var HTSCount = _context.HTS.Count(a => a.District == tuple.HTS.District) + 1;
                string DistrictCode = _context.District.Find(DistrictId).Code;
                string val = (HTSCount).ToString("D3");
                tuple.HTS.HTSCode = (DistrictCode + "-" + val);
                while (_context.LIPAssetTransfer.Count(a => a.LIPCode == tuple.HTS.HTSCode) > 0)
                {
                    val = (++HTSCount).ToString("D3");
                    tuple.HTS.HTSCode = (DistrictCode + "-" + val);
                }
                _context.Add(tuple.HTS);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }            
            return View(tuple);
        }

        // GET: HTSs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HTS == null)
            {
                return NotFound();
            }

            var hts = await _context.HTS.Include(a => a.Member).Include(a => a.Village.UnionCouncil.Tehsil.District).Where(a => a.HTSId == id).FirstOrDefaultAsync();
            if (hts == null)
            {
                return NotFound();
            }            
            var linker = _context.Village.Include(a => a.UnionCouncil.Tehsil).Where(a => a.VillageId == hts.VillageId).FirstOrDefault();
            ViewData["DistrictId"] = new SelectList(_context.District.Where(a => a.DistrictId == linker.UnionCouncil.Tehsil.DistrictId), "DistrictId", "Name", hts.Village.UnionCouncil.Tehsil.DistrictId);
            ViewData["TehsilId"] = new SelectList(_context.Tehsil.Where(a => a.DistrictId == linker.UnionCouncil.Tehsil.DistrictId), "TehsilId", "TehsilName", linker.UnionCouncil.TehsilId);
            ViewData["UnionCouncilId"] = new SelectList(_context.UnionCouncil.Where(a => a.TehsilId == linker.UnionCouncil.TehsilId), "UnionCouncilId", "UnionCouncilName", linker.UnionCouncilId);
            ViewData["VillageId"] = new SelectList(_context.Village.Where(a => a.UnionCouncilId == linker.UnionCouncilId), "VillageId", "Name", linker.VillageId);
            HTSMember Obj = new HTSMember();
            Obj.Member = _context.Member.Find(hts.MemberId);
            Obj.HTS = hts;
            return View(Obj);
        }

        // POST: HTSs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int DistrictId, DAL.Models.ViewModels.HTSMember tuple, IFormFile CNICAttachment, IFormFile AgricultureLandProofAttachment, IFormFile ApplicationFormAttachment, IFormFile TunnelSiteSuitabilityFormAttachment)
        {
            if (id != tuple.HTS.HTSId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (CNICAttachment != null && CNICAttachment.Length > 0)
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HTS\\CNIC\\" + randomNumber.ToString() + "\\");
                        string fileName = Path.GetFileName(CNICAttachment.FileName);
                        int randomNumber2 = random.Next(9999, 100000);
                        fileName = "CNIC" + randomNumber2.ToString() + Path.GetExtension(fileName);
                        tuple.HTS.CNICAttachment = Path.Combine("/Documents/HTS/CNIC/" + randomNumber.ToString() + "/" + fileName);//Server Path
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
                    if (AgricultureLandProofAttachment != null && AgricultureLandProofAttachment.Length > 0)
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HTS\\ALP\\" + randomNumber.ToString() + "\\");
                        string fileName = Path.GetFileName(AgricultureLandProofAttachment.FileName);
                        int randomNumber2 = random.Next(9999, 100000);
                        fileName = "CNIC" + randomNumber2.ToString() + Path.GetExtension(fileName);
                        tuple.HTS.AgricultureLandProofAttachment = Path.Combine("/Documents/HTS/ALP/" + randomNumber.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await AgricultureLandProofAttachment.CopyToAsync(stream);
                        }
                    }
                    if (ApplicationFormAttachment != null && ApplicationFormAttachment.Length > 0)
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HTS\\AF\\" + randomNumber.ToString() + "\\");
                        string fileName = Path.GetFileName(ApplicationFormAttachment.FileName);
                        int randomNumber2 = random.Next(9999, 100000);
                        fileName = "AF" + randomNumber2.ToString() + Path.GetExtension(fileName);
                        tuple.HTS.ApplicationFormAttachment = Path.Combine("/Documents/HTS/AF/" + randomNumber.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await ApplicationFormAttachment.CopyToAsync(stream);
                        }
                    }
                    if (TunnelSiteSuitabilityFormAttachment != null && TunnelSiteSuitabilityFormAttachment.Length > 0)
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(9999, 100000);
                        var rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Documents\\HTS\\TSS\\" + randomNumber.ToString() + "\\");
                        string fileName = Path.GetFileName(TunnelSiteSuitabilityFormAttachment.FileName);
                        int randomNumber2 = random.Next(9999, 100000);
                        fileName = "TSS" + randomNumber2.ToString() + Path.GetExtension(fileName);
                        tuple.HTS.TunnelSiteSuitabilityFormAttachment = Path.Combine("/Documents/HTS/TSS/" + randomNumber.ToString() + "/" + fileName);//Server Path
                        string sPath = Path.Combine(rootPath);
                        if (!System.IO.Directory.Exists(sPath))
                        {
                            System.IO.Directory.CreateDirectory(sPath);
                        }
                        string FullPathWithFileName = Path.Combine(sPath, fileName);
                        using (var stream = new FileStream(FullPathWithFileName, FileMode.Create))
                        {
                            await TunnelSiteSuitabilityFormAttachment.CopyToAsync(stream);
                        }
                    }
                    tuple.HTS.District = _context.District.Find(DistrictId).Name;
                    _context.Update(tuple.HTS);
                    _context.Update(tuple.Member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HTSExists(tuple.HTS.HTSId))
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
            return View(tuple);
        }

        // GET: HTSs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HTS == null)
            {
                return NotFound();
            }

            var HTS = await _context.HTS                
                .FirstOrDefaultAsync(m => m.HTSId == id);
            if (HTS == null)
            {
                return NotFound();
            }

            return View(HTS);
        }

        // POST: HTSs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HTS == null)
            {
                return Problem("Entity set 'ApplicationDbContext.HTS'  is null.");
            }
            var HTS = await _context.HTS.FindAsync(id);
            if (HTS != null)
            {
                _context.HTS.Remove(HTS);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HTSExists(int id)
        {
          return _context.HTS.Any(e => e.HTSId == id);
        }
    }
}
