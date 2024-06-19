//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Xml;
//using System.Data.SqlTypes;
//using DotNetNuke.Common.Utilities;
//using DotNetNuke.Entities.Users;
//using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
//using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
//using NitroSystem.Dnn.BusinessEngine.Components.Enums;
//using NitroSystem.Dnn.BusinessEngine.Api.Models;
//using System.Collections;
//using NitroSystem.Dnn.BusinessEngine.Utilities;
//using System.Dynamic;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using NitroSystem.Dnn.BusinessEngine.Components;
//using System.Text.RegularExpressions;
//using NitroSystem.Dnn.BusinessEngine.Components.Skins;
//using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;

//namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels.Mapping
//{
//    internal static class ViewModelMappingTemp
//    {
//        #region Private Constants

//        private const string EntityCachePrefix = "BE_Entity_";
//        private const string ActionsCachePrefix = "BE_Actions_";
//        private const string ViewModelCachePrefix = "BE_ViewModels_";

//        #endregion

//        #region Private Members

//        private static IEnumerable<DefinedListItemInfo> DefinedListItems { get; set; }
//        private static IEnumerable<DashboardPageInfo> DashboardPages { get; set; }

//        #endregion

//        #region Entities

//        public static EntityViewModel GetEntityViewModel(Guid entityID)
//        {
//            string cacheKey = EntityCachePrefix + entityID;

//            var result = DataCache.GetCache<EntityViewModel>(cacheKey);

//            if (1 == 1 || result == null)
//            {
//                var objEntityInfo = EntityRepository.Instance.GetEntity(entityID);
//                if (objEntityInfo != null)
//                {
//                    result = new EntityViewModel()
//                    {
//                        EntityID = objEntityInfo.EntityID,
//                        ScenarioID = objEntityInfo.ScenarioID,
//                        DatabaseID = objEntityInfo.DatabaseID,
//                        GroupID = objEntityInfo.GroupID,
//                        IsReadonly = objEntityInfo.IsReadonly,
//                        EntityType = objEntityInfo.EntityType,
//                        EntityName = objEntityInfo.EntityName,
//                        TableName = objEntityInfo.TableName,
//                        IsMultipleColumnsForPK = objEntityInfo.IsMultipleColumnsForPK,
//                        ViewOrder = objEntityInfo.ViewOrder,
//                        Columns = EntityColumnRepository.Instance.GetColumns(objEntityInfo.EntityID)
//                    };

//                    try
//                    {
//                        result.Settings = JsonConvert.DeserializeObject<Models.Entity.EntitySettings>(objEntityInfo.Settings);
//                    }
//                    catch
//                    {
//                    }

//                    DataCache.SetCache(cacheKey, result);
//                }
//            }

//            return result;
//        }

//        public static IEnumerable<EntityViewModel> GetEntitiesViewModel(Guid scenarioID)
//        {
//            var result = new List<EntityViewModel>();

//            var entities = EntityRepository.Instance.GetEntities(scenarioID);
//            if (entities != null)
//            {
//                foreach (var entity in entities)
//                {
//                    result.Add(GetEntityViewModel(entity.EntityID));
//                }
//            }

//            return result;
//        }

//        #endregion

//        #region Actions

//        public static ActionViewModel GetActionViewModel(Guid actionID)
//        {
//            var action = ActionRepository.Instance.GetAction(actionID);

//            return GetActionViewModel(action);
//        }

//        public static ActionViewModel GetActionViewModel(ActionInfo objActionInfo)
//        {
//            var action = new ActionViewModel()
//            {
//                ActionID = objActionInfo.ActionID,
//                ParentID = objActionInfo.ParentID,
//                ModuleID = objActionInfo.ModuleID,
//                FieldID = objActionInfo.FieldID,
//                ServiceID = objActionInfo.ServiceID,
//                ActionName = objActionInfo.ActionName,
//                ActionType = objActionInfo.ActionType,
//                Event = objActionInfo.Event,
//                IsServerSide = objActionInfo.IsServerSide,
//                HasPreScript = objActionInfo.HasPreScript,
//                HasPostScript = objActionInfo.HasPostScript,
//                PreScript = objActionInfo.PreScript,
//                PostScript = objActionInfo.PostScript,
//                ParentResultMode = objActionInfo.ParentResultMode,
//                ViewOrder = objActionInfo.ViewOrder,
//            };

