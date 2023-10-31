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
    [Table("Control", Schema = "mobile")]
    public class Control
    {
        [Key]
        public int ControlId { get; set; }
        [Required]
        [Display(Name = "Control Name")]
        public string Name { get; set; }        
        public bool IsExtended { get; set; }        
    }
}
