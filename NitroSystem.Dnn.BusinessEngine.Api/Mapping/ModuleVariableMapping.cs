using AutoMapper;
using Newtonsoft.Json;
using NitroSystem.Dnn.BusinessEngine.Api.ViewModels;
using NitroSystem.Dnn.BusinessEngine.Common.TypeCasting;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Mapping
{
    internal static class ModuleVariableMapping
    {
        #region Variable Mapping

        internal static IEnumerable<ModuleVariableViewModel> GetVariablesViewModel(Guid moduleGuid)
        {
            var variables = ModuleVariableRepository.Instance.GetVariables(moduleGuid);

            return GetVariablesViewModel(variables);
        }

        internal static IEnumerable<ModuleVariableViewModel> GetVariablesViewModel(IEnumerable<ModuleVariableInfo> variables)
        {
            var result = new List<ModuleVariableViewModel>();

            if (variables != null)
            {
                foreach (var variable in variables)
                {
                    var variableDTO = GetVariableViewModel(variable);
                    result.Add(variableDTO);
                }
            }

            return result;
        }

        internal static ModuleVariableViewModel GetVariableViewModel(ModuleVariableInfo objModuleVariableInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ModuleVariableInfo, ModuleVariableViewModel>()
                .ForMember(dest => dest.ViewModel, map => { map.PreCondition(source => (source.ViewModelID != null)); map.MapFrom(source => ViewModelMapping.GetViewModelViewModel(source.ViewModelID.Value)); });
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<ModuleVariableViewModel>(objModuleVariableInfo);

            return result;
        }

        #endregion
    }
}