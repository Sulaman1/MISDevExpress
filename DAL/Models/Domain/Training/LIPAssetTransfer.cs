using DAL.Models.Domain.MasterSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.Training
{
    [Table("LIPAssetTransfer", Schema = "training")]
    public class LIPAssetTransfer
    {
        [Key]
        public int LIPAssetTransferId { get; set; }             
        public string? LIPCode { get; set; }    
        public int PSC { get; set; }         
        [Display(Name = "Scanned CNIC")]
        public string? CNICAttachment { get; set; }
        [Display(Name = "LIP Form")]
        public string? LIPFormAttachment { get; set; }
        [Display(Name = "GRN Form")]
        public string? GRNAttachment { get; set; }
        [Display(Name = "Handing Taking Picture")]
        public string? PictureAttachment1 { get; set; }       
        public string? PictureAttachment2 { get; set; }       
        public string? PictureAttachment3 { get; set; }
        [Display(Name = "Internal Review")]
        public string? PictureAttachment4 { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CreatedOn { get; set; } = DateTime.Today.Date;                  
                                                               
        [Display(Name = "Latitude (max 6 decimal)")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Latitute { get; set; }
        [Display(Name = "Longitute (max 6 decimal)")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Longitute { get; set; }
        public bool IsSubmitted { get; set; } = false;
        public string? SubmittedBy { get; set; }
        public DateTime? SubmittedOnDate { get; set; }        
        public bool IsReviewed { get; set; } = false;
        public string? ReviewedBy { get; set; }
        public DateTime? ReviewedOn { get; set; }
        public bool IsAssetTransfer { get; set; } = false;
        public string? AssetTransferBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? AssetTransferOn { get; set; }
        public bool IsRejected { get; set; } = false;
        public bool IsInternalReviewed { get; set; } = false;
        public string? InternalRemarks { get; set; } = "";
        public string? RejectedComments { get; set; }
        public int DistrictId { get; set; }
        public string? District { get; set; }
        public int MemberId { get; set; }
        [Display(Name = "LIP Package")]
        public int LIPPackageId { get; set; }      
        public string? UserId { get; set; }
        public int VillageId { get; set; }
        [Display(Name = "Identified By")]
        public int IdentifiedById { get; set; }
        [Display(Name = "Collected By")]
        public string? DataCollectedBy { get; set; }
        [Display(Name = "Designation")]
        public string? DataCollectorDesignation { get; set; }                            
        public virtual Village? Village { get; set; }        
        public virtual LIPPackage? LIPPackage { get; set; }        
        public virtual Member? Member { get; set; }        
        public virtual IdentifiedBy? IdentifiedBy { get; set; }        
    }
}
