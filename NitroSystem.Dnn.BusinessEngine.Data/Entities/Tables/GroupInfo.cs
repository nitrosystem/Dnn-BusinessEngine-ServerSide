using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_Groups")]
    [PrimaryKey("GroupID", AutoIncrement = false)]
    [Cacheable("BE_Groups_", CacheItemPriority.Default, 20)]
    [Scope("ScenarioID")]
    public class GroupInfo
    {
        public Guid GroupID { get; set; }
        public Guid? ScenarioID { get; set; }
        public string GroupType { get; set; }
        public string ObjectType { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public bool IsSystemGroup { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}