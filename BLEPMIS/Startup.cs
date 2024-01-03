using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DAL.Models;
using BLEPMIS.Services;
using Constant.Permission;
using System;
using DBContext.Data;
using BAL.IRepository.MasterSetup;
using BAL.Services.MasterSetup;
using BAL.IRepository.MasterSetup.HR;
using BAL.IRepository.MasterSetup.Training;
using BAL.IRepository.MasterSetup.CD;
using BAL.Services.MasterSetup.HR;
using BAL.Services.MasterSetup.Training;
using BAL.Services.MasterSetup.CD;
using BAL.IRepository.MasterSetup.UserManagement;
using BAL.Services.MasterSetup.UserManagement;
using DevExpress.AspNetCore;
using System.IO;
using BLEPMIS.Services.Profile;

using DevExpress.AspNetCore.Reporting;
using DevExpress.Security.Resources;
using DevExpress.XtraReports.Web.Extensions;
using Microsoft.Extensions.Logging;
using BLEPMIS.Data;

//using WebDashboardDataSources;
//using DBContext.Data;
using DevExpress.AspNetCore.Internal;


namespace BLEPMIS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDevExpressControls();
            services.AddRazorPages();

            /**
               *Service for saving in file
             */
            services.AddScoped<ReportStorageWebExtension, CustomReportStorageFileWebExtension>();
            /**
               *Service for saving in database
             */
            //services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();

            services
            .AddMvc()
            .AddNewtonsoftJson();

            services.ConfigureReportingServices(configurator => {
                configurator.ConfigureReportDesigner(designerConfigurator => {
                    designerConfigurator.RegisterDataSourceWizardConfigFileConnectionStringsProvider();

                    // designerConfigurator.RegisterObjectDataSourceWizardTypeProvider<ObjectDataSourceWizardCustomTypeProvider>();
                    // designerConfigurator.RegisterObjectDataSourceConstructorFilterService<CustomObjectDataSourceConstructorFilterService>();
                });
                configurator.ConfigureWebDocumentViewer(viewerConfigurator => {
                    viewerConfigurator.UseCachedReportSourceBuilder();
                    viewerConfigurator.RegisterConnectionProviderFactory<CustomSqlDataConnectionProviderFactory>();
                    viewerConfigurator.RegisterEFContextProviderFactory<CustomEFContextProviderFactory>();
                });
                configurator.UseAsyncEngine();
            });
            services.AddDbContext<ReportDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("ReportsDataConnectionString")));
            //services.AddDbContext<OrdersContext>(options => options.UseSqlite(Configuration.GetConnectionString("NWindConnectionString")), ServiceLifetime.Transient);
            //services.AddDbContext<ReportDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Reporting")));

            ////////////////////////////////////////////////////////////////////////////////////////////////

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            string database1ConnectionString = configuration.GetConnectionString("DefaultConnection");
            string database2ConnectionString = configuration.GetConnectionString("DefaultConnectionYouth");

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Configure Database2Context
            services.AddDbContext<ApplicationDbContextYouth>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionYouth")));

            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));  

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ReportingLive")), ServiceLifetime.Transient);


            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 7;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();           
            services.AddControllersWithViews();
            string smtpServer = Configuration.GetSection("MailSettings:Host").Value;
            int.TryParse(Configuration.GetSection("MailSettings:Port").Value, out int port);            
            string password = Configuration.GetSection("MailSettings:Password").Value;
            string displayName = Configuration.GetSection("MailSettings:DisplayName").Value;
            string email = Configuration.GetSection("MailSettings:Mail").Value;

            EmailSender emailSender = new EmailSender(smtpServer, port, password, displayName, email);
            services.AddSingleton<IEmailSender>(emailSender);
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);//You can set Time   
            });
            services.AddTransient<IProvience, ProvienceService>();
            services.AddTransient<IDivision, DivisionService>();
            services.AddTransient<IDistrict, DistrictService>();
            services.AddTransient<ITehsil, TehsilService>();
            services.AddTransient<IUnionCouncil, UnionCouncilService>();
            services.AddTransient<ISection, SectionService>();
            services.AddTransient<IEmployee, EmployeeService>();
            services.AddTransient<IEmployeeContract, EmployeeContractService>();
            services.AddTransient<ITrainingHead, TrainingHeadService>();
            services.AddTransient<ITrainingType, TrainingTypeService>();
            services.AddTransient<ICommunityInstitution, CommunityInstitutionService>();
            services.AddTransient<ICommunityInstituteMember, CommunityInstituteMemberService>();
            services.AddTransient<IGeneralBusinessIdea, GeneralBusinessIdeaService>();
            services.AddTransient<IMember, MemberService>();         
            services.AddTransient<IVillage, VillageService>();         
            services.AddTransient<IUser, UserService>();         
            services.AddTransient<IRole, RoleService>();         
            services.AddTransient<IUserRole, UserRoleService>();         
            services.AddTransient<IPermission, PermissionService>();
            services.AddTransient<ProfileManager, ProfileManager>();
            services.ConfigureApplicationCookie(options => options.ExpireTimeSpan = TimeSpan.FromMinutes(60));            
            services.AddSwaggerGen();
            //services.AddDevExpressControls();
            // Use the AddMvc (or AddMvcCore) method to add MVC services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ReportDbContext db)
        {
            db.InitializeDatabase();
            var contentDirectoryAllowRule = DirectoryAccessRule.Allow(new DirectoryInfo(Path.Combine(env.ContentRootPath, "..", "Content")).FullName);
            AccessSettings.ReportingSpecificResources.TrySetRules(contentDirectoryAllowRule, UrlAccessRule.Allow());
            app.UseDevExpressControls();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "My API");

            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "Website",
                   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");               
            });
            //app.UseDevExpressControls();
        }
    }
}