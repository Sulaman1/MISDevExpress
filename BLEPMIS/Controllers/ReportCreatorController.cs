using BLEPMIS.Models;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.ReportDesigner;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BLEPMIS.Controllers
{
    public class ReportCreatorController : Controller
    {
        const string QueryBuilderHandlerUri = "/DXXQBMVC";
        const string ReportDesignerHandlerUri = "/DXXRDMVC";
        const string ReportViewerHandlerUri = "/DXXRDVMVC";
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DesignReport([FromServices] IReportDesignerClientSideModelGenerator clientSideModelGenerator, ReportingControlModel controlModel)
        {
            var model1 = new ReportDesignerModelWithDataSources
            {
                // Open your report here.
                Report = new XtraReport()
            };
            model1.DataSources = GetAvailableDataSources();


            //Models.CustomDesignerModel model = new Models.CustomDesignerModel();
            //var report = string.IsNullOrEmpty(controlModel.Id) ? new XtraReport() : null;
            //model.DesignerModel = CreateReportDesignerModel(clientSideModelGenerator, controlModel.Id, report);
            //model.Title = controlModel.Title;

            return View(model1);

            /**
             * 
            // Create a SQL data source.
            SqlDataSource dataSource = new SqlDataSource("Reporting");
            // SelectQuery query = SelectQueryFluentBuilder.AddTable("Products").SelectAllColumnsFromTable().Build("Products");
            // dataSource.Queries.Add(query);
            dataSource.RebuildResultSchema();

            //// Create a JSON data source.
            //JsonDataSource jsonDataSource = new JsonDataSource();
            //jsonDataSource.JsonSource = new UriJsonSource(
            //    new System.Uri("https://raw.githubusercontent.com/DevExpress-Examples/DataSources/master/JSON/customers.json"));
            //jsonDataSource.Fill();

            var model = new ReportDesignerModelWithDataSources
            {
                // Open your report here.
                Report = new SampleReport()
            };
            var dataSources = new Dictionary<string, object>();
            dataSources.Add("Reporting", dataSources);

            model.DataSources = dataSources;

             */

            //model.DataSources = new System.Collections.Generic.Dictionary<string, object> {
            //{ "Northwind", dataSource },
            //{ "JsonDataSource", jsonDataSource }  };     

            //return View(model);
        }


        public static ReportDesignerModel CreateReportDesignerModel(IReportDesignerClientSideModelGenerator clientSideModelGenerator, string reportName, XtraReport report)
        {
            var dataSources = GetAvailableDataSources();
            if (report != null)
            {
                return clientSideModelGenerator.GetModel(report, dataSources, ReportDesignerHandlerUri, ReportViewerHandlerUri, QueryBuilderHandlerUri);
            }
            return clientSideModelGenerator.GetModel(reportName, dataSources, ReportDesignerHandlerUri, ReportViewerHandlerUri, QueryBuilderHandlerUri);
        }
        public static Dictionary<string, object> GetAvailableDataSources()
        {
            var dataSources = new Dictionary<string, object>();
            // Create a SQL data source with the specified connection string.
            //SqlDataSource ds = new SqlDataSource("NWindConnectionString");
            SqlDataSource ds = new SqlDataSource("ReportingLive");
            //ds.ReplaceService(connectionProviderService, noThrow: true);
            // Create a SQL query to access the Products data table.
            //SelectQuery query = SelectQueryFluentBuilder.AddTable("Products").SelectAllColumnsFromTable().Build("Products");
            //ds.Queries.Add(query); 
            ds.RebuildResultSchema();
            dataSources.Add("ReportingLive", ds);
            return dataSources;
        }

    }
}
