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
    private readonly IRole _context;

    public RolesController(IRole context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var roles = await _context.GetAll();
        return View(roles);
    }
    [HttpPost]
    public async Task<IActionResult> AddRole(string roleName)
    {
        if(roleName != null)
            _context.AddRole(roleName);
        return RedirectToAction("Index");
    }
}
}