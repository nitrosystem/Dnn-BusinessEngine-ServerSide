using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Portals;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using NitroSystem.Dnn.BusinessEngine.Core.Infrastructure.SSR;
using NitroSystem.Dnn.BusinessEngine.Core.ModuleBuilder;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Framework.Services;
using NitroSystem.Dnn.BusinessEngine.Services;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.UI;

namespace NitroSystem.Dnn.BusinessEngine.Modules
{
    public partial class List : PortalModuleBase, IActionable
    {
        #region Properties

        public Guid? ModuleGuid
        {
            get
            {
                return ModuleRepository.Instance.GetModuleGuidByDnnModuleID(this.ModuleId) ?? null;
            }
        }

        public string ModuleName
        {
            get
            {
                return this.ModuleGuid != null ? ModuleRepository.Instance.GetModuleName(this.ModuleGuid.Value) : "";
            }
        }

        public string ScenarioName
        {
            get
            {
                return this.ModuleGuid != null ? ModuleRepository.Instance.GetModuleScenarioName(this.ModuleGuid.Value) : "";
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

        public bool IsModuleAllTabs
        {
            get
            {
                return this.ModuleConfiguration.AllTabs;
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

        public string ConnectionID
        {
            get
            {
                return Request.AnonymousID;
            }
        }

        public string ControlPanelUrl
        {
            get
            {
                string moduleParam = this.ModuleGuid == null ? "d" : "id";
                string moduleParamValue = this.ModuleGuid == null ? this.ModuleId.ToString() : this.ModuleGuid.ToString();

                return ResolveUrl(string.Format("~/DesktopModules/BusinessEngine/Studio.aspx?s={0}&p={1}&a={2}&{3}={4}&m=create-list&return-url={5}", this.ScenarioName, this.PortalId, this.PortalAlias.PortalAliasID, moduleParam, moduleParamValue, "/Default.aspx?tabid=" + this.TabId));
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (this.IsSSR)
            {
                var renderedTemplate = Service.RenderSSR(PortalSettings, ModuleGuid.Value, ConnectionID, this.SiteRoot + "default.aspx?" + this.Page.ClientQueryString, this.UserId);
                if (this.IsSSR && this.IsDisabledFrontFramework)
                    pnlSSR1.Controls.Add(new LiteralControl(renderedTemplate));
                else
                    pnlSSR2.Controls.Add(new LiteralControl(renderedTemplate));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var code = AntiForgery.GetHtml().ToHtmlString();
            pnlAntiForgery.Controls.Add(new LiteralControl(code));

            CtlPageResource.PortalAlias = this.PortalAlias.HTTPAlias;
            CtlPageResource.DnnTabID = this.TabId;
            CtlPageResource.DnnUserID = this.UserId;
            CtlPageResource.DnnUserDisplayName = this.UserInfo.DisplayName;
            CtlPageResource.ModuleGuid = this.ModuleGuid;
            CtlPageResource.ModuleName = this.ModuleName;
            CtlPageResource.IsModuleInAllTabs = this.IsModuleAllTabs;

            CtlPageResource.RegisterPageResources();
        }

        #endregion

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