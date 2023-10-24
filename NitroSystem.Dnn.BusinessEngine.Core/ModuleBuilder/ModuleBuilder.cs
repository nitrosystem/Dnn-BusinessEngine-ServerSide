using DotNetNuke.Entities.Portals;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Core.Appearance;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;
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
    public static class ModuleBuilder
    {
        public static string GetModuleStyles(Guid moduleID)
        {
            StringBuilder styles = new StringBuilder();

            List<Guid> moduleIds = new List<Guid>() { moduleID };

            bool isParentModule = ModuleRepository.Instance.IsModuleParent(moduleID);
            if (isParentModule)
            {
                moduleIds.AddRange(ModuleRepository.Instance.GetModuleIdsByParentID(moduleID));
            }

            var moduleStyles = ModuleFieldRepository.Instance.GetModulesStyles(string.Join(",", moduleIds));
            foreach (var moduleStyle in moduleStyles)
            {
                styles.AppendLine(moduleStyle);

                styles.AppendLine(System.Environment.NewLine);
            }

            styles.AppendLine(System.Environment.NewLine);

            return styles.ToString();
        }

        public static string GetModuleFieldsStyles(Guid moduleID)
        {
            StringBuilder styles = new StringBuilder();

            List<Guid> moduleIds = new List<Guid>() { moduleID };

            bool isParentModule = ModuleRepository.Instance.IsModuleParent(moduleID);
            if (isParentModule)
            {
                moduleIds.AddRange(ModuleRepository.Instance.GetModuleIdsByParentID(moduleID));
            }

            var cssFiles = ModuleFieldRepository.Instance.GetModulesFieldsCss(string.Join(",", moduleIds));
            foreach (var item in cssFiles)
            {
                string cssFilePath = item.Replace("[EXTPATH]", "~/DesktopModules/BusinessEngine/extensions");
                var fieldStyle = FileUtil.GetFileContent(HttpContext.Current.Server.MapPath(cssFilePath));

                if (!string.IsNullOrEmpty(fieldStyle))
                {
                    styles.AppendLine("/* --------------------------------------------------------");
                    styles.AppendLine(string.Format("/* -----   {0}   -----", cssFilePath));
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

            bool isParentModule = ModuleRepository.Instance.IsModuleParent(moduleID);
            if (isParentModule)
            {
                moduleIds.AddRange(ModuleRepository.Instance.GetModuleIdsByParentID(moduleID));
            }

            var fieldTypes = ModuleFieldTypeRepository.Instance.GetFieldTypes();

            var moduleFieldTypes = ModuleFieldRepository.Instance.GetModulesFieldTypes(string.Join(",", moduleIds));
            foreach (var fieldTypeName in moduleFieldTypes)
            {
                var fieldType = fieldTypes.FirstOrDefault(ft => ft.FieldType == fieldTypeName);
                if (fieldType != null && !string.IsNullOrEmpty(fieldType.FieldJsPath))
                {
                    scripts.AppendLine("//Start Field Type : " + fieldTypeName);

                    string jsFilePath = fieldType.FieldJsPath.Replace("[EXTPATH]", "~/DesktopModules/BusinessEngine/extensions");
                    var fieldScript = FileUtil.GetFileContent(HttpContext.Current.Server.MapPath(jsFilePath));

                    scripts.AppendLine(fieldScript);

                    scripts.AppendLine("//End Field Type : " + fieldTypeName);
                    scripts.AppendLine(System.Environment.NewLine);
                }
            }

            scripts.AppendLine(System.Environment.NewLine);

            return scripts.ToString();
        }

        public static string GetModuleActionsScripts(Guid moduleID)
        {
            StringBuilder scripts = new StringBuilder();

            List<Guid> moduleIds = new List<Guid>() { moduleID };

            bool isParentModule = ModuleRepository.Instance.IsModuleParent(moduleID);
            if (isParentModule)
            {
                moduleIds.AddRange(ModuleRepository.Instance.GetModuleIdsByParentID(moduleID));
            }

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

        public static List<PageResourceInfo> GetModuleResources(ModuleView module)
        {
            var result = new List<PageResourceInfo>();

            var skin = ModuleSkinManager.GetSkin(module.Skin);
            if (skin != null)
            {

                foreach (var item in skin.BaseCssFiles ?? Enumerable.Empty<string>())
                {
                    result.Add(new PageResourceInfo()
                    {
                        ResourceType = "css",
                        FilePath = skin.SkinPath + "/" + item,
                        LoadOrder = result.Count
                    });
                }

                foreach (var item in skin.BaseJsFiles ?? Enumerable.Empty<string>())
                {
                    result.Add(new PageResourceInfo()
                    {
                        ResourceType = "js",
                        FilePath = skin.SkinPath + "/" + item,
                        LoadOrder = result.Count
                    });
                }

                if (module.ModuleType == "Dashboard")
                {
                    foreach (var template in skin.DashboardTemplates ?? Enumerable.Empty<SkinTemplateInfo>())
                    {
                        foreach (var item in template.CssFiles ?? Enumerable.Empty<string>())
                        {
                            result.Add(new PageResourceInfo()
                            {
                                ResourceType = "css",
                                FilePath = skin.SkinPath + "/" + item,
                                LoadOrder = result.Count
                            });
                        }

                        foreach (var item in template.JsFiles ?? Enumerable.Empty<string>())
                        {
                            result.Add(new PageResourceInfo()
                            {
                                ResourceType = "js",
                                FilePath = skin.SkinPath + "/" + item,
                                LoadOrder = result.Count
                            });
                        }
                    }
                }

                if (module.ModuleType == "Form")
                {
                    foreach (var template in skin.FormTemplates ?? Enumerable.Empty<SkinTemplateInfo>())
                    {
                        foreach (var item in template.CssFiles ?? Enumerable.Empty<string>())
                        {
                            result.Add(new PageResourceInfo()
                            {
                                ResourceType = "css",
                                FilePath = skin.SkinPath + "/" + item,
                                LoadOrder = result.Count
                            });
                        }

                        foreach (var item in template.JsFiles ?? Enumerable.Empty<string>())
                        {
                            result.Add(new PageResourceInfo()
                            {
                                ResourceType = "js",
                                FilePath = skin.SkinPath + "/" + item,
                                LoadOrder = result.Count
                            });
                        }
                    }
                }

                if (module.ModuleType == "List")
                {

                    foreach (var template in skin.ListTemplates ?? Enumerable.Empty<SkinTemplateInfo>())
                    {
                        foreach (var item in template.CssFiles ?? Enumerable.Empty<string>())
                        {
                            result.Add(new PageResourceInfo()
                            {
                                ResourceType = "css",
                                FilePath = skin.SkinPath + "/" + item,
                                LoadOrder = result.Count
                            });
                        }

                        foreach (var item in template.JsFiles ?? Enumerable.Empty<string>())
                        {
                            result.Add(new PageResourceInfo()
                            {
                                ResourceType = "js",
                                FilePath = skin.SkinPath + "/" + item,
                                LoadOrder = result.Count
                            });
                        }
                    }
                }
                if (module.ModuleType == "Details")
                {

                    foreach (var template in skin.DetailsTemplates ?? Enumerable.Empty<SkinTemplateInfo>())
                    {
                        foreach (var item in template.CssFiles ?? Enumerable.Empty<string>())
                        {
                            result.Add(new PageResourceInfo()
                            {
                                ResourceType = "css",
                                FilePath = skin.SkinPath + "/" + item,
                                LoadOrder = result.Count
                            });
                        }

                        foreach (var item in template.JsFiles ?? Enumerable.Empty<string>())
                        {
                            result.Add(new PageResourceInfo()
                            {
                                ResourceType = "js",
                                FilePath = skin.SkinPath + "/" + item,
                                LoadOrder = result.Count
                            });
                        }
                    }
                }
            }

            return result;
        }

        public static IEnumerable<PageResourceInfo> GetModuleFieldsLibraryResources(Guid moduleID, int? tabID)
        {
            var result = new List<LibraryResourceInfo>();

            List<Guid> moduleIds = new List<Guid>() { moduleID };

            bool isParentModule = ModuleRepository.Instance.IsModuleParent(moduleID);
            if (isParentModule)
            {
                moduleIds.AddRange(ModuleRepository.Instance.GetModuleIdsByParentID(moduleID));
            }

            var moduleFieldTypes = ModuleFieldRepository.Instance.GetModulesFieldTypes(string.Join(",", moduleIds));
            foreach (var fieldTypeName in moduleFieldTypes)
            {
                var resources = LibraryResourceRepository.Instance.GetLibraryResources(fieldTypeName);

                result.AddRange(resources);
            }

            return result.Select(item => new PageResourceInfo()
            {
                CmsPageID = tabID != null ? tabID.ToString() : null,
                ModuleID = moduleID,
                ResourceType = item.ResourceType,
                FilePath = item.ResourcePath,
                Version = item.Version != null ? item.Version.ToString() : "",
                LoadOrder = item.LoadOrder,
            });
        }
    }
}
