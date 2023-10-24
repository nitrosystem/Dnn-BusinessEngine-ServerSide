using AutoMapper;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Api.Dto;
using NitroSystem.Dnn.BusinessEngine.Api.Enums;
using NitroSystem.Dnn.BusinessEngine.Api.Models;
using NitroSystem.Dnn.BusinessEngine.Api.ViewModels;
using NitroSystem.Dnn.BusinessEngine.Common.TypeCasting;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using NitroSystem.Dnn.BusinessEngine.Core.Dto;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Core.ModuleBuilder;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Framework.Contracts;
using NitroSystem.Dnn.BusinessEngine.Framework.Models;
using NitroSystem.Dnn.BusinessEngine.Framework.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Mapping
{
    internal static class ModuleFieldMappings
    {
        private const string FieldCachePrefix = "BE_ModuleFields_";

        #region Field Type Mapping

        internal static IEnumerable<ModuleFieldTypeViewModel> GetFieldTypesViewModel()
        {
            var fieldTypes = ModuleFieldTypeRepository.Instance.GetFieldTypes();

            return GetFieldTypesViewModel(fieldTypes);
        }

        internal static IEnumerable<ModuleFieldTypeViewModel> GetFieldTypesViewModel(IEnumerable<ModuleFieldTypeView> fieldTypes)
        {
            var result = new List<ModuleFieldTypeViewModel>();

            foreach (var objFieldTypeView in fieldTypes ?? Enumerable.Empty<ModuleFieldTypeView>())
            {
                var actionType = GetFieldTypeViewModel(objFieldTypeView);
                result.Add(actionType);
            }

            return result;
        }

        internal static ModuleFieldTypeViewModel GetFieldTypeViewModel(ModuleFieldTypeView objFieldTypeView)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ModuleFieldTypeView, ModuleFieldTypeViewModel>()
                .ForMember(dest => dest.Templates, map => map.MapFrom(source => ModuleFieldTypeTemplateRepository.Instance.GetTemplates(source.FieldType)));
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<ModuleFieldTypeViewModel>(objFieldTypeView);

            return result;
        }

        #endregion

        #region Field Mapping

        #region Field View Model

        internal static IEnumerable<ModuleFieldViewModel> GetFieldsViewModel(Guid moduleID, IServiceWorker serviceWorker = null, IModuleData moduleData = null, UserInfo userInfo = null)
        {
            string cacheKey = FieldCachePrefix + "ViewModel_" + moduleID;

            var result = DataCache.GetCache<IEnumerable<ModuleFieldViewModel>>(cacheKey);
            if (result == null)
            {
                var fields = ModuleFieldRepository.Instance.GetFields(moduleID);

                result = GetFieldsViewModel(fields, serviceWorker, moduleData, userInfo);

                DataCache.SetCache(cacheKey, result);
            }

            return result;
        }

        internal static ModuleFieldViewModel GetFieldViewModel(Guid fieldID, IServiceWorker serviceWorker = null, IModuleData moduleData = null, UserInfo userInfo = null)
        {
            var field = ModuleFieldRepository.Instance.GetField(fieldID);

            var result = GetFieldViewModel(field, serviceWorker, moduleData);

            return result;
        }

        internal static IEnumerable<ModuleFieldViewModel> GetFieldsViewModel(IEnumerable<ModuleFieldInfo> fields, IServiceWorker serviceWorker = null, IModuleData moduleData = null, UserInfo userInfo = null)
        {
            var result = new List<ModuleFieldViewModel>();

            foreach (var objFieldInfo in fields ?? Enumerable.Empty<ModuleFieldInfo>())
            {
                var roles = userInfo == null || userInfo.IsSuperUser || string.IsNullOrEmpty(objFieldInfo.AuthorizationViewField) ? null : objFieldInfo.AuthorizationViewField.Split(',');
                if (roles == null || userInfo.Roles == null || roles.Contains("All Users") || roles.Any(r => userInfo.Roles.Contains(r)))
                {
                    var field = GetFieldViewModel(objFieldInfo, serviceWorker, moduleData);

                    result.Add(field);
                }
            }

            return result;
        }

        internal static ModuleFieldViewModel GetFieldViewModel(ModuleFieldInfo objModuleFieldInfo, IServiceWorker serviceWorker = null, IModuleData moduleData = null)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ModuleFieldInfo, ModuleFieldViewModel>()
                .ForMember(dest => dest.AuthorizationViewField, map => map.MapFrom(source => source.AuthorizationViewField.Split(',')))
                .ForMember(dest => dest.ShowConditions, map => map.MapFrom(source => TypeCastingUtil<IEnumerable<ExpressionInfo>>.TryJsonCasting(source.ShowConditions)))
                .ForMember(dest => dest.EnableConditions, map => map.MapFrom(source => TypeCastingUtil<IEnumerable<ExpressionInfo>>.TryJsonCasting(source.EnableConditions)))
                .ForMember(dest => dest.FieldValues, map => map.MapFrom(source => TypeCastingUtil<IEnumerable<FieldValueInfo>>.TryJsonCasting(source.FieldValues)))
                .ForMember(dest => dest.Settings, map => map.MapFrom(source => ModuleFieldSettingRepository.Instance.GetFieldSettings(source.FieldID).ToDictionary(item => item.SettingName, item => TypeCastingUtil<object>.TryJsonCasting(item.SettingValue)) ?? new Dictionary<string, object>()))
                .ForMember(dest => dest.Actions, map => map.MapFrom(source => ActionMapping.GetActionsViewModel(source.ModuleID, source.FieldID)));
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<ModuleFieldViewModel>(objModuleFieldInfo);

            if (result.IsSelective && result.Settings != null && result.Settings.ContainsKey("DataSource"))
            {
                result.DataSource = GetFieldDataSourceItems(result.Settings["DataSource"].ToString(), serviceWorker, true);
            }

            return result;
        }

        #endregion

        #region Field DTO

        internal static IEnumerable<FieldDTO> GetFieldsDTO(Guid moduleID, IServiceWorker serviceWorker, UserInfo userInfo = null)
        {
            //string cacheKey = FieldCachePrefix + "DTO_" + moduleID;

            //var result = DataCache.GetCache<IEnumerable<FieldDTO>>(cacheKey);
            //if (result == null)
            // {
            var fields = ModuleFieldRepository.Instance.GetFields(moduleID);

            var result = GetFieldsDTO(fields, serviceWorker, userInfo);

            //DataCache.SetCache(cacheKey, result);
            //}

            return result;
        }

        internal static IEnumerable<FieldDTO> GetFieldsDTO(IEnumerable<ModuleFieldInfo> fields, IServiceWorker serviceWorker, UserInfo userInfo = null)
        {
            var result = new List<FieldDTO>();

            foreach (var objFieldInfo in fields ?? Enumerable.Empty<ModuleFieldInfo>())
            {
                var roles = userInfo == null || userInfo.IsSuperUser || string.IsNullOrEmpty(objFieldInfo.AuthorizationViewField) ? null : objFieldInfo.AuthorizationViewField.Split(',');
                if (roles == null || userInfo.Roles == null || roles.Contains("All Users") || roles.Any(r => userInfo.Roles.Contains(r)))
                {
                    var field = GetFieldDTO(objFieldInfo, serviceWorker);
                    result.Add(field);
                }
                else
                {
                    var field = new FieldDTO()
                    {
                        FieldID = objFieldInfo.FieldID,
                        ParentID = objFieldInfo.ParentID,
                        FieldName = objFieldInfo.FieldName,
                        IsShow = false,
                        Settings = new Dictionary<string, object>()
                    };
                    result.Add(field);
                }
            }

            return result;
        }

        internal static FieldDTO GetFieldDTO(ModuleFieldInfo objModuleFieldInfo, IServiceWorker serviceWorker)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ModuleFieldInfo, FieldDTO>()
                .ForMember(dest => dest.ShowConditions, map => map.MapFrom(source => TypeCastingUtil<IEnumerable<ExpressionInfo>>.TryJsonCasting(source.ShowConditions)))
                .ForMember(dest => dest.EnableConditions, map => map.MapFrom(source => TypeCastingUtil<IEnumerable<ExpressionInfo>>.TryJsonCasting(source.EnableConditions)))
                .ForMember(dest => dest.FieldValues, map => map.MapFrom(source => TypeCastingUtil<IEnumerable<FieldValueInfo>>.TryJsonCasting(source.FieldValues)))
                .ForMember(dest => dest.Settings, map => map.MapFrom(source => ModuleFieldSettingRepository.Instance.GetFieldSettings(source.FieldID).ToDictionary(item => item.SettingName, item => TypeCastingUtil<object>.TryJsonCasting(item.SettingValue)) ?? new Dictionary<string, object>()));
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<FieldDTO>(objModuleFieldInfo);

            if (result.IsSelective && result.Settings != null && result.Settings.ContainsKey("DataSource"))
            {
                result.DataSource = GetFieldDataSourceItems(result.Settings["DataSource"].ToString(), serviceWorker, true);
            }

            return result;
        }

        #endregion

        internal static FieldDataSourceResult GetFieldDataSourceItems(string dataSourceSettings, IServiceWorker serviceWorker, bool isServerSide = true)
        {
            FieldDataSourceResult result = new FieldDataSourceResult() { Items = new List<object>() };

            try
            {
                var dataSource = JsonConvert.DeserializeObject<FieldDataSourceInfo>(dataSourceSettings);

                var runServiceClientSide = dataSource.RunServiceClientSide != null ? dataSource.RunServiceClientSide.Value : true;

                if (dataSource.Type == FieldDataSourceType.StaticItems || dataSource.Type == FieldDataSourceType.UseDefinedList)
                {
                    var filters = dataSource.ListFilters == null ? "" : string.Join(" and ", dataSource.ListFilters.Select(f => string.Format("({0} {1} '{2}')", f.LeftExpression, f.EvalType, f.RightExpression)));

                    var list = DefinedListRepository.Instance.GetList(dataSource.ListID.Value);

                    var items = DefinedListMapping.GetListItems(dataSource.ListID.Value);

                    result.Items = from x in items.Cast<object>() select x;
                }
                else if (serviceWorker != null && dataSource.Type == FieldDataSourceType.DataSourceService && ((1 == 1) || (isServerSide && !runServiceClientSide) || (!isServerSide && runServiceClientSide)))
                {
                    var items = serviceWorker.RunService<ServiceResult>(dataSource.ServiceID.Value, dataSource.ServiceParams);
                    result.Items = items.Result.DataList;
                    result.TotalCount = items.Result.TotalCount;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        #endregion
    }
}