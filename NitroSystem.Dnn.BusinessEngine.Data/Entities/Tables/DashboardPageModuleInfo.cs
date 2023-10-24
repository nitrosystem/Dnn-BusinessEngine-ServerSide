using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_DashboardPageModules")]
    [PrimaryKey("PageModuleID", AutoIncrement = false)]
    [Cacheable("BE_DashboardPageModules_", CacheItemPriority.Default, 20)]
    [Scope("PageID")]
    public class DashboardPageModuleInfo
    {
        public Guid PageModuleID { get; set; }
        public Guid PageID { get; set; }
        public Guid ModuleID { get; set; }
        public int ViewOrder { get; set; }
    }
}