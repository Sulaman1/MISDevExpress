using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("HRDesignation", Schema = "HR")]
    public class HRDesignation
    {
        [Key]
        public int HRDesignationId { get; set; }
        [Required]
        public string DesignationName { set; get; }    
    }
}
