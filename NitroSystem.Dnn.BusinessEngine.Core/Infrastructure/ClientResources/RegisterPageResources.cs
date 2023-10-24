using DotNetNuke.Entities.Tabs;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroSystem.Dnn.BusinessEngine.Core.Infrastructure.ClientResources
{
    public static class RegisterPageResources
    {
        public static void RegisterResources(int tabID, Control pnlStyles, Control pnlScripts, string version)
        {
            List<string> files = new List<string>();

            foreach (var item in PageResourceRepository.Instance.GetPageResources(tabID.ToString()))
            {
                if (!files.Contains(item.FilePath))
                {
                    string filePath = item.FilePath;

                    string rtlFile = Path.GetDirectoryName(item.FilePath) + @"\" + Path.GetFileNameWithoutExtension(item.FilePath) + ".rtl.css";

                    if (!CultureInfo.CurrentCulture.TextInfo.IsRightToLeft && File.Exists(HttpContext.Current.Server.MapPath(rtlFile))) filePath = rtlFile;

                    version = version + "-" + item.Version.ToString();

                    if (item.ResourceType == "css")
                        ClientResourceManager.RegisterStyleSheet(pnlStyles, string.Format("~/{0}", filePath), version);
                    if (item.ResourceType == "js")
                        ClientResourceManager.RegisterScript(pnlScripts, string.Format("~/{0}", item.FilePath), version);

                    files.Add(item.FilePath);
                }
            }
        }
    }
}
