using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain.GRM
{
    [Table("GrievanceRedressal", Schema = "master")]
    public class GrievanceRedressal
    {
        [Key]
        public int GRMId { get; set; }
        [Required]
        [Display(Name = "District")]
        public string DistrictName { get; set; }
        [Required]
        [Display(Name = "Tehsil")]
        public string TehsilName { get; set; }
        [Display(Name = "GRM#")]
        [Required]
        public string GRMNumber { get; set; }
        [Required]
        [DisplayName("Complainant Name")]
        public string FullName { get; set; }
        [DisplayName("Telephone Number")]
        public string? ContactNumber { get; set; }
        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }
        [DisplayName("Alternate Name")]
        public string? AlternateName { get; set; }
        [DisplayName("Alternate Contact")]
        public string? AlternateContact { get; set; }        
        [DisplayName("Method to Contact You")]
        public string? MethodtoContact { get; set; }
        public string? Email { get; set; }
        [DisplayName("Is Alternate Contact")]
        public bool IsAlternateContact { get; set; } = false;
        [DisplayName("Is By Email")]
        public bool IsByEmail { get; set; }
        [DisplayName("Is By Phone")]
        public bool IsByPhone { get; set; }
        [DisplayName("Is By Mobile")]
        public bool IsByMobile { get; set; }
        [DisplayName("Is By Mail")]
        public bool IsByMail { get; set; }
        [DisplayName("Mailing Address")]
        public string? MailingAddress { get; set; }
        [DisplayName("I would like to pick up responses in person from Office")]
        public bool IsPickUpResponses { get; set; }
        [DisplayName("Office")]
        public string PickUpResponses { get; set; }
        [DisplayName("Complaint Channel")]
        public string ComplaintChannel { get; set; }
        [Required]
        public DateTime OnDateTime { get; set; }
        [DisplayName("Detail of GRM")]
        [Required]
        public string GRMDetail { get; set; }
        [DisplayName("You Can Use my personal detail")]        
        public bool CanUserPersonalDetail { get; set; }
        [DisplayName("You can use my name when talking about this complaint in community / general meetings")]
        public bool CanUserMyName { get; set; }
        [DisplayName("I do not want to disclose my name")]
        public bool DoDisclose { get; set; }
        [DisplayName("Written Documents")]
        public string? Attachment1 { get; set; }
        [DisplayName("Photocopies of Document")]
        public string? Attachment2 { get; set; }
        [DisplayName("Audio/Other")]
        public string? Attachment3 { get; set; }
        [DisplayName("Resolved Evidence")]
        public string? Attachment4 { get; set; }
        [DisplayName("Remarks (if any)")]
        public string? Remarks { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; } = true;        
    }
}
