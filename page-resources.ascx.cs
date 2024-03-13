using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using NitroSystem.Dnn.BusinessEngine.Services;
using NitroSystem.Dnn.BusinessEngine.Core.ModuleBuilder;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using Newtonsoft.Json;
using NitroSystem.Dnn.BusinessEngine.Core.Extensions.Manifest;
using System.Web.Helpers;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Client.ClientResourceManagement;
using NitroSystem.Dnn.BusinessEngine.Core.Appearance;
using System.Linq;
using NitroSystem.Dnn.BusinessEngine.Modules;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;
using System.Resources;

namespace NitroSystem.Dnn.BusinessEngine.WebControls
{
    public partial class PageResources : UserControl
    {
        #region Properties

        public string PortalAlias { get; set; }

        public int DnnTabID { get; set; }

        public int DnnUserID { get; set; }

        public Guid? ModuleGuid { get; set; }

        public string ModuleName { get; set; }

        public bool IsPanel { get; set; }

        public Control PanelResourcesControl { get; set; }

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
                string domainName = DotNetNuke.Common.Globals.GetPortalDomainName(this.PortalAlias, Request, true);
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

        public bool IsRegisteredPageResources
        {
            get
            {
                return this.Page.Header.FindControl("bEngine_PageResources") != null;
            }
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        internal void RegisterPageResources()
        {
            try
            {
                if (this.ModuleGuid != null)
                {
                    if (this.Page.Header.FindControl("bEngine_baseScript") == null)
                    {
                        var baseScript = new LiteralControl(@"
                            <script type=""text/javascript"">
                                window.bEngineGlobalSettings = {
                                    siteRoot: '" + this.SiteRoot + @"',
                                    apiBaseUrl: '" + this.ApiBaseUrl + @"',
                                    modulePath: '/DesktopModules/BusinessEngine/',
                                    userID: parseInt('" + this.DnnUserID + @"'),
                                    version: '" + this.Version + @"',
                                    debugMode: false
                                };
                            </script>"
                            );

                        baseScript.ID = "bEngine_baseScript";
                        this.Page.Header.Controls.Add(baseScript);
                    }

                    if (!this.IsRegisteredPageResources)
                    {
                        var resources = PageResourceRepository.Instance.GetActivePageResources(this.DnnTabID.ToString());
                        foreach (var item in resources)
                        {
                            RegisterPageResources(item.ResourceType, item.FilePath, item.LoadOrder);
                        }

                        this.Page.Header.Controls.Add(new LiteralControl(@"<span id=""bEngine_PageResources""><!--business engine registered resources--></span>"));
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void RegisterPageResources(string resourceType, string filepath, int priority)
        {
            if (this.IsPanel && this.PanelResourcesControl != null)
            {
                if (resourceType == "css")
                {
                    bool notFound = true;

                    if (1 == 1 || CultureInfo.CurrentCulture.TextInfo.IsRightToLeft)
                    {
                        string rtlFilePath = string.Empty;
                        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filepath);
                        if (!string.IsNullOrEmpty(fileNameWithoutExtension) && fileNameWithoutExtension.ToLower().EndsWith(".min"))
                            rtlFilePath = Path.GetDirectoryName(filepath) + @"\" + Path.GetFileNameWithoutExtension(fileNameWithoutExtension) + ".rtl.min" + Path.GetExtension(filepath);
                        else
                            rtlFilePath = Path.GetDirectoryName(filepath) + @"\" +
                                Path.GetFileNameWithoutExtension(filepath) + ".rtl" + Path.GetExtension(filepath);

                        if (File.Exists(MapPath(rtlFilePath)))
                        {
                            Core.Infrastructure.ClientResources.ClientResourceManager.RegisterStyleSheet(this.PanelResourcesControl, rtlFilePath, this.Version);
                            notFound = false;
                        }
                    }

                    if (notFound) Core.Infrastructure.ClientResources.ClientResourceManager.RegisterStyleSheet(this.PanelResourcesControl, filepath, this.Version);

                }
                if (resourceType == "js")
                    Core.Infrastructure.ClientResources.ClientResourceManager.RegisterScript(this.PanelResourcesControl, filepath, this.Version);
            }
            else
            {
                if (resourceType == "css")
                    ClientResourceManager.RegisterStyleSheet(base.Page, filepath, priority);
                if (resourceType == "js")
                    ClientResourceManager.RegisterScript(base.Page, filepath, priority);
            }
        }
    }
}