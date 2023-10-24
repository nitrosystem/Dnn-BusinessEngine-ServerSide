using DotNetNuke.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ServiceTypes")]
    [PrimaryKey("TypeID", AutoIncrement = false)]
    [Cacheable("BE_ServiceTypes_", CacheItemPriority.Default, 20)]
    public class ServiceTypeInfo
    {
        public Guid TypeID { get; set; }
        public Guid ExtensionID { get; set; }
        public Guid GroupID { get; set; }
        public string ServiceType { get; set; }
        public string ServiceSubtype { get; set; }
        public string Title { get; set; }
        public string ServiceComponent { get; set; }
        public string BusinessControllerClass { get; set; }
        public bool HasResult { get; set; }
        public string ResultType { get; set; }
        public bool SubmitApi { get; set; }
        public string DefaultDatabaseObjectType { get; set; }
        public int IconType { get; set; }
        public string Icon { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public int ViewOrder { get; set; }

        [IgnoreColumn]
        public string GroupName { get; set; }
    }
}
