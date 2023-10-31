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
    [Table("ToolModule", Schema = "mobile")]
    public class ToolModule
    {
        [Key]
        public string ToolModuleName { get; set; }
           
    }
}
