using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ModuleFieldTypeResources")]
    [PrimaryKey("ResourceID", AutoIncrement = false)]
    [Cacheable("BE_ModuleFieldTypeResources_", CacheItemPriority.Default, 20)]
    [Scope("FieldType")]
    public class ModuleFieldTypeResourceInfo
    {
        public Guid ResourceID { get; set; }
        public string FieldType { get; set; }
        public string TypeUseInFramework { get; set; }
        public string Title { get; set; }
        public bool IsCdn { get; set; }
        public string ResourceType { get; set; }
        public string ResourcePath { get; set; }
        public string Version { get; set; }
        public string LoadOrder { get; set; }
        public int ViewOrder { get; set; }
    }
}