using NitroSystem.Dnn.BusinessEngine.Framework.Models;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;

namespace NitroSystem.Dnn.BusinessEngine.Framework.Contracts
{
    public interface IAction
    {
        void Init(IActionWorker actionWorker, ActionDto action, IModuleData moduleData, IExpressionService expressionService, IServiceWorker serviceWorker);

        Task<object> ExecuteAsync<T>(bool isServerSide);
    }
}
