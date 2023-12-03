using DotNetNuke.Abstractions.Portals;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using NitroSystem.Dnn.BusinessEngine.Core.Infrastructure.ClientResources;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroSystem.Dnn.BusinessEngine.Modules.StudioPage
{
    public partial class Studio : System.Web.UI.Page
    {
        #region Properties

        public string ScenarioNameParam
        {
            get
            {
                return Request.QueryString["s"];
            }
        }

        public string PortalIDParam
        {
            get
            {
                return Request.QueryString["p"];
            }
        }

        public string PortalAliasIDParam
        {
            get
            {
                return Request.QueryString["a"];
            }
        }

        public string DnnModuleIDParam
        {
            get
            {
                return Request.QueryString["d"];
            }
        }

        public string ModuleIDParam
        {
            get
            {
                return Request.QueryString["id"];
            }
        }

        public string ModuleTypeParam
        {
            get
            {
                return Request.QueryString["m"];
            }
        }

        public UserInfo UserInfo
        {
            get
            {
                return UserController.Instance.GetCurrentUserInfo();
            }
        }

        public int UserID
        {
            get
            {
                return this.UserInfo.UserID;
            }
        }

        public Guid ScenarioID
        {
            get
            {
                var scenario = ScenarioRepository.Instance.GetScenario(this.ScenarioNameParam);
                return scenario != null ? scenario.ScenarioID : Guid.Empty;
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
                PortalAliasInfo objPortalAliasInfo = PortalAliasController.Instance.GetPortalAliasByPortalAliasID(int.Parse(this.PortalAliasIDParam));

                string domainName = DotNetNuke.Common.Globals.GetPortalDomainName(objPortalAliasInfo.HTTPAlias, Request, true);
                return domainName + "/DesktopModules/";
            }
        }

        public string ConnectionID
        {
            get
            {
                return Request.AnonymousID;
            }
        }

        public string Version
        {
            get
            {
                return Host.CrmVersion.ToString();
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #region EventHandler
        protected void Page_Init(object sender, EventArgs e)
        {
            var code = AntiForgery.GetHtml().ToHtmlString();
            pnlAntiForgery.Controls.Add(new LiteralControl(code));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.UserInfo.UserID == -1)
                Response.Redirect(DotNetNuke.Common.Globals.LoginURL(HttpUtility.UrlEncode(this.Request.Url.PathAndQuery), true));

            if (!this.UserInfo.IsInRole("Administrators"))
                Response.Redirect(DotNetNuke.Common.Globals.AccessDeniedURL());

            var resources = StudioResourceRepository.Instance.GetActiveStudioResources();
            foreach (var item in resources)
            {
                if (item.ResourceType == "css")
                    ClientResourceManager.RegisterStyleSheet(pnlResources, item.FilePath, "");
                if (item.ResourceType == "js")
                    ClientResourceManager.RegisterScript(pnlResources, item.FilePath, "");
            }
        }

        #endregion  
    }
}