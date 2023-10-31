using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("HREmployee", Schema = "HR")]
    public class HREmployee
    {
        [Key]        
        public int HREmployeeId { get; set; }
        [Display(Name = "Employee")]
        public string EmployeeName { get; set; }
        [Display(Name = "Father Name")]
        public string FatherName { get; set; }
        public string CNIC { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; } = "";
        [Display(Name = "PTCL Number")]
        public string PTCLNumber { get; set; } = "";
        public string Address { get; set; } = "";
        [Display(Name = "Mailing Address")]
        public string MailingAddress { get; set; } = "";
        [Display(Name = "Last Education Level")]
        public string LastEducationLevel { get; set; } = "";
        [Display(Name = "Domicile/Local")]
        public string DomicileLocal { get; set; } = "";
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; } = "";
        [Display(Name = "Bank Account")]
        public string BankAccount { get; set; } = "";
        public string Email { get; set; } = "";
        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        public DateTime? JoiningDate { get; set; }
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }
        public string Designation { get; set; } = "";
        [Display(Name = "District Of Work")]
        public string DistrictOfWork { get; set; } = "";
        [Display(Name = "CNIC")]
        public string? CNICAttachment { get; set; } = "";
        [Display(Name = "Joining Letter")]
        public string? JoiningLetterAttachment { get; set; } = "";
        [Display(Name = "CV")]
        public string? CVAttachment { get; set; } = "";
        public string Gender { get; set; } = "";
        public bool IsActive { get; set; } = true;
        [Display(Name = "Section")]
        public List<EmployeeContract>? Contracts { get; set; }                
        public string Section { get; set; }                
    }
}
