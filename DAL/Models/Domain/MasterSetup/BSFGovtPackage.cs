using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("BSFGovtPackage", Schema = "master")]
    public class BSFGovtPackage
    {
        [Key]
        public int BSFGovtPackageId { get; set; }
        [Display(Name = "Package Name")]
        public string PackageName { get; set; }                
        public string Description { get; set; }
    }
}
