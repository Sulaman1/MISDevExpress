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
public class PermissionController : Controller
{
    private readonly IPermission _permission;

    public PermissionController(IPermission permission)
    {
        _permission = permission;
    }

    public async Task<ActionResult> Index(string roleId, string message)
    {
        ViewBag.Message = message;
        var model = new PermissionViewModel();        
        model = await _permission.GetPermission(roleId);
        return View(model);
    }

    public async Task<IActionResult> Update(PermissionViewModel model)
    {
       _permission.Update(model);
        return RedirectToAction("Index", new { roleId = model.RoleId, message = "Role permissions updated successfully" });
    }
}
}