//            if (!string.IsNullOrEmpty(objActionInfo.ActionDetails))
//                try
//                {
//                    action.ActionDetails = JsonConvert.DeserializeObject<JObject>(objActionInfo.ActionDetails);
//                }
//                catch
//                {
//                }

//            if (!string.IsNullOrEmpty(objActionInfo.Params))
//                try
//                {
//                    action.Params = JsonConvert.DeserializeObject<IEnumerable<Actions.Models.ActionParamInfo>>(objActionInfo.Params);
//                }
//                catch
//                {
//                }

//            if (!string.IsNullOrEmpty(objActionInfo.Conditions))
//                try
//                {
//                    action.Conditions = JsonConvert.DeserializeObject<IEnumerable<ExpressionInfo>>(objActionInfo.Conditions);
//                }
//                catch
//                {
//                }
//            else
//                action.Conditions = new List<ExpressionInfo>();

//            return action;
//        }

//        public static IEnumerable<ActionViewModel> GetActionsViewModel(Guid moduleID, Guid? fieldID = null)
//        {
//            string cacheKey = ActionsCachePrefix + moduleID + "_" + fieldID;

//            var result = DataCache.GetCache<List<ActionViewModel>>(cacheKey);

//            if (result == null)
//            {
//                result = new List<ActionViewModel>();

//                var actions = fieldID != null ? ActionRepository.Instance.GetFieldActions(fieldID.Value) : ActionRepository.Instance.GetActions(moduleID);
//                foreach (var objActionInfo in actions)
//                {
//                    var action = GetActionViewModel(objActionInfo);

//                    result.Add(action);
//                }

//                //DataCache.SetCache(cacheKey, result);
//            }

//            return result;
//        }

//        #endregion

//        #region Payments

//        public static IEnumerable<PaymentMethodViewModel> GetPaymentMethodsViewModel(Guid scenarioID)
//        {
//            //string cacheKey = PaymentMethodsCachePrefix + scenarioID;

//            //var result = DataCache.GetCache<List<PaymentMethodViewModel>>(cacheKey);

//            List<PaymentMethodViewModel> result = null;

//            if (result == null)
//            {
//                result = new List<PaymentMethodViewModel>();

//                var paymentMethods = PaymentMethodRepository.Instance.GetPaymentMethods(scenarioID);
//                foreach (var pm in paymentMethods)
//                {
//                    var p = new PaymentMethodViewModel()
//                    {
//                        PaymentMethodID = pm.PaymentMethodID,
//                        ScenarioID = pm.ScenarioID,
//                        PaymentName = pm.PaymentName,
//                        SuccessPaymentMessage = pm.SuccessPaymentMessage,
//                        FailedPaymentMessage = pm.FailedPaymentMessage,
//                        Amount = pm.Amount,
//                    };

//                    result.Add(p);
//                }

//                //DataCache.SetCache(cacheKey, result);
//            }
//            return result;
//        }

//        #endregion

//        #region Services

//        public static ServiceViewModel GetServiceViewModel(Guid serviceID)
//        {
//            var objServiceInfo = ServiceRepository.Instance.GetService(serviceID);

//            return objServiceInfo != null ? GetServiceViewModel(objServiceInfo) : null;
//        }

//        public static ServiceViewModel GetServiceViewModel(ServiceInfo objServiceInfo)
//        {
//            var result = new ServiceViewModel()
//            {
//                ServiceID = objServiceInfo.ServiceID,
//                ScenarioID = objServiceInfo.ScenarioID,
//                ServiceName = objServiceInfo.ServiceName,
//                ServiceType = objServiceInfo.ServiceType,
//                ServiceSubtype = objServiceInfo.ServiceSubtype,
//                Command = objServiceInfo.Command,
//                Description = objServiceInfo.Description,
//                HasResult = objServiceInfo.HasResult,
//                ResultType = objServiceInfo.ResultType,
//                IsEnabled = objServiceInfo.IsEnabled,
//                AuthorizationRunService = !string.IsNullOrEmpty(objServiceInfo.AuthorizationRunService) ? objServiceInfo.AuthorizationRunService.Split(',') : new string[0],
//                ViewOrder = objServiceInfo.ViewOrder
//            };

//            try
//            {
//                //if (!string.IsNullOrEmpty(objServiceInfo.Settings)) result.Settings = JsonConvert.DeserializeObject<Hashtable>(objServiceInfo.Settings);

//            }
//            catch
//            {
//            }

