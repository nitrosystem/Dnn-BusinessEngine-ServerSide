using AutoMapper;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Urls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Api.Dto;
using NitroSystem.Dnn.BusinessEngine.Api.ViewModels;
using NitroSystem.Dnn.BusinessEngine.Common;
using NitroSystem.Dnn.BusinessEngine.Common.TypeCasting;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Mapping
{
    internal static class ActionMapping
    {
        private const string ActionCachePrefix = "BE_Actions_";

        #region Action Type Mapping

        internal static IEnumerable<ActionTypeViewModel> GetActionTypesViewModel()
        {
            var actionTypes = ActionTypeRepository.Instance.GetActionTypes();

            return GetActionTypesViewModel(actionTypes);
        }

        internal static IEnumerable<ActionTypeViewModel> GetActionTypesViewModel(IEnumerable<ActionTypeView> actionTypes)
        {
            var result = new List<ActionTypeViewModel>();

            foreach (var objActionTypeView in actionTypes ?? Enumerable.Empty<ActionTypeView>())
            {
                var actionType = GetActionTypeViewModel(objActionTypeView);
                result.Add(actionType);
            }

            return result;
        }

        internal static ActionTypeViewModel GetActionTypeViewModel(ActionTypeView objActionTypeView)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ActionTypeView, ActionTypeViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<ActionTypeViewModel>(objActionTypeView);

            return result;
        }

        #endregion

        #region Action Mapping

        internal static IEnumerable<ActionViewModel> GetActionsViewModel(Guid moduleID, Guid? fieldID = null)
        {
            var actions = fieldID != null ? ActionRepository.Instance.GetFieldActions(fieldID.Value) : ActionRepository.Instance.GetModuleActions(moduleID);

            return GetActionsViewModel(actions);
        }

        internal static IEnumerable<ActionViewModel> GetFieldActionsViewModel(Guid fieldID)
        {
            var actions = ActionRepository.Instance.GetFieldActions(fieldID);

            return GetActionsViewModel(actions);
        }

        internal static IEnumerable<ActionViewModel> GetActionsViewModel(IEnumerable<ActionInfo> actions)
        {
            var result = new List<ActionViewModel>();

            foreach (var objActionInfo in actions)
            {
                var action = GetActionViewModel(objActionInfo);
                result.Add(action);
            }

            return result;
        }

        internal static ActionViewModel GetActionViewModel(Guid actionID)
        {
            var objActionInfo = ActionRepository.Instance.GetAction(actionID);

            return GetActionViewModel(objActionInfo);
        }

        internal static ActionViewModel GetActionViewModel(ActionInfo objActionInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ActionInfo, ActionViewModel>()
                .ForMember(dest => dest.Settings, map => map.MapFrom(source => TypeCastingUtil<IDictionary<string, object>>.TryJsonCasting(source.Settings)))
                .ForMember(dest => dest.Params, map => map.MapFrom(source => ActionParamRepository.Instance.GetParams(source.ActionID)))
                .ForMember(dest => dest.Conditions, map => map.MapFrom(source => ActionConditionRepository.Instance.GetConditions(source.ActionID)))
                .ForMember(dest => dest.FieldName, map => { map.PreCondition(source => source.FieldID != null); map.MapFrom(source => ModuleFieldRepository.Instance.GetFieldName(source.FieldID.Value)); })
                .ForMember(dest => dest.CreatedByUserDisplayName, map => map.MapFrom(source => UserUtil.GetUserDisplayName(source.CreatedByUserID)))
                .ForMember(dest => dest.LastModifiedByUserDisplayName, map => map.MapFrom(source => UserUtil.GetUserDisplayName(source.LastModifiedByUserID)))
                .ForMember(dest => dest.ActionTypeIcon, map => map.MapFrom(source => ActionTypeRepository.Instance.GetActionTypeIcon(source.ActionType)))
                .ForMember(dest => dest.Results, map => map.MapFrom(source => GetActionResulstViewModel(source.ActionID)));
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<ActionViewModel>(objActionInfo);

            return result;
        }

        internal static IEnumerable<ActionResultViewModel> GetActionResulstViewModel(Guid actionID)
        {
            var result = new List<ActionResultViewModel>();

            var items = ActionResultRepository.Instance.GetResults(actionID);
            foreach (var item in items)
            {
                result.Add(GetActionResultViewModel(item));
            }

            return result;
        }

        internal static ActionResultViewModel GetActionResultViewModel(ActionResultInfo objActionResultInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ActionResultInfo, ActionResultViewModel>()
                .ForMember(dest => dest.Conditions, map => map.MapFrom(source => TypeCastingUtil<IEnumerable<ExpressionInfo>>.TryJsonCasting(source.Conditions)));
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<ActionResultViewModel>(objActionResultInfo);

            return result;
        }

        internal static IEnumerable<ActionDto> GetActionsDTO(Guid moduleID)
        {
            string cacheKey = ActionCachePrefix + "DTO_" + moduleID;

            var result = DataCache.GetCache<List<ActionDto>>(cacheKey);
            if (result == null)
            {
                result = new List<ActionDto>();

                var allActions = ActionRepository.Instance.GetActions(moduleID);
                var actions = allActions.Where(a => !a.IsServerSide);
                actions = actions.Concat(allActions.Where(a => a.IsServerSide).Select(a => new ActionInfo
                {
                    ActionID = a.ActionID,
                    ModuleID = a.ModuleID,
                    FieldID = a.FieldID,
                    ParentID = a.ParentID,
                    ActionName = a.ActionName,
                    ActionType = a.ActionType,
                    Event = a.Event,
                    IsServerSide = a.IsServerSide,
                    DisableConditionForPreScript = a.DisableConditionForPreScript,
                    HasPreScript = a.HasPreScript,
                    HasPostScript = a.HasPostScript,
                    PreScript = a.PreScript,
                    PostScript = a.PostScript,
                    ParentResultStatus = a.ParentResultStatus,
                    ViewOrder = a.ViewOrder,
                }));

                foreach (var objActionInfo in actions)
                {
                    result.Add(GetActionDTO(objActionInfo));
                }

                DataCache.SetCache(cacheKey, result);
            }

            return result;
        }

        internal static ActionDto GetActionDTO(ActionInfo objActionInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ActionInfo, ActionDto>()
                .ForMember(dest => dest.Params, map => map.MapFrom(source => ActionParamRepository.Instance.GetParams(source.ActionID)))
                .ForMember(dest => dest.Conditions, map => map.MapFrom(source => ActionConditionRepository.Instance.GetConditions(source.ActionID)))
                .ForMember(dest => dest.Results, map => map.MapFrom(source => ActionResultRepository.Instance.GetResults(source.ActionID)));
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<ActionDto>(objActionInfo);

            return result;
        }

        #endregion
    }
}