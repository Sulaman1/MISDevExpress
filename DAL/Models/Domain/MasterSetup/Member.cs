using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("Member", Schema = "master")]
    public class Member
    {
        [Key]
        public int MemberId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string MemberName { get; set; }
        [Required]
        [Display(Name = "Father Name/Husband")]
        public string? FatherName { get; set; }
        [Required]
        [Display(Name = "CNIC")]
        public string CNIC { get; set; }
        [Display(Name = "House Hold#")]
        public string? HouseHoldNo { get; set; }
        [Required]
        [Display(Name = "Cell No")]
        public string CellNo { get; set; }
        public string? AccountNumber { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        public int Age { get; set; }
        public bool IsAnyDisability { get; set; }
        public string? MaritalStatus { get; set; }
        [Display(Name = "Is Refugee ?")]
        public bool IsRefugee { get; set; } = false;
        [ForeignKey("BeneficiaryType")]
        public int BeneficiaryTypeId { get; set; }             
        [Display(Name = "Profile Picture")]
        public byte[]? ProfilePicture { get; set; }
        public int VillageId { get; set; }
        public virtual BeneficiaryType? BeneficiaryType { get; set; }
        public virtual Village? Village { get; set; }
    }
}
