using DAL.Models.Domain.MasterSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.Training
{
    [Table("LIPTrainingDetail", Schema = "training")]
    public class LIPTrainingDetail
    {
        [Key]
        public int LIPTrainingDetailId { get; set; }
        [Display(Name = "MemberTraining")]
        [ForeignKey("Training")]
        public int LIPTrainingId { get; set; }
        [Display(Name = "Beneficiary")]
        [ForeignKey("Member")]
        public int MemberId { get; set; }              
        public int PSCRanking { get; set; }      
        public string LIPNumber { get; set; }      
        public DateTime CreatedOn { get; set; } = DateTime.Today.Date;

        public virtual Member? Member { get; set; }
        public virtual LIPTraining? LIPTraining { get; set; }
    }
}
