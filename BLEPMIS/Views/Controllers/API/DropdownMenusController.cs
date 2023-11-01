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
    public class DropdownMenusController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DropdownMenusController(ApplicationDbContext context)
        {
            _context = context;
        }       

        [HttpGet]
        public async Task<ActionResult> GetDropdownMenu()
        {

            var menu = _context.DropdownMenu.ToList();
            return new JsonResult(menu);
        }
       
    }
}
