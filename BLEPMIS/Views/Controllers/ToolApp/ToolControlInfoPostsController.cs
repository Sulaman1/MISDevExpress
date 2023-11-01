using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.ToolApp.ToolAppPost;
using DBContext.Data;
using DAL.Models.Domain.ToolApp;

namespace BLEPMIS.Controllers.ToolApp
{
    public class ToolControlInfoPostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToolControlInfoPostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ToolControlInfoPosts
        public async Task<IActionResult> Index(int id)
        {
              return View(await _context.ToolInfoBasicPost.Include(a=>a.Tool).Where(a=>a.ToolId == id).ToListAsync());
        }
        public async Task<IActionResult> ToolList()
        {
            return View(await _context.Tool.Where(a=>a.IsActive == true).ToListAsync());
        }
        public async Task<IActionResult> ToolAnalysis(int id)
        {
            var applicationDbContext = await _context.SPToolAnalysis.FromSqlRaw("exec SPToolAnalysis {0} ", id).ToListAsync();
            return View(applicationDbContext);
        }
        public async Task<IActionResult> ToolListInActive()
        {
            return View(await _context.Tool.Where(a => a.IsActive == false).ToListAsync());
        }
        // GET: ToolControlInfoPosts/Details/5
        public async Task<IActionResult> Details(int id, int CId)
        {
                   
            var toolControl = await _context.ToolControlInfoPost.Include(a => a.Tool)
                .Where(m => m.ToolId == id && m.Counter == CId).OrderBy(a => a.OrderNo).ToListAsync();
            var tool = _context.Tool.Find(id);
            tool.ToolControl = new List<ToolControl>();
            int counter = 0;            
            foreach (var control in toolControl)
            {
                //tool.ToolControl.ElementAt(counter).ToolControlDetail = _context.ToolControlDetail.Where(a => a.ToolControlId == control.ToolControlId).ToList();
                ToolControl obj = new ToolControl();
                obj.ToolId = control.ToolId;
                obj.OrderNo = control.OrderNo;
                obj.Value = control.ControlValue;
                obj.ToolControlId = control.ToolControlId;
                obj.Name = control.ControlLebel;                
                obj.Control = _context.ToolControl.Include(a=>a.Control).Where(a=>a.ToolControlId == obj.ToolControlId).Select(a=> new Control { ControlId = a.ControlId, Name = a.Control.Name}).FirstOrDefault();
                if(obj.Control is not null)
                {
                    if (obj.Control.Name.Equals("Picture"))
                    {
                        if (obj.Value != null)
                        {
                            //var Info = obj.Value.Substring(obj.Value.IndexOf("latitude:"), obj.Value.Length - obj.Value.IndexOf("latitude:"));
                            //obj.Value = obj.Value.Remove(obj.Value.IndexOf("latitude:"), obj.Value.Length - obj.Value.IndexOf("latitude:"));
                            var info = _context.ToolInfoBasicPost.Where(a => a.ToolId == id && a.Counter == CId).FirstOrDefault();
                            ViewBag.Info = "Latitute:" + info.Latitute.ToString() + ", Longitute:" + info.Longitute.ToString() + ", DateTimeStamp:" + info.CurrentDateTime;
                            ViewBag.Latitute = info.Latitute;
                            ViewBag.Longitute = info.Longitute;                            
                        }
                    }
                    tool.ToolControl.Add(obj);
                    var details = _context.ToolInfoDetailPost.Where(a => a.ToolControlInfoPostId == control.ToolControlInfoPostId).ToList();
                    if (details != null)
                    {
                        tool.ToolControl.ElementAt(counter).ToolControlDetail = new List<ToolControlDetail>();
                        foreach (var detail in details)
                        {
                            ToolControlDetail obj2 = new ToolControlDetail();
                            obj2.Value = detail.ControlDetailValue;
                            obj2.Label = detail.Label;
                            tool.ToolControl.ElementAt(counter).ToolControlDetail.Add(obj2);
                        }
                    }
                    counter++;
                }
               
            }
            ViewBag.MDependency = _context.ToolModulePost.Where(a => a.ToolId == id && a.Counter == CId).ToList();
            if (tool == null)
            {
                return NotFound();
            }
            return View(tool);
        }

        // GET: ToolControlInfoPosts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToolControlInfoPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ToolControlInfoPostId,ControlName,ToolControlId,ToolId,OrderNo,ControlValue,ControlLebel")] ToolControlInfoPost toolControlInfoPost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toolControlInfoPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toolControlInfoPost);
        }

        // GET: ToolControlInfoPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ToolControlInfoPost == null)
            {
                return NotFound();
            }

            var toolControlInfoPost = await _context.ToolControlInfoPost.FindAsync(id);
            if (toolControlInfoPost == null)
            {
                return NotFound();
            }
            return View(toolControlInfoPost);
        }

        // POST: ToolControlInfoPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ToolControlInfoPostId,ControlName,ToolControlId,ToolId,OrderNo,ControlValue,ControlLebel")] ToolControlInfoPost toolControlInfoPost)
        {
            if (id != toolControlInfoPost.ToolControlInfoPostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toolControlInfoPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToolControlInfoPostExists(toolControlInfoPost.ToolControlInfoPostId))
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
            return View(toolControlInfoPost);
        }

        // GET: ToolControlInfoPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ToolControlInfoPost == null)
            {
                return NotFound();
            }

            var toolControlInfoPost = await _context.ToolControlInfoPost
                .FirstOrDefaultAsync(m => m.ToolControlInfoPostId == id);
            if (toolControlInfoPost == null)
            {
                return NotFound();
            }

            return View(toolControlInfoPost);
        }

        // POST: ToolControlInfoPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ToolControlInfoPost == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ToolControlInfoPost'  is null.");
            }
            var toolControlInfoPost = await _context.ToolControlInfoPost.FindAsync(id);
            if (toolControlInfoPost != null)
            {
                _context.ToolControlInfoPost.Remove(toolControlInfoPost);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToolControlInfoPostExists(int id)
        {
          return _context.ToolControlInfoPost.Any(e => e.ToolControlInfoPostId == id);
        }
    }
}
