using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("BLEPUserRole", Schema = "master")]
    public class BLEPUserRole
    {
        [Key]
        public int BLEPUserRoleId { get; set; }
        [Required]        
        public string Name { get; set; }
    }
}
