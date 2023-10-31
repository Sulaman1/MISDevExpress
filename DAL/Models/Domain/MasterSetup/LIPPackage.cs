using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("LIPPackage", Schema = "master")]
    public class LIPPackage
    {
        [Key]
        public int LIPPackageId { get; set; }
        [Display(Name = "Package Name")]
        public string PackageName { get; set; }
        [Display(Name = "Package Price")]
        public int PackagePrice { get; set; }
        public string Description { get; set; }
    }
}
