using System.Collections.Generic;

namespace BLEPMIS.Models.API
{
    public class ToolInfo
    {
        public int ToolId { get; set; }
        public string ToolName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int Counter { get; set; }
        public List<ToolControlInfo> ToolControlList { get; set; }
    }
    public class ToolControlInfo
    {
        public string ControlName { get; set; }
        public int ToolControlId { get; set; }
        public int ToolId { get; set; }
        public int OrderNo { get; set; }
        public string ControlValue { get; set; }
        public string ControlLebel { get; set; }
        public List<ToolInfoDetail> ToolDetailList { get; set; }
    }
    public class ToolInfoDetail
    {
        public int ToolControlDetailId { get; set; }
        public int ToolControlId { get; set; }
        public string Label { get; set; }
        public bool ControlDetailValue { get; set; }
    }
}
