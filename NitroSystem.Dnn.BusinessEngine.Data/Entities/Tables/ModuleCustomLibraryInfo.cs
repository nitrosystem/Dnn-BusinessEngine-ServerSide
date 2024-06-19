using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ModuleCustomLibraries")]
    [PrimaryKey("ItemID", AutoIncrement = false)]
    [Cacheable("BE_ModuleCustomLibraries_", CacheItemPriority.Default, 20)]
    [Scope("ModuleID")]
    public class ModuleCustomLibraryInfo
    {
        public Guid ItemID { get; set; }
        public Guid ModuleID { get; set; }
        public string LibraryName { get; set; }
        public string Version { get; set; }
        public string LocalPath { get; set; }
        public int LoadOrder { get; set; }

    }
}