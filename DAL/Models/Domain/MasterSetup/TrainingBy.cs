using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("TrainingBy", Schema = "master")]
    public class TrainingBy
    {
        [Key]
        [Display(Name = "Training By")]
        public int TrainingById { get; set; }
        [Required]       
        public string Name { get; set; }                
    }
}