using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class PaymentRepository
    {
        public static PaymentRepository Instance
        {
            get
            {
                return new PaymentRepository();
            }
        }

        private const string CachePrefix = "BE_Payments_";

        public Guid AddPayment(PaymentInfo objPaymentInfo)
        {
            objPaymentInfo.PaymentID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PaymentInfo>();
                rep.Insert(objPaymentInfo);

                DataCache.ClearCache(CachePrefix);

                return objPaymentInfo.PaymentID;
            }
        }

        public void UpdatePayment(PaymentInfo objPaymentInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PaymentInfo>();
                rep.Update(objPaymentInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeletePayment(Guid paymentID)
        {
            PaymentInfo objPaymentInfo = GetPayment(paymentID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PaymentInfo>();
                rep.Delete(objPaymentInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public PaymentInfo GetPayment(Guid paymentID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PaymentInfo>();
                return rep.GetById(paymentID);
            }
        }

        public PaymentInfo GetPaymentByKey(string paymentKey)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PaymentInfo>();

                var result = rep.Find("Where PaymentKey = @0", paymentKey);

                return result.Any() ? result.First() : null;
            }
        }

        public IEnumerable<PaymentInfo> GetPayments(int moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PaymentInfo>();

                return rep.Get(moduleID);
            }
        }
    }
}