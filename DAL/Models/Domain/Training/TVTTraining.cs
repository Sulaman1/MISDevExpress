using DAL.Models.Domain.MasterSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.Training
{
    [Table("TVTTraining", Schema = "training")]
    public class TVTTraining
    {
        [Key]
        public int TVTTrainingId { get; set; }
        [Display(Name = "Training Title")]
        public string? TrainingTitle { get; set; }               
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
        public DateTime StartDate { get; set; } = DateTime.Today.Date;
        public double Latitute { get; set; }
        public double Longitute { get; set; }
        public string Gender { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Today.Date;
        [ForeignKey("TrainingType")]
        [Display(Name = "Training Type")]
        public int TrainingTypeId { get; set; }
        public string TrainingCode { get; set; } = "";
        public int TotalMember { get; set; } = 0;
        [Display(Name = "Number of Days")]
        public int TotalDays { get; set; }
        [Display(Name = "Is Completed")]
        public bool IsCompleted { get; set; } = false;                
        public int TotalMale { get; set; }
        public int TotalFemale { get; set; }        
        public int DistrictId { get; set; }
        public string TrainingFormNo { get; set; }
        public string TrainingCenter{ get; set; }
        public string? District { get; set; }
        [Display(Name = "Village")]
        [ForeignKey("Village")]        
        public int VillageId { get; set; }
        public string TVTTrainer { get; set; }
        public virtual TrainingType? TrainingType { get; set; }              
        public virtual Village? Village { get; set; }        
    }
}
