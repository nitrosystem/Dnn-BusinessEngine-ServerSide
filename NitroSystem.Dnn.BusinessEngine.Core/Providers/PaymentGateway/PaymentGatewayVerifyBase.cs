using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Core.Providers.PaymentGateway
{
    public abstract class PaymentGatewayVerifyBase : System.Web.UI.Page
    {
        protected PaymentInfo GetPaymentInfo(string paymentKey)
        {
            var payment = PaymentRepository.Instance.GetPaymentByKey(paymentKey);
            return payment;
        }

        protected virtual void Verify(PaymentInfo payment, PaymentGatewayResult paymentResult)
        {
            //if (paymentResult.IsSuccess)
            //{
            //    payment.IsSuccess = paymentResult.IsSuccess;
            //    payment.ReferenceID = paymentResult.ReferenceID;
            //    payment.TrackingID = paymentResult.TrackingID;
            //    payment.Status = paymentResult.Status;
            //}
            //else
            //{ 
            //    payment.ErrorMessage = paymentResult.ErrorMessage;
            //}

            //PaymentRepository.Instance.UpdatePayment(payment);

            //Response.Redirect("/DesktopModules/BusinessEngine/PaymentGateway/Verify.aspx?key=" + payment.PaymentKey);
        }
    }
}