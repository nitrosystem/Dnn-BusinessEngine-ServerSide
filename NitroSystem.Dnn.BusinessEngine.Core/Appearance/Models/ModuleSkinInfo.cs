using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Core.Appearance
{
    public class ModuleSkinInfo
    {
        public string Title { get; set; }
        public string SkinPath { get; set; }
        public string[] BaseCssFiles { get; set; }
        public string[] BaseJsFiles { get; set; }
        public IEnumerable<SkinTemplateInfo> DashboardTemplates { get; set; }
        public IEnumerable<SkinTemplateInfo> FormTemplates { get; set; }
        public IEnumerable<SkinTemplateInfo> ListTemplates { get; set; }
        public IEnumerable<SkinTemplateInfo> DetailsTemplates { get; set; }
        public IEnumerable<FieldTypeInfo> FieldTypes { get; set; }
    }
}