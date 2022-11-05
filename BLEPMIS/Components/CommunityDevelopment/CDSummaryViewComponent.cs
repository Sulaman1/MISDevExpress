using DBContext.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BLEPMIS.Components.CommunityDevelopment
{
    public class CDSummaryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public CDSummaryViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int DId, int TId, int UCId, int CTId)
        {
            var applicationDbContext = await _context.CDSummary.Include(a => a.UnionCouncil.Tehsil).Include(a=>a.CommunityType).ToListAsync();
                      
            if (DId != 1)
            {
                if(TId != 0)
                {
                    if(UCId != 0)
                    {
                        applicationDbContext = applicationDbContext.Where(a => a.UnionCouncilId == UCId).ToList();
                    }
                    else
                    {
                        applicationDbContext = applicationDbContext.Where(a => a.UnionCouncil.TehsilId == TId).ToList();
                    }
                }
                else
                {
                    applicationDbContext = applicationDbContext.Where(a=>a.DistrictId == DId).ToList();
                }                
            }           
            if(CTId != 0)
            {
                applicationDbContext = applicationDbContext.Where(a => a.CommunityTypeId == CTId).ToList();
            }
            ViewBag.TotalCD = applicationDbContext.Count();
            return await Task.FromResult((IViewComponentResult)View("CDSummary", applicationDbContext));
        }
    }
}
