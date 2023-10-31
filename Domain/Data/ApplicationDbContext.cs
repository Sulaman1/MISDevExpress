using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.MasterSetup;
using DAL.Models.Domain.Training;
using DAL.Models.Domain.ToolApp;

namespace DBContext.Data
{
    public class ApplicationDbContext : AuditableIdentityContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }        
        public DbSet<DAL.Models.Domain.MasterSetup.MemberAfghanDetail> MemberAfghanDetail { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.Provience> Provience { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.Division> Division { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.District> District { get; set; }                
        public DbSet<DAL.Models.Domain.MasterSetup.Tehsil> Tehsil { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.UnionCouncil> UnionCouncil { get; set; }               
        public DbSet<DAL.Models.Domain.MasterSetup.Village> Village { get; set; }               
        public DbSet<DAL.Models.Domain.MasterSetup.BLEPUserRole> BLEPUserRole { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.Section> Section { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.HRSection> HRSection { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.Member> Member { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.Designation> Designation { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.BeneficiarySummary> BeneficiarySummary { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.HRQualificationLevel> HRQualificationLevel { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.HRDesignation> HRDesignation { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.CommunityInstitution> CommunityInstitution { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.CommunityInstituteMember> CommunityInstituteMember { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.CommunityType> CommunityType { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.TrainingHead> TrainingHead { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.TrainingType> TrainingType { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.TrainingBy> TrainingBy { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.IdentifiedBy> IdentifiedBy { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.TVTTrainedBy> TVTTrainedBy { get; set; }
        public DbSet<DAL.Models.Domain.GRM.GrievanceRedressal> GrievanceRedressal { get; set; }
        public DbSet<DAL.Models.Domain.Training.MemberTraining> MemberTraining { get; set; }
        public DbSet<DAL.Models.Domain.Training.TVTTraining> TVTTraining { get; set; }
        public DbSet<DAL.Models.Domain.Training.TVTTrainingMember> TVTTrainingMember { get; set; }
        public DbSet<DAL.Models.Domain.Training.LIPTraining> LIPTraining { get; set; }
        public DbSet<DAL.Models.Domain.Training.LIPAssetTransfer> LIPAssetTransfer { get; set; }
        public DbSet<DAL.Models.Domain.Training.LIPTrainingDetail> LIPTrainingDetail { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.BeneficiaryType> BeneficiaryType { get; set; }
        public DbSet<DAL.Models.Domain.Training.MemberTrainingDetail> MemberTrainingDetail { get; set; }
        public DbSet<DAL.Models.ViewModels.CDSummary> CDSummary { get; set; }
        public DbSet<DAL.Models.ViewModels.SPToolAnalysis> SPToolAnalysis { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.GeneralBusinessIdea> GeneralBusinessIdea { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.LIPPackage> LIPPackage { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.ConstructionType> ConstructionType { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.BSFGovtPackage> BSFGovtPackage { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.Employee> Employee { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.HREmployee> HREmployee { get; set; }
        public DbSet<DAL.Models.Domain.MasterSetup.EmployeeContract> EmployeeContract { get; set; }
        public DbSet<DAL.Models.Domain.BSF.BSFGov> BSFGov { get; set; }
        public DbSet<DAL.Models.Domain.BSF.Stage> Stage { get; set; }
        public DbSet<DAL.Models.Domain.BSF.BSFDepartment> BSFDepartment { get; set; }
        public DbSet<DAL.Models.Domain.BSF.BSFGovStage> BSFGovStage { get; set; }
        public DbSet<DAL.Models.Domain.BSF.BSFPrivate> BSFPrivate { get; set; }
        public DbSet<DAL.Models.Domain.BSF.BSFPrivateStage> BSFPrivateStage { get; set; }

        public DbSet<DAL.Models.Domain.HTSModule.HTS> HTS { get; set; }
        public DbSet<DAL.Models.Domain.HTSModule.HTSStage> HTSStage { get; set; }

        public DbSet<DAL.Models.Domain.ToolApp.Tool> Tool { get; set; }
        public DbSet<DAL.Models.Domain.ToolApp.ToolModule> ToolModule { get; set; }
        public DbSet<DAL.Models.Domain.ToolApp.DropdownMenu> DropdownMenu { get; set; }
        public DbSet<DAL.Models.Domain.ToolApp.DropdownenuAccess> DropdownenuAccess { get; set; }
        public DbSet<DAL.Models.Domain.ToolApp.Control> Control { get; set; }
        public DbSet<DAL.Models.Domain.ToolApp.ToolUserAccess> ToolUserAccess { get; set; }
        public DbSet<DAL.Models.Domain.ToolApp.ToolControl> ToolControl { get; set; }
        public DbSet<DAL.Models.Domain.ToolApp.ToolControlDetail> ToolControlDetail { get; set; }
        public DbSet<DAL.Models.Domain.ToolApp.ToolDetail> ToolDetail { get; set; }
        public DbSet<DAL.Models.Domain.ToolApp.ToolFieldOption> ToolFieldOption { get; set; }
        public DbSet<DAL.Models.Domain.ToolApp.ToolAppPost.ToolControlInfoPost> ToolControlInfoPost { get; set; }
        public DbSet<DAL.Models.Domain.ToolApp.ToolAppPost.ToolInfoDetailPost> ToolInfoDetailPost { get; set; }
        public DbSet<DAL.Models.Domain.ToolApp.ToolAppPost.ToolModulePost> ToolModulePost { get; set; }
        public DbSet<DAL.Models.Domain.ToolApp.ToolAppPost.ToolInfoBasicPost> ToolInfoBasicPost { get; set; }
        public DbSet<DAL.Models.ViewModels.LipIDO> LipIDO { get; set; }
    }
    public class ApplicationDbContextYouth : AuditableIdentityContext
    {
        public ApplicationDbContextYouth(DbContextOptions<ApplicationDbContextYouth> options)
            : base(options)
        {
        }
        public DbSet<DAL.Models.Domain.MasterSetup.District> District { get; set; }
    }
}
