using DAL.Models.Domain.MasterSetup;
using DAL.Models.Domain.Training;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL.Models.ViewModels

{
    public class LipIDO
    {
        [Key]
        public int LIPAssetTransferId { get; set; }
        [Display(Name = "LIP Code")] public string? LIPCode { get; set; }
        [Display(Name = "Name")] public string? MemberName { get; set; }
        [Display(Name = "Father's NAme")] public string? FatherName { get; set; }
        [Display(Name = "ACC/POR")] public string? CNIC { get; set; }
        [Display(Name = "Contact Number")] public string? CellNo { get; set; }
        [Display(Name = "Village")] public string? Name { get; set; }
        [Display(Name = "Package")] public string? PackageName { get; set; }

        public int UnionCouncilId { get; set; }
        public int TehsilId { get; set; }
        public int DistrictId { get; set; }
        public int LIPPackageId { get; set; }
        public string? Gender {get;set;} 


    }
}
