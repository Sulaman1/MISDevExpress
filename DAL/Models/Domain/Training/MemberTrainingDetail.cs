using DAL.Models.Domain.MasterSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.Training
{
    [Table("MemberTrainingDetail", Schema = "training")]
    public class MemberTrainingDetail
    {
        [Key]
        public int MemberTrainingDetailId { get; set; }
        [Display(Name = "MemberTraining")]
        [ForeignKey("Training")]
        public int MemberTrainingId { get; set; }
        [Display(Name = "Beneficiary")]
        [ForeignKey("CommunityInstituteMember")]
        public int CommunityInstituteMemberId { get; set; }      
        public DateTime CreatedOn { get; set; } = DateTime.Today.Date;

        public virtual CommunityInstituteMember? CommunityInstituteMember { get; set; }
        public virtual MemberTraining? MemberTraining { get; set; }
    }
}
