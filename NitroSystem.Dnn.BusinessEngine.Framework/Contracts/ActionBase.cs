using Newtonsoft.Json;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Framework.Contracts
{
    public abstract class ActionBase<T> : IAction
    {
        protected ActionDto Action { get; set; }
        protected IModuleData ModuleData { get; set; }
        protected IExpressionService ExpressionService { get; set; }
        protected IActionWorker ActionWorker { get; set; }
        protected IServiceWorker ServiceWorker { get; set; }
        protected virtual T Model { get; set; }

        public event EventHandler<ActionEventArgs> OnActionSuccessEvent;
        public event EventHandler<ActionEventArgs> OnActionErrorEvent;
        public event EventHandler<ActionEventArgs> OnActionCompletedEvent;

        public void Init(IActionWorker actionWorker, ActionDto action, IModuleData moduleData, IExpressionService expressionService, IServiceWorker serviceWorker)
        {
            this.ActionWorker = actionWorker;
            this.Action = action;
            this.ModuleData = moduleData;
            this.ExpressionService = expressionService;
            this.ServiceWorker = serviceWorker;

            try
            {
                if (!string.IsNullOrEmpty(action.Settings)) this.Model = JsonConvert.DeserializeObject<T>(action.Settings);
            }
            catch (Exception ex)
            {
                this.TryParseModel(action.Settings);
            }
        }

        public abstract bool TryParseModel(string actionDetails);

        public abstract Task<object> ExecuteAsync<T>(bool isServerSide);

        public void OnActionSuccess()
        {
            OnActionSuccessEvent?.Invoke(this, new ActionEventArgs());
        }

        public void OnActionError()
        {
            OnActionErrorEvent?.Invoke(this, new ActionEventArgs());
        }

        public void OnActionCompleted()
        {
            OnActionCompletedEvent?.Invoke(this, new ActionEventArgs());
        }
    }
}
