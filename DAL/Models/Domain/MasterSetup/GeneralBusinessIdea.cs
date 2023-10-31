using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("GeneralBusinessIdea", Schema = "master")]
    public class GeneralBusinessIdea
    {
        [Key]
        public int GeneralBusinessIdeaId { get; set; }
        [Display(Name = "General Business Idea")]
        public string GeneralBusinessIdeaName { get; set; }
    }
}
