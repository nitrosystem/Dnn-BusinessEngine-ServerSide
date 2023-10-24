using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Utilities
{
    public static class HtmlUtil
    {
        public static string RemoveHTML(string sText)
        {
            if (string.IsNullOrEmpty(sText))
            {
                return string.Empty;
            }
            sText = HttpUtility.HtmlDecode(sText);
            sText = HttpUtility.UrlDecode(sText);
            sText = sText.Trim();
            if (string.IsNullOrEmpty(sText))
            {
                return string.Empty;
            }
            string pattern = "<(.|\\n)*?>";
            sText = Regex.Replace(sText, pattern, string.Empty, RegexOptions.IgnoreCase);
            sText = HttpUtility.HtmlEncode(sText);
            return sText;
        }

        public static string RemoveScript(string sText)
        {
            if (string.IsNullOrEmpty(sText))
            {
                return string.Empty;
            }

            Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
            return rRemScript.Replace(sText, "");
        }

        public static bool IsHtmlFragment(string value)
        {
            return Regex.IsMatch(value, @"</?(p|div)>");
        }

        /// <summary>
        /// Remove tags from a html string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveTags(string value)
        {
            if (value != null)
            {
                value = CleanHtmlComments(value);
                value = CleanHtmlBehaviour(value);
                value = Regex.Replace(value, @"</[^>]+?>", " ");
                value = Regex.Replace(value, @"<[^>]+?>", "");
                value = value.Trim();
            }
            return value;
        }

        /// <summary>
        /// Clean script and styles html tags and content
        /// </summary>
        /// <returns></returns>
        public static string CleanHtmlBehaviour(string value)
        {
            value = Regex.Replace(value, "(<style.+?</style>)|(<script.+?</script>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return value;
        }

        /// <summary>
        /// Replace the html commens (also html ifs of msword).
        /// </summary>
        public static string CleanHtmlComments(string value)
        {
            //Remove disallowed html tags.
            value = Regex.Replace(value, "<!--.+?-->", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return value;
        }

        /// <summary>
        /// Adds rel=nofollow to html anchors
        /// </summary>
        public static string HtmlLinkAddNoFollow(string value)
        {
            return Regex.Replace(value, "<a[^>]+href=\"?'?(?!#[\\w-]+)([^'\">]+)\"?'?[^>]*>(.*?)</a>", "<a href=\"$1\" rel=\"nofollow\" target=\"_blank\">$2</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
    }
}
