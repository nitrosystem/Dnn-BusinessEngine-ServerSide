using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Installer.Log;
using DotNetNuke.Services.Mail;
using DotNetNuke.Services.Scheduling;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Utilities.Models;

namespace NitroSystem.Dnn.BusinessEngine.Components
{
    internal static class General
    {
        //internal static string GetFieldValue(string value)
        //{
        //    if (IsJsonObject(value))
        //    {
        //        var obj = JsonConvert.DeserializeObject<OptionInfo>(value);
        //        return obj != null && !string.IsNullOrEmpty(obj.OptionValue) ? obj.OptionValue : value;
        //    }
        //    else
        //        return value;
        //}

        //internal static string GetFieldText(string value)
        //{
        //    if (IsJsonObject(value))
        //    {
        //        var obj = JsonConvert.DeserializeObject<OptionInfo>(value);
        //        return obj != null && !string.IsNullOrEmpty(obj.OptionText) ? obj.OptionText : value;
        //    }
        //    else if (IsJsonArray(value))
        //    {
        //        List<string> contents = new List<string>();

        //        var items = JsonConvert.DeserializeObject<IEnumerable<OptionInfo>>(value);
        //        if (items != null)
        //        {
        //            foreach (var item in items)
        //            {
        //                if (item != null && !string.IsNullOrEmpty(item.OptionText))
        //                    contents.Add(item.OptionText);
        //                else if (item != null && !string.IsNullOrEmpty(item.OptionValue))
        //                    contents.Add(item.OptionValue);
        //            }

        //            return string.Join(", ", contents);
        //        }
        //    }

        //    return value;
        //}

        //internal static object GetImageFieldValue(string value)
        //{
        //    object result = null;
        //    try
        //    {
        //        if (IsJsonObject(value))
        //        {
        //            result = JsonConvert.DeserializeObject<UploadPhotoInfo>(value);
        //        }
        //        else if (IsJsonArray(value))
        //        {
        //            result = JsonConvert.DeserializeObject<IEnumerable<UploadPhotoInfo>>(value);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    return result;
        //}

        //internal static object GetVideoFieldValue(string value)
        //{
        //    object result = null;
        //    try
        //    {
        //        if (IsJsonObject(value))
        //        {
        //            result = JsonConvert.DeserializeObject<UploadVideoInfo>(value);
        //        }
        //        else if (IsJsonArray(value))
        //        {
        //            result = JsonConvert.DeserializeObject<IEnumerable<UploadVideoInfo>>(value);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    return result;
        //}

        //internal static bool IsJson(string value)
        //{
        //    if (string.IsNullOrEmpty(value)) return false;

        //    if ((value.StartsWith("{") && value.EndsWith("}")) || (value.StartsWith("[") && value.EndsWith("]")))
        //        return true;
        //    else
        //        return false;
        //}

        //internal static bool IsJsonObject(string value)
        //{
        //    if (string.IsNullOrEmpty(value)) return false;

        //    if (value.StartsWith("{") && value.EndsWith("}"))
        //        return true;
        //    else
        //        return false;
        //}

        //internal static bool IsJsonArray(string value)
        //{
        //    if (string.IsNullOrEmpty(value)) return false;

        //    if (value.StartsWith("[") && value.EndsWith("]"))
        //        return true;
        //    else
        //        return false;
        //}

        //internal static string ParsePublicTokens(string tokenName, dynamic item = null, UserInfo userInfo = null, PortalSettings portalSettings = null, string pageUrl = null, JObject localStorage = null, JObject sessionStorage = null)
        //{
        //    if (string.IsNullOrEmpty(tokenName)) return string.Empty;

        //    string result = tokenName;

        //    if (userInfo == null) userInfo = UserController.Instance.GetCurrentUserInfo();

        //    if (portalSettings == null) portalSettings = PortalSettings.Current;

        //    //parse page params
        //    if (!string.IsNullOrEmpty(pageUrl))
        //    {
        //        var pageParamMatches = Regex.Matches(tokenName, "\\{PageParam:(\\w+)\\}", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //        if (pageParamMatches != null && pageParamMatches.Count > 0)
        //        {
        //            foreach (Match match in pageParamMatches)
        //            {
        //                if (match.Groups.Count > 1)
        //                {
        //                    var key = match.Groups[1].Value;

