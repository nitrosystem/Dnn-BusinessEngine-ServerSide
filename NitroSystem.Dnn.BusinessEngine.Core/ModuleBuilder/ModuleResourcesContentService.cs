using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Core.ModuleBuilder
{
    public static class ModuleResourcesContentService
    {
        public static string GetModuleStyles(Guid moduleID)
        {
            StringBuilder styles = new StringBuilder();

            List<Guid> moduleIds = new List<Guid>() { moduleID };
            moduleIds.AddRange(ModuleRepository.Instance.GetModuleChildsID(moduleID));

            var moduleStyles = ModuleFieldRepository.Instance.GetModulesStyles(string.Join(",", moduleIds));
            foreach (var moduleStyle in moduleStyles)
            {
                styles.AppendLine(moduleStyle);

                styles.AppendLine(System.Environment.NewLine);
            }

            styles.AppendLine(System.Environment.NewLine);

            return styles.ToString();
        }

        public static string GetModuleFieldsThemeStyles(Guid moduleID)
        {
            StringBuilder styles = new StringBuilder();

            List<Guid> moduleIds = new List<Guid>() { moduleID };
            moduleIds.AddRange(ModuleRepository.Instance.GetModuleChildsID(moduleID));

            var cssFiles = ModuleFieldTypeThemeRepository.Instance.GetFieldsThemeCss(string.Join(",", moduleIds));
            foreach (var item in cssFiles)
            {
                string cssFilePath = item.Replace("[EXTPATH]", "~/DesktopModules/BusinessEngine/extensions");
                var fieldStyle = FileUtil.GetFileContent(HttpContext.Current.Server.MapPath(cssFilePath));

                if (!string.IsNullOrEmpty(fieldStyle))
                {
                    styles.AppendLine("/* --------------------------------------------------------");
                    styles.AppendLine(string.Format(" -----   {0}   -----", cssFilePath));
                    styles.AppendLine("-----------------------------------------------------------*/");

                    styles.AppendLine(fieldStyle);
                    styles.AppendLine(System.Environment.NewLine);
                }
            }

            styles.AppendLine(System.Environment.NewLine);

            return styles.ToString();
        }

        public static string GetModuleFieldsScripts(Guid moduleID)
        {
            StringBuilder scripts = new StringBuilder();

            List<Guid> moduleIds = new List<Guid>() { moduleID };
            moduleIds.AddRange(ModuleRepository.Instance.GetModuleChildsID(moduleID));

            var fieldTypes = ModuleFieldTypeRepository.Instance.GetFieldTypes();

            var moduleFieldTypes = ModuleFieldRepository.Instance.GetFieldTypes(string.Join(",", moduleIds));
            foreach (var fieldTypeName in moduleFieldTypes)
            {
                var fieldType = fieldTypes.FirstOrDefault(ft => ft.FieldType == fieldTypeName);
                if (fieldType != null)
                {
                    if (!string.IsNullOrEmpty(fieldType.FieldJsPath))
                    {
                        scripts.AppendLine("//Start Field Type : " + fieldTypeName);

                        string jsFilePath = fieldType.FieldJsPath.Replace("[EXTPATH]", "~/DesktopModules/BusinessEngine/extensions");
                        var fieldScript = FileUtil.GetFileContent(HttpContext.Current.Server.MapPath(jsFilePath));

                        scripts.AppendLine(fieldScript);

                        scripts.AppendLine("//End Field Type : " + fieldTypeName);
                        scripts.AppendLine(System.Environment.NewLine);
                    }

                    if (!string.IsNullOrEmpty(fieldType.DirectiveJsPath))
                    {
                        scripts.AppendLine("//Start Directive of Field Type : " + fieldTypeName);

                        string jsFilePath = fieldType.DirectiveJsPath.Replace("[EXTPATH]", "~/DesktopModules/BusinessEngine/extensions");
                        var fieldScript = FileUtil.GetFileContent(HttpContext.Current.Server.MapPath(jsFilePath));

                        scripts.AppendLine(fieldScript);

                        scripts.AppendLine("//End Directive of Field Type : " + fieldTypeName);
                        scripts.AppendLine(System.Environment.NewLine);
                    }
                }
            }

            scripts.AppendLine(System.Environment.NewLine);

            return scripts.ToString();
        }

        public static string GetModuleActionsScripts(Guid moduleID)
        {
            StringBuilder scripts = new StringBuilder();

            List<Guid> moduleIds = new List<Guid>() { moduleID };
            moduleIds.AddRange(ModuleRepository.Instance.GetModuleChildsID(moduleID));

            var actionTypes = ActionTypeRepository.Instance.GetActionTypes();

            var moduleActionTypes = ActionRepository.Instance.GetActionTypes(string.Join(",", moduleIds));
            foreach (var actionTypeName in moduleActionTypes)
            {
                var actionType = actionTypes.FirstOrDefault(ft => ft.ActionType == actionTypeName);
                if (actionType != null && !string.IsNullOrEmpty(actionType.ActionJsPath))
                {
                    scripts.AppendLine("//Start Action Type : " + actionTypeName);

                    string jsFilePath = actionType.ActionJsPath.Replace("[EXTPATH]", "~/DesktopModules/BusinessEngine/extensions");
                    var actionScript = FileUtil.GetFileContent(HttpContext.Current.Server.MapPath(jsFilePath));

                    scripts.AppendLine(actionScript);

                    scripts.AppendLine("//End Action Type : " + actionTypeName);
                    scripts.AppendLine(System.Environment.NewLine);
                }
            }

            scripts.AppendLine(System.Environment.NewLine);

            return scripts.ToString();
        }
    }
}
