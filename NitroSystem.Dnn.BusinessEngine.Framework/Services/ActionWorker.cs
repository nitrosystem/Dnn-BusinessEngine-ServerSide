using NitroSystem.Dnn.BusinessEngine.Framework.Contracts;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using NitroSystem.Dnn.BusinessEngine.Framework.Models;
using NitroSystem.Dnn.BusinessEngine.Framework.Services;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Framework.Mapping;
using System.Text.RegularExpressions;
using NitroSystem.Dnn.BusinessEngine.Framework.Enums;
using NitroSystem.Dnn.BusinessEngine.Common.TypeCasting;
using NitroSystem.Dnn.BusinessEngine.Core;
using System.Runtime.Remoting.Messaging;

namespace NitroSystem.Dnn.BusinessEngine.Framework.Services
{
    public class ActionWorker : IActionWorker
    {
        private readonly IModuleData _moduleData;
        private readonly IExpressionService _expressionService;
        private readonly IActionCondition _actionCondition;
        private readonly IServiceWorker _serviceWorker;

        public ActionWorker(IModuleData moduleData, IExpressionService expressionService, IActionCondition actionCondition, IServiceWorker serviceWorker)
        {
            this._moduleData = moduleData;
            this._expressionService = expressionService;
            this._actionCondition = actionCondition;
            this._serviceWorker = serviceWorker;
        }

        public ActionWorker()
        {
        }

        public async Task<object> CallActions(Guid moduleID, Guid? fieldID, string eventName)
        {
            var buffer = CreateBuffer(new Queue<ActionTree>(), moduleID, fieldID, eventName);

            if (buffer.Any())
                return await CallAction(buffer);
            else
                return null;
        }

        public async Task<object> CallAction(Guid actionID)
        {
            var action = ActionMapping.GetActionDTO(actionID);

            var buffer = action.IsServerSide && action.RunChildsInServerSide ? CreateBuffer(actionID) : new Queue<ActionTree>();
            var node = new ActionTree()
            {
                Action = action,
                CompletedActions = new Queue<ActionTree>(),
                SuccessActions = new Queue<ActionTree>(),
                ErrorActions = new Queue<ActionTree>()
            };

            buffer.Enqueue(node);

            return await CallAction(buffer);
        }

        public async Task<object> CallAction(Queue<ActionTree> buffer)
        {
            object result = null;

            var node = buffer.Dequeue();

            var action = node.Action;

            var checkConditions = action.Event == "OnPageLoad" || !action.CheckConditionsInClientSide;
            if (action != null && (!checkConditions || _actionCondition.IsTrueConditions(action.Conditions)))
            {
                ProccessActionParams(action.Params);

                IAction actionController = CreateInstance(action.ActionType);
                actionController.Init(this, action, this._moduleData, this._expressionService, this._serviceWorker);

                try
                {
                    result = await actionController.ExecuteAsync<object>(true);

                    var method = actionController.GetType().GetMethod("OnActionSuccessEvent");
                    if (method != null) method.Invoke(actionController, null);

                    if (node.SuccessActions.Any()) await CallAction(node.SuccessActions);
                }
                catch (Exception)
                {
                    var method = actionController.GetType().GetMethod("OnActionErrorEvent");
                    if (method != null) method.Invoke(actionController, null);

                    if (node.ErrorActions.Any()) await CallAction(node.ErrorActions);
                }
                finally
                {
                    var method = actionController.GetType().GetMethod("OnActionCompleted");
                    if (method != null) method.Invoke(actionController, null);

                    if (node.CompletedActions.Any()) await CallAction(node.CompletedActions);
                }

                if (buffer.Any())
                    return await CallAction(buffer);
                else
                    return result;
            }
            else
            {
                if (buffer.Any())
                    return await CallAction(buffer);
                else
                    return result;
            }
        }

        public IAction CreateInstance(string actionType)
        {
            var objActionTypeInfo = ActionTypeRepository.Instance.GetActionTypeByName(actionType);

            return ServiceLocator<IAction>.CreateInstance(objActionTypeInfo.BusinessControllerClass);
        }

        private Queue<ActionTree> CreateBuffer(Guid actionID)
        {
            var buffer = new Queue<ActionTree>();

            var action = ActionMapping.GetActionDTO(actionID);

            var node = new ActionTree()
            {
                Action = action,
                CompletedActions = new Queue<ActionTree>(),
                SuccessActions = new Queue<ActionTree>(),
                ErrorActions = new Queue<ActionTree>()
            };

            GetActionChilds(node.CompletedActions, action.ActionID, ActionResultStatus.OnCompleted);
            GetActionChilds(node.SuccessActions, action.ActionID, ActionResultStatus.OnCompletedSuccess);
            GetActionChilds(node.ErrorActions, action.ActionID, ActionResultStatus.OnCompletedError);

            buffer.Enqueue(node);

            return buffer;
        }

