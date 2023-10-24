using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Views
{
    [TableName("BusinessEngineView_ExplorerItems")]
    [PrimaryKey("ItemID", AutoIncrement = false)]
    [Scope("ScenarioID")]
    public class ExplorerItemView
    {
        public Guid ItemID { get; set; }
        public Guid? GroupID { get; set; }
        public Guid ScenarioID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public Guid? ParentID { get; set; }
        public Guid? DashboardPageParentID { get; set; }
        public int ViewOrder { get; set; }
    }
}