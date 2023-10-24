using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ModuleFields")]
    [PrimaryKey("FieldID", AutoIncrement = false)]
    [Cacheable("BE_ModuleFields_", CacheItemPriority.Default, 20)]
    [Scope("ModuleID")]
    public class ModuleFieldInfo
    {
        public Guid FieldID { get; set; }
        public Guid ModuleID { get; set; }
        public Guid? ParentID { get; set; }
        public string PaneName { get; set; }
        public string Template { get; set; }
        public string FieldType { get; set; }
        public string FieldName { get; set; }
        public string ViewModelProperty { get; set; }
        public string FieldText { get; set; }
        public bool IsGroup { get; set; }
        public bool IsRequired { get; set; }
        public bool IsShow { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsSelective { get; set; }
        public bool IsValuable { get; set; }
        public bool IsJsonValue { get; set; }
        public string ShowConditions { get; set; }
        public string EnableConditions { get; set; }
        public string FieldValues { get; set; }
        public string AuthorizationViewField { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}