//            try
//            {
//                //if (!string.IsNullOrEmpty(objServiceInfo.Params)) result.Params = JsonConvert.DeserializeObject<IEnumerable<Models.ServiceModels.ServiceParamInfo>>(objServiceInfo.Params);
//            }
//            catch
//            {
//            }

//            try
//            {
//                if (!string.IsNullOrEmpty(objServiceInfo.WebServiceOptions)) result.WebServiceOptions = JsonConvert.DeserializeObject<Models.ServiceModels.WebService.WebServiceOptions>(objServiceInfo.WebServiceOptions);
//            }
//            catch
//            {
//            }

//            return result;
//        }

//        public static IEnumerable<ServiceViewModel> GetServicesViewModel(Guid scenarioID)
//        {
//            var result = new List<ServiceViewModel>();

//            var services = ServiceRepository.Instance.GetServices(scenarioID);

//            foreach (var objServiceInfo in services)
//            {
//                result.Add(GetServiceViewModel(objServiceInfo));

//                //if (objServiceInfo.DatabaseObjectType == "StoredProcedure")
//                //{
//                //    var connection = new System.Data.SqlClient.SqlConnection(DotNetNuke.Data.DataProvider.Instance().ConnectionString);

//                //    service.DatabaseObjectParams = Dapper.SqlMapper.Query<string>(connection, string.Format("SELECT [Name] FROM sys.parameters WHERE object_id = object_id('dbo.{0}')", objServiceInfo.DatabaseObjectName));

//                //    if (1 == 2 && loadSpContent)
//                //        service.Command = Dapper.SqlMapper.QuerySingle<string>(connection, string.Format("SELECT [Definition] FROM sys.sql_modules WHERE objectproperty(OBJECT_ID, 'IsProcedure') = 1 AND OBJECT_NAME(OBJECT_ID) = '{0}'", objServiceInfo.DatabaseObjectName)); // or use EXEC sp_helptext N'dbo.GetFolders'
//                //}
//            }

//            return result;
//        }

//        #endregion

//        #region Dashboard

//        public static DashboardViewModel GetDashboardViewModel(Guid moduleGuid)
//        {
//            var objDashboardView = DashboardRepository.Instance.GetDashboardByModuleID(moduleGuid);
//            return objDashboardView != null ? GetDashboardViewModel(objDashboardView) : null;
//        }

//        public static DashboardViewModel GetDashboardViewModel(DashboardView objDashboardView)
//        {
//            var dashboard = new DashboardViewModel()
//            {
//                DashboardID = objDashboardView.DashboardID,
//                ModuleID = objDashboardView.ModuleID,
//                ScenarioID = objDashboardView.ScenarioID,
//                DashboardType = objDashboardView.DashboardType,
//                AuthorizationViewDashboard = !string.IsNullOrEmpty(objDashboardView.AuthorizationViewDashboard) ? objDashboardView.AuthorizationViewDashboard.Split(',') : null,
//                UniqueName = objDashboardView.UniqueName,
//                ModuleName = objDashboardView.ModuleName,
//                ModuleTitle = objDashboardView.ModuleTitle,
//                Skin = SkinManager.GetSkin(objDashboardView.Skin),
//                Template = objDashboardView.Template,
//                Theme = objDashboardView.Theme,
//                PortalID = objDashboardView.PortalID,
//                DnnModuleID = objDashboardView.DnnModuleID,
//                CreatedOnDate = objDashboardView.CreatedOnDate,
//                CreatedByUserID = objDashboardView.CreatedByUserID,
//                LastModifiedOnDate = objDashboardView.LastModifiedOnDate,
//                LastModifiedByUserID = objDashboardView.LastModifiedByUserID,
//                Settings = objDashboardView.Settings,
//                ViewOrder = objDashboardView.ViewOrder
//            };

//            return dashboard;
//        }


//        public static DashboardPageViewModel GetDashboardPageViewModel(DashboardPageInfo objDashboardPageInfo)
//        {
//            var page = new DashboardPageViewModel()
//            {
//                PageID = objDashboardPageInfo.PageID,
//                DashboardID = objDashboardPageInfo.DashboardID,
//                PageName = objDashboardPageInfo.PageName,
//                Title = objDashboardPageInfo.Title,
//                PageType = objDashboardPageInfo.PageType,
//                Url = objDashboardPageInfo.Url,
//                IsVisible = objDashboardPageInfo.IsVisible,
//                ViewOrder = objDashboardPageInfo.ViewOrder,
//                ParentID = objDashboardPageInfo.ParentID,
//                AuthorizationViewPage = !string.IsNullOrEmpty(objDashboardPageInfo.AuthorizationViewPage) ? objDashboardPageInfo.AuthorizationViewPage.Split(',') : null,
//                Pages = new List<DashboardPageViewModel>()
//            };

