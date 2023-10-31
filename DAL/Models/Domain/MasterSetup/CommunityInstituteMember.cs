using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("CommunityInstituteMember", Schema = "master")]
    public class CommunityInstituteMember
    {
        [Key]
        public int CommunityInstituteMemberId { get; set; }
     
        public int TotalTrainingAttempt { get; set; } = 0;               
        public DateTime OnDate { get; set; } = DateTime.Today.Date;
        [Required]
        [Display(Name = "Community Institution")]
        [ForeignKey("CommunityInstitution")]        
        public int CommunityInstitutionId { get; set; }
        [ForeignKey("Member")]
        public int MemberId { get; set; }
        public int DesignationId { get; set; }
        [Display(Name = "Code")]
        public string MISCode { get; set; }
        public virtual CommunityInstitution? CommunityInstitution { get; set; }        
        public virtual Member? Member { get; set; }
        public virtual Designation? Designation { get; set; }
    }
}
