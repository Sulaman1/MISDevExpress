using DAL.Models.Domain.MasterSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ViewModels
{
    public class SPToolAnalysis
    {
        [Key]
        public int ToolId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitute { get; set; }
        public double Longitute { get; set; }
        public string ControlLebel { get; set; }
        public string ControlName { get; set; }
        public string Response { get; set; }
    }
}
