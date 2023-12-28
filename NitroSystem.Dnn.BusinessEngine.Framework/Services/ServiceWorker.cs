using Dapper;
using DotNetNuke.Abstractions.Portals;
using DotNetNuke.Entities.Portals;
using Newtonsoft.Json;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Contracts;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Framework.Contracts;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using NitroSystem.Dnn.BusinessEngine.Framework.Mapping;
using NitroSystem.Dnn.BusinessEngine.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Framework.Services
{
    public class ServiceWorker : IServiceWorker
    {
        private readonly IModuleData _moduleData;
        private readonly IExpressionService _expressionService;

        public ServiceWorker(IModuleData moduleData, IExpressionService expressionService)
        {
            this._moduleData = moduleData;
            this._expressionService = expressionService;
        }

        public async Task<ServiceResult> RunServiceByAction<T>(ActionDto action)
        {
            return await RunService<T>(action.ServiceID.Value, action.Params);
        }

        public async Task<ServiceResult> RunService<T>(Guid serviceID, IEnumerable<IParamInfo> postedParams)
        {
            var service = ServiceMapping.GetServiceDTO(serviceID);

            if (service == null) throw new Exception(string.Format("Service : {0} is not found in the services", serviceID));

            ProcessServiceParams(service.Params, postedParams);

            var serviceController = CreateInstance(service.ServiceSubtype);
            serviceController.Init(this, service);

            var result = await serviceController.ExecuteAsync<T>();

            return result;
        }

        public IService CreateInstance(string serviceType)
        {
            var objServiceTypeInfo = ServiceTypeRepository.Instance.GetServiceTypeByName(serviceType);

            return ServiceLocator<IService>.CreateInstance(objServiceTypeInfo.BusinessControllerClass);
        }

        public DynamicParameters FillSqlParams(IEnumerable<IParamInfo> serviceParams)
        {
            var result = new DynamicParameters();

            foreach (var param in serviceParams ?? Enumerable.Empty<IParamInfo>())
            {
                result.Add(param.ParamName, param.ParamValue);
            }

            return result;
        }

        private void ProcessServiceParams(IEnumerable<IParamInfo> serviceParams, IEnumerable<IParamInfo> actionParams)
        {
            if (serviceParams == null || actionParams == null) return;

            foreach (var param in serviceParams ?? Enumerable.Empty<ServiceParamInfo>())
            {
                string paramType = (param.ParamType ?? string.Empty).ToLower();
                bool isString = paramType.StartsWith("nvarchar") || paramType.StartsWith("varchar") || paramType.StartsWith("char") || paramType.StartsWith("nchar") || paramType.StartsWith("text") || paramType.StartsWith("ntext");

                var findItem = actionParams.FirstOrDefault(a => a.ParamName == param.ParamName);
                if (findItem != null)
                {
                    if (findItem.ParamValue == null) continue;

                    string expression = findItem.ParamValue.ToString();
                    var value = this._expressionService.ParseExpression(expression, this._moduleData, new List<object>());
                    param.ParamValue = (1 == 1 || !isString) && string.IsNullOrEmpty(value) ? null : value;
                }
            }
        }
    }
}
