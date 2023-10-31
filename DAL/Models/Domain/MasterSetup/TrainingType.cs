using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("TrainingType", Schema = "master")]
    public class TrainingType
    {
        [Key]
        [Display(Name = "Training Type")]
        public int TrainingTypeId { get; set; }
        [Required]
        [Display(Name = "Training Type")]
        public string TrainingTypeName { get; set; }
        public string TrainingTypeCode { get; set; }
        [ForeignKey("TrainingHead")]
        [Display(Name = "Training Head")]
        public int TrainingHeadId { get; set; }
        public virtual TrainingHead? TrainingHead { get; set; }
    }
}
