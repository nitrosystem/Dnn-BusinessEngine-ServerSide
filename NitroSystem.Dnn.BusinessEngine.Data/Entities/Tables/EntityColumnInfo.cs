using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_EntityColumns")]
    [PrimaryKey("ColumnID", AutoIncrement = false)]
    [Cacheable("BE_EntityColumns_", CacheItemPriority.Default, 20)]
    [Scope("EntityID")]
    public class EntityColumnInfo
    {
        public Guid ColumnID { get; set; }
        public Guid EntityID { get; set; }
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public bool AllowNulls { get; set; }
        public string DefaultValue { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsUnique { get; set; }
        public bool IsComputedColumn { get; set; }
        public bool IsIdentity { get; set; }
        public string Formula { get; set; }
        public string Settings { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}