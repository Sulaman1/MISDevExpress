using DAL.Models.Domain.MasterSetup;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("MemberAfghanDetail", Schema = "master")]
    public class MemberAfghanDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Member")]
        public int MemberId { get; set; }
        //[Required]

        public short Age { get; set; }
        public string Education { get; set; }
        [DisplayName("Family Number")] public string? FamilyNumber { get; set; }
        public string Muhalla { get; set; }
        [DisplayName("Residing Since")] public string ResidingSince { get; set; }
        public string Disability { get; set; }        
        public bool? IsAnyDisability { get; set; }
        [DisplayName("Livelihood Source")] public string LivelihoodSource { get; set; }
        [DisplayName("Monthly Income")] public int MonthlyIncome { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Tehsil { get; set; }
        public string? UC { get; set; }
        public string? Village { get; set; }
        [DisplayName("Muhalla  ")] public string AfghanMuhalla { get; set; }
        [DisplayName("Reason Not Going back")] public string ReasonNotGoingback { get; set; }
        [DisplayName("Household Head Name")] public string HHName { get; set; }
        [DisplayName("Head Gender")] public string HHGender { get; set; }
        [DisplayName("Head Age")] public short HHAge { get; set; }
        [DisplayName("Is Head Female")] public bool IsHHFemale { get; set; }
        [DisplayName("0-4")] public short FamilyPersonM04 { get; set; }
        public short FamilyPersonF04 { get; set; }
        public short FamilyPersonM511 { get; set; }
        public short FamilyPersonF511 { get; set; }
        public short FamilyPersonM1217 { get; set; }
        public short FamilyPersonF1217 { get; set; }
        public short FamilyPersonM1859 { get; set; }
        public short FamilyPersonF1859 { get; set; }
        public short FamilyPersonM60 { get; set; }
        public short FamilyPersonF60 { get; set; }
        public short FamilyPersonSpecialPersonM { get; set; }
        public short FamilyPersonSpecialPersonF { get; set; }
        public bool FamilyPersonAgri { get; set; }
        public bool FamilyPersonLiveStock { get; set; }
        public bool FamilyPersonJob { get; set; }
        public bool FamilyPersonSkilled { get; set; }
        public bool FamilyPersonUnskilled { get; set; }
        public bool FamilyPersonBusiness { get; set; }
        [DisplayName("House Status")] public string HouseStatus { get; set; }
        [DisplayName("Income Source")] public string MeanIncomeSource { get; set; }
        [DisplayName("Income Source Other")] public string MeanIncomeSourceOther { get; set; }
        [DisplayName("Members Involved in Income")] public short MeanIncomeMembersInvolve { get; set; }
        [DisplayName("Monthly Income  ")] public int MeanIncomeMonthly { get; set; }
        [DisplayName("Monthly Expenditure  ")] public int MeanIncomeExpenditure { get; set; }
        [DisplayName("Previous Experience")] public bool PreviousExperience { get; set; }
        [DisplayName("Assistance Type")] public string? AssistanceType { get; set; }
        [DisplayName("Assistance Other")] public string? AssistanceOther { get; set; }
        [DisplayName("Training Insterest")] public string? TrainingInsterest { get; set; }
        [DisplayName("Training Other")] public string? TrainingOther { get; set; }
        [DisplayName("Socio Economic Status")] public string? SocioEconomicStatus { get; set; }
        public virtual Member Member { get; set; }



    }
}
