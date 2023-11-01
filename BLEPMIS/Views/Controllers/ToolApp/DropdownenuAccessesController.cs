using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.ToolApp;
using DBContext.Data;
using DAL.Models.Domain.MasterSetup;
using BAL.IRepository.MasterSetup;
using System.Collections;

namespace BLEPMIS.Controllers.ToolApp
{
    public class DropdownenuAccessesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DropdownenuAccessesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DropdownenuAccesses
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.ToolId = id;
              return View(await _context.DropdownenuAccess.Where(a=>a.ToolId == id).ToListAsync());
        }
        public async Task<IActionResult> _Index(int id, string module)
        {            
            var data = await _context.DropdownMenu.Where(a => a.ToolModuleName == module).ToListAsync();
            int IsDistrictExist = 0;
            int IsLIPPackageExist = 0;            
            foreach(var indicator in data)
            {
                if (indicator.DropdownMenuName.Equals("District")){
                    var DList = new SelectList(_context.DropdownenuAccess.Where(a => a.ToolId == id && a.DropdownMenuName == indicator.DropdownMenuName), "Value", "Value");
                    ViewBag.DistrictId = DList;
                    IsDistrictExist = DList.Count() > 0 ? 1 : 0;
                    ViewBag.IsDistrictExist = IsDistrictExist;
                }
                else if (indicator.DropdownMenuName.Equals("LIPPackage"))
                {
                    var LIPPList = new SelectList(_context.DropdownenuAccess.Where(a => a.ToolId == id && a.DropdownMenuName == indicator.DropdownMenuName), "Value", "Value");
                    ViewBag.PackageId = LIPPList;
                    IsLIPPackageExist = LIPPList.Count() > 0 ? 1 : 0;
                    ViewBag.IsLIPPackageExist = IsLIPPackageExist;
                }
            }
            return PartialView(data);
        }
        // GET: DropdownenuAccesses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DropdownenuAccess == null)
            {
                return NotFound();
            }

            var dropdownenuAccess = await _context.DropdownenuAccess
                .FirstOrDefaultAsync(m => m.DropdownenuAccessId == id);
            if (dropdownenuAccess == null)
            {
                return NotFound();
            }

            return View(dropdownenuAccess);
        }
        public async Task<JsonResult> GetDropdown(string DropdownMenuName)
        {
            List<GeneralDropdown> list = new List<GeneralDropdown>();
            if (DropdownMenuName.Equals("District"))
            {
                var data = await _context.District.Where(a => a.DistrictId > 1).ToListAsync();
                var List = data.Select(m => new SelectListItem()
                {
                    Text = m.Name.ToString(),
                    Value = m.Name.ToString(),
                });
                return Json(List);
            }
            else if (DropdownMenuName.Equals("LIPPackage"))
            {
                var data = _context.LIPPackage.ToList();
                var List = data.Select(m => new SelectListItem()
                {
                    Text = m.PackageName.ToString(),
                    Value = m.PackageName.ToString(),
                });
                return Json(List);
            }
           
            return Json(null);
        }
        public async Task<JsonResult> GetBeneficiaryList(string districtName, string packageName)
        {
            var districtId = _context.District.Where(a=>a.Name == districtName).Select(a=>a.DistrictId).FirstOrDefault();
            var packageId = _context.LIPPackage.Where(a=>a.PackageName == packageName).Select(a=>a.LIPPackageId).FirstOrDefault();
            var data = await _context.LIPAssetTransfer.Include(a=>a.Member).Where(a=>a.DistrictId == districtId && a.IsAssetTransfer == true && a.Member.BeneficiaryTypeId == 1 && a.LIPPackageId == packageId).Select(a=> new GeneralDropdown { Text = (a.LIPCode + " " + a.Member.MemberName), Value = a.LIPAssetTransferId.ToString()}).ToListAsync();
            var dataList = data.Select(m => new SelectListItem()
            {
                Text = m.Text.ToString(),
                Value = m.Value.ToString(),
            });
            return Json(dataList);
        }
        // GET: DropdownenuAccesses/Create
        public IActionResult Create(int id)
        {
            ViewData["DropdownMenuId"] = new SelectList(_context.DropdownMenu.Where(a=>a.ToolModuleName.Equals("LIP")), "DropdownMenuName", "DropdownMenuName");
            DropdownenuAccess obj = new DropdownenuAccess();
            obj.ToolId = id;
            obj.ToolModuleName = _context.Tool.Include(a => a.ToolModule).Where(a => a.ToolId == id).Select(a => a.ToolModule.ToolModuleName).FirstOrDefault();
            return View(obj);
        }

        // POST: DropdownenuAccesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DropdownenuAccessId,ToolId,DropdownMenuName,ToolModuleName,Value")] DropdownenuAccess dropdownenuAccess)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dropdownenuAccess);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {id = dropdownenuAccess.ToolId});
            }
            return View(dropdownenuAccess);
        }

        // GET: DropdownenuAccesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DropdownenuAccess == null)
            {
                return NotFound();
            }

            var dropdownenuAccess = await _context.DropdownenuAccess.FindAsync(id);
            if (dropdownenuAccess == null)
            {
                return NotFound();
            }
            return View(dropdownenuAccess);
        }

        // POST: DropdownenuAccesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DropdownenuAccessId,ToolId,DropdownMenuName,ToolModuleName,Value")] DropdownenuAccess dropdownenuAccess)
        {
            if (id != dropdownenuAccess.DropdownenuAccessId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dropdownenuAccess);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DropdownenuAccessExists(dropdownenuAccess.DropdownenuAccessId))
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
            return View(dropdownenuAccess);
        }

        // GET: DropdownenuAccesses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DropdownenuAccess == null)
            {
                return NotFound();
            }

            var dropdownenuAccess = await _context.DropdownenuAccess
                .FirstOrDefaultAsync(m => m.DropdownenuAccessId == id);
            if (dropdownenuAccess == null)
            {
                return NotFound();
            }

            return View(dropdownenuAccess);
        }

        // POST: DropdownenuAccesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DropdownenuAccess == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DropdownenuAccess'  is null.");
            }
            var dropdownenuAccess = await _context.DropdownenuAccess.FindAsync(id);
            if (dropdownenuAccess != null)
            {
                _context.DropdownenuAccess.Remove(dropdownenuAccess);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {id = dropdownenuAccess.ToolId});
        }

        private bool DropdownenuAccessExists(int id)
        {
          return _context.DropdownenuAccess.Any(e => e.DropdownenuAccessId == id);
        }
    }
}
