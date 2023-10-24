using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Views
{
    [TableName("BusinessEngineView_ServiceTypes")]
    [PrimaryKey("TypeID", AutoIncrement = false)]
    [Cacheable("BE_ServiceType_Views_", CacheItemPriority.Default, 20)]
    public class ServiceTypeView
    {
        public Guid TypeID { get; set; }
        public Guid GroupID { get; set; }
        public string GroupName { get; set; }
        public string ServiceType { get; set; }
        public string ServiceSubtype { get; set; }
        public string Title { get; set; }
        public string ServiceComponent { get; set; }
        public string ComponentSubParams { get; set; }
        public string BusinessControllerClass { get; set; }
        public bool HasResult { get; set; }
        public string ResultType { get; set; }
        public bool SubmitApi { get; set; }
        public string DefaultDatabaseObjectType { get; set; }
        public int IconType { get; set; }
        public string Icon { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public int GroupViewOrder { get; set; }
        public int ViewOrder { get; set; }
    }
}