using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DBContext.Data;
using DAL.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models.Domain.MasterSetup;
using DAL.Models.Domain.Training;
using System.Collections.Generic;
using DAL.Models.Domain.ToolApp;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BAL.IRepository.MasterSetup.CD;
using System.Diagnostics.Metrics;

namespace BLEPMIS.Controllers
{
    public class BarData
    {
        public string District { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int RefugeeMale { get; set; }
        public int RefugeeFemale { get; set; }
        public int Total { get; set; }

    }

    public class BSFData
    {
        public string District { get; set; }
        public int Government { get; set; }
        public int Private { get; set; }
    }

    public class CICIGData
    {
        public string District { get; set; }
        public int CIMaleCount { get; set; }
        public int CIFemaleCount { get; set; }
        public int CIGMaleCount { get; set; }
        public int CIGFemaleCount { get; set; }
        public int TotalCount { get; set; }
    }

    public class TrainingMF
    {
        public string District { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int Total { get; set; }
    }

    public class LipBifurHostData {
        public string District { get; set; }        
        public int GoatAndSheepPackage { get; set; }
        public int SewingAndPekoMachine { get; set; }
        public int TuckShop { get; set; }
        public int TotalCount { get; set; }
    }
    public class LipBifurRefData
    {
        public string District { get; set; }
        public int SunBasedFruitDrying { get; set; }
        public int BackyardKitchenGardening { get; set; }
        public int IDOPoultry { get; set; }
        public int TotalCount { get; set; }
    }
    public class HTS
    {
        public string District { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int Total { get; set; }
    }

    public class HTSTable
    {
        public string District { get; set; }
        public int MaleSmall { get; set; }
        public int FemaleSmall { get; set; }
        public int MaleBig { get; set; }
        public int FemaleBig { get; set; }
        public int Total { get; set; }
    }
    public class CountValues
    {
        public string District { get; set; }
        public int Count { get; set; }
    }

    public class CountValues2
    {
        public string District { get; set; }
        public int LiveStock { get; set; }
        public int Forest { get; set; }      
    }

    public class DistrictGenderCount
    {
        public string District { get; set; }
        public int GenderCount { get; set; }
    }

   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ApplicationDbContextYouth _contextYouth;
        private readonly UserManager<ApplicationUser> _userManager;
        public double pTotal = 0;
        public double pTotalCiCig = 0;

        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger, ApplicationDbContext context, ApplicationDbContextYouth contextYouth, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _contextYouth = contextYouth;

            _userManager = userManager;
            _configuration = configuration;
        }

        public List<BarData> GetBarData()
        {
            List<BarData> districtSummaries = new List<BarData>();
           
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            // For .NET Core, use: IConfiguration.GetConnectionString("YourConnectionStringName");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
               
                    SqlCommand command = new SqlCommand("TotalBenefSummary", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            BarData districtSummary = new BarData();
                            districtSummary.District = reader["District"].ToString();
                            districtSummary.Male = Convert.ToInt32(reader["Male"]);
                            districtSummary.RefugeeMale = Convert.ToInt32(reader["RefugeeMale"]);
                            districtSummary.Female = Convert.ToInt32(reader["Female"]);
                            districtSummary.RefugeeFemale = Convert.ToInt32(reader["RefugeeFemale"]);
                            districtSummary.Total = Convert.ToInt32(reader["Total"]);
                            if (reader["District"].ToString() != "NONE")
                            {
                                pTotal += districtSummary.Total;
                                districtSummaries.Add(districtSummary);
                            }
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                    }                               
            }

            return districtSummaries;
        }

