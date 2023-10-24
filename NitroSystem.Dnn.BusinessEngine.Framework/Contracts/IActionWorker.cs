using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using NitroSystem.Dnn.BusinessEngine.Framework.Models;
using NitroSystem.Dnn.BusinessEngine.Framework.Services;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Framework.Contracts
{
    public interface IActionWorker
    {
        Task<object> CallActions(Guid moduleID, Guid? fieldID, string eventName);

        Task<object> CallAction(Guid actionID);
        
        Task<object> CallAction(Queue<ActionTree> buffer);

        IAction CreateInstance(string actionType);

        void SetActionResults(ActionDto action, dynamic data);
    }
}
