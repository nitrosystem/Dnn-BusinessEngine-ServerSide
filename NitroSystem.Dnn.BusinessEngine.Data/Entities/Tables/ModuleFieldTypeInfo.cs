using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ModuleFieldTypes")]
    [PrimaryKey("TypeID", AutoIncrement = false)]
    [Cacheable("BE_ModuleFieldTypes_", CacheItemPriority.Default, 20)]
    public class ModuleFieldTypeInfo
    {
        public Guid TypeID { get; set; }
        public Guid ExtensionID { get; set; }
        public Guid GroupID { get; set; }
        public string FieldType { get; set; }
        public string Title  { get; set; }
        public string FieldComponent { get; set; }
        public string ComponentSubParams { get; set; }
        public string FieldJsPath { get; set; }
        public string DirectiveJsPath { get; set; }
        public string CustomEvents { get; set; }
        public bool IsGroup { get; set; }
        public bool IsValuable { get; set; }
        public bool IsSelective { get; set; }
        public bool IsJsonValue { get; set; }
        public bool IsContent { get; set; }
        public object DefaultSettings { get; set; }
        public string ValidationPattern { get; set; }
        public string IconType { get; set; }
        public string Icon { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public int ViewOrder { get; set; }

        [IgnoreColumn]
        public string GroupName { get; set; }
    }
}