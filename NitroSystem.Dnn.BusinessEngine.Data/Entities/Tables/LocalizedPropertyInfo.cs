using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_LocalizedProperties")]
    [PrimaryKey("ItemID")]
    [Cacheable("BE_LocalizedProperties_", CacheItemPriority.Default, 20)]
    [Scope("Language")]
    public class LocalizedPropertyInfo
    {
        public int ItemID { get; set; }
        public int EntityID { get; set; }
        public string Language { get; set; }
        public string LocaleKeyGroup { get; set; }
        public string LocaleKey { get; set; }
        public string LocaleValue { get; set; }
    }
}