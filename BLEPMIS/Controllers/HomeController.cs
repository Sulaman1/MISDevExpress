using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DBContext.Data;
using DAL.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BLEPMIS.Controllers
{
    //[AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);            
            return View();
        }
        public ActionResult IntermediateResultFramework()
        {
            ViewBag.CIEstablished = _context.CommunityInstitution.Count(a=>a.IsVerified);
            return View();
        }
        public IActionResult Index2()
        {           
            return View();
        }
        public IActionResult Graph(string year)
        {           
            return PartialView();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}