        public List<CICIGData> GetTotalCiCigData()
        {
            List<CICIGData> districtSummaries = new List<CICIGData>();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            // For .NET Core, use: IConfiguration.GetConnectionString("YourConnectionStringName");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand("TotalCICIGCount", connection);
                command.CommandType = CommandType.StoredProcedure;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        CICIGData districtSummary = new CICIGData();
                        districtSummary.District = reader["District"].ToString();
                        districtSummary.CIMaleCount = Convert.ToInt32(reader["CIMaleCount"]);
                        districtSummary.CIFemaleCount = Convert.ToInt32(reader["CIFemaleCount"]);
                        districtSummary.CIGMaleCount = Convert.ToInt32(reader["CIGMaleCount"]);
                        districtSummary.CIGFemaleCount = Convert.ToInt32(reader["CIGFemaleCount"]);
                        districtSummary.TotalCount = Convert.ToInt32(reader["TotalCount"]);
                        if (reader["District"].ToString() != "NONE")
                        {
                            pTotalCiCig += districtSummary.TotalCount;
                            districtSummaries.Add(districtSummary);
                        }
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Handle exceptions here
                }
            }

            return districtSummaries;
        }

        public List<TrainingMF> GetTrainingEventData()
        {           
            List<TrainingMF> trainingEventsDataList = new List<TrainingMF>();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            // For .NET Core, use: IConfiguration.GetConnectionString("YourConnectionStringName");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {               
                SqlCommand command = new SqlCommand("TrainingEvents", connection);
                command.CommandType = CommandType.StoredProcedure;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        TrainingMF trainingEventsData = new TrainingMF();
                        trainingEventsData.District = reader["District"].ToString();
                        trainingEventsData.Male = Convert.ToInt32(reader["Male"]);
                        trainingEventsData.Female = Convert.ToInt32(reader["Female"]);
                        trainingEventsData.Total = Convert.ToInt32(reader["Total"]);

                        trainingEventsDataList.Add(trainingEventsData);

                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Handle exceptions here
                }                
            }

            return trainingEventsDataList;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalBenf = GetBarData();
            ViewBag.CICIgBenfList = GetTotalCiCigData();
            var trainingEv = GetTrainingEventData();

            ViewBag.trainingEvents = trainingEv;

            var maleCBTT = 0;
            var femaleCBTT = 0;
            var grandT = 0;
            foreach (var obj in trainingEv)
            {
                maleCBTT += obj.Male;
                femaleCBTT += obj.Female;
            }
            grandT = maleCBTT + femaleCBTT;

            ViewBag.gT = grandT;
            ViewBag.fCBTT = femaleCBTT;
            ViewBag.mCBTT = maleCBTT;

            int c = _contextYouth.District.Count();

            int LIPCount = _context.LIPAssetTransfer.Count(x => new[] { 1, 2, 3, 4 }.Contains(x.IdentifiedById)); //-51 //target: Host: 4500, IDO: 4000, Youth: 2500 ... 8000, acheived
            double perLIP = ((double)LIPCount / 8378) * 100;
            //BSF TARGET: 18
            var BSFCount = _context.BSFGov.Count();
            double perBSF = ((double)BSFCount / 85) * 100;

            var HTSCount = _context.HTS.Count();//target 174

            int CICIGCount = _context.CommunityInstitution.Count(x => x.IsVerified == true);//CI/CIGs: 259, TARGET: 850
            double perCICIG = ((double)CICIGCount / 850) * 100;
            double totalProgress = ((double)pTotal / 56610) * 100;             

            var CICIGFemale = from mtd in _context.MemberTrainingDetail
                            join cim in _context.CommunityInstituteMember on mtd.CommunityInstituteMemberId equals cim.CommunityInstituteMemberId
                              join ci in _context.CommunityInstitution on cim.CommunityInstitutionId equals ci.CommunityInstitutionId
                              where ci.Gender == "Female"
                            select mtd;
            int CICIGFemaleBenef = CICIGFemale.Count();