        private Queue<ActionTree> CreateBuffer(Queue<ActionTree> buffer, Guid moduleID, Guid? fieldID, string eventName)
        {
            bool isServerSide = eventName == "OnPageInit" ? false : true;

            var actions = ActionMapping.GetActionsDTO(moduleID, fieldID, eventName, isServerSide).OrderBy(a => a.ViewOrder);

            foreach (var action in actions ?? Enumerable.Empty<ActionDto>())
            {
                var node = new ActionTree()
                {
                    Action = action,
                    CompletedActions = new Queue<ActionTree>(),
                    SuccessActions = new Queue<ActionTree>(),
                    ErrorActions = new Queue<ActionTree>()
                };

                GetActionChilds(node.CompletedActions, action.ActionID, ActionResultStatus.OnCompleted);
                GetActionChilds(node.SuccessActions, action.ActionID, ActionResultStatus.OnCompletedSuccess);
                GetActionChilds(node.ErrorActions, action.ActionID, ActionResultStatus.OnCompletedError);

                buffer.Enqueue(node);
            }

            return buffer;
        }

        private Queue<ActionTree> GetActionChilds(Queue<ActionTree> buffer, Guid parentID, ActionResultStatus parentResultStatus)
        {
            var actions = ActionMapping.GetActionsDTO(parentID, parentResultStatus).OrderBy(a => a.ViewOrder);

            foreach (var action in actions ?? Enumerable.Empty<ActionDto>())
            {
                var node = new ActionTree()
                {
                    Action = action,
                    CompletedActions = new Queue<ActionTree>(),
                    SuccessActions = new Queue<ActionTree>(),
                    ErrorActions = new Queue<ActionTree>()
                };

                GetActionChilds(node.CompletedActions, action.ActionID, ActionResultStatus.OnCompleted);
                GetActionChilds(node.SuccessActions, action.ActionID, ActionResultStatus.OnCompletedSuccess);
                GetActionChilds(node.ErrorActions, action.ActionID, ActionResultStatus.OnCompletedError);

                buffer.Enqueue(node);
            }

            return buffer;
        }

        private void ProccessActionParams(IEnumerable<ActionParamInfo> actionParams)
        {
            foreach (var item in actionParams ?? Enumerable.Empty<ActionParamInfo>())
            {
                string expression = item.ParamValue != null ? item.ParamValue.ToString() : "";
                item.ParamValue = this._expressionService.ParseExpression(expression, this._moduleData, new List<object>(), false, item.ExpressionParsingType);
            }
        }

        public void SetActionResults(ActionDto action, object data)
        {
            bool isServiceBase = action.ServiceID != null;

            var results = ActionResultRepository.Instance.GetResults(action.ActionID);
            foreach (var item in results)
            {
                var conditions = Enumerable.Empty<ExpressionInfo>();
                if (!string.IsNullOrWhiteSpace(item.Conditions)) conditions = TypeCastingUtil<IEnumerable<ExpressionInfo>>.TryJsonCasting(item.Conditions);

                bool isTrue = this._actionCondition.IsTrueConditions(conditions);

                if (isTrue)
                {
                    object value = isServiceBase && data != null ? ProcessActionResultsToken(item.RightExpression, data, isServiceBase) : null;
                    if (value == null)
                        value = this._expressionService.ParseExpression(item.RightExpression, this._moduleData, new List<object>(), false, item.ExpressionParsingType);

                    this._moduleData.SetData(item.LeftExpression, value);
                }
            }
        }

        private object ProcessActionResultsToken(string expression, object data, bool isServiceBase)
        {
            object result = null;

            if (data == null) return result;

            ServiceResult serviceResult = isServiceBase ? (data as ServiceResult) : null;
            JObject serviceData = isServiceBase ? JObject.FromObject(serviceResult) : null;

            var match = Regex.Match(expression, @"^(?:_ServiceResult)\.?(.[^{}:\$,]+)?$");
            if (match.Success && match.Groups.Count == 2)
            {
                var propertyPath = match.Groups[1].Value;

                if (isServiceBase && serviceData != null)
                {
                    result = serviceData.SelectToken(propertyPath);
                }
            }

            return result;
        }
    }
}
