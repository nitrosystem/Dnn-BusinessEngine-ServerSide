using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ModuleCustomResources")]
    [PrimaryKey("ItemID", AutoIncrement = false)]
    [Cacheable("BE_ModuleCustomResources_", CacheItemPriority.Default, 20)]
    [Scope("ModuleID")]
    public class ModuleCustomResourceInfo
    {
        public Guid ItemID { get; set; }
        public Guid ModuleID { get; set; }
        public string AddressType { get; set; }
        public string ResourceType { get; set; }
        public string ResourcePath { get; set; }
        public int LoadOrder { get; set; }

    }
}