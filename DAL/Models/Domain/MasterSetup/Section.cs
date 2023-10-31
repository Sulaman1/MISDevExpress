using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("Section", Schema = "master")]
    public class Section
    {
        [Key]
        [Display(Name = "Section")]
        public int SectionId { get; set; }
        [Required]
        [Display(Name = "Section")]
        public string Name { get; set; }
    }
}
