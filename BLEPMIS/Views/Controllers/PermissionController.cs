using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Constant.Constants;
using DAL.Models;
using DefaultSeed.Helpers;

namespace BLEPMIS.Controllers
{
[Authorize(Roles = "SuperAdmin,Administrator")]
public class PermissionController : Controller
{
        private readonly RoleManager<IdentityRole> _roleManager;
        public PermissionController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

    public async Task<ActionResult> Index(string roleId, string message)
    {
        ViewBag.Message = message;
            var model = new PermissionViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();
            allPermissions.GetPermissions(typeof(Permissions.Administrator), roleId);
            /*allPermissions.GetPermissions(typeof(Permissions.SuperAdmin), roleId);   */    
            allPermissions.GetPermissions(typeof(Permissions.CICIG), roleId);
            allPermissions.GetPermissions(typeof(Permissions.CICIGTraining), roleId);
            allPermissions.GetPermissions(typeof(Permissions.LIPTraining), roleId);
            allPermissions.GetPermissions(typeof(Permissions.LIPAssetTransfer), roleId);            
            allPermissions.GetPermissions(typeof(Permissions.BSFGovt), roleId);            
            allPermissions.GetPermissions(typeof(Permissions.BSFPrivate), roleId);            
            allPermissions.GetPermissions(typeof(Permissions.GRM), roleId);            
            allPermissions.GetPermissions(typeof(Permissions.HTS), roleId);            
            allPermissions.GetPermissions(typeof(Permissions.HR), roleId);            
            allPermissions.GetPermissions(typeof(Permissions.IDO), roleId);            
            allPermissions.GetPermissions(typeof(Permissions.MonitoringTool), roleId);            
            allPermissions.GetPermissions(typeof(Permissions.TVT), roleId);            
            var role = await _roleManager.FindByIdAsync(roleId);
            model.RoleId = roleId;
            var claims = await _roleManager.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;

            return View(model);
    }

    public async Task<IActionResult> Update(PermissionViewModel model)
    {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(role, claim.Value);
            }
            return RedirectToAction("Index", new { roleId = model.RoleId, message = "Role permissions updated successfully" });
    }
}
}
