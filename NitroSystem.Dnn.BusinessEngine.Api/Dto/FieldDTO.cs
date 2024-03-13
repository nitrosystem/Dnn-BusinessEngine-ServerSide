using NitroSystem.Dnn.BusinessEngine.Api.Models;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Contracts;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Dto
{
    public class FieldDTO
    {
        public string FieldName { get; set; }
        public Guid FieldID { get; set; }
        public Guid? ParentID { get; set; }
        public string PaneName { get; set; }
        public string FieldType { get; set; }
        public string FieldText { get; set; }
        public bool IsGroup { get; set; }
        public bool IsRequired { get; set; }
        public bool IsShow { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsSelective { get; set; }
        public bool IsValuable { get; set; }
        public bool IsJsonValue { get; set; }
        public string Value { get; set; }
        public string ThemeCssClass { get; set; }
        public IEnumerable<IExpressionInfo> ShowConditions { get; set; }
        public IEnumerable<IExpressionInfo> EnableConditions { get; set; }
        public IEnumerable<FieldValueInfo> FieldValues { get; set; }
        public FieldDataSourceResult DataSource { get; set; }
        public IDictionary<string, object> Settings { get; set; }
    }
}
