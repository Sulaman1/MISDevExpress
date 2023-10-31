using DAL.Models.Domain.MasterSetup;
using DAL.Models.Domain.Training;

namespace BLEPMIS.Areas.IP.Models.ViewModels
{
    public class LIPMemberAfgan
    {
        public Member Member { get; set; }
        public LIPAssetTransfer LIPAssetTransfer { get; set; }
        public MemberAfghanDetail MemberAfghanDetail { get; set; }
    }
}
