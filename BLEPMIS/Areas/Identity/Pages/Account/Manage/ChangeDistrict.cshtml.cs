using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using DBContext.Data;

namespace BLEPMIS.Areas.Identity.Pages.Account.Manage
{
    public partial class ChangeDistrictModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public ChangeDistrictModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }                        
        [TempData]
        public string StatusMessage { get; set; }         

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "District")]
            public int DistrictId { get; set; }            
        }       

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID.");
            }
            ViewData["UserId"] = user.Id;
            ViewData["DistrictId"] = new SelectList(_context.District, "DistrictId", "Name", user.DistrictId);            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID");
            }

            if (!ModelState.IsValid)
            {                
                return Page();
            }
            var districtId = user.DistrictId;            
            if (Input.DistrictId != districtId)
            {
                user.DistrictId = Input.DistrictId;
                await _userManager.UpdateAsync(user);
            }                                              
            StatusMessage = "User district has been updated";
            return RedirectToPage("ChangeDistrict",new { id = UserId });
        }
    }
}
