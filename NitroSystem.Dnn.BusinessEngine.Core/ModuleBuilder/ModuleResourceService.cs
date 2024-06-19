using DotNetNuke.Collections;
using DotNetNuke.Entities.Controllers;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Portals;
using Microsoft.JScript;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Core.Appearance;
using NitroSystem.Dnn.BusinessEngine.Core.Common;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace NitroSystem.Dnn.BusinessEngine.Core.ModuleBuilder
{
    public static class ModuleResourceService
    {
        #region Public Methods

        public static void UpdatePageResources(int tabID, PortalSettings portalSettings, bool isStandaloneDashboard, ProgressMonitoring monitoring)
        {
            var resources = new List<PageResourceInfo>();

            var modules = ModuleRepository.Instance.GetModulesByDnnTabID(tabID).OrderBy(m => m.ParentID);
            var moduleIds = modules.Select(m => m.ModuleID);
            foreach (var moduleID in moduleIds)
            {
                var module = ModuleRepository.Instance.GetModuleView(moduleID);

                if (isStandaloneDashboard && module.ModuleType != "Dashboard" && module.ParentID == null && module.DnnModuleID != null) continue;

                monitoring.Progress($"Loading module {module.ModuleName} ...", 50);

                //Get module base libraries resources
                if (module.ParentID == null)
                {
                    monitoring.Progress($"Get base framework & libraries for module {module.ModuleName}...", 55);

                    resources.AddRange(GetBaseFrameworksAndLibrariesResources(moduleID));

                    monitoring.Progress($"Get custom libraries for module {module.ModuleName}...", 60);

                    //Get module custom libraries & resources
                    var customLibraries = GetModuleCustomLibraryResources(moduleID);
                    foreach (var item in customLibraries)
                    {
                        resources.Add(new PageResourceInfo()
                        {
                            ModuleID = moduleID,
                            IsCustomResource = true,
                            LibraryName = item.LibraryName,
                            LibraryVersion = item.Version,
                            LibraryLogo = item.LibraryLogo,
                            ResourceType = item.ResourceType,
                            ResourcePath = item.ResourcePath
                        });
                    }

                    monitoring.Progress($"Get custom resources for module {module.ModuleName} ...", 65);

                    //Get module custom resources
                    var customResources = ModuleCustomResourceRepository.Instance.GetResources(moduleID);
                    foreach (var item in customResources)
                    {
                        resources.Add(new PageResourceInfo()
                        {
                            ModuleID = moduleID,
                            IsCustomResource = true,
                            ResourceType = item.ResourceType,
                            ResourcePath = item.ResourcePath
                        });
                    }
                }

                if (module.ModuleBuilderType == "HtmlEditor")
                {
                    if (module.ParentID == null)
                    {
                        monitoring.Progress($"Get Business Engine client app...", 70);

                        resources.AddRange(GetClientAppBaseResources(moduleID));
                    }

                    monitoring.Progress($"Create a custom-actions.js resource that include module actions...", 80);

                    //Update page resources for "Html Editor" module mode 
                    var moduleActionFile = string.Format("{0}/BusinessEngine/{1}/module--{2}/custom-actions.js", portalSettings.HomeSystemDirectory, module.ScenarioName, module.ModuleName);
                    resources.Add(new PageResourceInfo()
                    {
                        ModuleID = moduleID,
                        IsSystemResource = true,
                        ResourceType = "js",
                        ResourcePath = moduleActionFile,
                    });

                    monitoring.Progress($"Create custom css resource that writed by programmer & engineer in current scenario...", 85);

                    var moduleCustomCssFile = string.Format("{0}/BusinessEngine/{1}/module--{2}/custom.css", portalSettings.HomeSystemDirectory, module.ScenarioName, module.ModuleName);
                    resources.Add(new PageResourceInfo()
                    {
                        ModuleID = moduleID,
                        IsSystemResource = true,
                        ResourceType = "css",
                        ResourcePath = moduleCustomCssFile,
                    });

                    monitoring.Progress($"Create custom javascript resource that writed by programmer & engineer in current scenario...", 90);

                    var moduleCustomJsFile = string.Format("{0}/BusinessEngine/{1}/module--{2}/custom.js", portalSettings.HomeSystemDirectory, module.ScenarioName, module.ModuleName);
                    resources.Add(new PageResourceInfo()
                    {
                        ModuleID = moduleID,
                        IsSystemResource = true,
                        ResourceType = "js",
                        ResourcePath = moduleCustomJsFile,
                    });
                }
                else
                {
                    monitoring.Progress($"Get skin resource for module {module.ModuleName} & skin {module.Skin}...", 70);

                    //Get skin libraries & resources
                    resources.AddRange(GetSkinResources(module));

                    //Get client app resources
                    if (module.ParentID == null)
                    {
                        monitoring.Progress($"Get Business Engine client app...", 75);

                        resources.AddRange(GetClientAppBaseResources(moduleID));
                    }

                    monitoring.Progress($"Get custom theme resources used by module fields in skin {module.Skin}...", 75);

                    //Get skin themes that used by field and not in db themes list
                    resources.AddRange(GetSkinThemeThatUsedByField(moduleID));

                    monitoring.Progress($"Get custom resources used by module fields...", 80);

                    //Get used libraries for fields of the module 
                    resources.AddRange(GetModuleFieldsLibraryResources(moduleID));

                    if (module.ParentID == null)
                    {
                        monitoring.Progress($"Create css and js resources that writed by programmer & engineer in current scenario...", 90);

                        string moduleStylesFile = string.Format("{0}/BusinessEngine/{1}/module--{2}.css", portalSettings.HomeSystemDirectory, module.ScenarioName, module.ModuleName);
                        resources.Add(new PageResourceInfo()
                        {
                            ModuleID = moduleID,
                            IsSystemResource = true,
                            ResourceType = "css",
                            ResourcePath = moduleStylesFile
                        });

                        string moduleFieldsStylesFile = string.Format("{0}/BusinessEngine/{1}/module-fields--{2}.css", portalSettings.HomeSystemDirectory, module.ScenarioName, module.ModuleName);
                        resources.Add(new PageResourceInfo()
                        {
                            ModuleID = moduleID,
                            IsSystemResource = true,
                            ResourceType = "css",
                            ResourcePath = moduleFieldsStylesFile
                        });

                        string moduleScriptsFile = string.Format("{0}/BusinessEngine/{1}/module--{2}.js", portalSettings.HomeSystemDirectory, module.ScenarioName, module.ModuleName);
                        resources.Add(new PageResourceInfo()
                        {
                            ModuleID = moduleID,
                            IsSystemResource = true,
                            ResourceType = "js",
                            ResourcePath = moduleScriptsFile
                        });

                        string moduleActionFile = string.Format("{0}/BusinessEngine/{1}/module-action--{2}.js", portalSettings.HomeSystemDirectory, module.ScenarioName, module.ModuleName);
                        resources.Add(new PageResourceInfo()
                        {
                            ModuleID = moduleID,
                            IsSystemResource = true,
                            ResourceType = "js",
                            ResourcePath = moduleActionFile
                        });
                    }
                }
            }

            monitoring.Progress($"Delete old page resources in database and insert new page resources in database...", 100);

            PageResourceRepository.Instance.DeletePageResourcesByTabID(tabID);

            var submittedResources = new List<string>();
            var index = 0;
            foreach (var resource in resources.Where(r => !string.IsNullOrWhiteSpace(r.ResourcePath)))
            {
                string resourcePath = resource.ResourcePath.ToLower();

                if (submittedResources.Contains(resourcePath) == false)
                {
                    PageResourceRepository.Instance.AddPageResource(new PageResourceInfo()
                    {
                        CmsPageID = tabID,
                        ModuleID = resource.ModuleID,
                        IsBaseResource = resource.IsBaseResource,
                        IsSystemResource = resource.IsSystemResource,
                        IsCustomResource = resource.IsCustomResource,
                        IsSkinResource = resource.IsSkinResource,
                        FieldType = resource.FieldType,
                        LibraryName = resource.LibraryName,
                        LibraryVersion = resource.LibraryVersion,
                        LibraryLogo = resource.LibraryLogo,
                        ResourceType = resource.ResourceType,
                        ResourcePath = resource.ResourcePath,
                        IsActive = true,
                        LoadOrder = index++,
                    });
                }

                submittedResources.Add(resourcePath);
            }
        }

        #endregion

        #region Private Methods

        private static List<PageResourceInfo> GetBaseFrameworksAndLibrariesResources(Guid moduleID)
        {
            var result = new List<PageResourceInfo>();

            var angularLibraryResources = LibraryRepository.Instance.GetLibraryResources("angularjs", "1.8.2");
            angularLibraryResources.ForEach<LibraryView>(lr =>

                result.Add(new PageResourceInfo()
                {
                    ModuleID = moduleID,
                    IsBaseResource = true,
                    IsSystemResource = true,
                    LibraryName = lr.LibraryName,
                    LibraryVersion = lr.Version,
                    LibraryLogo = lr.LibraryLogo,
                    ResourceType = lr.ResourceType,
                    ResourcePath = lr.ResourcePath
                }));

            var lodashLibraryResources = LibraryRepository.Instance.GetLibraryResources("lodash", "4.17.21");
            lodashLibraryResources.ForEach<LibraryView>(lr =>

                result.Add(new PageResourceInfo()
                {
                    ModuleID = moduleID,
                    IsBaseResource = true,
                    IsSystemResource = true,
                    LibraryName = lr.LibraryName,
                    LibraryVersion = lr.Version,
                    LibraryLogo = lr.LibraryLogo,
                    ResourceType = lr.ResourceType,
                    ResourcePath = lr.ResourcePath
                }));

            return result;
        }

        private static List<PageResourceInfo> GetClientAppBaseResources(Guid moduleID)
        {
            var result = new List<PageResourceInfo>();

            var clientAppLibraryResources = LibraryRepository.Instance.GetLibraryResources("client-app", "1.0.0");
            clientAppLibraryResources.ForEach<LibraryView>(lr =>
                result.Add(new PageResourceInfo()
                {
                    ModuleID = moduleID,
                    IsBaseResource = true,
                    IsSystemResource = true,
                    LibraryName = lr.LibraryName,
                    LibraryVersion = lr.Version,
                    LibraryLogo = lr.LibraryLogo,
                    ResourceType = lr.ResourceType,
                    ResourcePath = lr.ResourcePath
                }));

            return result;
        }

        private static List<PageResourceInfo> GetSkinResources(ModuleView module)
        {
            var result = new List<PageResourceInfo>();

            var skin = ModuleSkinManager.GetSkin(module.ModuleID, module.ModuleType, module.ParentID, module.Skin);
            if (skin != null)
            {
                IEnumerable<SkinLibraryInfo> libraries = new List<SkinLibraryInfo>();

                if (module.ModuleType == "Dashboard")
                {
                    var template = skin.DashboardTemplates.FirstOrDefault(t => t.TemplateName == module.Template) ?? new SkinTemplateInfo();

                    //Get skin libraries & resources
                    libraries = template.Libraries ?? new List<SkinLibraryInfo>();
                    result.AddRange(GetSkinTemplateLibraryResources(module.ModuleID, libraries));

                    foreach (var item in template.CssFiles ?? Enumerable.Empty<string>())
                    {
                        result.Add(new PageResourceInfo()
                        {
                            ModuleID = module.ModuleID,
                            IsSystemResource = true,
                            ResourceType = "css",
                            ResourcePath = skin.SkinPath + "/" + item,
                        });
                    }

                    foreach (var item in template.JsFiles ?? Enumerable.Empty<string>())
                    {
                        result.Add(new PageResourceInfo()
                        {
                            ModuleID = module.ModuleID,
                            IsSystemResource = true,
                            ResourceType = "js",
                            ResourcePath = skin.SkinPath + "/" + item,
                        });
                    }
                }
                else if (module.ModuleType == "Form")
                {
                    var template = skin.FormTemplates.FirstOrDefault(t => t.TemplateName == module.Template) ?? new SkinTemplateInfo();

                    //Get skin libraries & resources
                    libraries = template.Libraries ?? new List<SkinLibraryInfo>();
                    result.AddRange(GetSkinTemplateLibraryResources(module.ModuleID, libraries));

                    foreach (var item in template.CssFiles ?? Enumerable.Empty<string>())
                    {
                        result.Add(new PageResourceInfo()
                        {
                            ModuleID = module.ModuleID,
                            IsBaseResource = true,
                            ResourceType = "css",
                            ResourcePath = skin.SkinPath + "/" + item,
                            IsActive = true
                        });
                    }

                    foreach (var item in template.JsFiles ?? Enumerable.Empty<string>())
                    {
                        result.Add(new PageResourceInfo()
                        {
                            ModuleID = module.ModuleID,
                            IsBaseResource = true,
                            ResourceType = "js",
                            ResourcePath = skin.SkinPath + "/" + item,
                            IsActive = true
                        });
                    }
                }
                else if (module.ModuleType == "List")
                {
                    var template = skin.ListTemplates.FirstOrDefault(t => t.TemplateName == module.Template) ?? new SkinTemplateInfo();

                    //Get skin libraries & resources
                    libraries = template.Libraries ?? new List<SkinLibraryInfo>();
                    result.AddRange(GetSkinTemplateLibraryResources(module.ModuleID, libraries));

                    foreach (var item in template.CssFiles ?? Enumerable.Empty<string>())
                    {
                        result.Add(new PageResourceInfo()
                        {
                            ModuleID = module.ModuleID,
                            IsBaseResource = true,
                            ResourceType = "css",
                            ResourcePath = skin.SkinPath + "/" + item,
                            IsActive = true
                        });
                    }

                    foreach (var item in template.JsFiles ?? Enumerable.Empty<string>())
                    {
                        result.Add(new PageResourceInfo()
                        {
                            ModuleID = module.ModuleID,
                            ResourceType = "js",
                            ResourcePath = skin.SkinPath + "/" + item,
                            LoadOrder = result.Count,
                            IsBaseResource = true,
                            IsActive = true
                        });
                    }
                }
                else if (module.ModuleType == "Details")
                {
                    var template = skin.DetailsTemplates.FirstOrDefault(t => t.TemplateName == module.Template) ?? new SkinTemplateInfo();

                    //Get skin libraries & resources
                    libraries = template.Libraries ?? new List<SkinLibraryInfo>();
                    result.AddRange(GetSkinTemplateLibraryResources(module.ModuleID, libraries));

                    foreach (var item in template.CssFiles ?? Enumerable.Empty<string>())
                    {
                        result.Add(new PageResourceInfo()
                        {
                            ResourceType = "css",
                            ResourcePath = skin.SkinPath + "/" + item,
                            LoadOrder = result.Count,
                            IsBaseResource = true,
                            IsActive = true
                        });
                    }

                    foreach (var item in template.JsFiles ?? Enumerable.Empty<string>())
                    {
                        result.Add(new PageResourceInfo()
                        {
                            ResourceType = "js",
                            ResourcePath = skin.SkinPath + "/" + item,
                            LoadOrder = result.Count,
                            IsBaseResource = true,
                            IsActive = true
                        });
                    }
                }

                //Add skin base resources
                foreach (var item in skin.BaseCssFiles ?? Enumerable.Empty<string>())
                {
                    result.Add(new PageResourceInfo()
                    {
                        ResourceType = "css",
                        ResourcePath = skin.SkinPath + "/" + item,
                        LoadOrder = result.Count,
                        IsBaseResource = true,
                        IsActive = true
                    });
                }

                foreach (var item in skin.BaseJsFiles ?? Enumerable.Empty<string>())
                {
                    result.Add(new PageResourceInfo()
                    {
                        ResourceType = "js",
                        ResourcePath = skin.SkinPath + "/" + item,
                        LoadOrder = result.Count,
                        IsBaseResource = true,
                        IsActive = true
                    });
                }
            }

            return result;
        }

        private static List<PageResourceInfo> GetSkinTemplateLibraryResources(Guid moduleID, IEnumerable<SkinLibraryInfo> libraries)
        {
            var result = new List<PageResourceInfo>();

            foreach (var library in libraries)
            {
                var resources = LibraryRepository.Instance.GetLibraryResources(library.LibraryName, library.Version);
                foreach (var item in resources ?? Enumerable.Empty<LibraryView>())
                {
                    result.Add(new PageResourceInfo()
                    {
                        ModuleID = moduleID,
                        IsSkinResource = true,
                        LibraryName = item.LibraryName,
                        LibraryVersion = item.Version,
                        LibraryLogo = item.LibraryLogo,
                        ResourceType = item.ResourceType,
                        ResourcePath = item.ResourcePath
                    });
                }
            }

            return result;
        }

        private static List<PageResourceInfo> GetSkinThemeThatUsedByField(Guid moduleID)
        {
            var result = new List<PageResourceInfo>();

            var fields = ModuleFieldTypeThemeRepository.Instance.GetFieldsUseSkinTheme(moduleID.ToString());
            foreach (var field in fields)
            {
                ModuleInfo objModuleInfo = ModuleRepository.Instance.GetModule(field.ModuleID);
                SkinInfo objSkinInfo = SkinRepository.Instance.GetSkin(objModuleInfo.Skin);

                var skin = ModuleSkinManager.GetSkin(objModuleInfo.ModuleID, objModuleInfo.ModuleType, objModuleInfo.ParentID, objSkinInfo);

                var cssFile = ((((skin.FieldTypes ?? Enumerable.Empty<FieldTypeInfo>()).FirstOrDefault(ft => ft.FieldType == field.FieldType) ?? new FieldTypeInfo()).Themes ?? Enumerable.Empty<FieldTypeThemeInfo>()).FirstOrDefault(t => t.ThemeName == field.Theme) ?? new FieldTypeThemeInfo()).ThemeCssPath;
                if (!string.IsNullOrWhiteSpace(cssFile))
                {
                    result.Add(new PageResourceInfo()
                    {
                        ModuleID = field.ModuleID,
                        ResourceType = "css",
                        ResourcePath = cssFile.Replace("[EXTPATH]", "/DesktopModules/BusinessEngine/extensions"),
                    });
                }
            }

            return result;
        }

        private static List<LibraryView> GetModuleCustomLibraryResources(Guid moduleID)
        {
            var result = new List<LibraryView>();

            var libraryies = ModuleCustomLibraryRepository.Instance.GetLibraries(moduleID);
            foreach (var library in libraryies)
            {
                result.AddRange(LibraryRepository.Instance.GetLibraryResources(library.LibraryName, library.Version));
            }

            return result;
        }

        private static IEnumerable<PageResourceInfo> GetModuleFieldsLibraryResources(Guid moduleID)
        {
            var result = new List<PageResourceInfo>();

            var moduleFieldTypes = ModuleFieldRepository.Instance.GetModulesFieldTypes(moduleID.ToString());
            foreach (var item in moduleFieldTypes)
            {
                var libraries = ModuleFieldTypeLibraryRepository.Instance.GetLibraries(item.FieldType);
                foreach (var library in libraries)
                {
                    var logo = LibraryRepository.Instance.GetLibraryLogo(library.LibraryName, library.Version);

                    var resources = LibraryRepository.Instance.GetLibraryResources(library.LibraryName, library.Version);
                    foreach (var resource in resources)
                    {
                        result.Add(new PageResourceInfo()
                        {
                            ModuleID = item.ModuleID,
                            FieldType = item.FieldType,
                            LibraryName = library.LibraryName,
                            LibraryVersion = library.Version,
                            LibraryLogo = logo,
                            ResourceType = resource.ResourceType,
                            ResourcePath = resource.ResourcePath,
                        });
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
