using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Core.Providers.SmsGateway
{
    public static class SmsManagerServiceLocator
    {
        public static ISmsManager smsService = null;

        public static ISmsManager SetInstance(ISmsManager smsManager)
        {
            if (smsService == null) return smsManager;
            return smsService;
        }
    }
}