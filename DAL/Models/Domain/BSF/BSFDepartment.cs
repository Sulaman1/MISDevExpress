using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models.Domain.BSF
{
    [Table("BSFDepartment", Schema = "master")]
    public class BSFDepartment
    {
        [Key]
        public int BSFDepartmentId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
