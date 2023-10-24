using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_Payments")]
    [PrimaryKey("PaymentID", AutoIncrement = false)]
    [Cacheable("BE_Payments_", CacheItemPriority.Default, 20)]
    [Scope("PaymentMethodID")]
    public class PaymentInfo
    {
        public Guid PaymentID { get; set; }
        public Guid PaymentMethodID { get; set; }
        public Guid ModuleID { get; set; }
        public int UserID { get; set; }
        public string PaymentKey { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public bool IsSuccess { get; set; }
        public int Status { get; set; }
        public string ReferenceNumber { get; set; }
        public string PaymentGateway { get; set; }
        public string PaymentParams { get; set; }
        public string ErrorMessage { get; set; }
    }
}