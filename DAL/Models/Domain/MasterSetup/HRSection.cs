using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("HRSection", Schema = "HR")]
    public class HRSection
    {
        [Key]
        [Display(Name = "Section")]
        public int HRSectionId { get; set; }
        [Required]
        [Display(Name = "Section")]
        public string Name { get; set; }
    }
}
