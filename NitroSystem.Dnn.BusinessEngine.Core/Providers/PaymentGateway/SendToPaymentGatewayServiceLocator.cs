using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Core.Providers.PaymentGateway
{
    public static class SendToPaymentGatewayServiceLocator
    {
        public static ISendToPaymentGateway gatewayPaymentService = null;

        public static ISendToPaymentGateway SetInstance(ISendToPaymentGateway gatewayPaymentManager)
        {
            if (gatewayPaymentService == null) return gatewayPaymentManager;
            return gatewayPaymentService;
        }
    }
}