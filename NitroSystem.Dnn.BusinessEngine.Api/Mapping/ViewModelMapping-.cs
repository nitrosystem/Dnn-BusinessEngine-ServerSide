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
    internal static class ViewModelMapping
    {
        #region ViewModel Mapping

        internal static IEnumerable<ViewModelViewModel> GetViewModelsViewModel(Guid scenarioID)
        {
            var viewModels = ViewModelRepository.Instance.GetViewModels(scenarioID);

            return GetViewModelsViewModel(viewModels);
        }

        internal static IEnumerable<ViewModelViewModel> GetViewModelsViewModel(IEnumerable<ViewModelInfo> viewModels)
        {
            var result = new List<ViewModelViewModel>();

            if (viewModels != null)
            {
                foreach (var viewModel in viewModels)
                {
                    var viewModelDTO = GetViewModelViewModel(viewModel);
                    result.Add(viewModelDTO);
                }
            }

            return result;
        }

        internal static ViewModelViewModel GetViewModelViewModel(Guid viewModelID)
        {
            var objViewModelInfo = ViewModelRepository.Instance.GetViewModel(viewModelID);

            return GetViewModelViewModel(objViewModelInfo);
        }

        internal static ViewModelViewModel GetViewModelViewModel(ViewModelInfo objViewModelInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ViewModelInfo, ViewModelViewModel>()
                .ForMember(dest => dest.Properties, map => map.MapFrom(source => ViewModelPropertyRepository.Instance.GetProperties(source.ViewModelID)));
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<ViewModelViewModel>(objViewModelInfo);

            return result;
        }

        #endregion
    }
}