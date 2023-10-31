using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain.BSF
{
    [Table("BSFGovStage", Schema = "master")]
    public class BSFGovStage
    {
        [Key]
        public int BSFGovStageId { get; set; }
        public int BSFGovId { get; set; }
        [Display(Name = "Stage#")]
        public string StageNumber { get; set; }
        [Display(Name = "Stage Name")]
        public string? StageName { get; set; }
        [Display(Name = "Stage Attachment")]
        public string? StageAttachment { get; set; }
        [Display(Name = "Before Pictures")]
        public string? BeforeAttachment { get; set; }
        [Display(Name = "After Pictures")]
        public string? AfterAttachment { get; set; }
        [Display(Name = "Amount Release")]
        public decimal AmountRelease { get; set; } = 0;
        [DataType(DataType.Date)]
        [Display(Name = "On Date")]
        public DateTime OnDate { get; set; } = DateTime.Today;
        public string UserId { get; set; }
        public bool IsCompleted { get; set; } = false;  
        public string CreatedBy { get; set; }
        public virtual BSFGov? BSFGov { get; set; }
    }
}
