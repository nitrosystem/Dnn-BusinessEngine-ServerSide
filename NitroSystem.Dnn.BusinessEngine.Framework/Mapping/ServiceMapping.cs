using AutoMapper;
using NitroSystem.Dnn.BusinessEngine.Common.TypeCasting;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using NitroSystem.Dnn.BusinessEngine.Framework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Framework.Mapping
{
    internal class ServiceMapping
    {
        internal static ServiceDto GetServiceDTO(Guid serviceID)
        {
            var service = ServiceRepository.Instance.GetService(serviceID);

            return GetServiceDTO(service);
        }

        internal static ServiceDto GetServiceDTO(ServiceInfo objServiceInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ServiceInfo, ServiceDto>()
                .ForMember(dest => dest.Params, map => map.MapFrom(source => ServiceParamRepository.Instance.GetParams(source.ServiceID)))
                .ForMember(dest => dest.ResultType, map => map.MapFrom(source => (ServiceResultType)source.ResultType));
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<ServiceDto>(objServiceInfo);

            return result;
        }
    }
}
