using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_Dashboards")]
    [PrimaryKey("DashboardID", AutoIncrement = false)]
    [Cacheable("BE_Dashboards_", CacheItemPriority.Default, 20)]
    public class DashboardInfo
    {
        public Guid DashboardID { get; set; }
        public Guid ModuleID { get; set; }
        public int DashboardType { get; set; }
        public string AuthorizationViewDashboard { get; set; }
        public string UniqueName { get; set; }
    }
}