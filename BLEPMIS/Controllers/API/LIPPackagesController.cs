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
    public class LIPPackagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LIPPackagesController(ApplicationDbContext context)
        {
            _context = context;
        }       

        [HttpGet]
        public async Task<ActionResult> GetLIPPackages()
        {

            var packages = _context.LIPPackage.Select(a=> new
            {
                packageId = a.LIPPackageId,
                packageName = a.PackageName

            }).ToList();
            return new JsonResult(packages);
        }
       
    }
}
