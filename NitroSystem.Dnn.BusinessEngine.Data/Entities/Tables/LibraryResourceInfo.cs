using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_LibraryResources")]
    [PrimaryKey("ResourceID", AutoIncrement = false)]
    [Cacheable("BE_LibraryResources_", CacheItemPriority.Default, 20)]
    [Scope("LibraryID")]
    public class LibraryResourceInfo
    {
        public Guid ResourceID { get; set; }
        public Guid LibraryID { get; set; }
        public string ResourceType { get; set; }
        public string ResourcePath { get; set; }
        public string ResourcePathRtl { get; set; }
        public int LoadOrder { get; set; }
    }
}