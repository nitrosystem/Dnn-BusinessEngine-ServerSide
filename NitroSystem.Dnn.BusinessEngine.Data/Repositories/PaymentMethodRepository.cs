using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class PaymentMethodRepository
    {
        public static PaymentMethodRepository Instance
        {
            get
            {
                return new PaymentMethodRepository();
            }
        }

        private const string CachePrefix = "BE_PaymentMethods_";

        public Guid AddPaymentMethod(PaymentMethodInfo objPaymentMethodInfo)
        {
            objPaymentMethodInfo.PaymentMethodID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PaymentMethodInfo>();
                rep.Insert(objPaymentMethodInfo);

                DataCache.ClearCache(CachePrefix);

                return objPaymentMethodInfo.PaymentMethodID;
            }
        }

        public void UpdatePaymentMethod(PaymentMethodInfo objPaymentMethodInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PaymentMethodInfo>();
                rep.Update(objPaymentMethodInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeletePaymentMethod(Guid paymentMethodID)
        {
            PaymentMethodInfo objPaymentMethodInfo = GetPaymentMethod(paymentMethodID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PaymentMethodInfo>();
                rep.Delete(objPaymentMethodInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public PaymentMethodInfo GetPaymentMethod(Guid paymentMethodID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PaymentMethodInfo>();
                return rep.GetById(paymentMethodID);
            }
        }

        public IEnumerable<PaymentMethodInfo> GetPaymentMethods(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PaymentMethodInfo>();

                return rep.Get(scenarioID);
            }
        }

        public IEnumerable<PaymentMethodInfo> GetPaymentMethods()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PaymentMethodInfo>();

                return rep.Get();
            }
        }
    }
}