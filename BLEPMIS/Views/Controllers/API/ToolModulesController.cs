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
using NuGet.Packaging.Core;

namespace BLEPMIS.Controllers.API
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ToolModulesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ToolModulesController(ApplicationDbContext context)
        {
            _context = context;
        }       

        [HttpGet]
        public async Task<ActionResult> GetToolModule()
        {

            var modules = _context.DropdownenuAccess.Select(a=> new
            {
                toolModuleId = a.DropdownenuAccessId,
                toolId = a.ToolId,
                dropdownMenuName = a.DropdownMenuName,
                toolModuleName = a.ToolModuleName,
                value = a.Value

            }).ToList();
            return new JsonResult(modules);
        }
       
    }
}
