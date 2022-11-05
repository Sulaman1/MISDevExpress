﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBContext.Data;
using DAL.Models.Domain.MasterSetup;
using Microsoft.AspNetCore.Http;
using System.IO;
using static Constant.Constants.Permissions;
using BAL.IRepository.MasterSetup.CD;

namespace BLEPMIS.Controllers.MasterSetup
{
    public class CommunityInstituteMembersController : Controller
    {
        private readonly ICommunityInstituteMember _context;
        private readonly IMember _member;

        public CommunityInstituteMembersController(ICommunityInstituteMember context, IMember member)
        {
            _context = context;
            _member = member;
        }

        // GET: CommunityInstituteMembers
        public async Task<IActionResult> Index()
        {            
            return View(await _context.GetAll());
        }
        public async Task<IActionResult> _Index(int id)
        {
            ViewBag.CommunityInstitutionId = id;            
            return PartialView(await _context.GetAllCIMember(id));
        }
        // GET: CommunityInstituteMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var communityInstituteMember = await _context.GetById(id);
            if (communityInstituteMember == null)
            {
                return NotFound();
            }

            return View(communityInstituteMember);
        }

        // GET: CommunityInstituteMembers/Create
        public async Task<IActionResult> Create(int id)
        {
            ViewBag.CIId = id;
            Member obj = new Member();
            ViewBag.CommunityInstitutionId = id;            
            obj.BeneficiaryTypeId = id < 3 ? 1 : 2;//KDA            
            ViewData["CommunityInstitutionId"] = new SelectList(await _context.GetAll(), "CommunityInstitutionId", "Name", id);
            ViewData["DesignationId"] = new SelectList(await _context.GetAllDesignation(), "DesignationId", "DesignationName");
            return View(obj);
        }

        // POST: CommunityInstituteMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member, IFormFile ProfilePicture, int CIId, int MemberId, int DesignationId)
        {
            if (ModelState.IsValid)
            {
                if(MemberId == 0)
                {                                        
                    _member.Insert(member, ProfilePicture);                    
                    MemberId = _member.Max();
                }
                else
                {
                    var result = _context.CountMemberInCIM(MemberId, CIId);
                    if (result > 0)
                    {
                        ModelState.AddModelError(nameof(member.CNIC), "Already member added with same CNIC!");
                        return BadRequest(ModelState);
                    }
                }
                _context.Insert(MemberId, CIId, DesignationId);
                return RedirectToAction(nameof(Create), new {CIId});
            }
            ViewData["CommunityInstitutionId"] = new SelectList(await _context.GetAllCI(), "CommunityInstitutionId", "Name", CIId);
            ViewData["DesignationId"] = new SelectList(await _context.GetAllDesignation(), "DesignationId", "DesignationName", DesignationId);
            return View(member);
        }

        public IActionResult AddBeneficiary()
        {
            Member obj = new Member();                        
            obj.BeneficiaryTypeId = 2;//KDA            
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBeneficiary(Member member, IFormFile ProfilePicture)
        {
            if (ModelState.IsValid)
            {
                var result = _member.Count(member.CNIC);
                if (result > 0)
                {
                    ModelState.AddModelError(nameof(member.CNIC), "CNIC already exist!");
                    return BadRequest(ModelState);
                }
                if (ProfilePicture != null && ProfilePicture.Length > 0)
                {
                    await using var memoryStream = new MemoryStream();
                    await ProfilePicture.CopyToAsync(memoryStream);
                    member.ProfilePicture = memoryStream.ToArray();
                }                         
                _member.Insert(member, ProfilePicture);
                _member.Save();
                return RedirectToAction(nameof(AddBeneficiary));
            }            
            return View(member);
        }
        // GET: CommunityInstituteMembers/Edit/5
        public async Task<IActionResult> Edit(int? id, int? CIMId, int? CIId)
        {
            if (id == null || CIMId == null)
            {
                return NotFound();
            }
            ViewBag.MemberId = id;
            ViewBag.CIId = CIId;
            ViewBag.CIMId = CIMId;
            var member = await _member.GetById(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["CommunityInstitutionId"] = new SelectList(await _context.GetCI(CIId ?? 0), "CommunityInstitutionId", "Name");
            ViewData["DesignationId"] = new SelectList(await _context.GetAllDesignation(), "DesignationId", "DesignationName");
            return View(member);
        }

        // POST: CommunityInstituteMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int CIId, int CIMId, Member member, IFormFile ProfilePicture, int DesignationId)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {                            
                    _member.Update(member, ProfilePicture);
                    CommunityInstituteMember communityInstituteMember = await _context.GetById(CIMId);
                    if(communityInstituteMember.DesignationId != DesignationId)
                    {
                        communityInstituteMember.DesignationId = DesignationId;
                        _context.Update(communityInstituteMember);
                    }
                    _context.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunityInstituteMemberExists(member.MemberId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details","CommunityInstitutions", new {id = CIId});
            }
            ViewData["CommunityInstitutionId"] = new SelectList(await _context.GetAllCI(), "CommunityInstitutionId", "Name", CIId);
            ViewData["DesignationId"] = new SelectList(await _context.GetAllDesignation(), "DesignationId", "DesignationName", DesignationId);
            return View(member);
        }

        // GET: CommunityInstituteMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var communityInstituteMember = await _context.GetById(id);
            if (communityInstituteMember == null)
            {
                return NotFound();
            }

            return View(communityInstituteMember);
        }

        // POST: CommunityInstituteMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CommunityInstituteMember'  is null.");
            }
            var communityInstituteMember = await _context.GetById(id);
            if (communityInstituteMember != null)
            {
                _context.Remove(communityInstituteMember);
            }
            
            _context.Save();
            return RedirectToAction("Details", "CommunityInstitutions", new { id = communityInstituteMember.CommunityInstitutionId });
        }

        private bool CommunityInstituteMemberExists(int id)
        {
          return _context.Exist(id);
        }
    }
}