        //                    var pageParams = !string.IsNullOrEmpty(pageUrl) ? new Uri(pageUrl).ParseQueryString() : null;
        //                    if (pageParams != null && pageParams.Count > 0)
        //                        result = result.Replace(match.Value, pageParams.Get(key));
        //                    else
        //                        result = result.Replace(match.Value, string.Empty);
        //                }
        //            }
        //        }
        //    }

        //    //parse local storage
        //    if (localStorage != null)
        //    {
        //        var loalStorageMatches = Regex.Matches(tokenName, "\\{LocalStorage:(\\w+)\\}", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //        if (loalStorageMatches != null && loalStorageMatches.Count > 0)
        //        {
        //            foreach (Match match in loalStorageMatches)
        //            {
        //                var key = match.Groups[1].Value;

        //                result = result.Replace(match.Value, localStorage.Value<string>(key));
        //            }
        //        }
        //    }

        //    //parse session storage
        //    if (sessionStorage != null)
        //    {
        //        var sessionStorageMatches = Regex.Matches(tokenName, "\\{SessionStorage:(\\w+)\\}", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //        if (sessionStorageMatches != null && sessionStorageMatches.Count > 0)
        //        {
        //            foreach (Match match in sessionStorageMatches)
        //            {
        //                var key = match.Groups[1].Value;

        //                result = result.Replace(match.Value, sessionStorage.Value<string>(key));
        //            }
        //        }
        //    }

        //    //parse user properties
        //    if (userInfo != null && userInfo.UserID > 0)
        //    {
        //        var userMatches = Regex.Matches(tokenName, "\\{CurrentUser:(\\w+)\\}", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //        if (userMatches != null && userMatches.Count > 0)
        //        {
        //            foreach (Match match in userMatches)
        //            {
        //                var key = match.Groups[1].Value;

        //                switch (key.ToLower())
        //                {
        //                    case "userid":
        //                        result = result.Replace(match.Value, userInfo.UserID.ToString());
        //                        break;
        //                    case "username":
        //                        result = result.Replace(match.Value, userInfo.Username);
        //                        break;
        //                    case "issuperuser":
        //                        result = result.Replace(match.Value, userInfo.IsSuperUser.ToString());
        //                        break;
        //                    case "email":
        //                        result = result.Replace(match.Value, userInfo.Email);
        //                        break;
        //                    case "isapproved":
        //                        result = result.Replace(match.Value, userInfo.Membership.Approved.ToString());
        //                        break;
        //                }
        //            }
        //        }
        //    }

        //    //parse user roles
        //    if (userInfo != null && userInfo.UserID > 0)
        //    {
        //        var userMatches = Regex.Matches(tokenName, "\\{UserRole:(\\w+)\\}", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //        if (userMatches != null && userMatches.Count > 0)
        //        {
        //            foreach (Match match in userMatches)
        //            {
        //                var key = match.Groups[1].Value;

        //                result = result.Replace(match.Value, userInfo.IsInRole(key).ToString());
        //            }
        //        }
        //    }

        //    //parse dnn properties
        //    if (userInfo != null && userInfo.UserID > 0)
        //    {
        //        var userMatches = Regex.Matches(tokenName, "\\{Dnn:(\\w+)\\}", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //        if (userMatches != null && userMatches.Count > 0)
        //        {
        //            foreach (Match match in userMatches)
        //            {
        //                var key = match.Groups[1].Value;

        //                switch (key.ToLower())
        //                {
        //                    case "portalid":
        //                        result = result.Replace(match.Value, portalSettings.PortalId.ToString());
        //                        break;
        //                    case "createdate":
        //                        result = result.Replace(match.Value, DateTime.Today.ToShortDateString());
        //                        break;
        //                }

        //            }
        //        }
        //    }

        //    //parse item fields
        //    if (item != null)
        //    {
        //        var fieldValueMatches = Regex.Matches(tokenName, "{Field:(\\w+)}", RegexOptions.IgnoreCase);
        //        if (fieldValueMatches != null && fieldValueMatches.Count > 0)
        //        {
        //            var itemValues = (item as IEnumerable<KeyValuePair<string, object>>);

        //            foreach (Match match in fieldValueMatches)
        //            {
        //                var fieldName = match.Groups[1].Value;
        //                var value = itemValues.FirstOrDefault(o => o.Key == fieldName);
        //                if (value.Value != null) result = result.Replace(match.Value, GetFieldText(value.Value.ToString()));
        //            }
        //        }
        //    }

