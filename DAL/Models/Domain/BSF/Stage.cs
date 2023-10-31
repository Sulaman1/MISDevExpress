using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models.Domain.BSF
{
    [Table("Stage", Schema = "master")]
    public class Stage
    {
        [Key]
        public int StageId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