//            select Gender from master.CommunityInstituteMember ci
//join training.MemberTrainingDetail mtd on mtd.CommunityInstituteMemberId = ci.CommunityInstituteMemberId
//join master.CommunityInstitution cim on cim.CommunityInstitutionId = ci.CommunityInstitutionId

            var CICIGMale = from mtd in _context.MemberTrainingDetail
                              join cim in _context.CommunityInstituteMember on mtd.CommunityInstituteMemberId equals cim.CommunityInstituteMemberId
                              join ci in _context.CommunityInstitution on cim.CommunityInstitutionId equals ci.CommunityInstitutionId
                              where ci.Gender == "Male"
                              select mtd;
            int CICIGMaleBenef = CICIGMale.Count();

            int CICIGFormed = _context.CommunityInstitution.Count(x => x.IsVerified == true);                                                        

            int LIPRFCount = _context.LIPAssetTransfer.Count(x => new[] { 4 }.Contains(x.IdentifiedById) && x.IsAssetTransfer == true);
            //int LIPHOSTCount = _context.LIPAssetTransfer.Count(x => new[] { 1, 2, 3 }.Contains(x.IdentifiedById) && x.IsAssetTransfer == true);
            int LIPHOSTCount = (from l in _context.LIPAssetTransfer
                               join m in _context.Member on l.MemberId equals m.MemberId
                               where (new[] { 1, 2, 3 }.Contains(l.IdentifiedById)) && l.IsAssetTransfer
                              select l).Count();

            ViewBag.LipRFBenef = LIPRFCount;
            ViewBag.LipHOSTBenef = LIPHOSTCount;
            ViewBag.BSFBenef = BSFCount;
            ViewBag.HTSBenef = HTSCount;          
            ViewBag.CICIGFemaleBenef = CICIGFemaleBenef;
            ViewBag.CICIGMaleBenef = CICIGMaleBenef;
            ViewBag.CICIFormed = CICIGFormed;

            ViewBag.PerLIP = perLIP;       
            ViewBag.PerCICIG = perCICIG;
            ViewBag.PerBSF = perBSF;
            ViewBag.PTotal = Convert.ToInt32(totalProgress);


            List<HTSTable> htsTable = (from hts in _context.HTS
                                       join member in _context.Member on hts.MemberId equals member.MemberId
                                       group new { hts, member } by hts.District into districtGroup
                                       select new HTSTable
                                       {
                                           District = districtGroup.Key,
                                           MaleSmall = districtGroup.Sum(item => item.hts.TunnelSize == "11000 Sqft" && item.member.Gender == "Male" ? 1 : 0),
                                           FemaleSmall = districtGroup.Sum(item => item.hts.TunnelSize == "11000 Sqft" && item.member.Gender == "Female" ? 1 : 0),
                                           MaleBig = districtGroup.Sum(item => item.hts.TunnelSize == "22000 Sqft" && item.member.Gender == "Male" ? 1 : 0),
                                           FemaleBig = districtGroup.Sum(item => item.hts.TunnelSize == "22000 Sqft" && item.member.Gender == "Female" ? 1 : 0),
                                           Total = districtGroup.Count()
                                       }).ToList();
            ViewBag.HTSTable = htsTable;

            List<HTS> htsList = (from hts in _context.HTS
                                             join member in _context.Member on hts.MemberId equals member.MemberId
                                             group new { hts, member } by hts.District into districtGroup
                                             select new HTS
                                             {
                                                 District = districtGroup.Key,
                                                 Male = districtGroup.Count(item => item.member.Gender == "Male"),
                                                 Female = districtGroup.Count(item => item.member.Gender == "Female"),
                                                 Total = districtGroup.Count(),
                                             }).ToList();
            ViewBag.HTSList = htsList;

            List<HTS> edpList = new List<HTS>();
            string[] stringsToAdd = { "Killa Abdullah", "Killa Saifullah", "Pishin", "Sherani", "Chagai", "Mastung", "Nushki", "Zhob" };
            for (int i = 0; i < 8; i++)
            {
                HTS htsObject = new HTS
                {
                    District = stringsToAdd[i],
                    Male = 0,
                    Female = 0,
                    Total = 0
                };

                edpList.Add(htsObject);
            }
            ViewBag.EDPList = edpList;

            //List<HTS> edpList = 
            //ViewBag.EDPList = edpList;


            List<CICIGData> cicigList= (from ci in _context.CommunityInstitution
                                      join ct in _context.CommunityType on ci.CommunityTypeId equals ct.CommunityTypeId
                                      where ci.IsVerified == true
                                      group ci by ci.District into groupedData
                                      select new CICIGData
                                      {
                                          District = groupedData.Key,
                                          CIMaleCount = groupedData.Sum(c => c.Gender == "Male" && c.CommunityTypeId == 1 ? 1 : 0),
                                          CIGMaleCount = groupedData.Sum(c => c.Gender == "Male" && c.CommunityTypeId == 2 ? 1 : 0),
                                          CIFemaleCount = groupedData.Sum(c => c.Gender == "Female" && c.CommunityTypeId == 1 ? 1 : 0),
                                          CIGFemaleCount = groupedData.Sum(c => c.Gender == "Female" && c.CommunityTypeId == 2 ? 1 : 0),
                                          TotalCount = groupedData.Count()
                                      }).ToList();

            ViewBag.CICIGList = cicigList;

       
            //List<CICIGData> cicigBenefList = (from cm in _context.CommunityInstituteMember
            //            join ci in _context.CommunityInstitution on cm.CommunityInstitutionId equals ci.CommunityInstitutionId
            //            join m in _context.Member on cm.MemberId equals m.MemberId
            //            where ci.IsVerified == true
            //            group ci by ci.District into groupedData
            //            select new CICIGData
            //            {
            //                District = groupedData.Key,
            //                CIMaleCount = groupedData.Sum(c => c.Gender == "Male" && c.CommunityTypeId == 1 ? 1 : 0),
            //                CIGMaleCount = groupedData.Sum(c => c.Gender == "Male" && c.CommunityTypeId == 2 ? 1 : 0),
            //                CIFemaleCount = groupedData.Sum(c => c.Gender == "Female" && c.CommunityTypeId == 1 ? 1 : 0),
            //                CIGFemaleCount = groupedData.Sum(c => c.Gender == "Female" && c.CommunityTypeId == 2 ? 1 : 0),
            //                TotalCount = groupedData.Count()
            //            }).ToList();

            //ViewBag.CICIgBenfList = cicigBenefList;          


            List<CountValues> bsfGov = (from b in _context.BSFGov
                                      group b by b.DistrictName into g
                                      select new CountValues
                                      {
                                          District = g.Key,
                                          Count = g.Count()
                                      }).ToList();

            List<CountValues2> bsfGov2 = (from bg in _context.BSFGov
                                          group bg by bg.DistrictName into districtGroup
                                          select new CountValues2
                                          {
                                              District = districtGroup.Key,
                                              LiveStock = districtGroup.Sum(bg => bg.DepartmentName == "Live Stock" ? 1 : 0),
                                              Forest = districtGroup.Sum(bg => bg.DepartmentName == "Forest" ? 1 : 0)                                             
                                          }).ToList();
            ViewBag.BsfGov2 = bsfGov2;


            List<CountValues> bsfPri = (from bsfPrivate in _context.BSFPrivate
                        group bsfPrivate by bsfPrivate.DistrictName into groupedData
                        select new CountValues
                        {
                            District = groupedData.Key,
                            Count = groupedData.Count()
                        }).ToList();

            List<BSFData> bsfList = new List<BSFData>();

            for (var i = 0; i < bsfGov.Count; i++)
            {
                BSFData bar = new BSFData
                {
                    District = bsfGov[i].District,
                    Government = bsfGov[i].Count,
                    Private = 0
                };

                bsfList.Add(bar);
            }

            ViewBag.BSFList = bsfList;

            List<BarData> lipData = (from l in _context.LIPAssetTransfer                                     
                                     join m in _context.Member on l.MemberId equals m.MemberId
                                     join d in _context.District on l.DistrictId equals d.DistrictId
                                     where l.IsAssetTransfer == true
                                     group new { d.Name, m.Gender, l.LIPCode } by d.Name into groupedData
                                     select new BarData
                                     {
                                         District = groupedData.Key,
                                         RefugeeMale = groupedData.Count(item => item.Gender == "Male" && item.LIPCode.StartsWith("RF")),
                                         RefugeeFemale = groupedData.Count(item => item.Gender == "Female" && item.LIPCode.StartsWith("RF")),
                                         Male = groupedData.Count(item => item.Gender == "Male" && !item.LIPCode.StartsWith("RF")),
                                         Female = groupedData.Count(item => item.Gender == "Female" && !item.LIPCode.StartsWith("RF")),
                                         Total = groupedData.Count()
                                     }).ToList();


            ViewBag.TBBarChartData = lipData;

            List<LipBifurHostData> lipBifurHostData = (from lat in _context.LIPAssetTransfer
                         join lp in _context.LIPPackage on lat.LIPPackageId equals lp.LIPPackageId
                         where lat.IsAssetTransfer == true && (lat.IdentifiedById == 1 || lat.IdentifiedById == 2 || lat.IdentifiedById == 3)
                         group new { lat, lp } by lat.District into grouped
                         select new LipBifurHostData
                         {
                             District = grouped.Key,                                                       
                             GoatAndSheepPackage = grouped.Sum(g => g.lp.PackageName == "Goat Package" || g.lp.PackageName == "Sheep" ? 1 : 0),
                             SewingAndPekoMachine = grouped.Sum(g => g.lp.PackageName == "Sewing and Peko Machine" ? 1 : 0),
                             TuckShop = grouped.Sum(g => g.lp.PackageName == "Tuck Shop" ? 1 : 0),
                             TotalCount = grouped.Count()
                         }).ToList();
            ViewBag.LipBifurHostData = lipBifurHostData;


            List<LipBifurRefData> lipBifurRefData = (from lat in _context.LIPAssetTransfer
                         join lp in _context.LIPPackage on lat.LIPPackageId equals lp.LIPPackageId
                         where lat.IsAssetTransfer == true && (lat.IdentifiedById == 4)
                         group new { lat, lp } by lat.District into grouped
                         select new LipBifurRefData
                         {
                             District = grouped.Key,
                             SunBasedFruitDrying = grouped.Sum(g => g.lp.PackageName == "Sun Based Fruit Drying" ? 1 : 0),
                             BackyardKitchenGardening = grouped.Sum(g => g.lp.PackageName == "Backyard Kitchen Gardening" ? 1 : 0),
                             IDOPoultry = grouped.Sum(g => g.lp.PackageName == "IDO Poultry" ? 1 : 0),
                             TotalCount = grouped.Count()
                         }).ToList();
            ViewBag.LipBifurRefData = lipBifurRefData;

            

            //var currentUser = await _userManager.GetUserAsync(HttpContext.User); 
            //int TotalBeneficiaries = 0;
            //TotalBeneficiaries = _context.LIPAssetTransfer.Include(a=>a.Member).Count(a => a.IsAssetTransfer == true);
            //var m = _context.LIPAssetTransfer.Include(a=>a.Member).Count(a => a.IsAssetTransfer == true && a.District == "Mastung");
            //var z = _context.LIPAssetTransfer.Include(a=>a.Member).Count(a => a.IsAssetTransfer == true && a.District == "Zhob");
            //var p = _context.LIPAssetTransfer.Include(a=>a.Member).Count(a => a.IsAssetTransfer == true && a.District == "Pishin");
            //var n = _context.LIPAssetTransfer.Include(a=>a.Member).Count(a => a.IsAssetTransfer == true && a.District == "Nushki");
            //var c = _context.LIPAssetTransfer.Include(a=>a.Member).Count(a => a.IsAssetTransfer == true && a.District == "Chagai");
            //var s = _context.LIPAssetTransfer.Include(a=>a.Member).Count(a => a.IsAssetTransfer == true && a.District == "Sherani");
            //var ks = _context.LIPAssetTransfer.Include(a=>a.Member).Count(a => a.IsAssetTransfer == true && a.District == "Killa Saifullah");
            //var ka = _context.LIPAssetTransfer.Include(a=>a.Member).Count(a => a.IsAssetTransfer == true && a.District == "Killa Abdullah");
            //TotalBeneficiaries += _context.MemberTrainingDetail.Include(a => a.MemberTraining).Count(a => a.MemberTraining.IsVerified == true);
            //TotalBeneficiaries += _context.CommunityInstituteMember.Include(a => a.CommunityInstitution).Count(a => a.CommunityInstitution.IsVerified == true);
            //ViewBag.TotalBeneficiaries = TotalBeneficiaries;
            //int DirectFemaleBeneficiaries = 0;
            //DirectFemaleBeneficiaries += _context.LIPAssetTransfer.Include(a => a.Member).Count(a => a.IsAssetTransfer == true && a.Member.Gender == "Female");
            //ViewBag.DirectRefugeeBeneficiaries = _context.LIPAssetTransfer.Include(a => a.Member).Count(a => a.Member.BeneficiaryTypeId == 3 && a.IsAssetTransfer == true);
            //TotalBeneficiaries += _context.MemberTrainingDetail.Include(a => a.MemberTraining).Include(a=>a.member).Count(a => a.MemberTraining.IsVerified == true);
            //ViewBag.DirectFemaleBeneficiaries = DirectFemaleBeneficiaries;
            //ViewBag.CIEstablished = _context.CommunityInstitution.Count(a => a.IsVerified);
            //ViewBag.LIPs = _context.LIPAssetTransfer.Count(a => a.IsAssetTransfer == true);
            //ViewBag.BSF = (_context.BSFGov.Count(a => a.IsCompleted == true)) + (_context.BSFPrivate.Count(a => a.IsCompleted == true));
            //ViewBag.CICIGTraining = _context.MemberTraining.Count(a => a.IsVerified == true);
            return View();
        }
        public async Task<ActionResult> _Index(int index, string ind)
        {
            ViewBag.Index = index;
            var data = await _context.BeneficiarySummary.ToListAsync();
            return PartialView(data);
        }

        public async Task<IActionResult> Check()
        {
            ViewBag.Index = 2;
            var data = await _context.BeneficiarySummary.ToListAsync();           
            return View(data);
        }

        public async Task<ActionResult> _IndexLIPDetail(string ind)
        {
            ViewBag.District = ind;
            var data = await _context.LIPAssetTransfer.Include(a=>a.Member).Where(a=>a.IsAssetTransfer == true && a.District == ind).Select(a=> new LIPAssetTransfer { LIPAssetTransferId = a.LIPAssetTransferId, LIPCode = a.LIPCode, PictureAttachment1= a.LIPPackage.PackageName, PictureAttachment2 = a.Member.MemberName, PictureAttachment3 = a.Member.CNIC, PictureAttachment4 = a.Member.Gender, MemberId = a.Member.BeneficiaryTypeId}).ToListAsync();
            return PartialView(data);
        }
        public async Task<ActionResult> _IndexLIP(int index, string ind)
        {
            ViewBag.Index = index;
            var tmp = await _context.LIPAssetTransfer.Include(a=>a.Member).GroupBy(x => x.District)
                      .Select(y => new BeneficiarySummary{
                          DistrictName = y.Key,
                          MaleBeneficiary = y.Count(x => x.IsAssetTransfer == true && x.Member.Gender == "Male" && x.Member.BeneficiaryTypeId == 1),
                          FemaleBeneficiary = y.Count(x => x.IsAssetTransfer == true && x.Member.Gender == "Female" && x.Member.BeneficiaryTypeId == 1),
                          MaleRefugeeBeneficiary = y.Count(x => x.IsAssetTransfer == true && x.Member.Gender == "Female" && x.Member.BeneficiaryTypeId == 3),
                          FemaleRefugeeBeneficiary = y.Count(x => x.IsAssetTransfer == true && x.Member.Gender == "Female" && x.Member.BeneficiaryTypeId == 3)
                      }).ToListAsync();            
            return PartialView(tmp);
        }
        public ActionResult IntermediateResultFramework()
        {
            ViewBag.CIEstablished = _context.CommunityInstitution.Count(a => a.IsVerified);
            return View();
        }
        public IActionResult Index2()
        {
            return View();
        }

        public IActionResult DetailDashboard()
        {

            return View();
        }

        public IActionResult PreDashboard()
        {
            return View();
        }

        public IActionResult Graph(string year)
        {
            return PartialView();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}