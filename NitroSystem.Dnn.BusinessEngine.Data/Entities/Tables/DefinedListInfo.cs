using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_DefinedLists")]
    [PrimaryKey("ListID", AutoIncrement = false)]
    [Cacheable("BE_DefinedLists_", CacheItemPriority.Default, 20)]
    [Scope("ScenarioID")]
    public class DefinedListInfo
    {
        public Guid ListID { get; set; }
        public Guid? ScenarioID { get; set; }
        public Guid? ParentID { get; set; }
        public Guid? FieldID { get; set; }
        public string ListName { get; set; }
        public string ListType { get; set; }
        public bool IsMultiLevelList { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}