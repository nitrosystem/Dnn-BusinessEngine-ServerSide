using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using NitroSystem.Dnn.BusinessEngine.Framework.Models;
using NitroSystem.Dnn.BusinessEngine.Framework.Services;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Contracts;

namespace NitroSystem.Dnn.BusinessEngine.Framework.Contracts
{
    public interface IServiceWorker
    {
        Task<ServiceResult> RunServiceByAction<T>(ActionDto action);

        Task<ServiceResult> RunService<T>(Guid serviceID, IEnumerable<IParamInfo> postedParams);

        IService CreateInstance(string serviceType);

        DynamicParameters FillSqlParams(IEnumerable<IParamInfo> postedParams);
    }
}
