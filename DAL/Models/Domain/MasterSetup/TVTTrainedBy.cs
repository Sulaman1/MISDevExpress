using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("TVTTrainedBy", Schema = "master")]
    public class TVTTrainedBy
    {
        [Key]
        [Display(Name = "TVT Trained By")]
        public int TVTTrainedById { get; set; }
        [Required]       
        public string Name { get; set; }                
    }
}