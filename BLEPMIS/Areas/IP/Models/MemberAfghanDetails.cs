using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLEPMIS.Areas.IP.Models
{
    [Table("MemberAfghanDetail", Schema = "master")]
    public class MemberAfghanDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Member")]
        public int MemberId { get; set; }
        //[Required]
        //AfgProvince
        //    AfgDistrict
        //    AfhTehsil
        //    AfgUC
        //    AfgVillage
        //    AfgMuhalla
        //    AfgReasonNotGoingback
        //    HHName
        //    HHGender
        //    HHAge
        //    IsFemale
        //    FamilyPer04
        //    FamilyPer511
        //    FamilyPer1217
        //    FamilyPer1859
        //    FamilyPer60
        //    FamilyPerSpecialPerson
        //    FamilyPerAgri
        //    FamilyPerLiveStock
        //    FamilyPerJob
        //    FamilyPerSkilled
        //    FamilyPerUnskilled
        //    FamilyPerBusiness
        //    HouseStatus
        //    IncomeSource
        //    IncomeSourceOther
        //    IncomeMembersInvolve
        //    IncomeMonthly
        //    IncomeExpenditure
        //    AssistanceType
        //    AssistanceOther
        //    TrainingInsterest
        //    SocioEconomicStatus

            
    }
}
