using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Portals.Internal;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
namespace NitroSystem.Dnn.BusinessEngine.Api.Base
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        private const string Realm = "Business Engine Realm";
        private const string AuthScheme = "Bearer";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //If the Authorization header is empty or null
            //then return Unauthorized
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

                // If the request was unauthorized, add the WWW-Authenticate header 
                // to the response which indicates that it require basic authentication
                if (actionContext.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", Realm));
                }
            }
            else
            {
                //Get the authentication token from the request header
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;

                //Decode the string
                string decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));

                //Convert the string into an string array
                string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');

                //First element of the array is the username
                string username = usernamePasswordArray[0];

                //Second element of the array is the password
                string password = usernamePasswordArray[1];

                var ps = PortalController.Instance.GetCurrentSettings();
                if (ps == null)
                {
                    throw new Exception("no-portal");
                }
                var portalSettings = new PortalSettings(ps.PortalId);

                var status = UserLoginStatus.LOGIN_FAILURE;
                var ipAddress = actionContext.Request.GetIPAddress() ?? string.Empty;

                var userInfo = UserController.ValidateUser(portalSettings.PortalId, username, password, "DNN", string.Empty, portalSettings.PortalName, ipAddress, ref status);
                if (userInfo == null)
                {
                    throw new Exception("bad-credentials");
                }

                var valid =
                   status == UserLoginStatus.LOGIN_SUCCESS ||
                   status == UserLoginStatus.LOGIN_SUPERUSER ||
                   status == UserLoginStatus.LOGIN_INSECUREADMINPASSWORD ||
                   status == UserLoginStatus.LOGIN_INSECUREHOSTPASSWORD;

                if (!valid)
                {
                    UserController.UserLogin(portalSettings.PortalId, userInfo, portalSettings.PortalName, ipAddress, true);
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

                    //throw new Exception("bad-credentials");
                }
                else
                {
                    var identity = new GenericIdentity(username);

                    IPrincipal principal = new GenericPrincipal(identity, null);
                    Thread.CurrentPrincipal = principal;

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = principal;
                    }

                    //HttpContext.Current.au
                }

                HttpContext.Current.Response.StatusCode = 200;
            }
        }

       

    }
}