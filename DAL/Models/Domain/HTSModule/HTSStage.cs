using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain.HTSModule
{
    [Table("HTSStage", Schema = "master")]
    public class HTSStage
    {
        [Key]
        public int HTSStageId { get; set; }
        public int HTSId { get; set; }
        [Display(Name = "Installment No")]
        public int InstallmentNo { get; set; }
        [Display(Name = "Amount Paid")]
        public int AmountPaid { get; set; }
        [Display(Name = "Attachment")]
        public string? StageAttachment { get; set; }
        [Display(Name = "Picture-1")]
        public string? Picture1 { get; set; }


        [Display(Name = "Picture-2")]
        public string? Picture2 { get; set; }
        [Display(Name = "Date of Payment")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateofPayment { get; set; } = DateTime.Today;
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public virtual HTS? HTS { get; set; }
    }
}
