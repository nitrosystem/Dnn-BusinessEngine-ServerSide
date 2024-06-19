using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Core.Appearance
{
    public class SkinTemplateInfo : ISkinTemplate
    {
        public string TemplateName { get; set; }
        public string Title { get; set; }
        public string TemplateImage { get; set; }
        public string LayoutTemplatePath { get; set; }
        public string LayoutCssPath { get; set; }
        public string Description { get; set; }
        public string DashboardType { get; set; }
        public bool IsDashboardTemplate { get; set; }
        public JObject BodyOptions { get; set; }
        public IEnumerable<SkinLibraryInfo> Libraries { get; set; }
        public string[] CssFiles { get; set; }
        public string[] JsFiles { get; set; }
    }
}