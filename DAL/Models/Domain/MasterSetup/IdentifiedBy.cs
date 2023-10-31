using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("IdentifiedBy", Schema = "master")]
    public class IdentifiedBy
    {
        [Key]
        [Display(Name = "Identified By")]
        public int IdentifiedById { get; set; }
        [Required]       
        public string Name { get; set; }                
    }
}