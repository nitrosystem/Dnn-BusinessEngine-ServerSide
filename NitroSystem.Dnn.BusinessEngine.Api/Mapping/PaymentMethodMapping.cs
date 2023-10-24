using AutoMapper;
using Newtonsoft.Json;
using NitroSystem.Dnn.BusinessEngine.Api.ViewModels;
using NitroSystem.Dnn.BusinessEngine.Common.TypeCasting;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Mapping
{
    internal static class PaymentMethodMapping
    {
        #region PaymentMethod Mapping

        internal static IEnumerable<PaymentMethodViewModel> GetPaymentMethodsViewModel(Guid scenarioID)
        {
            var paymentMethods = PaymentMethodRepository.Instance.GetPaymentMethods(scenarioID);

            return GetPaymentMethodsViewModel(paymentMethods);
        }

        internal static IEnumerable<PaymentMethodViewModel> GetPaymentMethodsViewModel(IEnumerable<PaymentMethodInfo> paymentMethods)
        {
            var result = new List<PaymentMethodViewModel>();

            if (paymentMethods != null)
            {
                foreach (var paymentMethod in paymentMethods)
                {
                    var paymentMethodDTO = GetPaymentMethodViewModel(paymentMethod);
                    result.Add(paymentMethodDTO);
                }
            }

            return result;
        }

        internal static PaymentMethodViewModel GetPaymentMethodViewModel(PaymentMethodInfo objPaymentMethodInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentMethodInfo, PaymentMethodViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<PaymentMethodViewModel>(objPaymentMethodInfo);

            return result;
        }

        #endregion
    }
}