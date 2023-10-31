using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DAL.Models.Domain.MasterSetup;

namespace DAL.Models.Domain.HTSModule
{
    [Table("HTS", Schema = "master")]
    public class HTS
    {
        [Key]
        public int HTSId { get; set; }
        [Display(Name = "Khasra No/Inteqal No")]
        public string Khasra { get; set; }
        public string? HTSCode { get; set; }
        [Display(Name = "Tunnel Size")]
        public string TunnelSize { get; set; }
        [Required]
        [Display(Name = "Latitude (max 6 decimal)")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Latitute { get; set; }
        [Required]
        [Display(Name = "Longitute (max 6 decimal)")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Longitute { get; set; }
        [Required]
        [Display(Name = "Total Grant")]
        public decimal TotalGrant { get; set; } = 0;               
        
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
        public bool IsCompleted { get; set; } = false;
        [Display(Name = "Is Submit Deposit of 25%")]
        public bool IsSubmitDeposit { get; set; } = true;        
        public int VillageId { get; set; }        
        public int MemberId { get; set; }
        [Display(Name = "CNIC")]
        public string? CNICAttachment { get; set; }
        [Display(Name = "Agriculture Land Proof")]
        public string? AgricultureLandProofAttachment { get; set; }
        [Display(Name = "Application Form")]
        public string? ApplicationFormAttachment { get; set; }
        [Display(Name = "Tunnel Site Suitability Form")]
        public string? TunnelSiteSuitabilityFormAttachment { get; set; }
        public string? District { get; set; }
        public virtual Member? Member { get; set; }
        public virtual Village? Village { get; set; }
    }
}