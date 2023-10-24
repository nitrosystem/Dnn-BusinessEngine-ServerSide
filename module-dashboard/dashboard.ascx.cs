using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Web.Client.ClientResourceManagement;
using NitroSystem.Dnn.BusinessEngine.Components;
using NitroSystem.Dnn.BusinessEngine.Core.Infrastructure.ClientResources;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroSystem.Dnn.BusinessEngine.Modules.Dashboard
{
    public partial class Dashboard : PortalModuleBase, IActionable
    {
        public Guid? ModuleGuid
        {
            get
            {
                return ModuleRepository.Instance.GetModuleGuidByDnnModuleID(this.ModuleId) ?? Guid.Empty;
            }
        }

        public string ModuleName
        {
            get
            {
                return this.ModuleGuid != null ? ModuleRepository.Instance.GetModuleName(this.ModuleGuid.Value) : "";
            }
        }

        public bool IsSSR
        {
            get
            {
                if (this.ModuleGuid == null) return false;

                var _isSSr = ModuleRepository.Instance.IsSSR(this.ModuleGuid.Value);

                return _isSSr == null ? false : _isSSr.Value;
            }
        }

        public bool IsDisabledFrontFramework
        {
            get
            {
                if (this.ModuleGuid == null) return false;

                var _isDisabledFrontFramework = ModuleRepository.Instance.IsDisabledFrontFramework(this.ModuleGuid.Value);

                return _isDisabledFrontFramework == null ? false : _isDisabledFrontFramework.Value;
            }
        }

        public DashboardView DashboardInfo
        {
            get
            {
                return this.ModuleGuid == null ? null : DashboardRepository.Instance.GetDashboardByModuleID(this.ModuleGuid.Value);
            }
        }

        public string ScenarioName
        {
            get
            {
                return this.ModuleGuid != null ? ModuleRepository.Instance.GetModuleScenarioName(this.ModuleGuid.Value) : "";
            }
        }

        public string BaseUrl
        {
            get
            {
                return "/";
            }
        }

        public string SiteRoot
        {
            get
            {
                string domainName = DotNetNuke.Common.Globals.AddHTTP(DotNetNuke.Common.Globals.GetDomainName(this.Context.Request)) + "/";
                return domainName;
            }
        }

        public string ApiBaseUrl
        {
            get
            {
                string domainName = DotNetNuke.Common.Globals.GetPortalDomainName(this.PortalAlias.HTTPAlias, Request, true);
                return domainName + "/DesktopModules/";
            }
        }

        public bool IsRegisteredPageResources
        {
            get
            {
                return this.Page.Header.FindControl("bEngine_PageResources") != null;
            }
        }

        public bool IsRtl
        {
            get
            {
                return CultureInfo.CurrentCulture.TextInfo.IsRightToLeft;
            }
        }

        public string bRtlCssClass
        {
            get
            {
                return IsRtl ? "b-rtl" : "";
            }
        }

        public string Version
        {
            get
            {
                if (this.UserInfo.IsSuperUser)
                    return Guid.NewGuid().ToString();
                else
                    return Host.CrmVersion.ToString();
            }
        }

        public string ControlPanelUrl
        {
            get
            {
                string moduleParam = this.ModuleGuid == null ? "d" : "id";
                string moduleParamValue = this.ModuleGuid == null ? this.ModuleId.ToString() : this.ModuleGuid.ToString();

                return ResolveUrl(string.Format("~/DesktopModules/BusinessEngine/Studio.aspx?s={0}&p={1}&a={2}&{3}={4}&m=create-dashboard&return-url={5}", this.ScenarioName, this.PortalId, this.PortalAlias.PortalAliasID, moduleParam, moduleParamValue, "/Default.aspx?tabid=" + this.TabId));
            }
        }

        public string ConnectionID
        {
            get
            {
                return Request.AnonymousID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsRegisteredPageResources)
            {
                RegisterPageResources.RegisterResources(this.TabId, this.pnlStyles, this.pnlScripts, this.Version);

                this.Page.Header.Controls.Add(new LiteralControl(@"<span id=""bEngine_PageResources""><!--business engine registered resources--></span>"));

            }

            var module = ModuleRepository.Instance.GetModule(this.ModuleGuid.Value);
            if (module != null && module.ModuleBuilderType == "HtmlEditor")
            {
                var scenarioName = ScenarioRepository.Instance.GetScenarioName(module.ScenarioID);

                var moduleJsPath = string.Format("{0}/BusinessEngine/{1}/module--{2}/custom.js", this.PortalSettings.HomeSystemDirectory, scenarioName, module.ModuleName);
                Core.Infrastructure.ClientResources.ClientResourceManager.RegisterScript(this.pnlScripts, moduleJsPath, module.Version.ToString());

                var moduleCssPath = string.Format("{0}/BusinessEngine/{1}/module--{2}/custom.css", this.PortalSettings.HomeSystemDirectory, scenarioName, module.ModuleName);
                Core.Infrastructure.ClientResources.ClientResourceManager.RegisterStyleSheet(this.pnlStyles, moduleCssPath, module.Version.ToString());
            }
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection();
                actions.Add(GetNextActionID(), "Settings", ModuleActionType.ModuleSettings, "", "", ControlPanelUrl, false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
                return actions;
            }
        }
    }
}