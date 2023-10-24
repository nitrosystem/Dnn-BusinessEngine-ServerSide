using DotNetNuke.Common.Utilities;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Api.Base
{
    internal class StudioModuleInfoProvider : IStudioModuleInfoProvider
    {
        private const string PortalIDKey = "PortalID";
        private const string PortalAliasIDKey = "PortalAliasID";
        private const string TabIDKey = "TabID";
        private const string ModuleIDKey = "ModuleID";

        public bool TryFindPortalID(HttpRequestMessage request, out int portalID)
        {
            portalID = FindInt(request, PortalIDKey);

            return portalID > Null.NullInteger ? true : false;
        }

        public bool TryFindPortalAliasID(HttpRequestMessage request, out int portalAliasID)
        {
            portalAliasID = FindInt(request, PortalAliasIDKey);

            return portalAliasID > Null.NullInteger ? true : false;
        }

        public bool TryFindTabID(HttpRequestMessage request, out int tabID)
        {
            tabID = FindInt(request, TabIDKey);

            return tabID > Null.NullInteger ? true : false;
        }

        public bool TryFindModuleID(HttpRequestMessage request, out int moduleID)
        {
            moduleID = FindInt(request, ModuleIDKey);

            return moduleID > Null.NullInteger ? true : false;
        }

        public bool TryFindModuleInfo(HttpRequestMessage request, out ModuleInfo moduleInfo)
        {
            int moduleID;
            if (TryFindModuleID(request, out moduleID))
            {
                //moduleInfo = ModuleRepository.Instance.GetModuleByDnnModuleID(moduleID);
                //return moduleInfo != null;

                moduleInfo = null;

                return false;

            }
            else
                moduleInfo = null;

            return false;
        }

        private static int FindInt(HttpRequestMessage requestMessage, string key)
        {
            string value = null;
            IEnumerable<string> values;
            if (requestMessage != null && requestMessage.Headers.TryGetValues(key, out values))
            {
                value = values.FirstOrDefault();
            }

            if (string.IsNullOrEmpty(value) && requestMessage?.RequestUri != null)
            {
                var queryString = HttpUtility.ParseQueryString(requestMessage.RequestUri.Query);
                value = queryString[key];
            }

            int id;
            return int.TryParse(value, out id) ? id : Null.NullInteger;
        }
    }
}
