using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_Providers")]
    [PrimaryKey("ProviderID", AutoIncrement = false)]
    [Cacheable("BE_Providers_", CacheItemPriority.Default, 20)]
    public class ProviderInfo
    {
        public Guid ProviderID { get; set; }
        public Guid ExtensionID { get; set; }
        public string ProviderType { get; set; }
        public string ProviderName { get; set; }
        public string Title { get; set; }
        public string ProviderComponent { get; set; }
        public string BusinessControllerClass { get; set; }
        public string Description { get; set; }
    }
}