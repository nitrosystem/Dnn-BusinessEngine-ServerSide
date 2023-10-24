using NitroSystem.Dnn.BusinessEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NitroSystem.Dnn.BusinessEngine.Core.Infrastructure.ClientResources
{
    public class ClientResourceManager
    {
        public static void LoadBatchJs(Page page, string filePath)
        {
            string contents = FileUtil.GetFileContent(filePath);

            string[] lines = contents.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            int i = 1000;

            foreach (var item in lines)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    //RegisterScript(page, string.Format("~/{0}", item), "");
                    ClientResourceManager.RegisterScript(page, string.Format("~/{0}", item), (i++).ToString());
                }
            }
        }

        public static void LoadBatchCss(Page page, string filePath)
        {
            string contents = FileUtil.GetFileContent(filePath);

            string[] lines = contents.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            int i = 10;

            foreach (var item in lines)
            {
                if (!string.IsNullOrEmpty(item)) ClientResourceManager.RegisterStyleSheet(page, string.Format("~/{0}", item), (i++).ToString());
            }
        }

        public static void RegisterStyleSheet(Control container, string cssFilePath, string version, string media = "all")
        {
            var link = new HtmlLink();
            link.Attributes.Add("type", "text/css");
            link.Attributes.Add("rel", "stylesheet");
            link.Href = link.ResolveUrl(cssFilePath) + "?ver=" + version;

            link.Attributes.Add("media", media);

            container.Controls.Add(link);
        }

        public static void RegisterScript(Control container, string scriptFilePath, string version)
        {
            var script = new HtmlGenericControl();
            script.TagName = "script";
            script.Attributes.Add("type", "text/javascript");
            script.Attributes.Add("src", script.ResolveUrl(scriptFilePath) + "?ver=" + version);

            container.Controls.Add(script);
        }
    }
}