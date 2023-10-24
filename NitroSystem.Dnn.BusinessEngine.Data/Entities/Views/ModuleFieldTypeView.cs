﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Views
{
    [TableName("BusinessEngineView_ModuleFieldTypes")]
    [PrimaryKey("TypeID", AutoIncrement = false)]
    [Cacheable("BE_FieldType_Views_", CacheItemPriority.Default, 20)]
    public class ModuleFieldTypeView
    {
        public Guid TypeID { get; set; }
        public Guid GroupID { get; set; }
        public string GroupName { get; set; }
        public string FieldType { get; set; }
        public string Title { get; set; }
        public string FieldComponent { get; set; }
        public string ComponentSubParams { get; set; }
        public string FieldJsPath { get; set; }
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
        public int GroupViewOrder { get; set; }
    }
}