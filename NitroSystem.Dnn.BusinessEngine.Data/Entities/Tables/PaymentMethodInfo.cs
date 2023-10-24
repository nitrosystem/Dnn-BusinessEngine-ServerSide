using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_PaymentMethods")]
    [PrimaryKey("PaymentMethodID", AutoIncrement = false)]
    [Cacheable("BE_PaymentMethods_", CacheItemPriority.Default, 20)]
    [Scope("ScenarioID")]
    public class PaymentMethodInfo
    {
        public Guid PaymentMethodID { get; set; }
        public Guid ScenarioID { get; set; }
        public string PaymentMethodName { get; set; }
        public string SuccessfulPaymentTemplate{ get; set; }
        public string UnsuccessfulPaymentTemplate { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}