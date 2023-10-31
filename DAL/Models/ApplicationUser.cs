using Microsoft.AspNetCore.Identity;
using DAL.Models.Domain.MasterSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int DistrictId { get; set; }
        public string? UserPassword { get; set; }
        public string ToolAccess { get; set; } = "";
        public int UsernameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture { get; set; }     
        public virtual District? District { get; set; }
    }
}