        //    switch (tokenName.ToLower())
        //    {
        //        case "{date:weekfirstday}":
        //            result = GetFirstDayOfWeek(DateTime.Today, CultureInfo.CurrentCulture).ToString("yyyy/MM/dd", new CultureInfo("en-US"));
        //            break;
        //        case "{date:weeklastday}":
        //            result = GetFirstDayOfWeek(DateTime.Today, CultureInfo.CurrentCulture).AddDays(7).ToString("yyyy/MM/dd", new CultureInfo("en-US"));
        //            break;
        //        case "{date:today}":
        //            result = DateTime.Today.ToString("yyyy/MM/dd", new CultureInfo("en-US"));
        //            break;
        //        case "{currentdate}":
        //            result = DateTime.Now.ToString("", new CultureInfo("en-US"));
        //            break;
        //        case "{currentuserid}":
        //            result = userInfo.UserID.ToString();
        //            break;
        //        case "{username}":
        //            result = userInfo.Username;
        //            break;
        //        case "{currenttabportalid}":
        //            result = portalSettings.PortalId.ToString();
        //            break;
        //    }

        //    return result;
        //}

        ////internal static bool CheckItemConditions(dynamic item, IEnumerable<ConditionInfo> conditions)
        ////{
        ////    if (item == null || conditions == null || conditions.Count() == 0) return true;

        ////    var values = (item as IEnumerable<KeyValuePair<string, object>>);

        ////    bool andResult = true;

        ////    var filters = conditions.Where(f => (f.FieldID != Guid.Empty || !string.IsNullOrEmpty(f.FieldName)) && !string.IsNullOrEmpty(f.RightExpression)).GroupBy(f => f.GroupName);
        ////    foreach (var group in filters)
        ////    {
        ////        var orResult = false;
        ////        foreach (var filter in group)
        ////        {
        ////            bool isTrue = false;

        ////            var value = values.FirstOrDefault(o => o.Key == filter.FieldName);
        ////            if (value.Value != null)
        ////            {
        ////                var fValue = GetFieldValue(value.Value.ToString());
        ////                var cValue = filter.RightExpression;
        ////                if (filter.IsBoolean)
        ////                {
        ////                    fValue = fValue.ToLower() == "true" || fValue.ToLower() == "1" ? "true" : "false";
        ////                    cValue = cValue.ToLower() == "true" || cValue.ToLower() == "1" ? "true" : "false";
        ////                }

        ////                switch (filter.EvalType)
        ////                {
        ////                    case "=":
        ////                        isTrue = fValue == cValue;
        ////                        break;
        ////                    case "!=":
        ////                        isTrue = fValue != cValue;
        ////                        break;
        ////                }
        ////            }
        ////            if (!orResult && isTrue) orResult = true;
        ////        }

        ////        if (!orResult)
        ////        {
        ////            andResult = false;
        ////            break;
        ////        }
        ////    }

        ////    return andResult;
        ////}

        ////internal static async Task<bool> SendNotification( ActionViewModel action, PortalSettings portalSettings, UserInfo userInfo)
        ////{
        ////    switch (action.ActionType)
        ////    {
        ////        case "SendEmail":
        ////            var emailDetails = JsonConvert.DeserializeObject<ActionEmailInfo>(action.ActionDetails.ToString());
        ////            foreach (var email in emailDetails.EmailTo)
        ////            {
        ////                string emailTo = ParsePublicTokens(email.text, null, userInfo);
        ////                await Task.Run(() =>
        ////                {
        ////                    Mail.SendEmail(portalSettings.Email, emailTo, ParsePublicTokens(emailDetails.Subject, null, userInfo), ParsePublicTokens(emailDetails.Body, null, userInfo));
        ////                });
        ////            }
        ////            break;
        ////        case "SendSMS":
        ////            var smsDetails = JsonConvert.DeserializeObject<ActionSmsInfo>(action.ActionDetails.ToString());
        ////            foreach (var mobile in smsDetails.SmsTo)
        ////            {
        ////                await Task.Run(() =>
        ////                {
        ////                    SendSms(portalSettings,"", ParsePublicTokens(mobile.text, null, userInfo), ParsePublicTokens(smsDetails.Body, null, userInfo));
        ////                });
        ////            }
        ////            break;
        ////        default:
        ////            break;
        ////    }

