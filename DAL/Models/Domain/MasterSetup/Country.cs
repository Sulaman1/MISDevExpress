
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("Country", Schema = "master")]
    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        [Required]        
        public string CountryName { set; get; }
        [Required]
        public string CountryCode { set; get; }              
    }
}
