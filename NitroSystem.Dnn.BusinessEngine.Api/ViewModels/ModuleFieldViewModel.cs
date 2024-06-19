using NitroSystem.Dnn.BusinessEngine.Api.Dto;
using NitroSystem.Dnn.BusinessEngine.Api.Models;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Contracts;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels
{
    public class ModuleFieldViewModel
    {
        public string FieldName { get; set; }
        public Guid FieldID { get; set; }
        public Guid ModuleID { get; set; }
        public Guid? ParentID { get; set; }
        public string PaneName { get; set; }
        public bool InheritTemplate { get; set; }
        public bool InheritTheme { get; set; }
        public string Template { get; set; }
        public string Theme { get; set; }
        public string ThemeCssClass { get; set; }
        public bool IsSkinTemplate { get; set; }
        public bool IsSkinTheme { get; set; }
        public string FieldType { get; set; }
        public string FieldText { get; set; }
        public bool IsGroup { get; set; }
        public bool IsRequired { get; set; }
        public bool IsShow { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsSelective { get; set; }
        public bool IsValuable { get; set; }
        public bool IsJsonValue { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
        public string Value { get; set; }
        public IEnumerable<string> AuthorizationViewField { get; set; }
        public IEnumerable<ExpressionInfo> ShowConditions { get; set; }
        public IEnumerable<ExpressionInfo> EnableConditions { get; set; }
        public IEnumerable<FieldValueInfo> FieldValues { get; set; }
        public IEnumerable<ActionViewModel> Actions { get; set; }
        public FieldDataSourceInfo DataSource { get; set; }
        public IDictionary<string, object> Settings { get; set; }
    }
}