using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("CommunityInstitution", Schema = "master")]
    public class CommunityInstitution
    {
        [Key]        
        public int CommunityInstitutionId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }         
        [Display(Name = "Code")]
        public string? CICode { get; set; }
        [Display(Name = "Total Household")]
        public int TotalHousehold { get; set; }
        [Display(Name = "No of Community Members")]
        //[Range(14, 26, ErrorMessage = "Please enter no between 15-25")]
        public int HHParticipated { get; set; }
        [Display(Name = "Venue")]
        public string Venue { get; set; }
        [Display(Name = "BLEP Representative")]
        public string OfficerName { get; set; }
        [Display(Name = "CI Selection Form")]
        public string? SeletionFormAttachment { get; set; }
        [Display(Name = "Village Profile")]
        public string? VillageProfileAttachment { get; set; }
        [Display(Name = "Term of Partnership")]
        public string? TOPAttachment { get; set; }
        [Display(Name = "Account#")]
        public string? CIAccount { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Date of Enrollment")]
        public DateTime OnDate { get; set; }
        [Display(Name = "Latitude (max 6 decimal)")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Latitude { get; set; }
        [Display(Name = "Longitute (max 6 decimal)")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Longitute { get; set; }
        [Display(Name = "Gender")]
        public string? Gender { get; set; }
        [Display(Name = "Union Council")]
        [ForeignKey("UnionCouncil")]
        public int UnionCouncilId { get; set; }
        [Display(Name = "Village")]
        [ForeignKey("Village")]
        public int VillageId { get; set; }
        public int DistrictId { get; set; }
        public string? District { get; set; }
        [ForeignKey("CommunityType")]
        public int CommunityTypeId { get; set; }
        public bool IsSubmittedForReview { get; set; } = false;
        public DateTime? SubmittedForReviewOnDate { get; set; }
        public string? SubmittedForReviewBy { get; set; }
        public bool IsReviewed { get; set; } = false;        
        public string? ReviewedBy { get; set; }
        public DateTime? ReviewedOn { get; set; }      
        public bool IsVerified { get; set; } = false;
        public string? VerifiedBy { get; set; }        
        public DateTime? VerifiedOn { get; set; }
        public bool IsRejected { get; set; } = false;
        public string? RejectedComments { get; set; }
        public virtual UnionCouncil? UnionCouncil { get; set; }
        public virtual Village? Village { get; set; }
        public virtual CommunityType? CommunityType { get; set; }
    }
}
