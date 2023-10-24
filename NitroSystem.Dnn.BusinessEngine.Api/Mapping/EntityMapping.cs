using AutoMapper;
using Newtonsoft.Json;
using NitroSystem.Dnn.BusinessEngine.Api.ViewModels;
using NitroSystem.Dnn.BusinessEngine.Common.TypeCasting;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Mapping
{
    internal static class EntityMapping
    {
        #region Entity Mapping

        internal static IEnumerable<EntityViewModel> GetEntitiesViewModel(Guid scenarioID)
        {
            var entities = EntityRepository.Instance.GetEntities(scenarioID);

            return GetEntitiesViewModel(entities);
        }

        internal static IEnumerable<EntityViewModel> GetEntitiesViewModel(IEnumerable<EntityInfo> entities)
        {
            var result = new List<EntityViewModel>();

            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    var entityDTO = GetEntityViewModel(entity);
                    result.Add(entityDTO);
                }
            }

            return result;
        }

        internal static EntityViewModel GetEntityViewModel(Guid entityID)
        {
            var objEntityInfo = EntityRepository.Instance.GetEntity(entityID);

            return GetEntityViewModel(objEntityInfo);
        }

        internal static EntityViewModel GetEntityViewModel(EntityInfo objEntityInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntityInfo, EntityViewModel>()
                .ForMember(dest => dest.Settings, map => map.MapFrom(source => TypeCastingUtil<IDictionary<string, object>>.TryJsonCasting(source.Settings)))
                .ForMember(dest => dest.Columns, map => map.MapFrom(source => EntityColumnRepository.Instance.GetColumns(source.EntityID)));
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<EntityViewModel>(objEntityInfo);

            return result;
        }

        #endregion
    }
}