using NitroSystem.Dnn.BusinessEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Core.Providers.PaymentGateway
{
    public interface ISendToPaymentGateway
    {
        PaymentGatewayParams SendToGateway(PaymentInfo paymentMethod);
    }

}