using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Views
{
    [TableName("BusinessEngineView_DashboardPageModules")]
    [PrimaryKey("PageModuleID", AutoIncrement = false)]
    [Cacheable("BE_DashboardPageModules_View_", CacheItemPriority.Default, 20)]
    [Scope("PageID")]
    public class DashboardPageModuleView
    {
        public Guid PageModuleID { get; set; }
        public Guid DashboardID { get; set; }
        public Guid PageID { get; set; }
        public Guid ParentID { get; set; }
        public Guid ModuleID { get; set; }
        public string PageName { get; set; }
        public string ModuleType { get; set; }
        public string ModuleBuilderType { get; set; }
        public string ModuleName { get; set; }
        public string ModuleTitle { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}