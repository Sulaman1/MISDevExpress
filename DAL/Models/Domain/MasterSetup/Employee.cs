using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Domain.MasterSetup
{
    [Table("Employee", Schema = "master")]
    public class Employee
    {
        [Key]        
        public int EmployeeId { get; set; }
        [Display(Name = "Employee")]
        public string EmployeeName { get; set; }
        [Display(Name = "Father Name")]
        public string FatherName { get; set; }
        public string CNIC { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; } = "";       
        public string Address { get; set; } = "";               
        public string Designation { get; set; } = "";     
        public bool IsActive { get; set; } = true;
        [Display(Name = "Section")]
        public int SectionId { get; set; }        
        public virtual Section? Section { get; set; }
    }
}
