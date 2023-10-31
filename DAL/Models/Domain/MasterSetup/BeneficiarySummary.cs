using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain.MasterSetup
{
    public class BeneficiarySummary
    {
        [Key]
        public int BeneficiarySummaryId { get; set; }
        [Display(Name = "District")]
        public string DistrictName { get; set; }
        [Display(Name = "Male Beneficiary")]
        public int MaleBeneficiary { get; set; }
        [Display(Name = "Female Beneficiary")]
        public int FemaleBeneficiary { get; set; }
        [Display(Name = "Male Refugee Beneficiary")]
        public int MaleRefugeeBeneficiary { get; set; }
        [Display(Name = "Female Refugee Beneficiary")]
        public int FemaleRefugeeBeneficiary { get; set; }
    }
}
