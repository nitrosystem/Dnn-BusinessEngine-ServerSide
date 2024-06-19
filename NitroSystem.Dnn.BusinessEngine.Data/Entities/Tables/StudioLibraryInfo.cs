using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_StudioLibraries")]
    [PrimaryKey("ItemID", AutoIncrement = false)]
    [Cacheable("BE_StudioLibraries_", CacheItemPriority.Default, 20)]
    public class StudioLibraryInfo
    {
        public Guid ItemID { get; set; }
        public string LibraryName { get; set; }
        public string Version { get; set; }
        public int LoadOrder { get; set; }
    }
}