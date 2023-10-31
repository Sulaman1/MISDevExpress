using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("BeneficiaryType", Schema = "master")]
    public class BeneficiaryType
    {
        [Key]
        public int BeneficiaryTypeId { get; set; }
        public string BeneficiaryTypeName { get; set; }
    }
}
