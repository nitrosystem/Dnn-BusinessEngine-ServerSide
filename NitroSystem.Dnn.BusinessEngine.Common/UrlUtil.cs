using DotNetNuke.Entities.Controllers;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Portals;
using NitroSystem.Dnn.BusinessEngine.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Utilities
{
    public sealed class UrlUtil
    {
        public static string TrateFriendlyURL(string title)
        {
            if (string.IsNullOrEmpty(title))                 return "";

            title = title.ToLower();

            string str = title;
            char[] arr = title.ToCharArray();
            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-')));
            str = new string(arr);
            str = str.Replace(' ', '-');

            if (str.Length > 150)
            {
                str = str.Substring(0, 150);
            }
            while ((str.Length > 1) && str.EndsWith("-"))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

        public static string GetFriendlyViewURL(string title, string urlTitle, DotNetNuke.Entities.Tabs.TabInfo tabInfo, bool checkURL, string replacepagename, params string[] additionalParameters)
        {
            var portalSettings = new PortalSettings(tabInfo.PortalID);

            string url = "~/default.aspx?tabid=" + tabInfo.TabID;
            if (additionalParameters != null)
            {
                foreach (string str2 in additionalParameters)
                {
                    url = url + "&" + str2;
                }
            }

            string portalAlias = portalSettings.DefaultPortalAlias;

            if (System.Web.HttpContext.Current == null)
            {
                return (DotNetNuke.Common.Globals.AddHTTP(portalAlias.ToLower()) + url.Substring(1));
            }
            if (HostController.Instance.GetSettingsDictionary().ContainsKey("UseFriendlyUrls") && (HostController.Instance.GetSettingsDictionary()["UseFriendlyUrls"].ToString() != "Y"))
            {
                return DotNetNuke.Common.Globals.AddHTTP(System.Web.HttpContext.Current.Request.Url.Host + DotNetNuke.Common.Globals.ResolveUrl(url));
            }
            if ((DotNetNuke.Services.Localization.LocaleController.Instance.GetLocales(tabInfo.PortalID).Count > 1) && portalSettings.EnableUrlLanguage)
            {
                string language = System.Web.HttpContext.Current.Request["language"];
                if (string.IsNullOrEmpty(language))
                {
                    url = url + "&language=" + System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                }
                else
                {
                    url = url + "&language=" + language;
                }
            }
            if (string.IsNullOrEmpty(title))
            {
                title = " ";
            }
            string pageName = TrateFriendlyURL(urlTitle);
            if (string.IsNullOrEmpty(urlTitle))
            {
                pageName = TrateFriendlyURL(title);
            }
            pageName = pageName + ".aspx";
            if (pageName.EndsWith("-.aspx"))
            {
                pageName = pageName.Replace("-.aspx", ".aspx");
            }
            if (PortalSettings.Current.PortalId != tabInfo.PortalID)
            {
                return DotNetNuke.Common.Globals.FriendlyUrl(tabInfo, url, pageName, portalAlias);
            }
            string friendlyurl = DotNetNuke.Common.Globals.FriendlyUrl(tabInfo, url, pageName);
            if (!checkURL)
            {
                return friendlyurl;
            }
            string friendlyurl2 = DotNetNuke.Common.Globals.FriendlyUrl(tabInfo, url);
            if (friendlyurl2 == friendlyurl)
            {
                if (friendlyurl2.EndsWith(".aspx"))
                {
                    friendlyurl = friendlyurl2.Substring(0, friendlyurl2.Length - 5) + "/" + pageName;
                }
                else
                {
                    if (pageName.EndsWith(".aspx"))
                    {
                        pageName = pageName.Substring(0, pageName.Length - 5);
                    }
                    friendlyurl = friendlyurl2 + "/" + pageName;
                }
            }
            if (string.IsNullOrEmpty(replacepagename))
            {
                return friendlyurl;
            }
            if (replacepagename.StartsWith("/") && (replacepagename.Length > 1))
            {
                replacepagename = replacepagename.Substring(1);
            }
            if (replacepagename.EndsWith("/"))
            {
                replacepagename = (replacepagename.Length > 1) ? replacepagename.Substring(0, replacepagename.Length - 1) : "";
            }
            if (string.IsNullOrEmpty(replacepagename))
            {
                return friendlyurl;
            }
            int index = friendlyurl2.IndexOf("://", StringComparison.Ordinal);
            string domain = "";
            if ((index > 0) && (friendlyurl2.Length > index + 3))
            {
                domain = friendlyurl2.Substring(index + 3);
                domain = friendlyurl2.Substring(0, index + 3) + domain.Substring(0, domain.IndexOf("/", StringComparison.Ordinal));
            }
            PortalAliasInfo info = PortalSettings.Current.PortalAlias;
            return (DotNetNuke.Common.Globals.AddHTTP(info.HTTPAlias) + "/" + replacepagename + "/" + pageName);
        }

        public static LinkInfo GetLinkData(string URL)
        {
            string sPage = GetPageFromURL(ref URL, string.Empty, string.Empty);
            LinkInfo link = new LinkInfo();
            if (string.IsNullOrEmpty(sPage))
            {
                return link;
            }
            string sTitle = string.Empty;
            string sDescription = string.Empty;
            string sImage = string.Empty;

            link.URL = URL;
            link.Images = new List<ImageInfo>();
            Match m = Regex.Match(sPage, "<(title)[^>]*?>((?:.|\\n)*?)</\\s*\\1\\s*>", RegexOptions.IgnoreCase & RegexOptions.Multiline);
            if (m.Success)
            {
                link.Title = m.Groups[2].ToString().Trim();
            }
            //
            Regex regExp = new Regex("<meta\\s*(?:(?:\\b(\\w|-)+\\b\\s*(?:=\\s*(?:\"[^\"]*\"|'[^']*'|[^\"'<> ]+)\\s*)?)*)/?\\s*>", RegexOptions.IgnoreCase & RegexOptions.Multiline);
            MatchCollection matches = default(MatchCollection);
            matches = regExp.Matches(sPage);
            int i = 0;
            foreach (Match match in matches)
            {
                string sTempDesc = match.Groups[0].Value;
                Regex subReg = new Regex("<meta[\\s]+[^>]*?(((name|property)*?[\\s]?=[\\s\\x27\\x22]+(.*?)[\\x27\\x22]+.*?)|(content*?[\\s]?=[\\s\\x27\\x22]+(.*?)[\\x27\\x22]+.*?))((content*?[\\s]?=[\\s\\x27\\x22]+(.*?)[\\x27\\x22]+.*?>)|(name*?[\\s]?=[\\s\\x27\\x22]+(.*?)[\\x27\\x22]+.*?>)|>)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
                foreach (Match subM in subReg.Matches(sTempDesc))
                {
                    if (subM.Groups[4].Value.ToUpperInvariant() == "OG:DESCRIPTION")
                    {
                        link.Description = subM.Groups[9].Value;
                    }
                    else if (subM.Groups[4].Value.ToUpperInvariant() == "DESCRIPTION".ToUpperInvariant())
                    {
                        link.Description = subM.Groups[9].Value;
                    }
                    if (subM.Groups[4].Value.ToUpperInvariant() == "OG:TITLE")
                    {
                        link.Title = subM.Groups[9].Value;
                    }

                    if (subM.Groups[4].Value.ToUpperInvariant() == "OG:IMAGE")
                    {
                        sImage = subM.Groups[9].Value;
                        ImageInfo img = new ImageInfo();
                        img.URL = sImage;
                        link.Images.Add(img);
                        i += 1;
                    }
                }
            }
            if (!string.IsNullOrEmpty(link.Description))
            {
                link.Description = HttpUtility.HtmlDecode(link.Description);
                link.Description = HttpUtility.UrlDecode(link.Description);
                link.Description = HtmlUtil.RemoveHTML(link.Description);
            }
            if (!string.IsNullOrEmpty(link.Title))
            {
                link.Title = link.Title.Replace("&amp;", "&");
            }
            regExp = new Regex("<img[\\s]+[^>]*?((alt*?[\\s]?=[\\s\\x27\\x22]+(.*?)[\\x27\\x22]+.*?)|(src*?[\\s]?=[\\s\\x27\\x22]+(.*?)[\\x27\\x22]+.*?))((src*?[\\s]?=[\\s\\x27\\x22]+(.*?)[\\x27\\x22]+.*?>)|(alt*?[\\s]?=[\\s\\x27\\x22]+(.*?)[\\x27\\x22]+.*?>)|>)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            matches = regExp.Matches(sPage);

            string imgList = string.Empty;
            string hostUrl = string.Empty;
            if (!URL.Contains("http"))
            {
                URL = "http://" + URL;
            }
            Uri uri = new Uri(URL);
            hostUrl = uri.Host;
            if (URL.Contains("https:"))
            {
                hostUrl = "https://" + hostUrl;
            }
            else
            {
                hostUrl = "http://" + hostUrl;
            }
            foreach (Match match in matches)
            {
                string sImg = match.Groups[5].Value;
                if (string.IsNullOrEmpty(sImg))
                {
                    sImg = match.Groups[8].Value;
                }
                if (!string.IsNullOrEmpty(sImg))
                {
                    if (!sImg.Contains("http"))
                    {
                        sImg = hostUrl + sImg;
                    }

                    ImageInfo img = new ImageInfo();
                    img.URL = sImg;
                    if (!imgList.Contains(sImg))
                    {
                        Bitmap bmp = GetImageFromURL(sImg);
                        if ((bmp != null))
                        {
                            if (bmp.Height > 25 & bmp.Height < 500 & bmp.Width > 25 & bmp.Width < 500)
                            {
                                link.Images.Add(img);
                                imgList += sImg;
                                i += 1;

                            }
                        }
                    }
                    if (i == 10)
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }

            }
            return link;
        }

        public static string GetPageFromURL(ref string url, string username, string password)
        {

            url = PrepareURL(url);
            HttpWebRequest objWebRequest = default(HttpWebRequest);
            HttpWebResponse objWebResponse = default(HttpWebResponse);
            CookieContainer cookies = new CookieContainer();
            Uri objURI = new Uri(url);
            objWebRequest = (HttpWebRequest)HttpWebRequest.Create(objURI);
            objWebRequest.KeepAlive = false;
            objWebRequest.Proxy = null;
            objWebRequest.CookieContainer = cookies;
            if (!string.IsNullOrEmpty(username) & !string.IsNullOrEmpty(password))
            {
                NetworkCredential nc = new NetworkCredential(username, password);
                objWebRequest.Credentials = nc;
            }
            string sHTML = string.Empty;
            try
            {
                objWebResponse = (HttpWebResponse)objWebRequest.GetResponse();
                Encoding enc = Encoding.UTF8;


                string contentType = objWebResponse.ContentType;

                if ((objWebRequest.HaveResponse == true) & objWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    objWebResponse.Cookies = objWebRequest.CookieContainer.GetCookies(objWebRequest.RequestUri);
                    using (Stream objStream = objWebResponse.GetResponseStream())
                    {
                        using (StreamReader objStreamReader = new StreamReader(objStream, enc))
                        {
                            sHTML = objStreamReader.ReadToEnd();
                            objStreamReader.Close();
                        }
                        objStream.Close();
                    }
                }
                objWebResponse.Close();
            }
            catch (Exception ex)
            {
            }

            return sHTML;
        }

        public static Bitmap GetImageFromURL(string url)
        {
            string sImgName = string.Empty;
            System.Net.WebRequest myRequest = default(System.Net.WebRequest);
            Bitmap bmp = null;
            try
            {
                myRequest = System.Net.WebRequest.Create(url);
                myRequest.Proxy = null;
                using (WebResponse myResponse = myRequest.GetResponse())
                {
                    using (Stream myStream = myResponse.GetResponseStream())
                    {
                        string sContentType = myResponse.ContentType;
                        string sExt = string.Empty;
                        if (sContentType.Contains("png"))
                        {
                            sExt = ".png";
                        }
                        else if (sContentType.Contains("jpg"))
                        {
                            sExt = ".jpg";
                        }
                        else if (sContentType.Contains("jpeg"))
                        {
                            sExt = ".jpg";
                        }
                        else if (sContentType.Contains("gif"))
                        {
                            sExt = ".gif";
                        }
                        if (!string.IsNullOrEmpty(sExt))
                        {
                            bmp = new Bitmap(myStream);
                        }

                    }
                }

                return bmp;
            }
            catch
            {
                return null;
            }
        }

        private static string PrepareURL(string url)
        {
            url = url.Trim();
            if (!url.StartsWith("http"))
            {
                url = "http://" + url;
            }
            if (url.Contains("https://"))
            {
                url = url.Replace("https://", "http://");
            }
            if (url.Contains("http://http://"))
            {
                url = url.Replace("http://http://", "http://");
            }
            if (!(url.IndexOf("http://") == 0))
            {
                url = "http://" + url;
            }
            Uri objURI = null;

            objURI = new Uri(url);
            return url;
        }

        /// <summary>
        /// Creates a URL And SEO friendly slug
        /// </summary>
        /// <param name="text">Text to slugify</param>
        /// <param name="maxLength">Max length of slug</param>
        /// <returns>URL and SEO friendly string</returns>
        public static string UrlFriendly(string text, int maxLength = 0)
        {
            // Return empty value if text is null
            if (text == null) return "";
            var normalizedString = text
                // Make lowercase
                .ToLowerInvariant()
                // Normalize the text
                .Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            var stringLength = normalizedString.Length;
            var prevdash = false;
            var trueLength = 0;
            char c;
            for (int i = 0; i < stringLength; i++)
            {
                c = normalizedString[i];
                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    // Check if the character is a letter or a digit if the character is a
                    // international character remap it to an ascii valid character
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        if (c < 128)
                            stringBuilder.Append(c);
                        else
                            stringBuilder.Append(RemapInternationalCharToAscii(c));
                        prevdash = false;
                        trueLength = stringBuilder.Length;
                        break;
                    // Check if the character is to be replaced by a hyphen but only if the last character wasn't
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.DashPunctuation:
                    case UnicodeCategory.OtherPunctuation:
                    case UnicodeCategory.MathSymbol:
                        if (!prevdash)
                        {
                            stringBuilder.Append('-');
                            prevdash = true;
                            trueLength = stringBuilder.Length;
                        }
                        break;
                }
                // If we are at max length, stop parsing
                if (maxLength > 0 && trueLength >= maxLength)
                    break;
            }
            // Trim excess hyphens
            var result = stringBuilder.ToString().Trim('-');
            // Remove any excess character to meet maxlength criteria
            return maxLength <= 0 || result.Length <= maxLength ? result : result.Substring(0, maxLength);
        }

        /// <summary>
        /// Remaps international characters to ascii compatible ones
        /// based of: https://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
        /// </summary>
        /// <param name="c">Charcter to remap</param>
        /// <returns>Remapped character</returns>
        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }
    }
}