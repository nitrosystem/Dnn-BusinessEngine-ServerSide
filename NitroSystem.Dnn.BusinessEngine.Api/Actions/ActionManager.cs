using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Components;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;

namespace NitroSystem.Dnn.BusinessEngine.Actions
{
    public class ActionManager
    {
        //public JObject Scope { get; set; }
        //public IEnumerable<ActionViewModel> ModuleActions { get; set; }

        //public ActionManager()
        //{
        //    Scope = new JObject();
        //}

        //public void CallActions(Page page, Guid moduleID, string eventName)
        //{
        //    ModuleActions = ViewModelMapping.GetActionsViewModel(moduleID);

        //    var actions = ModuleActions.Where(a => a.Event == eventName);

        //    CallActions(actions, page);
        //}

        //public void CallActions(IEnumerable<ActionViewModel> actions, Page page)
        //{
        //    foreach (var action in actions)
        //    {
        //        switch (action.ActionType)
        //        {
        //            case "GetDataRow":
        //                Hashtable paramList = new Hashtable();

        //                var serviceParams = action.ActionDetails["Params"] as JArray;
        //                foreach (JObject param in serviceParams)
        //                {
        //                    string name = param["ParamName"] != null ? param["ParamName"].Value<string>() : null;
        //                    string value = param["ParamValue"] != null ? param["ParamValue"].Value<string>() : null;
        //                    if (value != null) value = GetPropertyValue(value, page);

        //                    paramList.Add(name, value);
        //                }

        //                var service = ViewModelMapping.GetServiceViewModel(action.ServiceID.Value);

        //                var result = ServiceManager.ExecuteStoredProcedure(service, paramList);

        //                var json = JsonConvert.SerializeObject(result);

        //                var listName = action.ActionDetails["ResultName"].Value<string>().Replace("{", "").Replace("}", "");

        //                var jsonValue = JsonConvert.DeserializeObject<JObject>(json);
        //                foreach (JProperty item in jsonValue)
        //                {
        //                    if (item.Value != null && General.IsJsonObject(item.Value.ToString()))
        //                        item.Value = JsonConvert.DeserializeObject<JObject>(item.Value.ToString());
        //                    if (item.Value != null && General.IsJsonArray(item.Value.ToString()))
        //                        item.Value = JsonConvert.DeserializeObject<JArray>(item.Value.ToString());
        //                }

        //                Scope.Add(listName, jsonValue);

        //                break;
        //            case "SEO":
        //                var seo = action.ActionDetails["SeoDocument"] != null ? action.ActionDetails["SeoDocument"].Value<string>() : string.Empty;

        //                var objSeoInfo = JsonConvert.DeserializeObject<SeoInfo>(seo);

        //                //Basic Page Seo
        //                objSeoInfo.Page.Title = GetPropertyValue(objSeoInfo.Page.Title, page);
        //                objSeoInfo.Page.Description = GetPropertyValue(objSeoInfo.Page.Description, page);
        //                objSeoInfo.Page.Keywords = GetPropertyValue(objSeoInfo.Page.Keywords, page);

        //                page.Title = objSeoInfo.Page.Title;
        //                page.MetaDescription = objSeoInfo.Page.Description;
        //                page.MetaKeywords = objSeoInfo.Page.Keywords;

        //                //Meta Tags
        //                if (!string.IsNullOrEmpty(objSeoInfo.MetaTags))
        //                {
        //                    objSeoInfo.MetaTags = GetPropertyValue(objSeoInfo.MetaTags, page);

        //                    page.Header.Controls.Add(new LiteralControl(objSeoInfo.MetaTags));
        //                }

        //                //Structured Data
        //                if (objSeoInfo.StructuredData != null && objSeoInfo.StructuredData.Any())
        //                {
        //                    foreach (var scriptStr in objSeoInfo.StructuredData)
        //                    {
        //                        page.Header.Controls.Add(new LiteralControl(GetPropertyValue(scriptStr, page)));
        //                    }
        //                }

        //                break;
        //        }

