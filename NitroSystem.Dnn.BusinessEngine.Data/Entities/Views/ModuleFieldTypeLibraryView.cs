using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngineView_ModuleFieldTypeLibraries")]
    [PrimaryKey("ItemID", AutoIncrement = false)]
    [Cacheable("BE_ModuleFieldTypeLibraries_View_", CacheItemPriority.Default, 20)]
    [Scope("FieldType")]
    public class ModuleFieldTypeLibraryView
    {
        public Guid ItemID { get; set; }
        public Guid LibraryID { get; set; }
        public string LibraryName { get; set; }
        public string Logo { get; set; }
        public string Version { get; set; }
        public string FieldType { get; set; }
        public string ResourceType { get; set; }
        public string ResourcePath { get; set; }
        public int LoadOrder { get; set; }
    }
}