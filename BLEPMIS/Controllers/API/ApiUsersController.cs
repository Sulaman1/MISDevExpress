
using DAL.Models;
using DBContext.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BLEPMIS.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BHCIP.Areas.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApiUsersController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }

        //GET: api/ApiUsers
       [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiUser>>> GetApiUser()
        {
            return NotFound();
        }

        // GET: api/ApiUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiUser>> GetApiUser(string id)
        {
            ApiUser apiUser = new ApiUser
            {
                Username = "kashan@gmail.com",
                Password = "123"
            };

            if (apiUser == null)
            {
                return NotFound();
            }

            return apiUser;
        }

        // POST: api/ApiUsers
        [HttpPost]
        public async Task<JsonResult> PostApiUser(ApiUser apiUser)
        {
            bool status = false;
            string message = "Credential not matched";
            string role = "No role";
            string toolaccess = "";
            var user = await _signInManager.UserManager.FindByNameAsync(apiUser.Username);
            if (user == null)
            {
                return Json(new {  status, message, role, toolaccess });
            }
            

            var result = await _signInManager.PasswordSignInAsync(user, apiUser.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                status = true;                                
                toolaccess = user.ToolAccess;
            }
            message = "Credential matched";
            return Json(new { status, message, role, toolaccess, name = user.UserName});

            //string passwordhash=
            //var user = _context.Users.Where(a => a.Email == apiUser.Email && a.PhoneNumber==apiUser.Password).FirstOrDefaultAsync();
        }

        //[HttpGet]
        //public async Task<string> GetCurrentUserRegion()
        //{
        //    ApplicationUser usr = await GetCurrentUserAsync();
        //    //if (usr.RegionalAccess != null)
        //    return (usr.RegionalAccess);
        //    //else
        //    //    return ("a");
        //}
        //[HttpGet]
        //private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}