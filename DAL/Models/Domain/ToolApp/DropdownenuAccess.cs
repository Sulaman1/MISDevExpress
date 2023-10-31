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
    [Table("DropdownenuAccess", Schema = "mobile")]
    public class DropdownenuAccess
    {
        [Key]
        public int DropdownenuAccessId { get; set; }
        public int ToolId { get; set; }
        public string DropdownMenuName { get; set; }        
        public string ToolModuleName { get; set; }        
        public string Value { get; set; }        
               
    }
}
