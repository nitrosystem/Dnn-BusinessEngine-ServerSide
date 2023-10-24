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
    [Scope("ObjectName")]
    public class LibraryResourceInfo
    {
        public Guid ResourceID { get; set; }
        public Guid ExtensionID { get; set; }
        public string ObjectType { get; set; }
        public string ObjectName { get; set; }
        public string LibraryName { get; set; }
        public string Title { get; set; }
        public string ResourceType { get; set; }
        public string ResourcePath { get; set; }
        public string Version { get; set; }
        public int LoadOrder { get; set; }
    }
}