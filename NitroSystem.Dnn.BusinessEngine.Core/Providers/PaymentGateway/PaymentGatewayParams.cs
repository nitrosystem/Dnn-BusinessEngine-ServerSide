using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Core.Providers.PaymentGateway
{
    public class PaymentGatewayParams
    {
        public string PaymentKey { get; set; }
        public bool IsReadyForPayment { get; set; }
        public string GatewayUrl { get; set; }
        public string ErrorMessage { get; set; }
        public int GatewayType { get; set; }
    }
}