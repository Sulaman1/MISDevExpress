using DAL.Models.Domain.MasterSetup;
using DBContext.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BLEPMIS.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberInfosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public MemberInfosController(ApplicationDbContext context)
        {
            _context = context;
        }
        // POST api/<MemberInfosController>
        [HttpPost]
        public JsonResult MemberPost(RequestModel obj)
        {
            var member = _context.Member.Where(a=>a.CNIC == obj.cnic).FirstOrDefault();
            if (member == null)
            {
                return new JsonResult(new { status = false, message = "Not Exist" });
            }
            return new JsonResult(new { status = true, message = "Exist" });
        }
  
    }
}
