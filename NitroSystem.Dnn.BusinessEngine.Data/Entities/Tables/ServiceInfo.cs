using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_Services")]
    [PrimaryKey("ServiceID", AutoIncrement = false)]
    [Cacheable("BE_Services_", CacheItemPriority.Default, 20)]
    [Scope("ScenarioID")]
    public class ServiceInfo
    {
        public Guid ServiceID { get; set; }
        public Guid ScenarioID { get; set; }
        public Guid? GroupID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceType { get; set; }
        public string ServiceSubtype { get; set; }
        public bool IsEnabled { get; set; }
        public bool HasResult { get; set; }
        public byte ResultType { get; set; }
        public string AuthorizationRunService { get; set; }
        public string Settings { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int Version { get; set; }
        public int ViewOrder { get; set; }
    }
}