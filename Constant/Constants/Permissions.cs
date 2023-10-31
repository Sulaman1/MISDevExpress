using System.Collections.Generic;

namespace Constant.Constants
{
public static class Permissions
{
    public static List<string> GeneratePermissionsForModule(string module)
    {
        return new List<string>()
        {
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete",            
            $"Permissions.{module}.Verify",            
            $"Permissions.{module}.Review",            
            $"Permissions.{module}.AssetDistribution",            
            $"Permissions.{module}.InternalReview",            
            $"Permissions.{module}.UnSubmitted",            
            $"Permissions.{module}.SubmittedForReview",            
            $"Permissions.{module}.SubmittedForApproval",            
            $"Permissions.{module}.TransferAsset",            
            $"Permissions.{module}.AssetTransfferedList",            
            $"Permissions.{module}.Verified",            
            $"Permissions.{module}.InternalReview",            
            $"Permissions.{module}.Summary",                                               
        };
    }    
    public static class Administrator
    {
            public const string View = "Permissions.Administrator.View";
            public const string Create = "Permissions.Administrator.Create";
            public const string Edit = "Permissions.Administrator.Edit";
            public const string Delete = "Permissions.Administrator.Delete";
            public const string Verify = "Permissions.Administrator.Verify";
            public const string Review = "Permissions.Administrator.Review";
        }
        public static class HR
        {
            public const string View = "Permissions.HR.View";
            public const string Create = "Permissions.HR.Create";
            public const string Edit = "Permissions.HR.Edit";
            public const string Delete = "Permissions.HR.Delete";
        }
        public static class CICIG
        {
            //public const string View = "Permissions.CICIG.View";
            public const string Create = "Permissions.CICIG.Create";
            public const string Edit = "Permissions.CICIG.Edit";
            public const string Delete = "Permissions.CICIG.Delete";
            public const string Review = "Permissions.CICIG.Review";
            public const string Verify = "Permissions.CICIG.Verify";
            public const string SubmittedForReview = "Permissions.CICIG.SubmittedForReview";
            public const string SubmittedForApproval = "Permissions.CICIG.SubmittedForApproval";
            public const string Summary = "Permissions.CICIG.Summary";
            public const string Verified = "Permissions.CICIG.Verified";
            public const string Special = "Permissions.CICIG.Special";
        }
        public static class LIPTraining
        {
            public const string View = "Permissions.LIPTraining.View";
            public const string Create = "Permissions.LIPTraining.Create";
            public const string Edit = "Permissions.LIPTraining.Edit";
            public const string Delete = "Permissions.LIPTraining.Delete";
            public const string Review = "Permissions.LIPTraining.Review";            
            public const string Verify = "Permissions.LIPTraining.Verify";
        }
        public static class LIPAssetTransfer
        {
            public const string View = "Permissions.LIPAssetTransfer.View";
            public const string Create = "Permissions.LIPAssetTransfer.Create";
            public const string Edit = "Permissions.LIPAssetTransfer.Edit";
            public const string Delete = "Permissions.LIPAssetTransfer.Delete";
            public const string Review = "Permissions.LIPAssetTransfer.Review";
            public const string Verify = "Permissions.LIPAssetTransfer.Verify";
            public const string AssetDistribution = "Permissions.LIPAssetTransfer.AssetDistribution";
            public const string InternalReview = "Permissions.LIPAssetTransfer.InternalReview";
            public const string Unsubmitted = "Permissions.LIPAssetTransfer.Unsubmitted";
            public const string SubmittedForReview = "Permissions.LIPAssetTransfer.SubmittedForReview";
            public const string TransferAsset = "Permissions.LIPAssetTransfer.TransferAsset";            
            public const string AssetTransfferedList = "Permissions.LIPAssetTransfer.AssetTransfferedList";            
        }
        public static class BSFGovt
        {
            public const string View = "Permissions.BSFGovt.View";
            public const string Create = "Permissions.BSFGovt.Create";
            public const string Edit = "Permissions.BSFGovt.Edit";
            public const string Delete = "Permissions.BSFGovt.Delete";
            public const string Review = "Permissions.BSFGovt.Review";
            public const string Verify = "Permissions.BSFGovt.Verify";
        }
        public static class BSFPrivate
        {
            public const string View = "Permissions.BSFPrivate.View";
            public const string Create = "Permissions.BSFPrivate.Create";
            public const string Edit = "Permissions.BSFPrivate.Edit";
            public const string Delete = "Permissions.BSFPrivate.Delete";
            public const string Verify = "Permissions.BSFPrivate.Verify";
        }
        public static class HTS
        {
            public const string View = "Permissions.HTS.View";
            public const string Create = "Permissions.HTS.Create";
            public const string Edit = "Permissions.HTS.Edit";
            public const string Delete = "Permissions.HTS.Delete";            
            public const string Verify = "Permissions.HTS.Verify";
        }        
        public static class CICIGTraining
        {
            public const string View = "Permissions.CICIGTraining.View";
            public const string Create = "Permissions.CICIGTraining.Create";
            public const string Edit = "Permissions.CICIGTraining.Edit";
            public const string Delete = "Permissions.CICIGTraining.Delete";
            public const string Verify = "Permissions.CICIGTraining.Verify";
        }
        public static class GRM
        {
            public const string View = "Permissions.GRM.View";
            public const string Create = "Permissions.GRM.Create";
            public const string Edit = "Permissions.GRM.Edit";
            //public const string Delete = "Permissions.GRM.Delete";
            //public const string Verify = "Permissions.GRM.Verify";
        }
        public static class TVT
        {
            public const string View = "Permissions.TVT.View";
            public const string Create = "Permissions.TVT.Create";
            public const string Edit = "Permissions.TVT.Edit";
            public const string Delete = "Permissions.TVT.Delete";            
        }
        public static class IDO
        {
            public const string View = "Permissions.IDO.View";
            public const string Create = "Permissions.IDO.Create";
            public const string Edit = "Permissions.IDO.Edit";
            public const string Delete = "Permissions.IDO.Delete";
        }
        public static class MonitoringTool
        {
            public const string View = "Permissions.MonitoringTool.View";
            public const string Create = "Permissions.MonitoringTool.Create";
            public const string Edit = "Permissions.MonitoringTool.Edit";
            public const string Delete = "Permissions.MonitoringTool.Delete";
        }
    }
}