using DotNetNuke.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_DashboardPages")]
    [PrimaryKey("PageID", AutoIncrement = false)]
    [Cacheable("BE_DashboardPages_", CacheItemPriority.Default, 20)]
    [Scope("DashboardID")]
    public class DashboardPageInfo
    {
        public Guid PageID { get; set; }
        public Guid DashboardID { get; set; }
        public Guid? ParentID { get; set; }
        public Guid? ExistingPageID { get; set; }
        public int PageType { get; set; }
        public string PageName { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsVisible { get; set; }
        public bool InheritPermissionFromDashboard { get; set; }
        public string AuthorizationViewPage { get; set; }
        public string Settings { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}