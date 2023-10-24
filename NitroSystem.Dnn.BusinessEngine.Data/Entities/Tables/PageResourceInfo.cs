using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_PageResources")]
    [PrimaryKey("ResourceID", AutoIncrement = false)]
    [Cacheable("BE_PageResources_", CacheItemPriority.Default, 20)]
    [Scope("CmsPageID")]
    public class PageResourceInfo
    {
        public Guid ResourceID { get; set; }
        public Guid ModuleID { get; set; }
        public string CmsPageID { get; set; }
        public string ResourceType { get; set; }
        public bool? IsCustomResource { get; set; }
        public string FilePath { get; set; }
        public string Version { get; set; }
        public int LoadOrder { get; set; }
    }
}