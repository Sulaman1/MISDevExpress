using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain.ToolApp.ToolAppPost
{
    [Table("ToolInfoDetailPost", Schema = "mobile")]
    public class ToolInfoDetailPost
    {
        [Key]
        public int ToolInfoDetailPostId { get; set; }
        public int ToolControlInfoPostId { get; set; }
        public int ToolControlId { get; set; }
        public string Label { get; set; }
        public bool ControlDetailValue { get; set; }
        public virtual ToolControlInfoPost ToolControlInfoPost { get; set; }
    }
}
