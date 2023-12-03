using DotNetNuke.Abstractions.Portals;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Scheduling;
using DotNetNuke.Web.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Api.Dto;
using NitroSystem.Dnn.BusinessEngine.Api.Mapping;
using NitroSystem.Dnn.BusinessEngine.Api.Models;
using NitroSystem.Dnn.BusinessEngine.Api.ViewModels;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using NitroSystem.Dnn.BusinessEngine.Core.Infrastructure.SSR;
using NitroSystem.Dnn.BusinessEngine.Core.ModuleBuilder;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Framework.Contracts;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using NitroSystem.Dnn.BusinessEngine.Framework.Models;
using NitroSystem.Dnn.BusinessEngine.Framework.Services;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace NitroSystem.Dnn.BusinessEngine.Api.Controller
{
    public class ModuleController : DnnApiController
    {
        private readonly IModuleData _moduleData;
        private readonly IExpressionService _expressionService;
        private readonly IActionWorker _actionWorker;
        private readonly IServiceWorker _serviceWorker;

        public ModuleController(IModuleData moduleData, IActionWorker actionWorker, IServiceWorker serviceWorker, IExpressionService expressionService)
        {
            this._moduleData = moduleData;
            this._actionWorker = actionWorker;
            this._serviceWorker = serviceWorker;
            this._expressionService = expressionService;
        }

        #region Module 

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> GetModuleData(ModuleDTO postData)
        {
            try
            {
                var moduleGuid = Guid.Parse(Request.Headers.GetValues("ModuleID").First());

                string connectionID = postData.ConnectionID;

                var module = ModuleRepository.Instance.GetModuleView(moduleGuid);
                if (module == null) throw new Exception("Module Not Config");
                
                string moduleTemplateUrl = "";
                string moduleTemplateCssUrl = "";

                this._moduleData.InitModuleData(moduleGuid, connectionID, this.UserInfo.UserID, null, null, postData.PageUrl, !module.IsSSR);
                

                await _actionWorker.CallActions(moduleGuid, null, "OnPageInit"); // call "OnPageInit" event actions. Not important server side

                IEnumerable<FieldDTO> fields = null;
                if (module.ModuleBuilderType != "HtmlEditor")
                {
                    fields = ModuleFieldMappings.GetFieldsDTO(module.ModuleID, this._serviceWorker, this.UserInfo);
                }

                if (!module.IsSSR)
                {
                    await _actionWorker.CallActions(moduleGuid, null, "OnPageLoad"); // call "OnPageLoad" event actions that are server side

                    var modulePath = (PortalSettings.PortalId == module.PortalID ? PortalSettings.HomeSystemDirectory : new PortalSettings(module.PortalID).HomeSystemDirectory) + @"BusinessEngine/";
                    moduleTemplateUrl = module.ModuleBuilderType != "HtmlEditor" ?
                        string.Format("{0}/{1}/module--{2}.html?ver={3}-{4}", modulePath, module.ScenarioName, module.ModuleName, Host.CrmVersion, module.Version) :
                        string.Format("{0}/{1}/module--{2}/custom.html?ver={3}-{4}", modulePath, module.ScenarioName, module.ModuleName, Host.CrmVersion, module.Version);

                    if (module.ParentID != null && module.ModuleBuilderType == "HtmlEditor")
                        moduleTemplateCssUrl = string.Format("{0}/{1}/module--{2}/custom.css?ver={3}-{4}", modulePath, module.ScenarioName, module.ModuleName, Host.CrmVersion, module.Version);
                }

                foreach (var field in fields ?? Enumerable.Empty<FieldDTO>())
                {
                    var lightField = new
                    {
                        field.FieldID,
                        field.FieldName,
                        field.Value,
                        field.DataSource,
                        field.Settings
                    };

                    this._moduleData.SetFieldItem(field.FieldName, lightField);

                    if (field.IsSelective && field.Settings != null && field.Settings.ContainsKey("DataSource"))
                    {
                        field.DataSource = ModuleFieldMappings.GetFieldDataSourceItems(field.Settings["DataSource"].ToString(), this._serviceWorker, true);
                    }
                }

                var variables = ModuleVariableMapping.GetVariablesViewModel(moduleGuid);

                var actions = ActionMapping.GetActionsDTO(moduleGuid);

                var moduleData = this._moduleData.GetModuleData(connectionID, moduleGuid);

                if (this.UserInfo.UserID >= 0)
                    moduleData["CurrentUser"] = JObject.FromObject(new { this.UserInfo.UserID, this.UserInfo.DisplayName });

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Fields = fields,
                    Actions = actions,
                    ModuleBuilderType = module.ModuleBuilderType,
                    ModuleTemplateUrl = moduleTemplateUrl,
                    ModuleTemplateCssUrl = moduleTemplateCssUrl,
                    ConnectionID = connectionID,
                    Variables = variables,
                    Data = moduleData,
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetFieldDataSource(FieldDataSourceDTO postData)
        {
            try
            {
                this._moduleData.InitModuleData(postData.ModuleID, postData.ConnectionID, this.UserInfo.UserID, postData.Form, null, postData.PageUrl);

                var objModuleFieldInfo = ModuleFieldRepository.Instance.GetField(postData.FieldID);

                this._moduleData.SetData(string.Format("Field.{0}.Settings", objModuleFieldInfo.FieldName),
                    new
                    {
                        PageIndex = postData.PageIndex,
                        PageSize = postData.PageSize

                    }, false);

                var dataSourceSettings = ModuleFieldSettingRepository.Instance.GetFieldSetting(objModuleFieldInfo.FieldID, "DataSource");

                var result = ModuleFieldMappings.GetFieldDataSourceItems(dataSourceSettings.SettingValue, this._serviceWorker, false);

                var moduleData = this._moduleData.GetModuleData(postData.ConnectionID, postData.ModuleID);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    FieldName = objModuleFieldInfo.FieldName,
                    DataSource = result,
                    Data = moduleData
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

        #region Action

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> CallAction(ActionDto action)
        {
            try
            {
                this._moduleData.InitModuleData(action.ModuleID, action.ConnectionID, this.UserInfo.UserID, action.Form, action.Field, action.PageUrl);

                var result = await _actionWorker.CallAction(action.ActionID);

                var serviceResult = (result as ServiceResult);
                if (serviceResult != null && serviceResult.IsError)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, serviceResult);

                var moduleData = this._moduleData.GetModuleData(action.ConnectionID);

                return Request.CreateResponse(HttpStatusCode.OK, moduleData);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

        #region  Dashboard

        [AllowAnonymous]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetDashboardData()
        {
            try
            {
                var moduleGuid = Guid.Parse(Request.Headers.GetValues("ModuleID").First());

                var dashboard = DashboardRepository.Instance.GetDashboardByModuleID(moduleGuid);

                StringBuilder ngScripts = new StringBuilder();

                //string script = @"
                //    <script type=""text/ng-template"" id=""page{PageID}.html"">
                //        {ModuleContent}
                //    </script>";

                //var pagesModules = DashboardPageModuleRepository.Instance.GetDashboardModules(dashboard.DashboardID);
                //foreach (var page in pagesModules.GroupBy(m => new { m.PageID }))
                //{
                //    string template = string.Empty;

                //    if (page.Any())
                //    {
                //        Guid pageID = page.First().PageID;

                //        if (!allPages.Any(p => p.PageID == pageID)) continue;

                //        template = script.Replace("{PageID}", pageID.ToString());

                //        foreach (var module in page)
                //        {
                //            template = template.Replace("{ModuleContent}", string.Format(@"
                //                    <div id=""pnlBusinessEngine{0}"" data-module=""{0}"" ng-app=""BusinessEngineClientApp""  ng-controller=""moduleController as $"" ng-init=""onInitModule('{0}' ,'{1}')"" class=""w-100p""></div>{2}", module.ModuleID, module.ModuleName, "{ModuleContent}"));
                //        }

                //        template = template.Replace("{ModuleContent}", string.Empty);

                //        ngScripts.Append(template);
                //    }
                //}

                var allPages = DashboardPageRepository.Instance.GetPages(dashboard.DashboardID).Where(p => UserInfo.IsSuperUser || string.IsNullOrEmpty(p.AuthorizationViewPage) || UserInfo.Roles.Any(r => p.AuthorizationViewPage.Split(',').Contains(r)));
                var pages = DashboardMapping.GetDashboardPagesViewModel(dashboard.DashboardID, Guid.Empty, allPages);

                string baseUrl = "";
                if (dashboard.DashboardType == 2)
                {
                    var module = ModuleRepository.Instance.GetModule(dashboard.ModuleID);
                    int tabID = ModuleRepository.Instance.GetModuleTabID(module.DnnModuleID.Value).Value;
                    baseUrl = UrlUtil.GetFriendlyViewURL("", "", TabController.Instance.GetTab(tabID, PortalSettings.PortalId), true, "");
                }
                else
                {
                    var defaultDomainAddress = PortalController.GetPortalSetting("bDefaultDomainAddress", PortalSettings.PortalId, "");
                    string url = string.IsNullOrEmpty(defaultDomainAddress) ? (HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/") : defaultDomainAddress;
                    baseUrl = url + "DesktopModules/BusinessEngine/Dashboard.aspx?d=" + dashboard.UniqueName;
                }

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Dashboard = dashboard,
                    Pages = pages,
                    BaseUrl = baseUrl
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

        #region Temp

        [AllowAnonymous]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetPVA(Guid moduleID)
        {
            try
            {
                var Actions = ActionRepository.Instance.GetActions(moduleID);

                return Request.CreateResponse(HttpStatusCode.OK, Actions);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion
    }
}