using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_Entities")]
    [PrimaryKey("EntityID", AutoIncrement = false)]
    [Cacheable("BE_Entities_", CacheItemPriority.Default, 20)]
    [Scope("ScenarioID")]
    public class EntityInfo
    {
        public Guid EntityID { get; set; }
        public Guid ScenarioID { get; set; }
        public Guid? DatabaseID { get; set; }
        [IgnoreColumn]
        public Guid? GroupID { get; set; }
        public string EntityType { get; set; }
        public string EntityName { get; set; }
        public string TableName { get; set; }
        public bool IsReadonly { get; set; }
        public bool IsMultipleColumnsForPK { get; set; }
        public string Settings { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}