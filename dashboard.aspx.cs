using System;
using System.Web;
using System.Linq;
using System.Web.Script.Serialization;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using DotNetNuke.Entities.Modules;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;
using DotNetNuke.Entities.Host;
using NitroSystem.Dnn.BusinessEngine.Components;
using System.Text;
using DotNetNuke.Web.Client.ClientResourceManagement;
using System.Collections.Generic;
using System.IO;
using NitroSystem.Dnn.BusinessEngine.Core.Appearance;
using System.Web.Security;
using System.Web.Helpers;
using System.Web.UI;

namespace NitroSystem.Dnn.BusinessEngine
{
    public partial class Dashboard : System.Web.UI.Page
    {
        #region Properties

        private PortalSettings Portal
        {
            get
            {
                int portalID = DashboardInfo != null ? DashboardInfo.PortalID : PortalSettings.Current.PortalId;

                return new PortalSettings(portalID);
            }
        }

        public DashboardView DashboardInfo
        {
            get
            {
                return DashboardRepository.Instance.GetDashboardByUniqueName(Request.QueryString["d"]);
            }
        }

        public UserInfo UserInfo
        {
            get
            {
                string userName = HttpContext.Current != null ? HttpContext.Current.User.Identity.Name : "";
                if (string.IsNullOrEmpty(userName))
                    return new UserInfo();
                else
                {
                    var user = UserController.GetUserByName(userName);
                    return user != null ? user : new UserInfo();
                }
            }
        }

        public string PageTitle
        {
            get
            {
                return Portal.PortalName;
            }
        }

        public int UserID
        {
            get
            {
                return this.UserInfo.UserID;
            }
        }

        public int DnnTabID
        {
            get
            {
                //if (Request.QueryString["_t"] != null)
                //    return int.Parse(Request.QueryString["_t"]);
                //else
                //    return 0;

                return DashboardInfo != null ? ModuleController.Instance.GetModulesByDefinition(Portal.PortalId, "BusinessEngine.Dashboard").Cast<DotNetNuke.Entities.Modules.ModuleInfo>().First(m => m.ModuleID == DashboardInfo.DnnModuleID).TabID : -1;
            }
        }

        public int DnnModuleID
        {
            get
            {
                return DashboardInfo != null ? DashboardInfo.DnnModuleID : -1;
            }
        }

        public Guid? ModuleGuid
        {
            get
            {
                return ModuleRepository.Instance.GetModuleGuidByDnnModuleID(this.DnnModuleID);
            }
        }

        public string ModuleName
        {
            get
            {
                return this.ModuleGuid != null ? ModuleRepository.Instance.GetModuleName(this.ModuleGuid.Value) : "";
            }
        }

        public int PortalID
        {
            get
            {
                return this.Portal.PortalId;
            }
        }

        public string UserDisplayName
        {
            get
            {
                return this.UserInfo.DisplayName;
            }
        }

        public string UserPhoto
        {
            get
            {
                return string.Format("/DnnImageHandler.ashx?mode=profilepic&userId={0}&w=64", this.UserInfo.UserID);
            }
        }

        public string UserRoles
        {
            get
            {
                return this.UserInfo.IsSuperUser ? "Super User" : String.Join(",", this.UserInfo.Roles.Where(r => r != "Subscribers" && r != "Registered Users" && r != "All Users"));
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
                PortalAliasInfo objPortalAliasInfo = PortalAliasController.Instance.GetPortalAlias(DotNetNuke.Common.Globals.GetDomainName(this.Context.Request));

                string domainName = DotNetNuke.Common.Globals.GetPortalDomainName(objPortalAliasInfo != null ? objPortalAliasInfo.HTTPAlias : this.Portal.DefaultPortalAlias, Request, true);
                return domainName + "/DesktopModules/";
            }
        }

        public string Version
        {
            get
            {
                return Host.CrmVersion.ToString();
            }
        }

        public string ModuleType
        {
            get
            {
                return Request.QueryString["module"];
            }
        }

        public string ConnectionID
        {
            get
            {
                return Request.AnonymousID;
            }
        }

        public string BodyID { get; set; }
        public string BodyClass { get; set; }

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            pnlAntiForgery.Controls.Add(new LiteralControl(AntiForgery.GetHtml().ToHtmlString()));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.UserInfo.UserID == -1 || !(UserInfo.IsSuperUser || string.IsNullOrEmpty(this.DashboardInfo.AuthorizationViewDashboard) || UserInfo.Roles.Any(r => this.DashboardInfo.AuthorizationViewDashboard.Split(',').Contains(r))))
                Response.Redirect(DotNetNuke.Common.Globals.LoginURL(HttpUtility.UrlEncode(this.Request.Url.PathAndQuery), true));

            if (this.DashboardInfo != null)
            {
                List<string> files = new List<string>();

                foreach (var item in PageResourceRepository.Instance.GetPageResources(this.DnnTabID.ToString()))
                {
                    if (!files.Contains(item.FilePath))
                    {
                        string filePath = item.FilePath;

                        string rtlFile = Path.GetDirectoryName(item.FilePath) + @"\" + Path.GetFileNameWithoutExtension(item.FilePath) + ".rtl.css";

                        if (!System.Globalization.CultureInfo.CurrentCulture.TextInfo.IsRightToLeft && File.Exists(MapPath(rtlFile)))
                            filePath = rtlFile;

                        //if (item.ResourceType == "css")
                        //    ClientResourceManager.RegisterStyleSheet(this.Page, string.Format("~/{0}", filePath), 0);
                        //if (item.ResourceType == "js")
                        //    ClientResourceManager.RegisterScript(this, item.FilePath, 0, "BusinessEngine", "bEngineStyleSheet", item.Version.ToString());

                        string version = this.Version + "-" + item.Version.ToString();

                        if (item.ResourceType == "css")
                            Core.Infrastructure.ClientResources.ClientResourceManager.RegisterStyleSheet(pnlStyles, string.Format("~/{0}", filePath), version);
                        if (item.ResourceType == "js")
                            Core.Infrastructure.ClientResources.ClientResourceManager.RegisterScript(pnlScripts, string.Format("~/{0}", item.FilePath), version);

                        files.Add(item.FilePath);
                    }
                }

                var skin = ModuleSkinManager.GetSkin(this.DashboardInfo.Skin);
                if (skin != null)
                {
                    var template = skin.DashboardTemplates.FirstOrDefault(t => t.TemplateName == this.DashboardInfo.Template);
                    if (template != null && template.BodyOptions != null)
                    {
                        this.BodyID = template.BodyOptions.Value<string>("id");
                        this.BodyClass = template.BodyOptions.Value<string>("class");
                    }
                }
            }

            if (string.IsNullOrEmpty(this.BodyID)) this.BodyID = "bEngineDashboard";
        }
    }
}