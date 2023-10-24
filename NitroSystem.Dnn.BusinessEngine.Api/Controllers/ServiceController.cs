using DotNetNuke.Web.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Api.Dto;
using NitroSystem.Dnn.BusinessEngine.Api.Mapping;
using NitroSystem.Dnn.BusinessEngine.Components;
using NitroSystem.Dnn.BusinessEngine.Core.Manager;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace NitroSystem.Dnn.BusinessEngine.Api.Controller
{
    public class ServiceController : DnnApiController
    {
        #region Services

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage RunService(ActionDto action)
        {
            try
            {
                if (action.ServiceID != null)
                {
                    var service = ServiceMapping.GetServiceViewModel(action.ServiceID.Value);
                    if (service.ServiceSubtype == "SendSms")
                    {
                        //provider
                        string provider = service.Settings["Provider"].ToString();

                        //mobile
                        string mobile = action.Params.FirstOrDefault(a => a.ParamName == "Mobile").ParamValue.ToString();

                        //message
                        string message = action.Params.FirstOrDefault(a => a.ParamName == "Message").ParamValue.ToString();

                        string apiKey = "4B6F6D4F502F455473336837796F56436C6A704C596E513374396B49444C3971";
                        var sender = "10000022020020";

                        //var api = new KavenegarApi(apiKey);
                        //var res = api.Send(sender, mobile, message);
                        //if (res.Status != 5)
                        //{
                        //}

                        var param = new Dictionary<string, object>
                        {
                            {"receptor", mobile},
                            {"template", "bookreadingkhameneibook"},
                            {"token", message},
                            {"type", 0},
                        };
                        var responsebody = SendSms("https://api.kavenegar.com/v1/" + apiKey + "/verify/lookup.json", param);

                        //var request = WebRequest.Create("https://api.kavenegar.com/v1/" + apiKey + "/verify/lookup.json");
                        //request.ContentType = "application/json; charset=utf-8";
                        //request.Method = "POST";
                        ////request.Headers.Add("Authorization", "Basic YWU3OGQxZTctYmFhZi00ZGQzLWE5NjctNWFkMWQ5NGEwZTdjOmVjMTRkYzgzLTY1YmEtNGI1MC1iNWI0LTZiNWYwMjc2ZDZmZA");

                        //var item = new { receptor = , token = "", template = "" };
                        //var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);

                        //byte[] dataStream = Encoding.UTF8.GetBytes(json);

                        //request.ContentLength = dataStream.Length;
                        //Stream newStream = request.GetRequestStream();

                        //newStream.Write(dataStream, 0, dataStream.Length);
                        //newStream.Close();

                        //var response = (HttpWebResponse)request.GetResponse();

                        //string text;
                        //using (var sr = new StreamReader(response.GetResponseStream()))
                        //{
                        //    text = sr.ReadToEnd();
                        //}

                        //var result = JsonConvert.DeserializeObject<JObject>(text);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, UserInfo.UserID);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [DnnAuthorize(AuthTypes = "JWT")]
        public HttpResponseMessage GetUserID()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, UserInfo.UserID);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SSR()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Habib");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        private string SendSms(string path, Dictionary<string, object> _params)
        {
            string responseBody = "";
            string postdata = "";

            byte[] byteArray;
            if (_params != null)
            {
                postdata = _params.Keys.Aggregate(postdata,
                    (current, key) => current + string.Format("{0}={1}&", key, _params[key]));
                byteArray = Encoding.UTF8.GetBytes(postdata);
            }
            else
            {
                byteArray = new byte[0];
            }

            var webRequest = (HttpWebRequest)WebRequest.Create(path);
            webRequest.Method = "POST";
            webRequest.Timeout = -1;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            using (Stream webpageStream = webRequest.GetRequestStream())
            {
                webpageStream.Write(byteArray, 0, byteArray.Length);
            }

            HttpWebResponse webResponse;
            try
            {
                using (webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (var reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
                //JsonConvert.DeserializeObject<ReturnResult>(responseBody);
                return responseBody;
            }
            catch (WebException webException)
            {
                return "";
            }
        }

        //[AllowAnonymous]
        //[HttpPost]
        //[DnnAuthorize(AuthTypes = "JWT")]
        //public async Task<HttpResponseMessage> RunService(ServiceDTO postdata)
        //{
        //    var result = new ServiceResultInfo();
        //    string query = string.Empty;

        //    try
        //    {
        //        var service = ServiceRepository.Instance.GetService(postdata.ServiceID);

        //        result.ServiceID = service.ServiceID;
        //        result.ServiceName = service.ServiceName;

        //        if (!service.IsEnabled)
        //            throw new Exception("Service is not enabled!");

        //        var authorizationRunService = !string.IsNullOrEmpty(service.AuthorizationRunService) ? service.AuthorizationRunService.Split(',') : new[] { "All Users" };
        //        if (!(UserInfo.IsSuperUser || authorizationRunService.Contains("All Users") || authorizationRunService.Any(r => UserInfo.Roles.Contains(r))))
        //            throw new Exception("Access Denied!");

        //        IEnumerable<ServiceParamInfo> serviceParams = null;
        //        try
        //        {
        //            //if (!string.IsNullOrEmpty(service.Params)) serviceParams = JsonConvert.DeserializeObject<IEnumerable<ServiceParamInfo>>(service.Params);
        //        }
        //        catch
        //        {
        //        }

        //        Models.ServiceModels.WebService.WebServiceOptions webServiceOptions = null;
        //        try
        //        {
        //            if (!string.IsNullOrEmpty(service.WebServiceOptions)) webServiceOptions = JsonConvert.DeserializeObject<Models.ServiceModels.WebService.WebServiceOptions>(service.WebServiceOptions);
        //        }
        //        catch
        //        {
        //        }

        //        dynamic bolbol = null;
        //        string command = service.Command;

        //        if (service.ServiceType == "Database")
        //        {
        //            if (service.DatabaseObjectType == "StoredProcedure")
        //            {
        //                //var postedParams = new Dictionary<string, string>();
        //                //foreach (var item in postdata.Params.Keys)
        //                //{
        //                //    postedParams.Add(item.ToString(), postdata.Params[item] != null ? postdata.Params[item].ToString() : string.Empty);
        //                //}
        //                //var postedParams = postdata.Params.Cast<DictionaryEntry>().ToDictionary(kvp => (string)kvp.Key, kvp => (string)kvp.Value);
        //                //bolbol = await ServiceManager.ExecuteStoredProcedure(service, postdata.Params);
        //            }
        //            else
        //            {
        //                var connection = new System.Data.SqlClient.SqlConnection(DotNetNuke.Data.DataProvider.Instance().ConnectionString);

        //                var matches = Regex.Matches(command, "{Token:\\w+}", RegexOptions.IgnoreCase);
        //                foreach (Match match in matches)
        //                {
        //                    var value = postdata.Params[Regex.Match(match.Value, "(?<={Token:)\\w+", RegexOptions.IgnoreCase).Value];
        //                    if (value == null) value = string.Empty;

        //                    command = command.Replace(match.Value, value.ToString());
        //                }

        //                matches = Regex.Matches(command, "{GregorianDate:([^}]*)}", RegexOptions.IgnoreCase);
        //                foreach (Match match in matches)
        //                {
        //                    var value = Regex.Match(match.Value, "(?<={GregorianDate:)[^}]+(?=\\})", RegexOptions.IgnoreCase).Value;
        //                    DateTime temp;
        //                    if (DateTime.TryParse(value.ToString(), out temp))
        //                        value = temp.ToString(new CultureInfo("en-US"));

        //                    command = command.Replace(match.Value, value.ToString());
        //                }

        //                query = command;

        //                if (service.ResultType == "List")
        //                {
        //                    bolbol = Dapper.SqlMapper.Query<dynamic>(connection, query, commandTimeout: int.MaxValue);
        //                }
        //                else if (service.ResultType == "Object")
        //                {
        //                    var data = Dapper.SqlMapper.Query<dynamic>(connection, query, commandTimeout: int.MaxValue);
        //                    bolbol = data != null && data.Any() ? data.First() : null;
        //                }
        //                else if (service.ResultType == "Scaler")
        //                {
        //                    bolbol = Dapper.SqlMapper.ExecuteScalar(connection, query, commandTimeout: int.MaxValue);
        //                }
        //                else
        //                {
        //                    Dapper.SqlMapper.Execute(connection, query, commandTimeout: int.MaxValue);
        //                }
        //            }
        //        }
        //        else if (service.ServiceType == "PublicServices")
        //        {
        //            var matches = Regex.Matches(command, "{Token:\\w+}", RegexOptions.IgnoreCase);
        //            foreach (Match match in matches)
        //            {
        //                var value = postdata.Params[Regex.Match(match.Value, "(?<={Token:)\\w+(?=\\})", RegexOptions.IgnoreCase).Value];
        //                if (value == null) value = string.Empty;

        //                command = command.Replace(match.Value, value.ToString());
        //            }

        //            //switch (service.ServiceSubtype)
        //            //{
        //            //    case "LoginUser":
        //            //        bolbol = ServiceManager.LoginUser(service, command, PortalSettings);
        //            //        break;
        //            //    case "RegisterUser":
        //            //        bolbol = ServiceManager.RegisterUser(service, command, PortalSettings);
        //            //        break;
        //            //    case "LoginOrRegisterUser":
        //            //        bolbol = ServiceManager.LoginOrRegisterUser(service, command, PortalSettings);
        //            //        break;
        //            //    case "ApproveUser":
        //            //        bolbol = ServiceManager.ApproveUser(service, command, PortalSettings);
        //            //        break;
        //            //    case "ResetPassword":
        //            //        bolbol = ServiceManager.ResetUserPassword(service, command, PortalSettings);
        //            //        break;
        //            //    case "GetUserInfo":
        //            //        bolbol = ServiceManager.GetUserInfo(service, command, PortalSettings);
        //            //        break;
        //            //    case "DeleteUser":
        //            //        bolbol = ServiceManager.DeleteUser(service, command, PortalSettings);
        //            //        break;
        //            //    case "GrantUserRole":
        //            //        bolbol = ServiceManager.GrantUserRole(service, command, PortalSettings);
        //            //        break;
        //            //    case "UpdateUserProfile":
        //            //        bolbol = ServiceManager.UpdateUserProfile(service, command, PortalSettings);
        //            //        break;
        //            //    case "GetUserID":
        //            //        bolbol = ServiceManager.GetUserIDByUsername(service, command, PortalSettings);
        //            //        break;
        //            //    case "SendEmail":
        //            //        ServiceManager.SendEmail(service, command, PortalSettings);
        //            //        break;
        //            //    case "SendSms":
        //            //        ServiceManager.SendSms(service, command, PortalSettings);
        //            //        break;
        //            //}
        //        }
        //        //if (service.ServiceType == "WebService")
        //        //{
        //        //    var webServiceResult = new JObject();

        //        //    string url = ServiceManager.ProccessWebServiceParamValue(webServiceOptions.Url, postdata.Params);

        //        //    if (webServiceOptions.Params != null)
        //        //    {
        //        //        List<string> queryParams = new List<string>();
        //        //        foreach (var item in webServiceOptions.Params)
        //        //        {
        //        //            queryParams.Add(item.ParamName + "=" + ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params));
        //        //        }

        //        //        if (queryParams.Count > 0) url += "?" + string.Join("&", queryParams);
        //        //    }

        //        //    command = url;

        //        //    var request = (HttpWebRequest)WebRequest.Create(url);

        //        //    request.Method = webServiceOptions.Method;

        //        //    if (webServiceOptions.Headers != null)
        //        //    {
        //        //        foreach (var item in webServiceOptions.Headers.Where(h => !h.IsSystem || h.IsSelected))
        //        //        {
        //        //            switch (item.ParamName)
        //        //            {
        //        //                case "Accept":
        //        //                    request.Accept = ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params);
        //        //                    break;
        //        //                case "Connection":
        //        //                    request.Connection = ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params);
        //        //                    break;
        //        //                case "Content-Length":
        //        //                    request.ContentLength = long.Parse(ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params));
        //        //                    break;
        //        //                case "Content-Type":
        //        //                    request.ContentType = ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params);
        //        //                    break;
        //        //                case "Date":
        //        //                    request.Date = DateTime.Parse(ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params));
        //        //                    break;
        //        //                case "Expect":
        //        //                    request.Expect = ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params);
        //        //                    break;
        //        //                case "Host":
        //        //                    request.Host = ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params);
        //        //                    break;
        //        //                case "If-Modified-Since":
        //        //                    request.IfModifiedSince = DateTime.Parse(ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params));
        //        //                    break;
        //        //                case "Range":
        //        //                    request.AddRange(int.Parse(ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params)));
        //        //                    break;
        //        //                case "Referer":
        //        //                    request.Referer = ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params);
        //        //                    break;
        //        //                case "Transfer-Encoding":
        //        //                    request.TransferEncoding = ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params);
        //        //                    break;
        //        //                case "User-Agent":
        //        //                    request.UserAgent = ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params);
        //        //                    break;
        //        //                case "Proxy-Connection":
        //        //                    //request.Proxy = ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params);;
        //        //                    break;
        //        //                default:
        //        //                    request.Headers.Add(item.ParamName, ServiceManager.ProccessWebServiceParamValue(item.ParamValue, postdata.Params));
        //        //                    break;
        //        //            }
        //        //        }
        //        //    }

        //        //    if (webServiceOptions.Authorization != null)
        //        //    {
        //        //        if (webServiceOptions.Authorization.Type == "Bearer")
        //        //        {
        //        //            string bearer = ServiceManager.ProccessWebServiceParamValue(webServiceOptions.Authorization.Bearer, postdata.Params);

        //        //            request.Headers.Add("Authorization", bearer);
        //        //        }
        //        //        else if (webServiceOptions.Authorization.Type == "Basic")
        //        //        {
        //        //            string username = ServiceManager.ProccessWebServiceParamValue(webServiceOptions.Authorization.BasicAuth.Username, postdata.Params);
        //        //            string password = ServiceManager.ProccessWebServiceParamValue(webServiceOptions.Authorization.BasicAuth.Password, postdata.Params);

        //        //            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
        //        //            request.Headers.Add("Authorization", "Basic " + encoded);
        //        //        }
        //        //    }

        //        //    if (!string.IsNullOrEmpty(webServiceOptions.BodyRaw))
        //        //    {
        //        //        string body = ServiceManager.ProccessWebServiceParamValue(webServiceOptions.BodyRaw, postdata.Params);

        //        //        byte[] dataStream = Encoding.UTF8.GetBytes(body);

        //        //        request.ContentLength = dataStream.Length;
        //        //        Stream newStream = request.GetRequestStream();

        //        //        newStream.Write(dataStream, 0, dataStream.Length);
        //        //        newStream.Close();
        //        //    }

        //        //    try
        //        //    {
        //        //        var response = (HttpWebResponse)request.GetResponse();

        //        //        webServiceResult.Add(new JProperty("StatusCode", response.StatusCode));
        //        //        webServiceResult.Add(new JProperty("StatusDescription", response.StatusDescription));

        //        //        webServiceResult.Add(new JProperty("Headers", JObject.Parse("{}")));

        //        //        foreach (var key in response.Headers.AllKeys)
        //        //        {
        //        //            (webServiceResult["Headers"] as JObject).Add(new JProperty(key, response.Headers[key]));
        //        //        }

        //        //        string text;
        //        //        using (var sr = new StreamReader(response.GetResponseStream()))
        //        //        {
        //        //            text = sr.ReadToEnd();
        //        //        }

        //        //        webServiceResult.Add(new JProperty("Body", text));

        //        //    }
        //        //    catch (Exception ex2)
        //        //    {
        //        //        throw new Exception(ex2.Message);
        //        //    }

        //        //    bolbol = webServiceResult;
        //        //}

        //        result.StatusCode = HttpStatusCode.OK;
        //        result.Data = bolbol;
        //        result.IsError = false;
        //        //result.DebugQuery = UserInfo.IsSuperUser ? command : string.Empty;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.IsError = true;
        //        result.StatusCode = HttpStatusCode.InternalServerError;
        //        result.ErrorMessage = ex.Message;
        //    }

        //    return Request.CreateResponse(result.StatusCode, result);
        //}

        //[AllowAnonymous]
        //[HttpGet]
        //[DnnAuthorize(AuthTypes = "JWT")]

        //public async Task<HttpResponseMessage> GET(string s, [FromUri] object param)
        //{
        //    try
        //    {
        //        var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

        //        var service = ServiceRepository.Instance.GetServiceByName(scenarioID, s, true);

        //        Hashtable serviceParams = new Hashtable();

        //        var url = HttpContext.Current.Request.Url.Query;
        //        url = string.IsNullOrEmpty(url) ? "" : url.Replace("?", "");
        //        foreach (string item in url.Split('&'))
        //        {
        //            string[] part = item.Split('=');

        //            serviceParams[part[0]] = part[1];
        //        }

        //        var content = await RunService(new ServiceDTO() { ServiceID = service.ServiceID, Params = serviceParams });

        //        var serviceResult = await content.Content.ReadAsAsync<ServiceResultInfo>();

        //        var data = serviceResult.StatusCode == HttpStatusCode.OK ? serviceResult.Data : serviceResult.ErrorMessage;

        //        return Request.CreateResponse(serviceResult.StatusCode, data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[DnnAuthorize(AuthTypes = "JWT")]
        //public async Task<HttpResponseMessage> POST([FromUri] string s, [FromBody] JObject postData)
        //{
        //    try
        //    {
        //        var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

        //        var service = ServiceRepository.Instance.GetServiceByName(scenarioID, s, true);

        //        Hashtable serviceParams = new Hashtable();

        //        foreach (var item in postData.Properties())
        //        {
        //            switch (item.Value.Type)
        //            {
        //                case JTokenType.None:
        //                    break;
        //                case JTokenType.Object:
        //                    break;
        //                case JTokenType.Array:
        //                    break;
        //                case JTokenType.Constructor:
        //                    break;
        //                case JTokenType.Property:
        //                    break;
        //                case JTokenType.Comment:
        //                    break;
        //                case JTokenType.Integer:
        //                    serviceParams[item.Name] = postData[item.Name].Value<long>();
        //                    break;
        //                case JTokenType.Float:
        //                    serviceParams[item.Name] = postData[item.Name].Value<float>();
        //                    break;
        //                case JTokenType.String:
        //                    serviceParams[item.Name] = postData[item.Name].Value<string>();
        //                    break;
        //                case JTokenType.Boolean:
        //                    serviceParams[item.Name] = postData[item.Name].Value<bool>();
        //                    break;
        //                case JTokenType.Null:
        //                    break;
        //                case JTokenType.Undefined:
        //                    break;
        //                case JTokenType.Date:
        //                    serviceParams[item.Name] = postData[item.Name].Value<DateTime>();
        //                    break;
        //                case JTokenType.Raw:
        //                    break;
        //                case JTokenType.Bytes:
        //                    break;
        //                case JTokenType.Guid:
        //                    break;
        //                case JTokenType.Uri:
        //                    break;
        //                case JTokenType.TimeSpan:
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }

        //        var content = await RunService(new ServiceDTO() { ServiceID = service.ServiceID, Params = serviceParams });

        //        var serviceResult = await content.Content.ReadAsAsync<ServiceResultInfo>();

        //        var data = serviceResult.StatusCode == HttpStatusCode.OK ? serviceResult.Data : serviceResult.ErrorMessage;

        //        return Request.CreateResponse(serviceResult.StatusCode, data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        #endregion
    }
}
