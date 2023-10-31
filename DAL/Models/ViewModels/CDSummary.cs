using DAL.Models.Domain.MasterSetup;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models.ViewModels
{
    public class CDSummary
    {
        [Key]
        public int CommunityInstitutionId { get; set; }
        [Display(Name = "District")]
        public string Name { get; set; }
        [Display(Name = "Tehsil")]
        public string TehsilName { get; set; }
        [Display(Name = "Union Council")]
        public string UnionCouncilName { get; set; }
        [Display(Name = "CI/CIG Form")]
        public int CDForm { get; set; }    
        public int DistrictId { get; set; }    
        public int UnionCouncilId { get; set; }    
        public int CommunityTypeId { get; set; }
        [Display(Name = "Submitted For Review")]
        public int SubmittedForReview { get; set; }
        [Display(Name = "Submitted For Approval")]
        public int SubmittedForApproval { get; set; }    
        public int Approved { get; set; }  
        public virtual UnionCouncil UnionCouncil  { get; set; }  
        public virtual CommunityType CommunityType { get; set; }  

    }
}
