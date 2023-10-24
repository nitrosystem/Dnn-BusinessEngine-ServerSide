using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels
{
    public class PaymentMethodViewModel
    {
        public Guid PaymentMethodID { get; set; }
        public Guid ScenarioID { get; set; }
        public int PortalID { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaymentDescription { get; set; }
        public string ReturnUrl { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}
