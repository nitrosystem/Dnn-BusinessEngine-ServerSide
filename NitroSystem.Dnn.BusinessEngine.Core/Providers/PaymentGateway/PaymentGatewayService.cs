using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Core.Providers.PaymentGateway
{
   public static class PaymentGatewayService
    {
        public static PaymentGatewayParams SendToGateway(HttpRequest request,int userID, string paymentData)
        {
            return null;

            //byte[] data = Convert.FromBase64String(paymentData);
            //var base64Decoded = ASCIIEncoding.ASCII.GetString(data);

            //var paymentParamsStr = HttpUtility.UrlDecode(base64Decoded);
            //var paymentParams = JsonConvert.DeserializeObject<JObject>(paymentParamsStr);

            //Guid paymentMethodID = Guid.Parse(paymentParams.Value<string>("PaymentMethodID"));
            //Guid moduleID = Guid.Parse(paymentParams.Value<string>("ModuleID"));
            //string paymentGateway = paymentParams.Value<string>("PaymentGateway");
            //int portalID = paymentParams.Value<int>("PortalID");
            //int amount = paymentParams.Value<int>("Amount");

            //var objPaymentMethodInfo = PaymentMethodRepository.Instance.GetPaymentMethod(paymentMethodID);

            //var paymentMethod = new PaymentInfo()
            //{
            //    PaymentMethodID = objPaymentMethodInfo.PaymentMethodID,
            //    PaymentName = objPaymentMethodInfo.PaymentName,
            //    Amount = amount != 0 ? amount : objPaymentMethodInfo.Amount,
            //    PortalID = portalID,
            //    ReturnUrl = request.Url.GetLeftPart(UriPartial.Authority) + "/DesktopModules/BusinessEngine/PaymentGateway/Verify.aspx"
            //};

            //var gatewayProvider = ProviderRepository.Instance.GetProviders("PaymentGateway").FirstOrDefault(p => p.ProviderName == paymentGateway);
            //if (gatewayProvider == null) throw new Exception("Provider not found!.");

            //var gatewayPaymentInterface = (ISendToPaymentGateway)Activator.CreateInstance(Type.GetType(gatewayProvider.TypeName));
            //var gatewayPaymentService = SendToPaymentGatewayServiceLocator.SetInstance(gatewayPaymentInterface);
            //var gatewayParams = gatewayPaymentService.SendToGateway(paymentMethod);

            //if (!gatewayParams.IsReadyForPayment) throw new Exception(gatewayParams.ErrorMessage);

            //PaymentRepository.Instance.AddPayment(new Data.Entities.Tables.PaymentInfo()
            //{
            //    PaymentMethodID = paymentMethodID,
            //    PaymentGateway = paymentGateway,
            //    PaymentKey = gatewayParams.PaymentKey,
            //    PaymentDate = DateTime.Now,
            //    Amount = amount,
            //    PortalID = portalID,
            //    UserID = userID,
            //    ModuleID = moduleID,
            //    IsSuccess = false,
            //    PaymentParams = paymentParamsStr
            //});

            //return gatewayParams;
        }

        public static PaymentResultInfo VerifyPayment(string paymentKey)
        {
            return null;

            //var result = new PaymentResultInfo();

            //var payment = PaymentRepository.Instance.GetPaymentByKey(paymentKey);

            ////var paymentParams = JsonConvert.DeserializeObject<JObject>(payment.PaymentParams);

            //result.PortalID = payment.PortalID;
            //result.UserID = payment.UserID;
            //result.ModuleID = payment.ModuleID;
            //result.IsSuccess = payment.IsSuccess;
            //result.TransactionNumber = payment.ReferenceID;
            //result.PaymentStatus = payment.Status;
            //result.PaymentParams = payment.PaymentParams;

            //var objPaymentMethodInfo = PaymentMethodRepository.Instance.GetPaymentMethod(payment.PaymentMethodID);

            //result.FailedPaymentMessage = ParseToken(objPaymentMethodInfo.FailedPaymentMessage, payment);
            //result.SuccessPaymentMessage = ParseToken(objPaymentMethodInfo.SuccessPaymentMessage, payment);

            //if (payment.IsSuccess)
            //{
            //    result.SuccessPaymentMessage = result.SuccessPaymentMessage.Replace("{ReferenceID}", payment.ReferenceID);
            //    result.SuccessPaymentMessage = result.SuccessPaymentMessage.Replace("{Status}", payment.Status.ToString());
            //    result.SuccessPaymentMessage = result.SuccessPaymentMessage.Replace("{TrackingID}", payment.TrackingID);
            //}

            //return result;
        }

        //private static string ParseToken(string content, Data.Entities.Tables.PaymentInfo paymentResult)
        //{
        //    content = content.Replace("{Status}", paymentResult.Status.ToString());
        //    content = content.Replace("{TrackingID}", paymentResult.TrackingID);
        //    content = content.Replace("{ReferenceID}", paymentResult.ReferenceID);
        //    content = content.Replace("{ErrorMessage}", paymentResult.ErrorMessage);

        //    return content;
        //}
    }
}
