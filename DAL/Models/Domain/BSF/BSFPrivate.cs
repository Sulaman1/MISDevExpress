using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DAL.Models.Domain.MasterSetup;

namespace DAL.Models.Domain.BSF
{
    [Table("BSFPrivate", Schema = "master")]
    public class BSFPrivate
    {
        [Key]
        public int BSFPrivateId { get; set; }
        [Required]
        [Display(Name = "Business Idea")]
        public int GeneralBusinessIdeaId { get; set; }
        [Required]
        [Display(Name = "Beneficiary Name")]
        public string? BeneficiaryName { get; set; }
        public string? Education { get; set; }
        [Display(Name = "Business Experience In Years")]
        public string? ExperienceInYear  { get; set; }
        [Display(Name = "Business Field Experience")]
        public string? BusinessFieldExperience { get; set; }      
        [Required]
        [Display(Name = "District Name")]
        public string? DistrictName { get; set; }
        [Display(Name = "Proposed Business District")]
        public string? ProposedBusinessDistrict{ get; set; }
        [Required]
        [Display(Name = "CNIC")]
        public string? CNIC { get; set; }
        public int Age { get; set; }
        [Display(Name = "Structure of Proposed Business")]
        public string? StructureofProposedBusiness { get; set; }
        [Display(Name = "Is Completed")]
        public bool IsCompleted { get; set; } = false;
        public bool IsRefuge { get; set; }
        [Required]
        [Display(Name = "Latitude (max 6 decimal)")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Latitute { get; set; }
        [Required]
        [Display(Name = "Longitute (max 6 decimal)")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Longitute { get; set; }
        [Required]        
        public string? Gender { get; set; }
        [Required]
        public string? Address { get; set; }
        [Display(Name = "Proposed Business Name")]
        public string? ProposedBusinessName { get; set; }
        [Display(Name = "Personal NTN")]
        public string? NTN { get; set; }
        //[Required]        
        //public string? Email { get; set; }        
        [Required]
        [Display(Name = "Cell Number")]
        public string? CellNumber { get; set; }
        [Required]
        [Display(Name = "Total Grant")]
        public decimal TotalGrant { get; set; } = 0;               
        //[Display(Name = "Business Plan")]
        //public string? BusinessPlanAttachment { get; set; }        
        //[Display(Name = "Fisibility Report")]
        //public string? FisibilityReportAttachment { get; set; }
        //[Display(Name = "Contract Award")]
        //public string? ContractAwardAttachment { get; set; }
        //[Display(Name = "CNIC")]
        //public string? CNICAttachment { get; set; }
        [Display(Name = "NTN")]
        public string? NTNAttachment { get; set; }
        [Display(Name = "Existing Business Name")]
        public string? ExistingBusinessName { get; set; }
        [Display(Name = "Registration With GOP (Law)")]
        public string? RegistrationWithGOP { get; set; }
        [Display(Name = "Date of Registration")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime DateofRegistration { get; set; }
        [Display(Name = "Nature of Existing Business")]
        public string? NatureofExistingBusiness { get; set; }
        [Display(Name = "Detail of Employment Job for Proposed Business")]
        public string? DetailofEmploymentJobforProposedBusiness { get; set; }
        //[Display(Name = "Bank Name")]
        //public string? BankName { get; set; }
        [Display(Name = "Total Applicant in Rupees (30%)")]
        public string TotalApplicantInRupees { get; set; }
        [Display(Name = "Total Grant to change Total Amount Requested from BLEP in Rupees (70%)")]
        public string? TotalGrantInRupees{ get; set; }
        [Display(Name = "Current Registration Status")]
        public string? CurrentRegistrationStatus { get; set; }
        public string? BusinessNature { get; set; }
        //[Display(Name = "Account Title")]
        //public string? AccountTitle { get; set; }
        //[Display(Name = "Bank Address")]
        //public string? BankAddress { get; set; }
        //[Display(Name = "Organization NTN")]
        //public string? OrganizationNTN { get; set; }
        [Display(Name = "Business Sector")]
        public string? BusinessSector { get; set; }
        [Display(Name = "Site Supervisor Name")]
        public string? SitesupervisorName { get; set; }
        [Display(Name = "Site Supervisor Contact#")]
        public string? SitesupervisorContactNumber { get; set; }
        [Display(Name = "Number of Job Beneficiaries")]
        public string? NumberofJobBeneficiaries { get; set; }
        public string? OtherAttachment { get; set; }
        [Display(Name = "Created On")]
        public DateTime? CreatedOn { get; set; } = DateTime.Today;
        [Display(Name = "User Id")]
        public string? UserId { get; set; }
        [Display(Name = "Created By")]
        public string? CreatedBy { get; set; }
        [Display(Name = "Verified By")]
        public string? VerifiedBy { get; set; }
        [Display(Name = "Verified On")]
        public DateTime? VerifiedOn { get; set; }
        public virtual GeneralBusinessIdea? GeneralBusinessIdea { get; set; }
    }
}