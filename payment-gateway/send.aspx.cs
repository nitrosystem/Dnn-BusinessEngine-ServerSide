using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Components;
using NitroSystem.Dnn.BusinessEngine.Core.Providers.PaymentGateway;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroSystem.Dnn.BusinessEngine.PaymentGateway
{
    public partial class Send : Page
    {
        private int UserID
        {
            get
            {
                string userName = HttpContext.Current != null ? HttpContext.Current.User.Identity.Name : "";

                var user = UserController.GetUserByName(userName);

                return user != null ? user.UserID : -1;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var q=Request.QueryString["q"];

            var gatewayParams = PaymentGatewayService.SendToGateway(Request, this.UserID, q);
            if (gatewayParams.GatewayType == 0)
            {
                Response.Redirect(gatewayParams.GatewayUrl);
            }
            else if (gatewayParams.GatewayType == 1)
            {
                Response.Write(gatewayParams.GatewayUrl);
                ClientScript.RegisterStartupScript(typeof(Page), "ClientScript", gatewayParams.GatewayUrl, false);
            }
        }
    }
}