//            if (!string.IsNullOrEmpty(objDashboardPageInfo.Data))
//            {
//                try
//                {
//                    page.Data = JsonConvert.DeserializeObject<Hashtable>(objDashboardPageInfo.Data);
//                }
//                catch
//                {
//                }
//            }

//            return page;
//        }

//        public static IEnumerable<DashboardPageViewModel> GetDashboardPagesViewModel(Guid dashboardID, Guid rootItemID, IEnumerable<DashboardPageInfo> pages = null)
//        {
//            var result = new List<DashboardPageViewModel>();

//            if (pages == null)
//                DashboardPages = DashboardPageRepository.Instance.GetPages(dashboardID);
//            else
//                DashboardPages = pages;

//            IEnumerable<DashboardPageInfo> items;
//            if (rootItemID == Guid.Empty)
//            {
//                items = DashboardPages.Where(c => c.ParentID == null);
//            }
//            else
//            {
//                items = DashboardPages.Where(c => c.PageID == rootItemID);
//            }

//            foreach (var item in items)
//            {
//                result.Add(PopulateDashboardPageItems(item));
//            }

//            return result;
//        }

//        private static DashboardPageViewModel PopulateDashboardPageItems(DashboardPageInfo page)
//        {
//            var newPage = GetDashboardPageViewModel(page);

//            var childs = DashboardPages.Where(c => c.ParentID == page.PageID);
//            foreach (var item in childs)
//            {
//                newPage.Pages.Add(PopulateDashboardPageItems(item));
//            }

//            return newPage;
//        }

//        #endregion

//        #region Defined Lists

//        public static IEnumerable<DefinedListViewModel> GetDefinedListsViewModel(IEnumerable<DefinedListInfo> lists)
//        {
//            var result = new List<DefinedListViewModel>();

//            foreach (DefinedListInfo list in lists)
//            {
//                result.Add(new DefinedListViewModel()
//                {
//                    ListID = list.ListID,
//                    ScenarioID = list.ScenarioID,
//                    ListName = list.ListName,
//                    ListType = list.ListType,
//                    ParentID = list.ParentID,
//                    DependencyID = list.DependencyID,
//                    Data = list.Data,
//                    ViewOrder = list.ViewOrder,
//                    Items = GetDefinedListItemsViewModel(list.ListID, null)
//                });
//            }

//            return result;
//        }

//        public static DefinedListItemViewModel GetDefinedListItemViewModel(DefinedListItemInfo objDefinedListItemInfo)
//        {
//            var item = new DefinedListItemViewModel()
//            {
//                ItemID = objDefinedListItemInfo.ItemID,
//                Text = objDefinedListItemInfo.Text,
//                Value = objDefinedListItemInfo.Value,
//                Content = objDefinedListItemInfo.Content,
//                ViewOrder = objDefinedListItemInfo.ViewOrder,
//                ParentID = objDefinedListItemInfo.ParentValue,
//                Items = new List<DefinedListItemViewModel>()
//            };

//            if (!string.IsNullOrEmpty(objDefinedListItemInfo.Data))
//            {
//                try
//                {
//                    item.Data = JsonConvert.DeserializeObject(objDefinedListItemInfo.Data);
//                }
//                catch
//                {
//                }
//            }

//            return item;
//        }

//        public static IEnumerable<DefinedListItemViewModel> GetDefinedListItemsViewModel(Guid listID, string parentValue)
//        {
//            IEnumerable<DefinedListItemInfo> items = DefinedListItemRepository.Instance.GetItems(listID).OrderBy(c => c.ViewOrder);

//            var result = new List<DefinedListItemViewModel>();

//            if (string.IsNullOrEmpty(parentValue))
//            {
//                items = items.Where(c => c.ParentValue == null);
//            }
//            else
//            {
//                items = items.Where(c => c.ParentValue == parentValue);
//            }

//            foreach (var item in items)
//            {
//                result.Add(PopulateDefinedListItems(items, item));
//            }

//            return result;
//        }

