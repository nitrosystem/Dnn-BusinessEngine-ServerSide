using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Base
{
    internal interface IStudioModuleInfoProvider
    {
        bool TryFindPortalID(HttpRequestMessage request, out int tabId);
        bool TryFindPortalAliasID(HttpRequestMessage request, out int tabId);
        bool TryFindTabID(HttpRequestMessage request, out int tabId);
        bool TryFindModuleID(HttpRequestMessage request, out int tabId);
        bool TryFindModuleInfo(HttpRequestMessage request, out ModuleInfo moduleInfo);
    }
}
