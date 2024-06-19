using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Core.Appearance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Contract
{
    public interface ISkinTemplate
    {
         string TemplateName { get; set; }
         string Title { get; set; }
         string TemplateImage { get; set; }
         string LayoutTemplatePath { get; set; }
         string LayoutCssPath { get; set; }
         string Description { get; set; }
         bool IsDashboardTemplate { get; set; }
    }
}
