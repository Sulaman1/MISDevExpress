using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("HRQualificationLevel", Schema = "HR")]
    public class HRQualificationLevel
    {
        [Key]
        public int HRQualificationLevelId { get; set; }
        [Required]
        [Display(Name="Qualification Level")]
        public string Name { set; get; }    
    }
}
