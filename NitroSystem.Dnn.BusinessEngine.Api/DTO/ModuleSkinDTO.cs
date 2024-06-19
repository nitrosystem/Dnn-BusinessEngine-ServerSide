using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.DTO
{
    public class ModuleSkinDTO
    {
        public Guid ModuleID { get; set; }
        public string Skin { get; set; }
        public string Template { get; set; }
        public string Theme { get; set; }
        public bool EnableFieldsDefaultTemplate { get; set; }
        public bool EnableFieldsDefaultTheme { get; set; }
        public string FieldsDefaultTemplate { get; set; }
        public string FieldsDefaultTheme { get; set; }
        public string LayoutTemplate { get; set; }
        public string LayoutCss { get; set; }
    }
}
