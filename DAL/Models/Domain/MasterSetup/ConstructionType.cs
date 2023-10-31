using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("ConstructionType", Schema = "master")]
    public class ConstructionType
    {
        [Key]
        [Display(Name = "Construction Type")]
        public int ConstructionTypeId { get; set; }
        [Required]       
        public string Name { get; set; }                
    }
}