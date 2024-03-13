using AutoMapper;
using DotNetNuke.Entities.Portals;
using Newtonsoft.Json;
using NitroSystem.Dnn.BusinessEngine.Api.ViewModels;
using NitroSystem.Dnn.BusinessEngine.Common.TypeCasting;
using NitroSystem.Dnn.BusinessEngine.Core.Appearance;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Mapping
{
    internal static class ModuleMapping
    {
        #region Module Mapping

        internal static IEnumerable<ModuleViewModel> GetModulesViewModel(Guid scenarioID)
        {
            var modules = ModuleRepository.Instance.GetModules(scenarioID);

            return GetModulesViewModel(modules);
        }

        internal static IEnumerable<ModuleViewModel> GetModulesViewModel(IEnumerable<ModuleInfo> modules)
        {
            var result = new List<ModuleViewModel>();

            foreach (var objFieldInfo in modules ?? Enumerable.Empty<ModuleInfo>())
            {
                var module = GetModuleViewModel(objFieldInfo);
                result.Add(module);
            }

            return result;
        }

        internal static ModuleViewModel GetModuleViewModel(Guid moduleID)
        {
            var objModuleInfo = ModuleRepository.Instance.GetModule(moduleID);

            return GetModuleViewModel(objModuleInfo);
        }

        internal static ModuleViewModel GetModuleViewModel(ModuleInfo objModuleInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ModuleInfo, ModuleViewModel>()
                .ForMember(dest => dest.Settings, map => map.MapFrom(source => TypeCastingUtil<IDictionary<string, object>>.TryJsonCasting(source.Settings)))
                .ForMember(dest => dest.ModuleSkin, map => { map.PreCondition(source => source != null && source.ModuleID != Guid.Empty); map.MapFrom(source => ModuleSkinManager.GetSkin(objModuleInfo.ModuleID, objModuleInfo.ModuleName, objModuleInfo.ParentID ,(objModuleInfo.ParentID != null ? ModuleRepository.Instance.GetModule(objModuleInfo.ParentID.Value).Skin : objModuleInfo.Skin)) ?? new ModuleSkinInfo()); });
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<ModuleViewModel>(objModuleInfo);

            if (result != null && result.ModuleBuilderType == "HtmlEditor")
            {
                var scenarioName = ScenarioRepository.Instance.GetScenarioName(result.ScenarioID);
                var ps = new PortalSettings(result.PortalID);

                result.CustomHtml = FileUtil.GetFileContent(string.Format("{0}/BusinessEngine/{1}/module--{2}/custom.html", ps.HomeSystemDirectoryMapPath, scenarioName, result.ModuleName));
                result.CustomJs = FileUtil.GetFileContent(string.Format("{0}/BusinessEngine/{1}/module--{2}/custom.js", ps.HomeSystemDirectoryMapPath, scenarioName, result.ModuleName));
                result.CustomCss = FileUtil.GetFileContent(string.Format("{0}/BusinessEngine/{1}/module--{2}/custom.css", ps.HomeSystemDirectoryMapPath, scenarioName, result.ModuleName));
            }

            return result;
        }

        #endregion
    }
}