using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Profile;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Mail;
using Newtonsoft.Json;
using NitroSystem.Dnn.BusinessEngine.Core.Providers.SmsGateway;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Core.Manager
{
    public class ServiceManager
    {
        //public static object RunService(Guid serviceID, Hashtable serviceParams, int portalID, UserInfo userInfo)
        //{
        //    var service = ServiceRepository.Instance.GetServiceByID(portalID, serviceID, true);

        //    if (service == null) return null;

        //    if (!(userInfo.IsSuperUser || service.AuthorizationRunService.Split(',').Contains("All Users") || service.AuthorizationRunService.Split(',').Any(r => userInfo.Roles.Contains(r))))
        //        throw new Exception("Access Denied!");

        //    string command = service.Command;

        //    var matches = Regex.Matches(command, "{Token:\\w+}", RegexOptions.IgnoreCase);
        //    foreach (Match match in matches)
        //    {
        //        var value = serviceParams[Regex.Match(match.Value, "(?<={Token:)\\w+(?=\\})", RegexOptions.IgnoreCase).Value];
        //        if (value == null) value = string.Empty;

        //        command = command.Replace(match.Value, value.ToString());
        //    }

        //    dynamic result = null;

        //    if (service.ServiceSubtype == "SqlQuery")
        //    {
        //        string query = command;

        //        var connection = new SqlConnection(DataProvider.Instance().ConnectionString);

        //        if (service.ResultType == "List")
        //        {
        //            result = Dapper.SqlMapper.Query<dynamic>(connection, query);
        //        }
        //        else if (service.ResultType == "Object")
        //        {
        //            var data = Dapper.SqlMapper.Query<dynamic>(connection, query);
        //            result = data != null && data.Any() ? data.First() : null;
        //        }
        //        else if (service.ResultType == "Scaler")
        //        {
        //            result = Dapper.SqlMapper.ExecuteScalar(connection, query);
        //        }
        //        else
        //        {
        //            Dapper.SqlMapper.Execute(connection, query);
        //        }
        //    }

        //    return result;
        //}

        //public static ServiceUserLoginInfo LoginUser(ServiceInfo service, string command, PortalSettings portalSettings)
        //{
        //    var result = new ServiceUserLoginInfo();

        //    var options = new ServiceUserLoginOptions();

        //    var match = Regex.Match(command, "{Username:(.*?)}", RegexOptions.IgnoreCase);
        //    string username = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) ? match.Groups[1].Value : string.Empty;

        //    match = Regex.Match(command, "{Password:(.*?)}", RegexOptions.IgnoreCase);
        //    string password = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) ? match.Groups[1].Value : string.Empty;

        //    match = Regex.Match(command, "{Options:RememberMe:(true|false)}", RegexOptions.IgnoreCase);
        //    options.RememberMe = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) ? bool.Parse(match.Groups[1].Value) : false;

        //    match = Regex.Match(command, "{Options:EmailAsUsername:(true|false)}", RegexOptions.IgnoreCase);
        //    options.EmailAsUsername = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) ? bool.Parse(match.Groups[1].Value) : false;

        //    match = Regex.Match(command, "{Options:MobileAsUsername:(true|false)}", RegexOptions.IgnoreCase);
        //    options.MobileAsUsername = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) ? bool.Parse(match.Groups[1].Value) : false;

        //    UserInfo user = null;

        //    string ip = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "";

        //    UserLoginStatus status = UserLoginStatus.LOGIN_FAILURE;

        //    user = UserController.GetUserByName(portalSettings.PortalId, username);

        //    //email
        //    //if (user == null && options.EmailAsUsername && GeneralUtil.IsValidEmail(username))
        //    //{
        //    //    user = UserController.GetUserByEmail(portalSettings.PortalId, username);
        //    //    if (user != null) username = user.Username;
        //    //}

        //    //mobile
        //    //if (user == null && options.MobileAsUsername)
        //    //{
        //    //    var profileDefinition = ProfileController.GetPropertyDefinitionByName(portalSettings.PortalId, "Cell");
        //    //    if (profileDefinition != null)
        //    //    {
        //    //        string query = string.Format("Select UserID From dbo.UserProfile Where PropertyDefinitionID={0} and PropertyValue={1}", profileDefinition.PropertyDefinitionId, username);
        //    //        var dbResult = DbUtil.ExecuteScaler(query);
        //    //        if (dbResult != null)
        //    //        {
        //    //            user = UserController.GetUserById(portalSettings.PortalId, int.Parse(dbResult.ToString()));
        //    //            if (user != null) username = user.Username;
        //    //        }
        //    //    }
        //    //}

        //    //if (user == null)
        //    //{
        //    //    var u = DbUtil.ExecuteReader(string.Format("Select u.UserID,up.PortalID From dbo.Users u inner join dbo.UserPortals up on u.UserID = up.UserID Where u.Username='{0}'", username));
        //    //    if (u.Read())
        //    //    {
        //    //        user = UserController.GetUserById((int)u["PortalID"], (int)u["UserID"]);

        //    //        var portal = PortalController.Instance.GetPortal(portalSettings.PortalId);

        //    //        UserController.CopyUserToPortal(user, portal, true);
        //    //    }
        //    //}

        //    UserController.ValidateUser(portalSettings.PortalId, username, password, "", portalSettings.PortalName, ip, ref status);

        //    if (user != null && status == UserLoginStatus.LOGIN_USERLOCKEDOUT) UserController.UnLockUser(user);

        //    if (status == UserLoginStatus.LOGIN_USERNOTAPPROVED || status == UserLoginStatus.LOGIN_SUCCESS || status == UserLoginStatus.LOGIN_SUPERUSER)
        //    {
        //        const int timeout = 525600; // Timeout is in minutes, 525600 = 365 days; 1 day = 1440.
        //        var ticket = new System.Web.Security.FormsAuthenticationTicket(username, true, timeout);
        //        string encrypted = System.Web.Security.FormsAuthentication.Encrypt(ticket);
        //        var cookie = new HttpCookie("MYDNNOAUTH", encrypted)
        //        {
        //            Expires = System.DateTime.Now.AddMinutes(timeout),
        //            HttpOnly = true
        //        };
        //        HttpContext.Current.Response.Cookies.Add(cookie);

        //        user = user == null ? UserController.GetUserByName(username) : user;
        //        if (user == null || !user.Membership.Approved)
        //        {
        //            result.Status = UserLoginStatus.LOGIN_FAILURE;

        //            RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -102 } });
        //        }
        //        else
        //        {
        //            //if (!user.IsInRole("Registered Users"))
        //            //{
        //            //    var registeredRole = RoleController.Instance.GetRoleByName(portalSettings.PortalId, "Registered Users");
        //            //    RoleController.AddUserRole(user, registeredRole, portalSettings, RoleStatus.Approved, Null.NullDate, Null.NullDate, false, false);
        //            //}

        //            UserController.UserLogin(portalSettings.PortalId, user, portalSettings.PortalName, ip, options.RememberMe);

        //            result.Status = status;
        //            result.UserID = user.UserID;
        //            result.Username = user.Username;
        //            result.DisplayName = user.DisplayName;
        //        }
        //    }
        //    else
        //        RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -101 } });

        //    if (user == null || status == UserLoginStatus.LOGIN_FAILURE)
        //        RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -101 } });

        //    return result;
        //}

        //public static ServiceUserRegisterInfo RegisterUser(ServiceInfo service, string command, PortalSettings portalSettings)
        //{
        //    var result = new ServiceUserRegisterInfo();

        //    //first name
        //    var match = Regex.Match(command, "{User:FirstName:(.*?)}", RegexOptions.IgnoreCase);
        //    string firstName = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;
        //    //last name
        //    match = Regex.Match(command, "{User:LastName:(.*?)}", RegexOptions.IgnoreCase);
        //    string lastName = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;
        //    //display name
        //    match = Regex.Match(command, "{User:DisplayName:(.*?)}", RegexOptions.IgnoreCase);
        //    string displayName = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;
        //    //email
        //    match = Regex.Match(command, "{User:Email:(.*?)}", RegexOptions.IgnoreCase);
        //    string email = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;
        //    //user name
        //    match = Regex.Match(command, "{User:Username:(.*?)}", RegexOptions.IgnoreCase);
        //    string username = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;
        //    //password
        //    match = Regex.Match(command, "{User:Password:(.*?)}", RegexOptions.IgnoreCase);
        //    string password = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;
        //    //repeat password
        //    match = Regex.Match(command, "{User:RepeatPassword:(.*?)}", RegexOptions.IgnoreCase);
        //    string repeatPassword = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    var options = new ServiceUserRegisterOptions();
        //    //register type
        //    match = Regex.Match(command, "{Options:RegisterType:(\\d)}", RegexOptions.IgnoreCase);
        //    if (match != null && !string.IsNullOrEmpty(match.Groups[1].Value)) options.RegisterType = int.Parse(match.Groups[1].Value);
        //    //email as username
        //    match = Regex.Match(command, "{Options:EmailAsUsername:(true|false)}", RegexOptions.IgnoreCase);
        //    if (match != null && !string.IsNullOrEmpty(match.Groups[1].Value)) options.EmailAsUsername = bool.Parse(match.Groups[1].Value);
        //    //register in all portals
        //    match = Regex.Match(command, "{Options:RegisterInAllPortals:(true|false)}", RegexOptions.IgnoreCase);
        //    if (match != null && !string.IsNullOrEmpty(match.Groups[1].Value)) options.RegisterInAllPortals = bool.Parse(match.Groups[1].Value);

        //    if (password != repeatPassword)
        //        RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -101 } });

        //    if (!UserController.ValidatePassword(password))
        //        RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -104 } });

        //    bool userRegsitered = false;
        //    UserInfo objUserInfo = null;
        //    UserCreateStatus userCreatedStatus = UserCreateStatus.UnexpectedError;

        //    var oldUser = UserController.GetUserByName(portalSettings.PortalId, username);
        //    if (oldUser != null && !oldUser.Membership.Approved)
        //    {
        //        oldUser.Membership.Approved = true;
        //        UserController.UpdateUser(portalSettings.PortalId, oldUser);

        //        if (options.RegisterType != 0) oldUser.Membership.Approved = false;

        //        string ip = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "";

        //        UserLoginStatus status = UserLoginStatus.LOGIN_FAILURE;
        //        UserController.ValidateUser(portalSettings.PortalId, username, password, "", portalSettings.PortalName, ip, ref status);
        //        if (status == UserLoginStatus.LOGIN_FAILURE)
        //        {
        //            var pass = MembershipProvider.Instance().ResetPassword(oldUser, string.Empty);
        //            if (!MembershipProvider.Instance().ChangePassword(oldUser, pass, password))
        //            {
        //                UserController.UpdateUser(portalSettings.PortalId, oldUser);

        //                RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -104 } });
        //            }
        //        }

        //        objUserInfo = oldUser;

        //        userRegsitered = true;

        //        userCreatedStatus = UserCreateStatus.Success;
        //    }

        //    if (!userRegsitered)
        //    {
        //        objUserInfo = new UserInfo()
        //        {
        //            PortalID = portalSettings.PortalId,
        //            Username = options.EmailAsUsername ? email : username,
        //            Email = email,
        //            FirstName = firstName,
        //            LastName = lastName,
        //            DisplayName = !string.IsNullOrEmpty(displayName) ? displayName : (firstName + " " + lastName),
        //        };

        //        objUserInfo.Profile.FirstName = firstName;
        //        objUserInfo.Profile.LastName = lastName;

        //        objUserInfo.Membership.Password = password;
        //        objUserInfo.Membership.PasswordConfirm = repeatPassword;
        //        objUserInfo.Membership.Approved = options.RegisterType == 0 ? true : false;

        //        userCreatedStatus = UserController.CreateUser(ref objUserInfo);
        //    }

        //    if (userCreatedStatus == UserCreateStatus.UserAlreadyRegistered || userCreatedStatus == UserCreateStatus.UsernameAlreadyExists || userCreatedStatus == UserCreateStatus.DuplicateUserName)
        //        RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -102 } });
        //    else if (userCreatedStatus == UserCreateStatus.InvalidUserName)
        //        RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -103 } });
        //    else if (userCreatedStatus == UserCreateStatus.InvalidPassword)
        //        RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -104 } });
        //    else if (userCreatedStatus == UserCreateStatus.PasswordMismatch)
        //        RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -102 } });
        //    else if (userCreatedStatus == UserCreateStatus.BannedPasswordUsed || userCreatedStatus == UserCreateStatus.InvalidPassword)
        //        RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -105 } });
        //    else if (userCreatedStatus == UserCreateStatus.DuplicateEmail)
        //        RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -106 } });
        //    else if (userCreatedStatus == UserCreateStatus.Success)
        //    {
        //        result.UserID = objUserInfo.UserID;
        //        result.Status = UserLoginStatus.LOGIN_SUCCESS;
        //        result.Username = objUserInfo.Username;
        //        result.DisplayName = objUserInfo.DisplayName;

        //        if (!objUserInfo.IsInRole("Registered Users"))
        //        {
        //            var registeredRole = RoleController.Instance.GetRoleByName(portalSettings.PortalId, "Registered Users");
        //            RoleController.AddUserRole(objUserInfo, registeredRole, portalSettings, RoleStatus.Approved, Null.NullDate, Null.NullDate, false, false);
        //        }

        //        //if (options.RegisterType == 1 && !objUserInfo.IsInRole("Unverified Users"))
        //        //{
        //        //    var unverifiedRole = RoleController.Instance.GetRoleByName(portalSettings.PortalId, "Unverified Users");
        //        //    RoleController.AddUserRole(objUserInfo, unverifiedRole, portalSettings, RoleStatus.Approved, Null.NullDate, Null.NullDate, false, false);
        //        //}

        //        UserController.UpdateUser(portalSettings.PortalId, objUserInfo);

        //        //if (options.RegisterInAllPortals)
        //        //{
        //        //    var portals = PortalController.Instance.GetPortals();
        //        //    foreach (PortalInfo portal in portals)
        //        //    {
        //        //        if (UserController.GetUserById(portal.PortalID, objUserInfo.UserID) == null)
        //        //        {
        //        //            UserController.CopyUserToPortal(objUserInfo, portal, true);

        //        //            var u = UserController.GetUserById(portal.PortalID, objUserInfo.UserID);

        //        //            if (!u.IsInRole("Registered Users"))
        //        //            {
        //        //                var registeredRole = RoleController.Instance.GetRoleByName(portalSettings.PortalId, "Registered Users");
        //        //                RoleController.AddUserRole(u, registeredRole, portalSettings, RoleStatus.Approved, Null.NullDate, Null.NullDate, false, false);
        //        //            }

        //        //            u.Membership.Approved = options.RegisterType == 0 ? true : false;
        //        //            UserController.UpdateUser(portal.PortalID, u);

        //        //            //if (options.RegisterType == 1)
        //        //            //{
        //        //            //    if (u.IsInRole("Subscribers"))
        //        //            //    {
        //        //            //        var subscribersRole = RoleController.Instance.GetRoleByName(portal.PortalID, "Subscribers");
        //        //            //        RoleController.DeleteUserRole(u, subscribersRole, new PortalSettings(portal.PortalID), false);
        //        //            //    }

        //        //            //    if (!u.IsInRole("Unverified Users"))
        //        //            //    {
        //        //            //        var unverifiedRole = RoleController.Instance.GetRoleByName(portal.PortalID, "Unverified Users");
        //        //            //        RoleController.AddUserRole(u, unverifiedRole, new PortalSettings(portal.PortalID), RoleStatus.Approved, Null.NullDate, Null.NullDate, false, false);
        //        //            //    }
        //        //            //}
        //        //        }
        //        //    }
        //        //}

        //        var profileCollection = new ProfilePropertyDefinitionCollection();
        //        var types = new ListController().GetListEntryInfoItems("DataType");
        //        foreach (Match m in Regex.Matches(command, "{Profile:(.*?):(.*?)}", RegexOptions.IgnoreCase))
        //        {
        //            if (m.Groups == null || m.Groups.Count < 3) continue;

        //            string fieldName = m.Groups[1].Value;
        //            string fieldValue = m.Groups[2].Value;

        //            if (!string.IsNullOrEmpty(fieldValue))
        //            {

        //                var profileDefinition = ProfileController.GetPropertyDefinitionByName(portalSettings.PortalId, fieldName);
        //                if (profileDefinition == null)
        //                {
        //                    string dnnDataTypeStr = "Text";
        //                    var objDnnDataType = types.FirstOrDefault(t => t.Value == dnnDataTypeStr);

        //                    profileDefinition = new ProfilePropertyDefinition(portalSettings.PortalId);
        //                    profileDefinition.PortalId = portalSettings.PortalId;
        //                    profileDefinition.PropertyName = fieldName;
        //                    profileDefinition.DataType = objDnnDataType != null ? objDnnDataType.EntryID : types.First().EntryID;
        //                    profileDefinition.PropertyCategory = "Basic";
        //                    profileDefinition.DefaultVisibility = UserVisibilityMode.AllUsers;
        //                    profileDefinition.Length = 500;
        //                    profileDefinition.Visible = true;
        //                    profileDefinition.Required = false;
        //                    ProfileController.AddPropertyDefinition(profileDefinition);
        //                }

        //                if (types.FirstOrDefault(t => t.EntryID == profileDefinition.DataType).Value == "Image" && !string.IsNullOrEmpty(fieldValue))
        //                {
        //                    var filePath = HttpContext.Current.Server.MapPath(fieldValue);
        //                    if (File.Exists(filePath))
        //                    {
        //                        var filename = Path.GetFileName(filePath);
        //                        var fileBytes = File.ReadAllBytes(filePath);
        //                        var stream = new MemoryStream(fileBytes);
        //                        var contentType = MimeMapping.GetMimeMapping(Path.GetFileName(filePath));

        //                        var folder = FolderManager.Instance.GetUserFolder(objUserInfo);

        //                        var file = FileManager.Instance.AddFile(folder, filename, stream, true, false, contentType, objUserInfo.UserID);

        //                        fieldValue = file.FileId.ToString();
        //                    }
        //                }

        //                profileDefinition.PropertyValue = fieldValue;
        //                profileCollection.Add(profileDefinition);
        //            }
        //        }

        //        ProfileController.UpdateUserProfile(objUserInfo, profileCollection);
        //    }
        //    else
        //        RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -120 } });

        //    return result;
        //}

        //public static int LoginOrRegisterUser(ServiceInfo service, string command, PortalSettings portalSettings)
        //{
        //    try
        //    {
        //        LoginUser(service, command, portalSettings);
        //    }
        //    catch
        //    {
        //        RegisterUser(service, command, portalSettings);
        //        LoginUser(service, command, portalSettings);
        //    }

        //    return 1;
        //}

        //public static ServiceUserRegisterInfo ApproveUser(ServiceInfo service, string command, PortalSettings portalSettings)
        //{
        //    var result = new ServiceUserRegisterInfo();

        //    //user name
        //    var match = Regex.Match(command, "{Username:(.*?)}", RegexOptions.IgnoreCase);
        //    string username = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    //user id
        //    match = Regex.Match(command, "{UserID:(.*?)}", RegexOptions.IgnoreCase);
        //    int userID = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? int.Parse(match.Groups[1].Value) : -1;

        //    UserInfo user = null;
        //    if (userID != -1)
        //        user = UserController.GetUserById(portalSettings.PortalId, userID);
        //    else if (!string.IsNullOrEmpty(username))
        //        user = UserController.GetUserByName(portalSettings.PortalId, username);

        //    if (user != null)
        //    {
        //        var portals = PortalController.Instance.GetPortals();
        //        foreach (PortalInfo portal in portals)
        //        {
        //            var u = UserController.GetUserById(portal.PortalID, user.UserID);
        //            if (u != null)
        //            {
        //                if (u.IsInRole("Unverified Users"))
        //                {
        //                    var unverifiedRole = RoleController.Instance.GetRoleByName(portal.PortalID, "Unverified Users");
        //                    RoleController.DeleteUserRole(u, unverifiedRole, new PortalSettings(portal.PortalID), false);
        //                }

        //                UserController.ApproveUser(u);

        //                u.Membership.Approved = true;
        //                UserController.UpdateUser(portal.PortalID, u);
        //            }
        //        }
        //    }
        //    else
        //        throw new Exception("error");

        //    return result;
        //}

        //public static object GetUserIDByUsername(ServiceInfo service, string command, PortalSettings portalSettings)
        //{
        //    var options = new ServiceUserLoginOptions();

        //    var match = Regex.Match(command, "{Username:(.*?)}", RegexOptions.IgnoreCase);
        //    string username = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) ? match.Groups[1].Value : string.Empty;

        //    var user = UserController.GetUserByName(portalSettings.PortalId, username);

        //    if (user != null)
        //        return new { UserID = user.UserID };
        //    else
        //        throw new Exception("نام کاربری اشتباه می باشد!");
        //}

        //public static ServiceUserRegisterInfo ResetUserPassword(ServiceInfo service, string command, PortalSettings portalSettings)
        //{
        //    var result = new ServiceUserRegisterInfo();

        //    //user name
        //    var match = Regex.Match(command, "{Username:(.*?)}", RegexOptions.IgnoreCase);
        //    string username = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    //user id
        //    match = Regex.Match(command, "{UserID:(.*?)}", RegexOptions.IgnoreCase);
        //    int userID = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? int.Parse(match.Groups[1].Value) : -1;

        //    //password
        //    match = Regex.Match(command, "{Password:(.*?)}", RegexOptions.IgnoreCase);
        //    string password = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    //repeat password
        //    match = Regex.Match(command, "{RepeatPassword:(.*?)}", RegexOptions.IgnoreCase);
        //    string repeatPassword = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    if (password != repeatPassword)
        //        RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -101 } });

        //    UserInfo user = null;
        //    if (userID != -1)
        //        user = UserController.GetUserById(portalSettings.PortalId, userID);
        //    else if (!string.IsNullOrEmpty(username))
        //        user = UserController.GetUserByName(portalSettings.PortalId, username);

        //    if (user == null)
        //    {
        //        var u = DbUtil.ExecuteReader(string.Format("Select u.UserID,up.PortalID From dbo.Users u inner join dbo.UserPortals up on u.UserID = up.UserID Where u.Username='{0}'", username));
        //        if (u.Read())
        //        {
        //            user = UserController.GetUserById((int)u["PortalID"], (int)u["UserID"]);

        //            var portal = PortalController.Instance.GetPortal(portalSettings.PortalId);

        //            UserController.CopyUserToPortal(user, portal, true);
        //        }
        //    }

        //    if (user != null)
        //    {
        //        if (user.Membership.LockedOut) UserController.UnLockUser(user);

        //        if (!user.Membership.Approved)
        //        {
        //            var portals = PortalController.Instance.GetPortals();
        //            foreach (PortalInfo portal in portals)
        //            {
        //                var u = UserController.GetUserById(portal.PortalID, user.UserID);
        //                if (u != null)
        //                {
        //                    UserController.ApproveUser(u);

        //                    u.Membership.Approved = true;
        //                    UserController.UpdateUser(portal.PortalID, u);
        //                }
        //            }

        //            user = UserController.GetUserById(portalSettings.PortalId, user.UserID);
        //        }

        //        var pass = MembershipProvider.Instance().ResetPassword(user, string.Empty);

        //        if (!MembershipProvider.Instance().ChangePassword(user, pass, password))
        //            RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -104 } });
        //        else
        //        {
        //            result.Status = UserLoginStatus.LOGIN_SUCCESS;
        //            result.UserID = user.UserID;
        //        }
        //    }
        //    else
        //        RaiseException(new Exception(), new Dictionary<object, object> { { "Status", -102 }, { "Message", "User not found!" } });

        //    return result;
        //}

        //public static dynamic GetUserInfo(ServiceInfo service, string command, PortalSettings portalSettings)
        //{
        //    var result = new object();

        //    //user name
        //    var match = Regex.Match(command, "{Username:(.*?)}", RegexOptions.IgnoreCase);
        //    string username = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    //user id
        //    match = Regex.Match(command, "{UserID:(.*?)}", RegexOptions.IgnoreCase);
        //    int userID = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? int.Parse(match.Groups[1].Value) : -1;

        //    UserInfo user = null;
        //    if (userID != -1)
        //        user = UserController.GetUserById(portalSettings.PortalId, userID);
        //    else if (!string.IsNullOrEmpty(username))
        //        user = UserController.GetUserByName(portalSettings.PortalId, username);

        //    if (user != null)
        //    {
        //        var str = Newtonsoft.Json.JsonConvert.SerializeObject(user);

        //        result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(str);
        //    }
        //    else
        //        throw new Exception("error");

        //    return result;
        //}

        //public static ServiceUserRegisterInfo DeleteUser(ServiceInfo service, string command, PortalSettings portalSettings)
        //{
        //    var result = new ServiceUserRegisterInfo();

        //    //user id
        //    var match = Regex.Match(command, "{UserID:(.*?)}", RegexOptions.IgnoreCase);
        //    int userID = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? int.Parse(match.Groups[1].Value) : -1;

        //    //user name
        //    match = Regex.Match(command, "{Username:(.*?)}", RegexOptions.IgnoreCase);
        //    string username = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    var portals = PortalController.Instance.GetPortals();
        //    foreach (PortalInfo portal in portals)
        //    {
        //        var u = userID != -1 ? UserController.GetUserById(portal.PortalID, userID) : UserController.GetUserByName(portal.PortalID, username);
        //        if (u != null)
        //        {
        //            username = u.Username;

        //            UserController.DeleteUser(ref u, false, true);
        //            UserController.RemoveUser(u);
        //        }

        //        //UserController.RemoveDeletedUsers(portal.PortalID);
        //    }

        //    DbUtil.ExecuteSql(string.Format(@"delete from dbo.aspnet_Membership where UserId in
        //                (select userid from dbo.aspnet_Users where username = '{0}')
        //                    delete from dbo.aspnet_Users where username = '{0}'", username));

        //    DataCache.ClearCache();

        //    return result;
        //}

        //public static ServiceUserRegisterInfo GrantUserRole(ServiceInfo service, string command, PortalSettings portalSettings)
        //{
        //    var result = new ServiceUserRegisterInfo();

        //    //user name
        //    var match = Regex.Match(command, "{Username:(.*?)}", RegexOptions.IgnoreCase);
        //    string username = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    //user id
        //    match = Regex.Match(command, "{UserID:(.*?)}", RegexOptions.IgnoreCase);
        //    int userID = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? int.Parse(match.Groups[1].Value) : -1;

        //    //role name
        //    match = Regex.Match(command, "{RoleName:(.*?)}", RegexOptions.IgnoreCase);
        //    string roleName = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    //register in all portals
        //    match = Regex.Match(command, "{Options:GrantInAllPortals:(true|false)}", RegexOptions.IgnoreCase);
        //    bool inAllPortals = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) ? bool.Parse(match.Groups[1].Value) : false;

        //    UserInfo user = null;
        //    if (userID != -1)
        //        user = UserController.GetUserById(portalSettings.PortalId, userID);
        //    else if (!string.IsNullOrEmpty(username))
        //        user = UserController.GetUserByName(portalSettings.PortalId, username);

        //    if (user != null && !string.IsNullOrEmpty(roleName))
        //    {
        //        var portals = PortalController.Instance.GetPortals();
        //        foreach (PortalInfo portal in portals)
        //        {
        //            if (portal.PortalID == portalSettings.PortalId || inAllPortals)
        //            {
        //                var u = UserController.GetUserById(portal.PortalID, user.UserID);
        //                if (u != null)
        //                {
        //                    if (!u.IsInRole(roleName))
        //                    {
        //                        var role = RoleController.Instance.GetRoleByName(portal.PortalID, roleName);
        //                        if (role == null)
        //                        {
        //                            RoleController.Instance.AddRole(new RoleInfo()
        //                            {
        //                                PortalID = portal.PortalID,
        //                                RoleName = roleName,
        //                                RoleGroupID = Null.NullInteger,
        //                                IsPublic = false,
        //                                Status = RoleStatus.Approved
        //                            });
        //                            role = RoleController.Instance.GetRoleByName(portal.PortalID, roleName);
        //                        }

        //                        RoleController.AddUserRole(u, role, new PortalSettings(portal.PortalID), RoleStatus.Approved, Null.NullDate, Null.NullDate, false, false);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //        throw new Exception("error");

        //    return result;
        //}

        //public static ServiceUserRegisterInfo UpdateUserProfile(ServiceInfo service, string command, PortalSettings portalSettings)
        //{
        //    var result = new ServiceUserRegisterInfo();

        //    //user id
        //    var match = Regex.Match(command, "{User:UserID:(.*?)}", RegexOptions.IgnoreCase);
        //    int userID = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) ? int.Parse(match.Groups[1].Value) : -1;
        //    //first name
        //    match = Regex.Match(command, "{User:FirstName:(.*?)}", RegexOptions.IgnoreCase);
        //    string firstName = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;
        //    //last name
        //    match = Regex.Match(command, "{User:LastName:(.*?)}", RegexOptions.IgnoreCase);
        //    string lastName = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;
        //    //display name
        //    match = Regex.Match(command, "{User:DisplayName:(.*?)}", RegexOptions.IgnoreCase);
        //    string displayName = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;
        //    //email
        //    match = Regex.Match(command, "{User:Email:(.*?)}", RegexOptions.IgnoreCase);
        //    string email = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;
        //    //user name
        //    match = Regex.Match(command, "{User:Username:(.*?)}", RegexOptions.IgnoreCase);
        //    string username = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;
        //    //password
        //    match = Regex.Match(command, "{User:Password:(.*?)}", RegexOptions.IgnoreCase);
        //    string password = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;
        //    //repeat password
        //    match = Regex.Match(command, "{User:RepeatPassword:(.*?)}", RegexOptions.IgnoreCase);
        //    string repeatPassword = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    var portals = PortalController.Instance.GetPortals();
        //    foreach (PortalInfo portal in portals)
        //    {
        //        var user = UserController.GetUserById(portalSettings.PortalId, userID);

        //        if (user != null)
        //        {

        //            if (!string.IsNullOrEmpty(firstName)) user.FirstName = firstName;
        //            if (!string.IsNullOrEmpty(lastName)) user.LastName = lastName;
        //            if (!string.IsNullOrEmpty(email)) user.Email = email;

        //            if (!string.IsNullOrEmpty(displayName))
        //                user.DisplayName = displayName;
        //            else if (!string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(lastName))
        //                user.DisplayName = firstName + " " + lastName;

        //            UserController.UpdateUser(portal.PortalID, user);

        //            var profileCollection = new ProfilePropertyDefinitionCollection();
        //            var types = new ListController().GetListEntryInfoItems("DataType");
        //            foreach (Match m in Regex.Matches(command, "{Profile:(.*?):(.*?)}", RegexOptions.IgnoreCase))
        //            {
        //                if (m.Groups == null || m.Groups.Count < 3) continue;

        //                string fieldName = m.Groups[1].Value;
        //                string fieldValue = m.Groups[2].Value;

        //                if (!string.IsNullOrEmpty(fieldValue))
        //                {

        //                    var profileDefinition = ProfileController.GetPropertyDefinitionByName(portalSettings.PortalId, fieldName);
        //                    if (profileDefinition == null)
        //                    {
        //                        string dnnDataTypeStr = "Text";
        //                        var objDnnDataType = types.FirstOrDefault(t => t.Value == dnnDataTypeStr);

        //                        profileDefinition = new ProfilePropertyDefinition(portalSettings.PortalId);
        //                        profileDefinition.PortalId = portalSettings.PortalId;
        //                        profileDefinition.PropertyName = fieldName;
        //                        profileDefinition.DataType = objDnnDataType != null ? objDnnDataType.EntryID : types.First().EntryID;
        //                        profileDefinition.PropertyCategory = "Basic";
        //                        profileDefinition.DefaultVisibility = UserVisibilityMode.AllUsers;
        //                        profileDefinition.Length = 500;
        //                        profileDefinition.Visible = true;
        //                        profileDefinition.Required = false;
        //                        ProfileController.AddPropertyDefinition(profileDefinition);
        //                    }

        //                    if (types.FirstOrDefault(t => t.EntryID == profileDefinition.DataType).Value == "Image" && !string.IsNullOrEmpty(fieldValue))
        //                    {
        //                        var filePath = HttpContext.Current.Server.MapPath(fieldValue);
        //                        if (File.Exists(filePath))
        //                        {
        //                            var filename = Path.GetFileName(filePath);
        //                            var fileBytes = File.ReadAllBytes(filePath);
        //                            var stream = new MemoryStream(fileBytes);
        //                            var contentType = MimeMapping.GetMimeMapping(Path.GetFileName(filePath));

        //                            var folder = FolderManager.Instance.GetUserFolder(user);

        //                            var file = FileManager.Instance.AddFile(folder, filename, stream, true, false, contentType, user.UserID);

        //                            fieldValue = file.FileId.ToString();
        //                        }
        //                    }

        //                    profileDefinition.PropertyValue = fieldValue;
        //                    profileCollection.Add(profileDefinition);
        //                }
        //            }

        //            ProfileController.UpdateUserProfile(user, profileCollection);
        //        }
        //    }

        //    return result;
        //}

        //public static void SendEmail(ServiceInfo service, string command, PortalSettings portalSettings)
        //{
        //    var result = new ServiceUserLoginInfo();

        //    //email
        //    var match = Regex.Match(command, "{Email:(.*?)}", RegexOptions.IgnoreCase);
        //    string email = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    //subject
        //    match = Regex.Match(command, "{Subject:(.*?)}", RegexOptions.IgnoreCase);
        //    string subject = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    //body
        //    match = Regex.Match(Regex.Replace(command, @"\r\n?|\n", ""), "{Body:(.*?)}", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);
        //    string body = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    Mail.SendEmail(portalSettings.Email, email, subject, body);
        //}

        //public static void SendSms(ServiceInfo service, string command, PortalSettings portalSettings)
        //{
        //    //provider
        //    var match = Regex.Match(command, "{Provider:(.*?)}", RegexOptions.IgnoreCase);
        //    string provider = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    //mobile
        //    match = Regex.Match(command, "{Mobile:(.*?)}", RegexOptions.IgnoreCase);
        //    string mobile = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    //message
        //    match = Regex.Match(command, "{Message:(.*?)}", RegexOptions.IgnoreCase);
        //    string body = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    //message
        //    //match = Regex.Match(command, @"\[(\w+):(.[^\[]+)\]", RegexOptions.IgnoreCase);
        //    //string body = match != null && !string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;

        //    SmsGatewayService.SendSms(portalSettings, provider, mobile, body);
        //}

        //public static async Task<dynamic> ExecuteStoredProcedure(ServiceInfo service, Hashtable serviceParams)
        //{
        //    dynamic result = null;

        //    string connectionString = DotNetNuke.Data.DataProvider.Instance().ConnectionString;

        //    if (service.DatabaseID != null)
        //    {
        //        connectionString = DatabaseRepository.Instance.GetDatabase(service.DatabaseID.Value).ConnectionString;
        //    }

        //    var connection = new SqlConnection(connectionString);

        //    var queryParameters = new Dapper.DynamicParameters();

        //    var paramList = new List<ServiceParamInfo>();

        //    if (!string.IsNullOrEmpty(service.Params))
        //        paramList = JsonConvert.DeserializeObject<List<ServiceParamInfo>>(service.Params);

        //    foreach (var p in paramList)
        //    {
        //        queryParameters.Add(p.ParamName, serviceParams[p.ParamName] != null ? serviceParams[p.ParamName].ToString() : (!string.IsNullOrEmpty(p.DefaultValue) ? p.DefaultValue : null));
        //    }

        //    await Task.Run(() =>
        //    {
        //        if (service.ServiceSubtype == "SubmitEntity")
        //        {
        //            result = Dapper.SqlMapper.ExecuteScalar(connection, "[dbo]." + service.DatabaseObjectName, queryParameters, commandTimeout: int.MaxValue, commandType: CommandType.StoredProcedure);
        //        }
        //        else if (service.ServiceSubtype == "GetDataRow")
        //        {
        //            var list = Dapper.SqlMapper.Query(connection, "[dbo]." + service.DatabaseObjectName, queryParameters, commandTimeout: int.MaxValue, commandType: CommandType.StoredProcedure);
        //            if (list.Any()) result = list.First();
        //        }
        //        else if (service.ServiceSubtype == "GetListDataSource")
        //        {
        //            result = Dapper.SqlMapper.Query(connection, "[dbo]." + service.DatabaseObjectName, queryParameters, commandTimeout: int.MaxValue, commandType: CommandType.StoredProcedure);
        //        }
        //    });

        //    return result;
        //}

        //private static void RaiseException(Exception ex, Dictionary<object, object> data)
        //{
        //    foreach (var key in data.Keys)
        //    {
        //        ex.Data.Add(key, data[key]);
        //    }

        //    throw ex;
        //}

        //public static string ProccessWebServiceParamValue(string token, Hashtable webServiceParams)
        //{
        //    if (string.IsNullOrEmpty(token)) return string.Empty;

        //    var matches = Regex.Matches(token, "{Token:\\w+}", RegexOptions.IgnoreCase);
        //    foreach (Match match in matches)
        //    {
        //        var value = webServiceParams[Regex.Match(match.Value, "(?<={Token:)\\w+(?=\\})", RegexOptions.IgnoreCase).Value];
        //        if (value == null) value = string.Empty;

        //        token = token.Replace(match.Value, value.ToString());
        //    }

        //    return token;
        //}
    }
}