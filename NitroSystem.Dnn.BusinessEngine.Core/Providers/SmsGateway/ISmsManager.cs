using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Core.Providers.SmsGateway
{
    public interface ISmsManager
    {
        int SendSms(int portalID, string mobile, string message, IEnumerable<TokenBase> tokens = null);
    }
}