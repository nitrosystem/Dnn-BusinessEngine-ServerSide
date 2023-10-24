using AutoMapper;
using Newtonsoft.Json;
using NitroSystem.Dnn.BusinessEngine.Api.ViewModels;
using NitroSystem.Dnn.BusinessEngine.Common.TypeCasting;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using NitroSystem.Dnn.BusinessEngine.Framework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Mapping
{
    internal static class ServiceMapping
    {
        #region Service Type Mapping

        internal static IEnumerable<ServiceTypeViewModel> GetServiceTypesViewModel()
        {
            var serviceTypes = ServiceTypeRepository.Instance.GetServiceTypes();

            return GetServiceTypesViewModel(serviceTypes);
        }

        internal static IEnumerable<ServiceTypeViewModel> GetServiceTypesViewModel(IEnumerable<ServiceTypeView> serviceTypes)
        {
            var result = new List<ServiceTypeViewModel>();

            foreach (var objServiceTypeView in serviceTypes ?? Enumerable.Empty<ServiceTypeView>())
            {
                var serviceType = GetServiceTypeViewModel(objServiceTypeView);
                result.Add(serviceType);
            }

            return result;
        }

        internal static ServiceTypeViewModel GetServiceTypeViewModel(ServiceTypeView objServiceTypeView)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ServiceTypeView, ServiceTypeViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<ServiceTypeViewModel>(objServiceTypeView);

            return result;
        }

        #endregion

        #region Service Mapping

        internal static IEnumerable<ServiceViewModel> GetServicesViewModel(Guid scenarioID)
        {
            var services = ServiceRepository.Instance.GetServices(scenarioID);

            return GetServicesViewModel(services);
        }

        internal static IEnumerable<ServiceViewModel> GetServicesViewModel(IEnumerable<ServiceInfo> services)
        {
            var result = new List<ServiceViewModel>();

            if (services != null)
            {
                foreach (var objServiceInfo in services)
                {
                    var service = GetServiceViewModel(objServiceInfo);
                    result.Add(service);
                }
            }

            return result;
        }

        internal static ServiceViewModel GetServiceViewModel(Guid serviceID)
        {
            var objServiceInfo = ServiceRepository.Instance.GetService(serviceID);

            return GetServiceViewModel(objServiceInfo);
        }

        internal static ServiceViewModel GetServiceViewModel(ServiceInfo objServiceInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ServiceInfo, ServiceViewModel>()
                .ForMember(dest => dest.AuthorizationRunService, map => map.MapFrom(source => source.AuthorizationRunService.Split(',')))
                .ForMember(dest => dest.Params, map => map.MapFrom(source => ServiceParamRepository.Instance.GetParams(source.ServiceID)))
                .ForMember(dest => dest.Settings, map => map.MapFrom(source => TypeCastingUtil<IDictionary<string, object>>.TryJsonCasting(source.Settings)))
                .ForMember(dest => dest.ResultType, map => map.MapFrom(source => (ServiceResultType)source.ResultType));
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<ServiceViewModel>(objServiceInfo);

            return result;
        }

        #endregion
    }
}