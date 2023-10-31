using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("CommunityType", Schema = "master")]
    public class CommunityType
    {
        [Key]
        public int CommunityTypeId { get; set; }
        [Required]
        [Display(Name = "Community Type")]
        public string Name { get; set; }
        [Required]
        [MaxLength(3, ErrorMessage = "Code cannot exceeding from 3 digit")]
        public string Code { get; set; }
    }
}
