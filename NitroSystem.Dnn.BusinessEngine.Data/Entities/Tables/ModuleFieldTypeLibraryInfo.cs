using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ModuleFieldTypeLibraries")]
    [PrimaryKey("ItemID", AutoIncrement = false)]
    [Cacheable("BE_ModuleFieldTypeLibraries_", CacheItemPriority.Default, 20)]
    [Scope("FieldType")]
    public class ModuleFieldTypeLibraryInfo
    {
        public Guid ItemID { get; set; }
        public Guid LibraryID { get; set; }
        public string FieldType { get; set; }
    }
}