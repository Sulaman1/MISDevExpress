using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain.ToolApp.ToolAppPost
{
    [Table("ToolControlInfoPost", Schema = "mobile")]
    public class ToolControlInfoPost
    {
        [Key]
        public int ToolControlInfoPostId { get; set; }
        public string ControlName { get; set; }
        public int ToolControlId { get; set; }
        public int ToolId { get; set; }
        public int OrderNo { get; set; }
        public int Counter { get; set; }
        public string? ControlValue { get; set; }
        public string ControlLebel { get; set; }        
        public virtual Tool Tool { get; set; }
    }
}
