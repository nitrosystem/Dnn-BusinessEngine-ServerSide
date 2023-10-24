using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_DefinedListItems")]
    [PrimaryKey("ItemID", AutoIncrement = false)]
    [Cacheable("BE_DefinedListItems_", CacheItemPriority.Default, 20)]
    [Scope("ListID")]
    public class DefinedListItemInfo
    {
        public Guid ItemID { get; set; }
        public Guid ListID { get; set; }
        public int ItemLevel { get; set; }
        public string ParentValue { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string Data { get; set; }
        public int ViewOrder { get; set; }
    }
}