using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using System;
using System.Globalization;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Modules
{
    public partial class Studio : PortalModuleBase
    {
        public string ScenarioNameParam
        {
            get
            {
                return Request.QueryString["s"];
            }
        }

        public string ModuleTypeParam
        {
            get
            {
                return Request.QueryString["m"];
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

        public Guid? ModuleGuid
        {
            get
            {
                return ModuleRepository.Instance.GetModuleGuidByDnnModuleID(this.ModuleId);
            }
        }

        public string ApplicationPath
        {
            get
            {
                return DotNetNuke.Common.Globals.ApplicationPath;
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

        public bool IsRtl
        {
            get
            {
                return CultureInfo.CurrentCulture.TextInfo.IsRightToLeft;
            }
        }

        public string Version
        {
            get
            {
                return Host.CrmVersion.ToString();
            }
        }
    }
}