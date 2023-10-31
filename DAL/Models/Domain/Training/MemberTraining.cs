using DAL.Models.Domain.MasterSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.Training
{
    [Table("MemberTraining", Schema = "training")]
    public class MemberTraining
    {
        [Key]
        public int MemberTrainingId { get; set; }
        [Display(Name = "Training Title")]
        public string? TrainingName { get; set; }               
        public string? Description { get; set; }        
        [Display(Name = "Upload Attendance")]
        public string? AttendanceAttachment { get; set; }
        [Display(Name = "Upload Report")]
        public string? ReportAttachment { get; set; }
        [Display(Name = "Upload Sessoin Plan")]
        public string? SessionPlanAttachment { get; set; }
        public string? PictureAttachment1 { get; set; }       
        public string? PictureAttachment2 { get; set; }       
        public string? PictureAttachment3 { get; set; }       
        public string? PictureAttachment4 { get; set; }       
        [DataType(DataType.Date)]        
        [Display(Name = "Start Date")]
        public DateTime TrainingOn { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Today.Date;                  
        [ForeignKey("Employee")]
        [Display(Name = "Trainer")]
        public int EmployeeId { get; set; }                                    
        [ForeignKey("TrainingType")]
        [Display(Name = "Training Type")]
        public int TrainingTypeId { get; set; }
        public string MemberTrainingCode { get; set; } = "";
        public int TotalMember { get; set; } = 0;
        [Display(Name = "Number of Days")]
        public int TotalDays { get; set; }
        [Display(Name = "Number of Classes")]
        public int TotalClasses { get; set; } = 1;
        public bool IsSubmittedForReview { get; set; } = false;
        public DateTime? SubmittedForReviewOnDate { get; set; }
        public string? SubmittedForReviewBy { get; set; }
        public bool IsReviewed { get; set; } = false;
        public string? ReviewedBy { get; set; }
        public DateTime? ReviewedOn { get; set; }
        [Display(Name = "Is Completed")]
        public bool IsVerified { get; set; } = false;
        public string? VerifiedBy { get; set; }
        public DateTime? VerifiedOn { get; set; }
        public bool IsRejected { get; set; } = false;
        public string? RejectedComments { get; set; }
        public int DistrictId { get; set; }
        public string? District { get; set; }
        [Display(Name = "Village")]
        [ForeignKey("Village")]        
        public int VillageId { get; set; }
        public virtual TrainingType? TrainingType { get; set; }
        public virtual Employee? Employee { get; set; }        
        public virtual Village? Village { get; set; }        
    }
}
