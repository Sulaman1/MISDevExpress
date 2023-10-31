using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain.ToolApp.ToolAppPost
{
    [Table("ToolInfoBasicPost", Schema = "mobile")]
    public class ToolInfoBasicPost
    {
        [Key]
        public int ToolInfoBasicPostId { get; set; }
        public int ToolId { get; set; }
        public double Latitute { get; set; }
        public double Longitute { get; set; }
        public string CurrentDateTime { get; set; }
        public string Username { get; set; }
        public int Counter { get; set; }
        public DateTime? UploadedDate { get; set; }
        public virtual Tool Tool { get; set; }        
    }
}
