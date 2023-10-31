using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain.ToolApp.ToolAppPost
{
    [Table("ToolModulePost", Schema = "mobile")]
    public class ToolModulePost
    {
        [Key]
        public int ToolModulePostId { get; set; }
        public int ToolId { get; set; }
        public string? ToolModuleName { get; set; }
        public string DropdownMenuName { get; set; }
        public string? Value { get; set; }
        public int Counter { get; set; }
        public bool IsSelected { get; set; }        
    }
}
