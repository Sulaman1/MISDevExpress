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
    [Table("Tool", Schema = "mobile")]
    public class Tool
    {
        [Key]
        public int ToolId { get; set; }
        [Required]
        [Display(Name = "Tool Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Module")]
        [ForeignKey("ToolModule")]
        public string ToolModuleName { get; set; }        
        public bool IsActive { get; set; } = true;
        [DataType(DataType.Date)]        
        public DateTime? CreatedOn { get; set; } = DateTime.Now.Date;
        public ICollection<ToolControl>? ToolControl { get; set; }
        public virtual ToolModule? ToolModule { get; set; }
    }
}
