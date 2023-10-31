using DAL.Models.Domain.MasterSetup;
using DAL.Models.Domain.Training;

namespace DAL.Models.ViewModels
{
    public class LIPMemberAfghan
    {
        public Member Member { get; set; }
        public LIPAssetTransfer LIPAssetTransfer { get; set; }
        public MemberAfghanDetail MemberAfghanDetail { get; set; }
    }
}
