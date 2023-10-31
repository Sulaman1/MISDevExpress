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
    public class BeneficiariesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BeneficiariesController(ApplicationDbContext context)
        {
            _context = context;
        }       

        [HttpGet]
        public async Task<ActionResult> GetBeneficiaries()
        {
            var beneficiaries = await _context.LIPAssetTransfer.Include(a=>a.Member).Where(a=>a.IsAssetTransfer == true && a.Member.BeneficiaryTypeId == 1).Select(a=> new
            {
                LIPAssetTransferId = a.LIPAssetTransferId,
                LIPName = a.LIPCode + " " + a.Member.MemberName,                
                LIPPackageId = a.LIPPackageId,
                DistrictId = a.DistrictId
            }).ToListAsync();
            return new JsonResult(beneficiaries);
        }       
    }
}
