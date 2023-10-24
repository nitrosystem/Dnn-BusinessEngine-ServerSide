using NitroSystem.Dnn.BusinessEngine.Framework.Models;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using Dapper;
using DotNetNuke.Abstractions.Portals;

namespace NitroSystem.Dnn.BusinessEngine.Framework.Contracts
{
    public interface IService
    {
        void Init(IServiceWorker serviceWorker, ServiceDto service);

        Task<ServiceResult> ExecuteAsync<T>();
    }
}