//        public static IEnumerable<DefinedListItemInfo> GetDefinedListItemAndChilds(Guid listID, Guid? rootID, string prefix = "")
//        {
//            var result = new List<DefinedListItemInfo>();

//            DefinedListItems = DefinedListItemRepository.Instance.GetItems(listID).OrderBy(c => c.ViewOrder);

//            IEnumerable<DefinedListItemInfo> items;

//            if (rootID == null)
//            {
//                items = DefinedListItems.Where(c => c.ParentValue == null);
//            }
//            else
//            {
//                items = DefinedListItems.Where(c => c.ItemID == rootID);
//            }

//            foreach (var item in items)
//            {
//                result = PopulateDefinedListItemsInOneLevel(result, item, 0, prefix);
//            }

//            return result;
//        }

//        private static DefinedListItemViewModel PopulateDefinedListItems(IEnumerable<DefinedListItemInfo> result, DefinedListItemInfo item)
//        {
//            var newCategory = GetDefinedListItemViewModel(item);

//            var childs = result.Where(c => c.ParentValue == item.Value);

//            foreach (var i in childs)
//            {
//                newCategory.Items.Add(PopulateDefinedListItems(result, i));
//            }

//            return newCategory;
//        }

//        public static List<DefinedListItemInfo> PopulateDefinedListItemsInOneLevel(List<DefinedListItemInfo> result, DefinedListItemInfo item, int level = 0, string prefix = "")
//        {
//            item.Level = level;

//            if (!string.IsNullOrEmpty(prefix))
//            {
//                string _prefix = string.Empty;
//                for (int i = 0; i < level; i++)
//                {
//                    _prefix += prefix;
//                }

//                item.Text = _prefix + item.Text;
//            }

//            if (result == null) result = new List<DefinedListItemInfo>();

//            result.Add(item);

//            var childs = result.Where(c => c.ParentValue == item.Value);
//            foreach (var i in childs)
//            {
//                PopulateDefinedListItemsInOneLevel(result, i, level + 1, prefix);
//            }

//            return result;
//        }

//        public static List<DefinedListItemInfo> PopulateDefinedListItemsInOneLevel2(List<DefinedListItemInfo> result, DefinedListItemViewModel item, int level = 0, string prefix = "")
//        {
//            item.Level = level;

//            if (!string.IsNullOrEmpty(prefix))
//            {
//                string _prefix = string.Empty;
//                for (int i = 0; i < level; i++)
//                {
//                    _prefix += prefix;
//                }

//                item.Text = _prefix + item.Text;
//            }

//            if (result == null) result = new List<DefinedListItemInfo>();

//            if (item.ItemID == Guid.Empty) item.ItemID = Guid.NewGuid();

//            result.Add(new DefinedListItemInfo()
//            {
//                ItemID = item.ItemID,
//                ListID = item.ListID,
//                Text = item.Text,
//                Value = item.Value,
//                Content = item.Content,
//                Data = JsonConvert.SerializeObject(item.Data),
//                Level = item.Level,
//                ParentValue = item.ParentID,
//                ViewOrder = item.ViewOrder,
//            });

//            var childs = result.Where(c => c.ParentValue == item.Value);
//            if (item.Items != null)
//            {
//                foreach (var i in item.Items)
//                {
//                    i.ParentID = item.Value;

//                    PopulateDefinedListItemsInOneLevel2(result, i, level + 1, prefix);
//                }
//            }

//            return result;
//        }

//        #endregion

//        #region Module Fields

//        internal static IEnumerable<ModuleFieldViewModel> GetModuleFieldsViewModel(Guid moduleID, string[] userRoles = null, bool isSuperUser = false, string pageUrl = null)
//        {
//            //var result = DataCache.GetCache<List<ModuleFieldViewModel>>("BE_ModuleFields_View" + moduleID);
//            var result = new List<ModuleFieldViewModel>();

//            if (1 == 1/*result == null*/)
//            {
//                var fields = ModuleFieldRepository.Instance.GetFields(moduleID);

//                foreach (var f in fields)
//                {
//                    var roles = isSuperUser || string.IsNullOrEmpty(f.AuthorizationViewField) ? null : f.AuthorizationViewField.Split(',');
//                    if (roles == null || userRoles == null || roles.Contains("All Users") || roles.Any(r => userRoles.Contains(r)))
//                    {
//                        var field = GetModuleFieldViewModel(f, pageUrl);
//                        result.Add(field);
//                    }
//                }

