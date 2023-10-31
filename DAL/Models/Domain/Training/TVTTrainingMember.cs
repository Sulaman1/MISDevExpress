using DAL.Models.Domain.MasterSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.Training
{
    [Table("TVTTrainingMember", Schema = "training")]
    public class TVTTrainingMember
    {
        [Key]
        public int TVTTrainingMemberId { get; set; }
        [Display(Name = "MemberTraining")]
        [ForeignKey("TVTTraining")]
        public int TVTTrainingId { get; set; }
        [Display(Name = "Beneficiary")]
        [ForeignKey("Member")]
        public int MemberId { get; set; }
        [Display(Name = "MIS Code")]
        public string? BeneficiaryMISCode { get; set; }      
        public int Age { get; set; }
        [Display(Name = "Education Doc")]
        public string? EducationDocAttachment { get; set; }
        [Display(Name = "Certificate")]
        public string? CertificateAttachment { get; set; }
        [Display(Name = "CNIC")]
        public string? CNICAttachment { get; set; }
        [Display(Name = "Admission Form")]
        public string? AdmissionFormAttachment { get; set; }      
        public string PWD { get; set; }
        [Display(Name = "Self Employed")]
        public string SelfEmployed { get; set; }
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; } = "";  
        public string RPL { get; set; }
        [Display(Name = "Preferred Skill 1")]
        public string PreferredSkill1 { get; set; }
        [Display(Name = "Preferred Skill 2")]
        public string PreferredSkill2 { get; set; }
        [Display(Name = "Preferred Skill 3")]
        public string PreferredSkill3 { get; set; }
        [Display(Name = "Preferred Skill 4")]
        public string PreferredSkill4 { get; set; }
        [Display(Name = "Identified By")]
        public string IdentifiedBy { get; set; }
        [Display(Name = "Designation")]
        public string Designation { get; set; }
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; } = DateTime.Today.Date;
        public virtual Member? Member { get; set; }
        public virtual TVTTraining? TVTTraining { get; set; }
    }
}