        ////    return false;
        ////}

        ////internal static async Task CallAction(ActionViewModel action, PortalSettings portalSettings, UserInfo userInfo)
        ////{
        ////    //var options = OptionsRepository.Instance.GetOptions(moduleID);

        ////    //var item = action.ItemID != 0 ? ItemController.GetItem(moduleID, action.ItemID) : null;

        ////    //var actionList = ActionRepository.Instance.GetActions(moduleID).Where(a => a.Event == action.Event);

        ////    //var actions = new List<ActionInfo>();
        ////    //foreach (var a in actionList)
        ////    //{
        ////    //    var objActionConditions = JsonConvert.DeserializeObject<ActionConditions>(a.ActionDetails);
        ////    //    if (objActionConditions.Conditions == null || (objActionConditions.Conditions != null && General.CheckItemConditions(item, objActionConditions.Conditions)))
        ////    //        actions.Add(a);
        ////    //}

        ////    await SendNotification( action, portalSettings, userInfo);
        ////}

        //private static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        //{
        //    DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
        //    DateTime firstDayInWeek = dayInWeek.Date;
        //    while (firstDayInWeek.DayOfWeek != firstDay)
        //        firstDayInWeek = firstDayInWeek.AddDays(-1);

        //    return firstDayInWeek;
        //}

        ////internal static string ProcessFieldValue(JArray values, int itemID, EntityViewModel entity, PortalSettings portalSettings, UserInfo userInfo)
        ////{
        ////    if (values == null || values.Count == 0) return null;

        ////    dynamic item = null;

        ////    var value = values[0].Value<string>("RightExpression");
        ////    switch (value.ToLower())
        ////    {
        ////        //case "{systemfield:moduleid}":
        ////        //    return entity.ModuleID.ToString();
        ////        //case "{systemfield:createdbyuserid}":
        ////        //    item = itemID == 0 ? null : ItemController.GetItem(entity.ModuleID, itemID);
        ////        //    if (item != null) return Convert.ToString(item.UserID);
        ////        //    return userInfo.UserID.ToString();
        ////        case "{systemfield:currentuserid}":
        ////            return userInfo.UserID.ToString();
        ////        case "{systemfield:userdisplayname}":
        ////            return userInfo.DisplayName;
        ////        case "{systemfield:useremail}":
        ////            return userInfo.Email;
        ////        //case "{systemfield:createdondate}":
        ////        //    item = itemID == 0 ? null : ItemController.GetItem(entity.ModuleID, itemID);
        ////        //    if (item != null)
        ////        //    {
        ////        //        var createdOnDate = Convert.ToString(item.CreatedOnDate);
        ////        //        if (!string.IsNullOrEmpty(createdOnDate)) return DateTime.Parse(createdOnDate).ToString(new CultureInfo("en-US"));
        ////        //    }
        ////        //    return DateTime.Now.ToString(new CultureInfo("en-US"));
        ////        case "{systemfield:currentdatetime}":
        ////            return DateTime.Now.ToString(new CultureInfo("en-US"));
        ////        case "{systemfield:currentdate":
        ////            return DateTime.Today.ToString(new CultureInfo("en-US"));
        ////        //case "{systemfield:expiredondate}":
        ////        //    item = itemID == 0 ? null : ItemController.GetItem(entity.ModuleID, itemID);
        ////        //    if (item != null)
        ////        //    {
        ////        //        var expiredOnDate = Convert.ToString(item.ExpiredOnDate);
        ////        //        if (!string.IsNullOrEmpty(expiredOnDate)) return DateTime.Parse(expiredOnDate).ToString(new CultureInfo("en-US"));
        ////        //    }

        ////        //    var mbdSettings = SettingsRepository.Instance.GetModuleSettings(entity.ModuleID);
        ////        //    if (mbdSettings["ItemExpirationTime"] != null && int.Parse(mbdSettings["ItemExpirationTime"].ToString()) > 0)
        ////        //        return DateTime.Now.AddDays(int.Parse(mbdSettings["ItemExpirationTime"].ToString())).ToString("", new CultureInfo("en-US"));
        ////        //    else
        ////        //        return DateTime.Today.AddYears(100).ToString("", new CultureInfo("en-US"));
        ////        case "{systemfield:random}":
        ////            return new Random().Next(1111, 9999).ToString();
        ////        default:
        ////            return value;
        ////    }
        ////}


    }
}