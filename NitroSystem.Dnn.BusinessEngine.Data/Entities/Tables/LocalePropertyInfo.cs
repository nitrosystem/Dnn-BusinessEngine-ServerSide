using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_LocaleProperties")]
    [PrimaryKey("PropertyID", AutoIncrement = false)]
    [Cacheable("BE_LocaleProperties_", CacheItemPriority.Default, 20)]
    [Scope("ScenarioID")]
    public class LocalePropertyInfo
    {
        public Guid PropertyID { get; set; }
        public Guid ScenarioID { get; set; }
        public string GroupName { get; set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
    }
}