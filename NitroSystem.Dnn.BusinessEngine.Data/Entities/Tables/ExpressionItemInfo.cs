using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_Expressions")]
    [PrimaryKey("ItemID", AutoIncrement = false)]
    [Cacheable("BE_Expressions_", CacheItemPriority.Default, 20)]
    [Scope("TypeID")]
    public class ExpressionItemInfo
    {
        public Guid ItemID { get; set; }
        public Guid TypeID { get; set; }
        public string Type { get; set; }
        public string LeftExpression { get; set; }
        public string EvalType { get; set; }
        public string RightExpression { get; set; }
        public string GroupName { get; set; }
    }
}