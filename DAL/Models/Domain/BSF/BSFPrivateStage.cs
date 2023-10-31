using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain.BSF
{
    [Table("BSFPrivateStage", Schema = "master")]
    public class BSFPrivateStage
    {
        [Key]
        public int BSFPrivateStageId { get; set; }
        public int BSFPrivateId { get; set; }
        public int StageNumber { get; set; } = 1;
        [Display(Name = "Stage Name")]
        public string StageName { get; set; }
        [Display(Name = "Attachment")]
        public string? StageAttachment { get; set; }
        [Display(Name = "Before Pictures")]
        public string? BeforeAttachment { get; set; }
        [Display(Name = "After Pictures")]
        public string? AfterAttachment { get; set; }
        [Display(Name = "Amount Release")]
        public decimal AmountRelease { get; set; } = 0;
        [Display(Name = "On Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OnDate { get; set; } = DateTime.Today;
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public virtual BSFPrivate? BSFPrivate { get; set; }
    }
}
