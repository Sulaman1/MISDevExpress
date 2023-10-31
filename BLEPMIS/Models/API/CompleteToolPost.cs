using DAL.Models.Domain.ToolApp;
using DAL.Models.Domain.ToolApp.ToolAppPost;
using NuGet.Common;
using System;
using System.Collections.Generic;

namespace BLEPMIS.Models.API
{
    public class CompleteToolPost
    {
        public ToolInfo toolInfo { get; set; }       
        public List<ToolModulePost> toolModules { get; set; }
        public double Latitute { get; set; }
        public double Longitute { get; set; }
        public string CurrentDateTime { get; set; }
        public string Username { get; set; }
    }
}
