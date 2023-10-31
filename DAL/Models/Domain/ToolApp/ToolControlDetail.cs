using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL.Models.Domain.ToolApp
{
    [Table("ToolControlDetail", Schema = "mobile")]
    public class ToolControlDetail
    {
        [Key]
        public int ToolControlDetailId { get; set; }        
        public int ToolControlId { get; set; }        
        [Display(Name = "Option Title")]
        public string Label { get; set; }
        public bool Value { get; set; }
        [Display(Name = "Control Title")]
        public virtual ToolControl? ToolControl { get; set; }        
    }
}
