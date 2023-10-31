using DAL.Models.Domain.MasterSetup;
using DAL.Models.Domain.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ViewModels
{
    public class LIPMember
    {
        public Member Member { get; set; }
        public LIPAssetTransfer LIPAssetTransfer { get; set; }
    }
}
