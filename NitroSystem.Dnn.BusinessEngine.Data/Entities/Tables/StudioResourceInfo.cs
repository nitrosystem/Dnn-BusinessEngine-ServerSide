using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_StudioResources")]
    [PrimaryKey("ResourceID", AutoIncrement = false)]
    [Cacheable("BE_StudioResources_", CacheItemPriority.Default, 20)]
    [Scope("ExtensionID")]
    public class StudioResourceInfo
    {
        public Guid ResourceID { get; set; }
        public Guid? ExtensionID { get; set; }
        public string ResourceType { get; set; }
        public string FilePath { get; set; }
        public bool IsActive { get; set; }
        public bool IsDebug { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID{ get; set; }
        public int Priority { get; set; }
        public int LoadOrder { get; set; }
    }
}