using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("Tehsil", Schema = "master")]
    public class Tehsil
    {
        [Key]
        public int TehsilId { get; set; }
        [Display(Name = "Tehsil")]
        public string TehsilName { get; set; }
        public int DistrictId { get; set; }        
        public virtual District? District { get; set; }
    }
}
