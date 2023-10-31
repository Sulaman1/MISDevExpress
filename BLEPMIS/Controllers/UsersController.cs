using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using DBContext.Data;
using BAL.IRepository.MasterSetup.UserManagement;

namespace BLEPMIS.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class UsersController : Controller
{
    private readonly IUser _context;    
    public UsersController(IUser context)
    {
            _context = context;         
    }
    public async Task<IActionResult> Index(string message)
    {
        ViewBag.Message = message;
        var allUsersExceptCurrentUser = await _context.GetAll(HttpContext.User);        
        return View(allUsersExceptCurrentUser);
    }
}
}