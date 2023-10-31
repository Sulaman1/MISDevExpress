using DAL.Models.Domain.HTSModule;
using DAL.Models.Domain.MasterSetup;
using DAL.Models.Domain.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ViewModels
{
    public class HTSMember
    {
        public Member Member { get; set; }
        public HTS HTS { get; set; }
    }
}
