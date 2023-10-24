using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Core.Providers.PaymentGateway
{
    public class PaymentGatewayResult
    {
        public int Status { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string ReferenceID { get; set; }
        public string TrackingID { get; set; }
    }
}