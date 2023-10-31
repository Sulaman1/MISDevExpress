using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("Division", Schema = "master")]
    public class Division
    {
        [Key]
        public int DivisionId { get; set; }
        [Required]
        [Display(Name = "Division")]
        public string Name { get; set; }
        [Required]
        [MaxLength(3, ErrorMessage = "Code cannot exceeding from 3 digit")]
        public string Code { get; set; }
        public string? Description { get; set; }
        [ForeignKey("Provience")]
        [Display(Name = "Provience")]
        public int ProvienceId { get; set; }
        public virtual Provience? Provience { get; set; }
    }
}
