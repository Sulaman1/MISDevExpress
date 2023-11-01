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
    public class DistrictsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DistrictsController(ApplicationDbContext context)
        {
            _context = context;
        }       

        [HttpGet]
        public async Task<ActionResult> GetDistricts()
        {
            var districts = await _context.District.Where(a=>a.DistrictId > 1).Select(a=> new
            {
                DistrictId = a.DistrictId,
                DistrictName = a.Name
            }).ToListAsync();
            return new JsonResult(districts);
        }
       
    }
}
