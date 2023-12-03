using NitroSystem.Dnn.BusinessEngine.Api.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels
{
    public class ModuleFieldTypeViewModel
    {
        public Guid TypeID { get; set; }
        public Guid GroupID { get; set; }
        public string GroupName { get; set; }
        public string FieldType { get; set; }
        public string Title { get; set; }
        public string FieldComponent { get; set; }
        public string ComponentParams { get; set; }
        public string FieldJsPath { get; set; }
        public bool IsGroup { get; set; }
        public bool IsValuable { get; set; }
        public bool IsSelective { get; set; }
        public bool IsJsonValue { get; set; }
        public bool IsContent { get; set; }
        public object DefaultSettings { get; set; }
        public string ValidationPattern { get; set; }
        public int IconType { get; set; }
        public string Icon { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreateByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public int ViewOrder { get; set; }
        public int GroupViewOrder { get; set; }

        public IEnumerable<ModuleFieldTypeTemplateInfo> Templates { get; set; }
    }
}
