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
    [Table("ToolUserAccess", Schema = "mobile")]
    public class ToolUserAccess
    {
        [Key]
        public int ToolUserAccessId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public int ToolId { get; set; }
        [Required]
        [Display(Name = "User")]
        public string Username { get; set; }        
        public virtual Tool Tool { get; set; }        
        public virtual ApplicationUser ApplicationUser { get; set; }        
    }
}
