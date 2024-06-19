using DotNetNuke.ComponentModel.DataAnnotations;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Extensions.Manifest.PackageModels
{
    public class FieldTypeInfo
    {
        public string GroupName { get; set; }
        public string FieldType { get; set; }
        public string Title { get; set; }
        public string FieldComponent { get; set; }
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
        public string Icon { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public int ViewOrder { get; set; }
        public IEnumerable<ModuleFieldTypeTemplateInfo> Templates { get; set; }
        public IEnumerable<ModuleFieldTypeLibraryInfo> Libraries { get; set; }
    }
}