        //        var childActions = ModuleActions.Where(a => a.ParentID == action.ActionID);
        //        if (childActions.Any()) CallActions(childActions, page);
        //    }
        //}

        //public string GetPropertyValue(string propertyStr, Page page, string pageUrl = null, UserInfo userInfo = null, PortalSettings portalSettings = null, JObject localStorage = null, JObject sessionStorage = null)
        //{
        //    if (string.IsNullOrEmpty(propertyStr)) return propertyStr;

        //    //Page Params
        //    var matches = Regex.Matches(propertyStr, "{PageParam:(\\w+)}", RegexOptions.IgnoreCase);
        //    foreach (Match match in matches)
        //    {
        //        var paramName = match.Groups[1].Value;

        //        var value = page.Request.QueryString[paramName];
        //        if (value == null)
        //        {
        //            var friendlyParams = page.Request.RawUrl.Split('/').ToList();
        //            if (friendlyParams.Contains(paramName) && friendlyParams.Count >= friendlyParams.IndexOf(paramName) + 2)
        //                value = friendlyParams[friendlyParams.IndexOf(paramName) + 1];
        //        }

        //        propertyStr = propertyStr.Replace(match.Value, value);
        //    }

        //    //Scope
        //    while (true)
        //    {
        //        if (!string.IsNullOrEmpty(propertyStr))
        //        {
        //            matches = Regex.Matches(propertyStr, "\\{(.[^{}:\\$]+)\\}", RegexOptions.IgnoreCase);

        //            if (matches == null || matches.Count == 0) break;

        //            foreach (Match match in matches)
        //            {
        //                if (match != null && match.Length > 1)
        //                {
        //                    var value = Scope.SelectToken(match.Groups[1].Value);

        //                    if (value != null && value.Type != JTokenType.Object && value.Type != JTokenType.Array)
        //                        propertyStr = propertyStr.Replace(match.Value, value.Value<string>());
        //                    else
        //                        propertyStr = propertyStr.Replace(match.Value, string.Empty);
        //                }
        //            }
        //        }
        //        else
        //            break;

        //    }

        //    return propertyStr;
        //}

        //public static object GetPropertyValue(object src, string propName)
        //{
        //    if (src == null) return propName;
        //    if (string.IsNullOrEmpty(propName)) return propName;

        //    if (propName.Contains("."))//complex type nested
        //    {
        //        var temp = propName.Split(new char[] { '.' }, 2);
        //        return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
        //    }
        //    else
        //    {
        //        var prop = (src as JObject)[propName];
        //        if (prop.Type == JTokenType.String && General.IsJson(prop.Value<string>())) prop = JsonConvert.DeserializeObject<JObject>(prop.Value<string>());

        //        switch (prop.Type)
        //        {
        //            case JTokenType.None:
        //                break;
        //            case JTokenType.Object:
        //                return prop;
        //                break;
        //            case JTokenType.Array:
        //                return prop;
        //                break;
        //            case JTokenType.Constructor:
        //                break;
        //            case JTokenType.Property:
        //                break;
        //            case JTokenType.Comment:
        //                break;
        //            case JTokenType.Integer:
        //                return prop.Value<long>();
        //                break;
        //            case JTokenType.Float:
        //                return prop.Value<float>();
        //                break;
        //            case JTokenType.String:
        //                return prop.Value<string>();
        //                break;
        //            case JTokenType.Boolean:
        //                return prop.Value<bool>();
        //                break;
        //            case JTokenType.Null:
        //                break;
        //            case JTokenType.Undefined:
        //                break;
        //            case JTokenType.Date:
        //                return prop.Value<DateTime>();
        //                break;
        //            case JTokenType.Raw:
        //                break;
        //            case JTokenType.Bytes:
        //                return prop.Value<byte>();
        //                break;
        //            case JTokenType.Guid:
        //                return prop.Value<Guid>();
        //                break;
        //            case JTokenType.Uri:
        //                return prop.Value<Uri>();
        //                break;
        //            case JTokenType.TimeSpan:
        //                return prop.Value<TimeSpan>();
        //                break;
        //            default:
        //                break;
        //        }

        //        return null;
        //    }
        //}
    }
}