//                // DataCache.SetCache("BE_ModuleFields_View" + moduleID, result);
//            }

//            return result;
//        }

//        internal static ModuleFieldViewModel GetModuleFieldViewModel(Guid fieldID)
//        {
//            var field = ModuleFieldRepository.Instance.GetField(fieldID);

//            return GetModuleFieldViewModel(field);
//        }

//        internal static ModuleFieldViewModel GetModuleFieldViewModel(ModuleFieldInfo objFieldInfo, string pageUrl = null)
//        {
//            var field = new ModuleFieldViewModel()
//            {
//                FieldID = objFieldInfo.FieldID,
//                ModuleID = objFieldInfo.ModuleID,
//                FieldType = objFieldInfo.FieldType,
//                FieldName = objFieldInfo.FieldName,
//                ParentID = objFieldInfo.ParentID,
//                PaneName = objFieldInfo.PaneName,
//                FieldText = objFieldInfo.FieldText,
//                IsGroup = objFieldInfo.IsGroup,
//                IsValuable = objFieldInfo.IsValuable,
//                IsRequired = objFieldInfo.IsRequired,
//                IsShow = objFieldInfo.IsShow,
//                IsEnabled = objFieldInfo.IsEnabled,
//                IsSelective = objFieldInfo.IsSelective,
//                IsJsonValue = objFieldInfo.IsJsonValue,
//                IsActionable = objFieldInfo.IsActionable,
//                ViewOrder = objFieldInfo.ViewOrder,
//                AuthorizationViewField = !string.IsNullOrEmpty(objFieldInfo.AuthorizationViewField) ? objFieldInfo.AuthorizationViewField.Split(',') : null,
//                Actions = GetActionsViewModel(Guid.Empty, objFieldInfo.FieldID),
//                Settings = new Dictionary<string, object>()
//            };

//            try
//            {
//                if (objFieldInfo.ShowConditions != null)
//                {
//                    field.ShowConditions = JsonConvert.DeserializeObject<IEnumerable<ExpressionInfo>>(objFieldInfo.ShowConditions);
//                }
//            }
//            catch (Exception ex)
//            {
//            }

//            try
//            {
//                //field.Settings = ModuleFieldSettingRepository.Instance.GetFieldSettings(objFieldInfo.FieldID).ToDictionary(item => item.SettingName, item => item.SettingValue); ;

//                if (field.IsSelective && field.DataSource != null)
//                {
//                    var dataSource = JsonConvert.DeserializeObject<FieldDataSourceInfo>(field.DataSource);

//                    field.Options = GetModuleFieldOptionsViewModel(ref field, dataSource, pageUrl);
//                }
//            }
//            catch (Exception ex)
//            {
//            }

//            return field;
//        }

//        internal static IEnumerable<object> GetModuleFieldOptionsViewModel(ref ModuleFieldViewModel field, FieldDataSourceInfo dataSource = null, string pageUrl = null)
//        {
//            IEnumerable<object> result = null;

//            if (dataSource.Type == 0 || dataSource.Type == 1)
//            {
//                var filters = dataSource.ListFilters == null ? "" : string.Join(" and ", dataSource.ListFilters.Select(f => string.Format("({0} {1} '{2}')", f.LeftExpression, f.EvalType, f.RightExpression)));

//                var list = DefinedListRepository.Instance.GetList(dataSource.ListID.Value);

//                var items = DefinedListItemRepository.Instance.GetItemsWithFilters(list.ListID, filters);
//                var viewItems = new List<DefinedListItemViewModel>();
//                foreach (var item in items)
//                {
//                    viewItems.Add(GetDefinedListItemViewModel(item));
//                }

//                dataSource.ListName = list.ListName;

//                result = from x in viewItems.Cast<object>()
//                         select x;

//                field.DataSource = JsonConvert.SerializeObject(dataSource);
//            }

//            return result;
//        }

//        #endregion

//        #region Modules

//        public static ModuleViewModel GetModuleViewModel(Guid moduleID)
//        {
//            var objBeModuleInfo = ModuleRepository.Instance.GetModule(moduleID);

//            return objBeModuleInfo != null ? GetModuleViewModel(objBeModuleInfo) : null;
//        }

//        public static ModuleViewModel GetModuleViewModel(ModuleInfo objBeModuleInfo)
//        {
//            var skinName = objBeModuleInfo.ParentID != null ? ModuleRepository.Instance.GetModule(objBeModuleInfo.ParentID.Value).Skin : objBeModuleInfo.Skin;

