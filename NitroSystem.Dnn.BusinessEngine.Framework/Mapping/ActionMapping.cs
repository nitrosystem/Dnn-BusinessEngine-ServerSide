using AutoMapper;
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
    public class ActionMapping
    {
        public static IEnumerable<ActionDto> GetActionsDTO(Guid moduleID, Guid? fieldID, string eventName, bool? isServerSide = true)
        {
            var actions = ActionRepository.Instance.GetActions(moduleID, fieldID, eventName, isServerSide);

            return GetActionsDTO(actions);
        }

        public static IEnumerable<ActionDto> GetActionsDTO(Guid parentID, ActionResultStatus parentResultStatus, bool isServerSide = true)
        {
            var actions = ActionRepository.Instance.GetActions(parentID, (byte)parentResultStatus, isServerSide);

            return GetActionsDTO(actions);
        }

        public static IEnumerable<ActionDto> GetActionsDTO(IEnumerable<ActionInfo> actions)
        {
            var result = new List<ActionDto>();

            if (actions != null)
            {
                foreach (var action in actions)
                {
                    var actionDTO = GetActionDTO(action);
                    result.Add(actionDTO);
                }
            }

            return result;
        }

        public static ActionDto GetActionDTO(string actionName)
        {
            var objActionInfo = ActionRepository.Instance.GetAction(actionName);

            return GetActionDTO(objActionInfo);
        }

        public static ActionDto GetActionDTO(Guid actionID)
        {
            var objActionInfo = ActionRepository.Instance.GetAction(actionID);

            return GetActionDTO(objActionInfo);
        }

        public static ActionDto GetActionDTO(ActionInfo objActionInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ActionInfo, ActionDto>()
                .ForMember(dest => dest.Params, map => map.MapFrom(source => ActionParamRepository.Instance.GetParams(source.ActionID)))
                .ForMember(dest => dest.Conditions, map => map.MapFrom(source => ActionConditionRepository.Instance.GetConditions(source.ActionID)));
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<ActionDto>(objActionInfo);

            return result;
        }
    }
}
