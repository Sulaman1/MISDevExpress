using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("District", Schema = "master")]
    public class District
    {
        [Key]
        public int DistrictId { get; set; }
        [Required]
        [Display(Name = "District")]
        public string Name { get; set; }
        [Required]
        [MaxLength(3, ErrorMessage = "Code cannot exceeding from 3 digit")]
        public string? Code { get; set; }
        public string? Description { get; set; }
        [ForeignKey("Division")]
        [Display(Name = "Division")]
        public int DivisionId { get; set; }
        public virtual Division? Division { get; set; }
    }
}
