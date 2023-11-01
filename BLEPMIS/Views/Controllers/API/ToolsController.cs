using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.ToolApp;
using DBContext.Data;
using Microsoft.AspNetCore.Authorization;
using BLEPMIS.Models.API;

namespace BLEPMIS.Controllers.API
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ToolsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetTools()
        {
            /*var tools = (await _context.ToolControl.Include(a => a.Tool).Include(a=>a.Control).ToListAsync()).Select(x => new
            {
                ToolId = x.ToolId,
                ToolName = x.Tool.Name,
                Description = x.Tool.Description,
                ControlName = x.Name,
                ToolControlId = x.ControlId,
                OrderNo = x.OrderNo,
                IsActive = x.IsActive
            }).Where(a=>a.IsActive == true).OrderBy(a=>a.ToolId).OrderBy(a=>a.OrderNo).ToList();*/
            List<ToolInfo> toolInfos = new List<ToolInfo>();
            toolInfos = _context.Tool.Where(a => a.IsActive == true).Select(a=> new ToolInfo { ToolId = a.ToolId, ToolName = a.Name, Description = a.Description, IsActive = a.IsActive}).ToList();            
            foreach (var toolInfo in toolInfos)
            {                
                toolInfo.ToolControlList = _context.ToolControl.Include(a=>a.Control).Where(a => a.ToolId == toolInfo.ToolId).Select(a=> new ToolControlInfo { ControlLebel = a.Name, ToolId=a.ToolId, ToolControlId = a.ToolControlId, ControlValue = a.Value, ControlName = a.Control.Name, OrderNo = a.OrderNo}).ToList();
                foreach (var control in toolInfo.ToolControlList)
                {
                    List<ToolInfoDetail> tooldetail = new List<ToolInfoDetail>();
                    control.ToolDetailList = _context.ToolControlDetail.Where(a => a.ToolControlId == control.ToolControlId).Select(a => new ToolInfoDetail { ToolControlDetailId = a.ToolControlDetailId, ToolControlId = a.ToolControlId, Label = a.Label, ControlDetailValue = a.Value }).ToList();
                }
            }
            BLEPTool obj = new BLEPTool();
            obj.ToolList = toolInfos;
            

            return new JsonResult(obj);
        }       

        // GET: api/Tools/5
      ///*  [HttpGet("{id}")]
      //  public async Task<ActionResult<Tool>> GetTool(int id)
      //  {
      //      var tool = await _context.Tool.FindAsync(id);

      //      if (tool == null)
      //      {
      //          return NotFound();
      //      }

      //      return tool;
      //  }*/

        // PUT: api/Tools/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTool(int id, Tool tool)
        {
            if (id != tool.ToolId)
            {
                return BadRequest();
            }

            _context.Entry(tool).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToolExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tools
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tool>> PostTool(Tool tool)
        {
            _context.Tool.Add(tool);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTool", new { id = tool.ToolId }, tool);
        }

        // DELETE: api/Tools/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTool(int id)
        {
            var tool = await _context.Tool.FindAsync(id);
            if (tool == null)
            {
                return NotFound();
            }

            _context.Tool.Remove(tool);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToolExists(int id)
        {
            return _context.Tool.Any(e => e.ToolId == id);
        }
    }
}
