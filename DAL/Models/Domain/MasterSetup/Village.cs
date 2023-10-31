using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("Village", Schema = "master")]
    public class Village
    {
        [Key]
        public int VillageId { get; set; }
        [Required]
        [Display(Name = "Village")]
        public string Name { get; set; }
        [ForeignKey("UnionCouncil")]
        [Display(Name = "UnionCouncil")]
        public int UnionCouncilId { get; set; }
        public virtual UnionCouncil? UnionCouncil { get; set; }

    }
}
