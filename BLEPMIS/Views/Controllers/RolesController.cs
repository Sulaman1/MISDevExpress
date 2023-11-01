using BAL.IRepository.MasterSetup.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BLEPMIS.Controllers
{
[Authorize(Roles ="SuperAdmin, Administrator")]
public class RolesController : Controller
{
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

   public async Task<IActionResult> Index()
    {            
        return View(await _roleManager.Roles.Where(a => a.Id != "e9cabf5e-0bd2-4f34-ad54-8af22f32c607").ToListAsync());
    }
    [HttpPost]
    public async Task<IActionResult> AddRole(string roleName)
    {
        if(roleName != null)
            await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
        return RedirectToAction("Index");
    }
}
}