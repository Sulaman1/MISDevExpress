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
    [Table("ToolControl", Schema = "mobile")]
    public class ToolControl
    {
        [Key]
        public int ToolControlId { get; set; }
        public int ToolId { get; set; }
        public int ControlId { get; set; }        
        [Display(Name = "Control Title")]
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public int OrderNo { get; set; }
        public string? Value { get; set; }
        public virtual Control? Control { get; set; }
        public virtual Tool? Tool { get; set; }
        public List<ToolControlDetail>? ToolControlDetail { get; set; }
    }
}
