using DotNetNuke.Entities.Portals;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Providers.SmsGateway
{
    public static class SmsGatewayService
    {
        //public static void SendSms(PortalSettings portalSettings, string provider, string mobile, string message, bool tokenBase = false, string templateID = null, Dictionary<string, string> tokens = null)

        public static void SendSms(PortalSettings portalSettings, string provider, string mobile, string message, IEnumerable<TokenBase> tokens = null)
        {
            var smsProvider = ProviderRepository.Instance.GetProviders("SmsProvider").FirstOrDefault(p => p.ProviderName == provider);

            //send sms
            var smsServiceInterface = (ISmsManager)Activator.CreateInstance(Type.GetType(smsProvider.BusinessControllerClass));
            var smsService = SmsManagerServiceLocator.SetInstance(smsServiceInterface);
            smsService.SendSms(portalSettings.PortalId, mobile, message, tokens);
        }
    }
}