//            var result = new ModuleViewModel()
//            {
//                ModuleID = objBeModuleInfo.ModuleID,
//                ScenarioID = objBeModuleInfo.ScenarioID,
//                ParentID = objBeModuleInfo.ParentID,
//                Wrapper = objBeModuleInfo.Wrapper,
//                ModuleType = objBeModuleInfo.ModuleType,
//                ModuleName = objBeModuleInfo.ModuleName,
//                ModuleTitle = objBeModuleInfo.ModuleTitle,
//                Skin = SkinManager.GetSkin(skinName),
//                Template = objBeModuleInfo.Template,
//                Theme = objBeModuleInfo.Theme,
//                BaseTemplate = objBeModuleInfo.BaseTemplate,
//                CustomCss = objBeModuleInfo.CustomCss,
//                PortalID = objBeModuleInfo.PortalID,
//                DnnModuleID = objBeModuleInfo.DnnModuleID,
//                ViewOrder = objBeModuleInfo.ViewOrder
//            };

//            try
//            {
//                result.Settings = JsonConvert.DeserializeObject<IDictionary<string, object>>(objBeModuleInfo.Settings);
//            }
//            catch
//            {
//            }

//            return result;
//        }

//        #endregion

//        #region View Models

//        public static ViewModelViewModel GetViewModelViewModel(Guid viewModelID)
//        {
//            string cacheKey = ViewModelCachePrefix + viewModelID;

//            var result = DataCache.GetCache<ViewModelViewModel>(cacheKey);

//            if (1 == 1 || result == null)
//            {
//                var objViewModelInfo = ViewModelRepository.Instance.GetViewModel(viewModelID);
//                if (objViewModelInfo != null)
//                {
//                    result = new ViewModelViewModel()
//                    {
//                        ViewModelID = objViewModelInfo.ViewModelID,
//                        ScenarioID = objViewModelInfo.ScenarioID,
//                        ViewModelName = objViewModelInfo.ViewModelName,
//                        Type = objViewModelInfo.Type,
//                        CreatedOnDate = objViewModelInfo.CreatedOnDate,
//                        CreatedByUserID = objViewModelInfo.CreatedByUserID,
//                        LastModifiedOnDate = objViewModelInfo.LastModifiedOnDate,
//                        LastModifiedByUserID = objViewModelInfo.LastModifiedByUserID,
//                        Description = objViewModelInfo.Description,
//                        ViewOrder = objViewModelInfo.ViewOrder,
//                        Properties = ViewModelPropertyRepository.Instance.GetProperties(objViewModelInfo.ViewModelID)
//                    };

//                    //var propertyList = new List<ViewModelPropertyViewModel>();

//                    //var properties = ViewModelPropertyRepository.Instance.GetProperties(objViewModelInfo.ViewModelID);
//                    //foreach (var prop in properties.Where(p => p.ParentID == null))
//                    //{
//                    //    var property = new ViewModelPropertyViewModel()
//                    //    {
//                    //        PropertyID = prop.PropertyID,
//                    //        PropertyName = prop.PropertyName,
//                    //        PropertyType = prop.PropertyType,
//                    //        ListType=prop.ListType,
//                    //        ViewOrder = prop.ViewOrder,
//                    //        Properties = properties.Where(p => p.ParentID == prop.PropertyID).Select(p => new ViewModelPropertyViewModel()
//                    //        {
//                    //            PropertyID = p.PropertyID,
//                    //            PropertyName = p.PropertyName,
//                    //            PropertyType = p.PropertyType,
//                    //            ViewOrder = p.ViewOrder,
//                    //        })
//                    //    };

//                    //    propertyList.Add(property);
//                    //}

//                    //result.Properties = propertyList;

//                    DataCache.SetCache(cacheKey, result);
//                }
//            }

//            return result;
//        }

//        public static IEnumerable<ViewModelViewModel> GetViewModelsViewModel(Guid scenarioID)
//        {
//            var result = new List<ViewModelViewModel>();

//            var viewModels = ViewModelRepository.Instance.GetViewModels(scenarioID);
//            if (viewModels != null)
//            {
//                foreach (var viewModel in viewModels)
//                {
//                    result.Add(GetViewModelViewModel(viewModel.ViewModelID));
//                }
//            }

//            return result;
//        }

//        #endregion
//    }
//}