using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using DotNetNuke.Common.Utilities;

namespace NitroSystem.Dnn.BusinessEngine.Utilities
{
    public static class ResxFileUtil
    {
        #region Resx File Methods

        public static void UpdateStringInResx(string resource, string curCulture, string key, string value)
        {
            string currentCulture = (curCulture.ToLower() == "en-us" ? "" : "." + curCulture);
            var resourceFilePath = HttpRuntime.AppDomainAppPath + string.Format("{0}{1}.resx", resource, currentCulture);
            if (!File.Exists(resourceFilePath))
                resourceFilePath = HttpRuntime.AppDomainAppPath + string.Format("{0}.resx", resource);

            if (!File.Exists(resourceFilePath))
                return;

            try
            {
                XmlNode node;
                XmlNode nodeData;
                XmlAttribute attr;

                var resDoc = new XmlDocument();
                resDoc.Load(resourceFilePath);

                node = resDoc.SelectSingleNode("//root/data[@name='" + key + "']/value");
                if (node == null)
                {
                    //missing entry
                    nodeData = resDoc.CreateElement("data");
                    attr = resDoc.CreateAttribute("name");
                    attr.Value = key;
                    nodeData.Attributes.Append(attr);
                    resDoc.SelectSingleNode("//root").AppendChild(nodeData);

                    node = nodeData.AppendChild(resDoc.CreateElement("value"));
                }
                node.InnerXml = value;

                resDoc.Save(resourceFilePath);
            }
            catch
            {
            }
        }

        public static Hashtable GetLocalesFromResxFile(string resource, string culture)
        {
            string currentCulture = (culture.ToLower() == "en-us" ? "" : "." + culture);
            var resourceFilePath = HttpRuntime.AppDomainAppPath + string.Format("{0}{1}.resx", resource, currentCulture);
            if (!File.Exists(resourceFilePath))
                resourceFilePath = HttpRuntime.AppDomainAppPath + string.Format("{0}.resx", resource);

            var cacheKey = Path.GetFileNameWithoutExtension(resourceFilePath);

            Hashtable result = null;// DataCache.GetCache<Hashtable>(cacheKey);
            if (result == null)
            {
                result = new Hashtable();

                //async
                //try
                //{
                //    using (var file = new FileStream(resourceFilePath, FileMode.Open))
                //    using (XmlReader r = XmlReader.Create(file, new XmlReaderSettings() { Async = true }))
                //    {
                //        r.MoveToContent();
                //        r.ReadToDescendant("data");

                //        while (await r.ReadAsync())
                //        {
                //            if ((r.NodeType == XmlNodeType.Element))
                //            {
                //                string name = r.GetAttribute("name");

                //                if (string.IsNullOrEmpty(name)) continue;

                //                string val = string.Empty;
                //                if (r.ReadToDescendant("value"))
                //                {
                //                    val = await r.ReadInnerXmlAsync();
                //                    r.ReadToNextSibling("data");
                //                }

                //                if (result[name] == null)
                //                {
                //                    result.Add(name, val);
                //                }
                //                else
                //                {
                //                    result[name] = val;
                //                }
                //            }
                //        }
                //    }
                //    DataCache.SetCache(cacheKey, result);
                //}
                //catch (Exception ex2)
                //{
                //}

                var d = new XmlDocument();
                bool xmlLoaded = false;
                try
                {
                    d.Load(resourceFilePath);
                    xmlLoaded = true;
                }
                catch
                {
                    xmlLoaded = false;
                }
                if (xmlLoaded)
                {
                    XmlNode n = null;
                    foreach (XmlNode n_loopVariable in d.SelectNodes("root/data"))
                    {
                        try
                        {
                            n = n_loopVariable;
                            if (n.NodeType != XmlNodeType.Comment)
                            {
                                string name = n.Attributes["name"].Value;
                                string val = n.SelectSingleNode("value").InnerXml;
                                if (!string.IsNullOrEmpty(name) && name.EndsWith(".Text")) name = name.Replace(".Text", string.Empty);
                                if (result[name] == null)
                                {
                                    result.Add(name, val);
                                }
                                else
                                {
                                    result[name] = val;
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                DataCache.SetCache(cacheKey, result);
            }

            return result;
        }

        public static string GetString(string resource, string curCulture, string key)
        {
            var locales = GetLocalesFromResxFile(resource, curCulture);

            return locales[key].ToString();
        }

        #endregion  
    }
}