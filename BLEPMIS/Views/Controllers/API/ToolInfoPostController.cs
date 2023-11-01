using BLEPMIS.Models.API;
using DAL.Models.Domain.ToolApp.ToolAppPost;
using DBContext.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLEPMIS.Controllers.API
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ToolInfoPostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToolInfoPostController(ApplicationDbContext context)
        {
            _context = context;
        }
        // POST: api/ApiEPIs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<JsonResult> PostTool1Post(CompleteToolPost completeTool)
        {
            bool status = false;
            string message = "Failed to Upload!";
            if (completeTool == null) return Json(new { status, message });           
            if (completeTool.toolInfo.ToolControlList.Count > 0)
            {                
                while(_context.ToolControlInfoPost.Count(a => a.ToolId == completeTool.toolInfo.ToolId && a.Counter == completeTool.toolInfo.Counter) > 0)
                {
                    completeTool.toolInfo.Counter++;
                }                
                var basicInfo = new ToolInfoBasicPost();
                basicInfo.Latitute = completeTool.Latitute;
                basicInfo.Longitute = completeTool.Longitute;
                basicInfo.CurrentDateTime = completeTool.CurrentDateTime;
                basicInfo.Username = completeTool.Username;
                basicInfo.ToolId = completeTool.toolInfo.ToolId;
                basicInfo.Counter = completeTool.toolInfo.Counter;
                basicInfo.UploadedDate = DateTime.Now;
                _context.ToolInfoBasicPost.Add(basicInfo);
                await _context.SaveChangesAsync();
                foreach(var toolControl in completeTool.toolInfo.ToolControlList)
                {
                    var obj = new ToolControlInfoPost();
                    obj.ControlValue = toolControl.ControlValue;
                    obj.ControlName = toolControl.ControlName;
                    obj.ToolControlId = toolControl.ToolControlId;
                    obj.ToolId = completeTool.toolInfo.ToolId;
                    obj.OrderNo = toolControl.OrderNo;
                    obj.Counter = completeTool.toolInfo.Counter;
                    obj.ControlLebel = toolControl.ControlLebel;                    
                    _context.Add(obj);
                    _context.SaveChanges();
                    var MaxId = _context.ToolControlInfoPost.Max(a => a.ToolControlInfoPostId);
                    if (toolControl.ToolDetailList != null && toolControl.ToolDetailList.Count > 0)
                    {
                        foreach (var detail in toolControl.ToolDetailList)
                        {
                            var obj2 = new ToolInfoDetailPost();
                            obj2.ToolControlId = detail.ToolControlId;
                            obj2.Label = detail.Label;
                            obj2.ControlDetailValue = detail.ControlDetailValue;                                                       
                            obj2.ToolControlInfoPostId = MaxId;                                                       
                            _context.Add(obj2);
                        }
                    }
                    //---------------------------                    
                }
                if (completeTool.toolModules != null)
                {
                    List<ToolModulePost> toolModules = new List<ToolModulePost>();
                    foreach (var detail in completeTool.toolModules.Where(a=>a.IsSelected == true))
                    {
                        var obj2 = new ToolModulePost();
                        obj2.ToolId = detail.ToolId;
                        obj2.ToolModuleName = detail.ToolModuleName;
                        obj2.DropdownMenuName = detail.DropdownMenuName;
                        obj2.Value = detail.Value;
                        obj2.Counter = completeTool.toolInfo.Counter;
                        _context.Add(obj2);
                    }
                }
                await _context.SaveChangesAsync();
                status = true;
                message = "Uploaded Successfully!";
            }

            return Json(new { status, message });
        }
    }
}
