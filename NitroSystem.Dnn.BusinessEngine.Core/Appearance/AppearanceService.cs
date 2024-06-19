using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Appearance
{
    public static class AppearanceService
    {
        public static ISkinTemplate GetSkinTemplate(string templateName, string moduleType, ModuleSkinInfo skinInfo)
        {
            switch (moduleType)
            {
                case "Form":
                    return (skinInfo.FormTemplates ?? Enumerable.Empty<SkinTemplateInfo>()).FirstOrDefault(t => t.TemplateName == templateName);
                case "List":
                    return (skinInfo.ListTemplates ?? Enumerable.Empty<SkinTemplateInfo>()).FirstOrDefault(t => t.TemplateName == templateName);
                case "Details":
                    return (skinInfo.DetailsTemplates ?? Enumerable.Empty<SkinTemplateInfo>()).FirstOrDefault(t => t.TemplateName == templateName);
                case "Dashboard":
                    return (skinInfo.DashboardTemplates ?? Enumerable.Empty<SkinTemplateInfo>()).FirstOrDefault(t => t.TemplateName == templateName);
                default: return null;
            }
        }
    }
}
