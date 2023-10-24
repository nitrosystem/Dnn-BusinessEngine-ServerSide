using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Components;
using NitroSystem.Dnn.BusinessEngine.Core.Providers.PaymentGateway;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroSystem.Dnn.BusinessEngine.PaymentGateway
{
    public partial class Verify : System.Web.UI.Page
    {
        public int PortalID { get; set; }
        
        public int UserID { get; set; }
        
        public Guid ModuleID { get; set; }

        public string BaseUrl
        {
            get
            {
                return "/";
            }
        }

        public string SiteRoot
        {
            get
            {
                string httpStr = HttpContext.Current.Request.IsSecureConnection ? "https://" : "http://";
                string domain = httpStr + PortalAliasController.Instance.GetPortalAlias(new PortalSettings(PortalID).DefaultPortalAlias, PortalID).HTTPAlias.ToLower().Replace(httpStr, "");
                return domain.EndsWith("/") ? domain : domain + "/";
            }
        }

        public string Version
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }

        public object Q { get; set; }

        public string TN { get; set; }

        public int PS { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            var paymentKey = Request.Params["key"];

            var paymentResult = PaymentGatewayService.VerifyPayment(paymentKey);

            this.Q = paymentResult.PaymentParams;
            this.PortalID = paymentResult.PortalID;
            this.UserID = paymentResult.UserID;
            this.ModuleID = paymentResult.ModuleID;
            this.TN = paymentResult.TransactionNumber;
            this.PS = paymentResult.PaymentStatus;

            if (paymentResult.IsSuccess)
            {
                pnlContent.InnerHtml = paymentResult.SuccessPaymentMessage;
            }
            else
            {
                pnlContent.InnerHtml = paymentResult.FailedPaymentMessage;
            }
        }
    }
}