using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.MasterSetup;
using DBContext.Data;

namespace BLEPMIS.Controllers.MasterSetup
{
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Member.Where(a=>a.BeneficiaryTypeId == 1).Select(a=> new Member { MemberId=a.MemberId, MemberName = a.MemberName, FatherName=a.FatherName, CNIC=a.CNIC, CellNo=a.CellNo, Gender=a.Gender});
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .Include(m => m.BeneficiaryType)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }      
        public async Task<IActionResult> Edit2(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }            
            return View(member);
        }

        // POST: Districts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2(int id, Member member)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var IsExist = _context.Member.Count(a=>a.CNIC == member.CNIC);
                    if (IsExist > 1)
                    {
                        ModelState.AddModelError(nameof(member.CNIC), "Already exist with same name!");
                        return View(member);
                    }
                    //_context.Entry(district).CurrentValues.SetValues(district);
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                    //await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                    /*_context.Update(district);
                    await _context.SaveChangesAsync();*/
                }
                catch (DbUpdateConcurrencyException)
                {
                   
                }
                return RedirectToAction(nameof(Index));
            }            
            return View(member);
        }

    }
}
