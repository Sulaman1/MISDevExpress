using DAL.Models.Domain.MasterSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.Training
{
    [Table("LIPTraining", Schema = "training")]
    public class LIPTraining
    {
        [Key]
        public int LIPTrainingId { get; set; }
        [Display(Name = "Training Title")]
        public string? TrainingName { get; set; }
        [Display(Name = "Training Form Number")]
        public string? TrainingFormNumber { get; set; }               
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
        [Display(Name = "Total Member")]
        public int TotalMember { get; set; } = 0;
        [Display(Name = "Total Male")]
        public int TotalMale { get; set; } = 0;
        [Display(Name = "Total Female")]
        public int TotalFemale { get; set; } = 0;
        [Display(Name = "Number of Days")]
        public int TotalDays { get; set; }
        public bool IsSubmitted { get; set; } = false;
        public bool IsVerified { get; set; } = false;
        public string? VerifiedBy { get; set; }
        public DateTime? VerifiedOn { get; set; }
        public int DistrictId { get; set; }       
        public int VillageId { get; set; }
        public virtual TrainingType? TrainingType { get; set; }
        public virtual Employee? Employee { get; set; }           
        public virtual Village? Village { get; set; }        
    }
}
