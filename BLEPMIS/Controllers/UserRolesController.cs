using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Constant.Constants;
using DAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAL.IRepository.MasterSetup.UserManagement;
using DefaultSeed.Seeds;

namespace BLEPMIS.Controllers
{
[Authorize(Roles = "SuperAdmin,Administrator")]
public class UserRolesController : Controller
{
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRolesController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string userId, string message)
    {
        ViewBag.Message = message;
        var viewModel = new List<UserRolesViewModel>();
        foreach (var role in _roleManager.Roles.Where(a => a.Id != "e9cabf5e-0bd2-4f34-ad54-8af22f32c607").ToList())
        {
            var userRolesViewModel = new UserRolesViewModel
            {
                RoleName = role.Name
            };
            if (await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(userId), role.Name))
            {
                userRolesViewModel.Selected = true;
            }
            else
            {
                userRolesViewModel.Selected = false;
            }
            viewModel.Add(userRolesViewModel);
        }
        var model = new ManageUserRolesViewModel()
        {
            UserId = userId,
            UserRoles = viewModel
        };
        var user = await _userManager.FindByIdAsync(userId);
        ViewBag.FullName = user.FirstName + " " + user.LastName;
        return View(model);
    }

    public async Task<IActionResult> Update(string id, ManageUserRolesViewModel model)
    {        
        var user = await _userManager.FindByIdAsync(id);
        var roles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, roles);
        result = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
        var currentUser = await _userManager.GetUserAsync(User);
        await _signInManager.RefreshSignInAsync(currentUser);
        await DefaultUsers.SeedSuperAdminAsync(_userManager, _roleManager);
        return RedirectToAction("Index", new { userId = id, message = "User role(s) updated successfully!" });
    }
}
}