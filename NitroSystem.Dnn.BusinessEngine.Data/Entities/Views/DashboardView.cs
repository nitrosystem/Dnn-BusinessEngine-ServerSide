using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Views
{
    [TableName("BusinessEngineView_Dashboards")]
    [PrimaryKey("DashboardID", AutoIncrement = false)]
    [Cacheable("BE_Dashboards_View_", CacheItemPriority.Default, 20)]
    [Scope("ScenarioID")]
    public class DashboardView
    {
        public Guid DashboardID { get; set; }
        public Guid ModuleID { get; set; }
        public Guid ScenarioID { get; set; }
        public byte DashboardType { get; set; }
        public string UniqueName { get; set; }
        public string ModuleName { get; set; }
        public string ModuleTitle { get; set; }
        public string Skin { get; set; }
        public string Template { get; set; }
        public string Theme { get; set; }
        public int PortalID { get; set; }
        public int DnnModuleID { get; set; }
        public string AuthorizationViewDashboard { get; set; }
        public object Settings { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}