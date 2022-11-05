using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Constant.Constants;
using DAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAL.IRepository.MasterSetup.UserManagement;

namespace BLEPMIS.Controllers
{
[Authorize(Roles = "SuperAdmin,Administrator")]
public class UserRolesController : Controller
{
    private readonly IUserRole _userRole;
  
    public UserRolesController(IUserRole userRole)
    {
        _userRole = userRole;
    }

    public async Task<IActionResult> Index(string userId, string message)
    {
        ViewBag.Message = message;
        var viewModel = new List<UserRolesViewModel>();
        viewModel = await _userRole.GetAll(userId);
        var model = new ManageUserRolesViewModel()
        {
            UserId = userId,
            UserRoles = viewModel
        };
        var user = await _userRole.GetUser(userId);
        ViewBag.FullName = user.FirstName + " " + user.LastName;
        return View(model);
    }

    public async Task<IActionResult> Update(string id, ManageUserRolesViewModel model)
    {
        _userRole.Update(id, model, User);
        return RedirectToAction("Index", new { userId = id, message = "User role(s) updated successfully!" });
    }
}
}