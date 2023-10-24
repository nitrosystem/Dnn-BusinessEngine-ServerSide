using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Models
{
    public class PaymentInfo
    {
        public Guid PaymentMethodID { get; set; }
        public Guid ScenarioID { get; set; }
        public string PaymentName { get; set; }
        public int PortalID { get; set; }
        public decimal Amount { get; set; }
        public string PaymentDescription { get; set; }
        public string ReturnUrl { get; set; }
    }
}
