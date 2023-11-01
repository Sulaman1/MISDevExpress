
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.MasterSetup;
using BAL.IRepository.MasterSetup;
using DBContext.Data;
using DAL.Models.Domain.BSF;
using System.Linq;
using DAL.Models.Domain.ToolApp;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using DAL.Models;
using BAL.IRepository.MasterSetup.UserManagement;
using Microsoft.AspNetCore.Identity;

namespace BLEPMIS.Controllers.ToolApp
{    
    public class ToolsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ToolsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Tool.ToListAsync());
        }
        public async Task<IActionResult> _IndexToolAccess(string UId)
        {
            return PartialView(await _context.ToolUserAccess.Include(a=>a.Tool).Where(a=>a.UserId == UId).ToListAsync());
        }
        public async Task<IActionResult> UserAccess(int id, string UId, bool test = true)
        {
            ViewBag.UId = UId;
            ViewBag.ToolId = id;
            ViewData["DistrictId"] = new SelectList(_context.District, "DistrictId", "Name");
            if (UId is not null)
            {
                ViewData["UserId"] = new SelectList(_context.Users.Select(a => new ApplicationUser { Id = a.Id, UserName = a.FirstName + " " + a.LastName }), "Id", "UserName", UId);
            }
            else
            {
                //ViewData["UserId"] = new SelectList(_context.Users.Select(a => new ApplicationUser { Id = a.Id, UserName = a.UserName }), "Id", "UserName");
            }
            return View();
        }
        public async Task<IActionResult> ViewToolAccess(int id)
        {
            var data = await _context.ToolUserAccess.Include(a=>a.ApplicationUser).Where(a => a.ToolId == id).Select(a=> new ToolUserAccess { Username = a.ApplicationUser.FirstName + " " + a.ApplicationUser.LastName, Tool = a.Tool, ToolUserAccessId = a.ToolUserAccessId}).ToListAsync();
            return View(data);
        }
        public async Task<JsonResult> GetUsers(int districtId)
        {
            List<ApplicationUser> users = _context.Users.Where(a=>a.DistrictId == districtId).Select(a=> new ApplicationUser { Id=a.Id, FirstName = a.FirstName, LastName = a.LastName}).ToList();
            var userList = users.Select(m => new SelectListItem()
            {
                Text = m.FirstName + " " + m.LastName,
                Value = m.Id,
            });
            return Json(userList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserAccess(int toolId, string userId)
        {
            if (ModelState.IsValid)
            {
                var IsExist = _context.ToolUserAccess.Count(a=>a.UserId.Equals(userId) && a.ToolId == toolId);
                if(IsExist > 0)
                {
                    return RedirectToAction(nameof(UserAccess), new {id = toolId, UId = userId});
                }
                ToolUserAccess obj = new ToolUserAccess();
                obj.ToolId = toolId;
                obj.UserId = userId;
                obj.Username = _context.Users.Find(userId).UserName;               
                _context.Add(obj);
                var currentuser = await _context.Users.FindAsync(userId);
                currentuser.ToolAccess += ",T" + toolId + ",";
                _context.Update(currentuser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(UserAccess), new { id = toolId, UId = userId });
            }
            return View();
        }
        public async Task<IActionResult> _Index(int id)
        {
            ViewBag.Id = id;
            return PartialView(await _context.ToolControl.Include(a=>a.Control).Where(a=>a.ToolId == id).ToListAsync());
        }
        public async Task<IActionResult> _IndexToolControlDetail(int id)
        {
            return PartialView(await _context.ToolControlDetail.Include(a => a.ToolControl.Control).Where(a => a.ToolControlId == id).ToListAsync());
        }
        // GET: Tools/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tool == null)
            {
                return NotFound();
            }
            var toolControl = await _context.ToolControl.Include(a=>a.Tool).Include(a=>a.Control)
                .Where(m => m.ToolId == id).OrderBy(a=>a.OrderNo).ToListAsync();
            var tool = _context.Tool.Find(id);
            tool.ToolControl = toolControl;
            int counter = 0;
            foreach(var control in toolControl)
            {
                tool.ToolControl.ElementAt(counter).ToolControlDetail = _context.ToolControlDetail.Where(a=>a.ToolControlId == control.ToolControlId).ToList();
                counter++;
            }
            if (tool == null)
            {
                return NotFound();
            }
            return View(tool);
        }

        public async Task<IActionResult> DuplicateTool(int? id)
        {
            if (id == null || _context.Tool == null)
            {
                return NotFound();
            }
            var obj = new Tool();
            obj = _context.Tool.Find(id);            
            obj.ToolId = 0;
            obj.Name += " (copy)";
            _context.Add(obj);
            _context.SaveChanges();
            var MaxId = _context.Tool.Max(a => a.ToolId);
            //----------------------------------------------------
            var toolControl = await _context.ToolControl.Where(m => m.ToolId == id).OrderBy(a => a.OrderNo).ToListAsync();            
            foreach(var control in toolControl)
            {             
                var details = _context.ToolControlDetail.Where(a => a.ToolControlId == control.ToolControlId).ToList();
                //----------------------------------
                var TC = new ToolControl();
                TC.ToolId = MaxId;
                TC.ControlId = control.ControlId;
                TC.IsActive = control.IsActive;
                TC.Name = control.Name;
                TC.OrderNo = control.OrderNo;
                TC.Value = control.Value;               
                _context.Add(TC);
                _context.SaveChanges();
                //----------------------------------
                var MaxControlToolId = _context.ToolControl.Max(a => a.ToolControlId); 
                if(details != null)
                {
                    foreach(var detail in details)
                    {
                        var obj3 = new ToolControlDetail();
                        obj3.Value = detail.Value;
                        obj3.Label = detail.Label;                                                                   
                        obj3.ToolControlId = MaxControlToolId;                                                                                         
                        _context.Add(obj3);
                        _context.SaveChanges();
                    }                    
                }
            }
           
            return RedirectToAction(nameof(Index));
        }

        // GET: Tools/Create
        public IActionResult Create()
        {
            ViewData["ToolModuleId"] = new SelectList(_context.ToolModule, "ToolModuleName", "ToolModuleName");
            return View();
        }

        // POST: Tools/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tool tool)
        {
            if (ModelState.IsValid)
            {
                tool.CreatedOn = DateTime.Now;
                _context.Add(tool);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }            
            return View(tool);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tool == null)
            {
                return NotFound();
            }

            var dropdownenuAccess = await _context.Tool.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, Tool dropdownenuAccess)
        {
            if (id != dropdownenuAccess.ToolId)
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
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dropdownenuAccess);
        }
        public IActionResult AddControl(int id)
        {
            ToolControl obj = new ToolControl();
            obj.ToolId = id;
            obj.IsActive = true;
            obj.Value = "";
            obj.OrderNo = _context.ToolControl.Count(a => a.ToolId == id) + 1;
            ViewData["ControlId"] = new SelectList(_context.Control, "ControlId", "Name");
            ViewData["LIPPackageId"] = new SelectList(_context.LIPPackage, "PackageName", "PackageName");
            return View(obj);
        }

        // POST: Tools/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddControl(ToolControl toolControl)
        {
            if (ModelState.IsValid)
            {                
                _context.Add(toolControl);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new {id = toolControl.ToolId});
            }
            return View(toolControl);
        }
        public IActionResult AddControlOption(int id)
        {
            ToolControlDetail obj = new ToolControlDetail();
            obj.ToolControlId = id;
            obj.Value = false;
            var toolControl = _context.ToolControl.Include(a=>a.Control).Include(a=>a.Tool).Where(a=>a.ToolControlId == id).FirstOrDefault();
            ViewBag.ControlName = toolControl.Name;
            ViewBag.ToolId = toolControl.ToolId;
            ViewBag.Control = toolControl.Control.Name;
            ViewBag.ToolName = toolControl.Tool.Name;
            return View(obj);
        }

        // POST: Tools/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddControlOption(ToolControlDetail tool)
        {
            if (ModelState.IsValid)
            {                
                _context.Add(tool);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AddControlOption), new {id = tool.ToolControlId});
            }
            return View(tool);
        }
        public async Task<IActionResult> EditTool(int? id)
        {
            if (id == null || _context.Tool == null)
            {
                return NotFound();
            }

            var tool = await _context.Tool.FindAsync(id);
            if (tool == null)
            {
                return NotFound();
            }
            ViewData["ToolModuleId"] = new SelectList(_context.ToolModule.Where(a=>a.ToolModuleName.Equals(tool.ToolModuleName)), "ToolModuleName", "ToolModuleName");
            return View(tool);
        }

        // POST: Tools/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTool(int id, Tool tool)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   
                    _context.Update(tool);
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                ViewData["ToolModuleId"] = new SelectList(_context.ToolModule.Where(a => a.ToolModuleName.Equals(tool.ToolModuleName)), "ToolModuleName", "ToolModuleName");
                return RedirectToAction(nameof(Index));
            }
            return View(tool);
        }
        // GET: Tools/Edit/5
        public async Task<IActionResult> EditToolControlOrder(int? id)
        {
            if (id == null || _context.Tool == null)
            {
                return NotFound();
            }
            ViewBag.Id = id;
            var tool = await _context.ToolControl.Where(a=>a.ToolId == id).ToListAsync();
            if (tool == null)
            {
                return NotFound();
            }
            List<ToolControl> objs = new List<ToolControl>();
            foreach(var toolControl in tool)
            {
                objs.Add(toolControl);
            }
            return View(objs);
        }

        // POST: Tools/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public async Task<IActionResult> EditToolControlOrder(int id, List<ToolControl> toolControlList)
        {           
            if (ModelState.IsValid)
            {
                try
                {
                    foreach(var toolControl in toolControlList)
                    {
                        _context.Update(toolControl);
                    }                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {                   
                }
                return RedirectToAction(nameof(Index));
            }
            return View(toolControlList);
        }

        // GET: Tools/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tool == null)
            {
                return NotFound();
            }

            var tool = await _context.Tool
                .FirstOrDefaultAsync(m => m.ToolId == id);
            if (tool == null)
            {
                return NotFound();
            }
            ViewBag.IsExist = _context.ToolControlInfoPost.Count(a => a.ToolId == id);
            return View(tool);
        }

        // POST: Tools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tool == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tool'  is null.");
            }
            var tool = await _context.Tool.FindAsync(id);
            if (tool != null)
            {
                _context.Tool.Remove(tool);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Revoke(int? id)
        {
            if (id == null || _context.ToolUserAccess == null)
            {
                return NotFound();
            }

            var tool = await _context.ToolUserAccess
                .FirstOrDefaultAsync(m => m.ToolUserAccessId == id);
            if (tool == null)
            {
                return NotFound();
            }

            return View(tool);
        }

        // POST: Tools/Delete/5
        [HttpPost, ActionName("Revoke")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RevokeConfirmed(int id)
        {
            if (_context.ToolUserAccess == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tool'  is null.");
            }
            var tool = await _context.ToolUserAccess.FindAsync(id);
            if (tool != null)
            {
                _context.ToolUserAccess.Remove(tool);
                var currentuser = await _context.Users.FindAsync(tool.UserId);
                string str = ",T" + tool.ToolId + ",";
                currentuser.ToolAccess = currentuser.ToolAccess.Replace(str, "");
                _context.Update(currentuser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewToolAccess), new {id = tool.ToolId});
        }

        // GET: Tools/Delete/5
        public async Task<IActionResult> DeleteControlOption(int? id)
        {
            if (id == null || _context.Tool == null)
            {
                return NotFound();
            }

            var tool = await _context.ToolControlDetail.Include(a=>a.ToolControl)
                .FirstOrDefaultAsync(m => m.ToolControlDetailId == id);
            if (tool == null)
            {
                return NotFound();
            }

            return View(tool);
        }

        // POST: Tools/Delete/5
        [HttpPost, ActionName("DeleteControlOption")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteControlOptionConfirmed(int id)
        {
            if (_context.Tool == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tool'  is null.");
            }
            var tool = await _context.ToolControlDetail.FindAsync(id);
            if (tool != null)
            {
                _context.ToolControlDetail.Remove(tool);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AddControlOption), new {id = tool.ToolControlId});
        }

        public async Task<IActionResult> DeleteAccess(int? id)
        {
            if (id == null || _context.ToolUserAccess == null)
            {
                return NotFound();
            }

            var tool = await _context.ToolUserAccess
                .FirstOrDefaultAsync(m => m.ToolUserAccessId == id);
            if (tool == null)
            {
                return NotFound();
            }

            return View(tool);
        }

        // POST: Tools/Delete/5
        [HttpPost, ActionName("DeleteAccess")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccessConfirmed(int id)
        {
            if (_context.ToolUserAccess == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tool'  is null.");
            }
            var tool = await _context.ToolUserAccess.FindAsync(id);
            if (tool != null)
            {
                _context.ToolUserAccess.Remove(tool);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UserAccess), new { id = tool.ToolId, username = tool.Username, userId = tool.UserId });
        }
        private bool ToolExists(int id)
        {
            return _context.Tool.Any(e => e.ToolId == id);
        }
    }
}
