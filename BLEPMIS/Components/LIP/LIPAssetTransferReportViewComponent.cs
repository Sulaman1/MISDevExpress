using DAL.Models.Domain.MasterSetup;
using DAL.Models.Domain.Training;
using DBContext.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BLEPMIS.Components.CommunityDevelopment
{
    public class LIPAssetTransferReportViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public LIPAssetTransferReportViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int DId, int TId, int UCId, int PId, bool? IsRefugee)
        {
            var applicationDbContext = await _context.LIPAssetTransfer.Include(a => a.LIPPackage).Include(a => a.Member).Include(a => a.IdentifiedBy).Select(a => new LIPAssetTransfer { DistrictId = a.DistrictId, Latitute = a.Latitute, Longitute = a.Longitute, LIPCode = a.LIPCode, LIPAssetTransferId = a.LIPAssetTransferId, LIPPackage = new LIPPackage { PackageName = a.LIPPackage.PackageName }, IdentifiedBy = new IdentifiedBy { Name = a.IdentifiedBy.Name }, Member = new Member { MemberName = a.Member.MemberName, FatherName = a.Member.FatherName, CellNo = a.Member.CellNo, BeneficiaryTypeId = a.Member.BeneficiaryTypeId, CNIC = a.Member.CNIC, Village = new Village { VillageId = a.VillageId, Name = a.Village.Name, UnionCouncilId = a.Village.UnionCouncilId, UnionCouncil = new UnionCouncil { UnionCouncilName = a.Village.UnionCouncil.UnionCouncilName, TehsilId = a.Village.UnionCouncil.TehsilId, Tehsil = new Tehsil { TehsilName = a.Village.UnionCouncil.Tehsil.TehsilName } } } }, IsSubmitted = a.IsSubmitted, IsReviewed = a.IsReviewed, IsAssetTransfer = a.IsAssetTransfer }).Where(a => a.IsReviewed == true && a.IsAssetTransfer == false).ToListAsync();
            if (IsRefugee == true)
            {
                applicationDbContext = applicationDbContext.Where(a => a.Member.BeneficiaryTypeId == 3).ToList();
            }
            else
            {
                applicationDbContext = applicationDbContext.Where(a => a.Member.BeneficiaryTypeId == 1).ToList();
            }
            if (DId != 1)
            {
                if(TId != 0)
                {
                    if(UCId != 0)
                    {
                        applicationDbContext = applicationDbContext.Where(a => a.Member.Village.UnionCouncilId == UCId).ToList();
                    }
                    else
                    {
                        applicationDbContext = applicationDbContext.Where(a => a.Member.Village.UnionCouncil.TehsilId == TId).ToList();
                    }
                }
                else
                {
                    applicationDbContext = applicationDbContext.Where(a=>a.DistrictId == DId).ToList();
                }                
            }           
            if(PId != 0)
            {
                applicationDbContext = applicationDbContext.Where(a => a.LIPPackageId == PId).ToList();
            }
            ViewBag.TotalLIP = applicationDbContext.Count();
            return await Task.FromResult((IViewComponentResult)View("LIPAssetTransferReport", applicationDbContext));
        }
    }
}
