using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("TrainingHead", Schema = "master")]
    public class TrainingHead
    {
        [Key]
        public int TrainingHeadId { get; set; }
        [Required]
        [Display(Name = "Training Head")]
        public string TrainingHeadName { get; set; }
        [Display(Name = "Code")]
        public string TrainingHeadCode { get; set; }
    }
}
