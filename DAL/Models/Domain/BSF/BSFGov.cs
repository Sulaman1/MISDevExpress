using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL.Models.Domain.BSF
{
    [Table("BSFGov", Schema = "master")]
    public class BSFGov
    {
        [Key]
        public int BSFGovId { get; set; }        
        [Display(Name = "BSF Code")]
        public string BSFName { get; set; } = "";
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        [Display(Name = "Site Name")]
        public string SiteName { get; set; }
        [Display(Name = "District")]
        public string DistrictName { get; set; } = "";        
        [Display(Name = "Tehsil")]
        public string TehsilName { get; set; } = "";       
        [Required]
        [Display(Name = "Latitute (max 6 decimal)")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Latitute { get; set; }
        [Required]
        [Display(Name = "Longitute (max 6 decimal)")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Longitute { get; set; }
        [Required]
        [Display(Name = "Dept Contact#")]
        public string DepartmentContactNumber { get; set; }        
        [Display(Name = "Fax#")]
        public string? DepartmentFaxNumber { get; set; }        
        [Display(Name = "Website")]
        public string? DepartmentWebsite { get; set; }        
        [Display(Name = "Focal Person Name")]
        public string? FocalPersonName { get; set; }        
        [Display(Name = "Designation")]
        public string? Designation { get; set; }
        [Required]
        [Display(Name = "Package")]
        public string BSFGovtPackage { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Construction Type")]
        public string ConstructionType { get; set; }     
        [Required]
        [Display(Name = "Total Grant")]
        public decimal TotalGrant { get; set; } = 0;        
        [Display(Name = "Focal Person Cell#")]
        public string? FocalPersonCellNumber { get; set; }
        [Required]
        [Display(Name = "On Site Person")]
        public string OnSitePersonName { get; set; }
        [Display(Name = "On Site Person Cell#")]
        public string? OnSitePersonCellNumber { get; set; }        
        [Display(Name = "Scope of Work")]
        public string? WorkScopeAttachment { get; set; }               
        [Required]
        [Display(Name = "Financial Progress")]
        public decimal FinancialProgress { get; set; } = 0;
        [Required]
        [Display(Name = "Physical Progress")]
        public decimal PhysicalProgress { get; set; } = 0;
        [Display(Name = "Total Stage")]
        public int TotalComponent { get; set; } = 0;                
        public int CompletedComponent { get; set; } = 0;                
        public decimal GrantAlocated { get; set; } = 0;                
        public DateTime CreatedOn { get; set; } = DateTime.Today;
        public string UserId { get; set; }
        [Display(Name = "Construction Type")]
        public string IsNew { get; set; } = "";
        [Display(Name = "Is Completed")]
        public bool IsCompleted { get; set; } = false;
        public string CreatedBy { get; set; }      
        public string? VerifiedBy { get; set; }      
        public DateTime? VerifiedOn { get; set; }              
    }
}
