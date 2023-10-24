using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Models
{
  public  class PaymentResultInfo
    {
        public int PortalID { get; set; }
        public int UserID { get; set; }
        public Guid ModuleID { get; set; }
        public bool IsSuccess { get; set; }
        public string TransactionNumber { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentParams { get; set; }
        public string SuccessPaymentMessage { get; set; }
        public string FailedPaymentMessage { get; set; }
    }
}
