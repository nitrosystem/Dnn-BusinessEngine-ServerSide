using DotNetNuke.Services.UserRequest;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System.Net.Http;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Api.Base
{
    internal static class HttpRequestMessageExtensions
    {
        private delegate bool TryMethod<T>(IStudioModuleInfoProvider provider, HttpRequestMessage request, out T output);

        internal static int FindPortalID(this HttpRequestMessage request)
        {
            return IterateModuleInfoProviders(request, TryFindPortalID, -1);
        }

        internal static int FindPortalAliasID(this HttpRequestMessage request)
        {
            return IterateModuleInfoProviders(request, TryFindPortalAliasID, -1);
        }

        internal static int FindTabID(this HttpRequestMessage request)
        {
            return IterateModuleInfoProviders(request, TryFindTabID, -1);
        }

        internal static int FindModuleID(this HttpRequestMessage request)
        {
            return IterateModuleInfoProviders(request, TryFindModuleID, -1);
        }

        internal static ModuleInfo FindModuleInfo(this HttpRequestMessage request)
        {
            return IterateModuleInfoProviders<ModuleInfo>(request, TryFindModuleInfo, null);
        }

        private static bool TryFindPortalID(IStudioModuleInfoProvider provider, HttpRequestMessage request, out int output)
        {
            return provider.TryFindPortalID(request, out output);
        }

        private static bool TryFindPortalAliasID(IStudioModuleInfoProvider provider, HttpRequestMessage request, out int output)
        {
            return provider.TryFindPortalAliasID(request, out output);
        }

        private static bool TryFindTabID(IStudioModuleInfoProvider provider, HttpRequestMessage request, out int output)
        {
            return provider.TryFindTabID(request, out output);
        }

        private static bool TryFindModuleID(IStudioModuleInfoProvider provider, HttpRequestMessage request, out int output)
        {
            return provider.TryFindModuleID(request, out output);
        }

        private static bool TryFindModuleInfo(IStudioModuleInfoProvider provider, HttpRequestMessage request, out ModuleInfo output)
        {
            return provider.TryFindModuleInfo(request, out output);
        }

        private static T IterateModuleInfoProviders<T>(HttpRequestMessage request, TryMethod<T> func, T fallback)
        {
            var providers = request.GetConfiguration().GetModuleInfoProviders();

            foreach (var provider in providers)
            {
                T output;
                if (func(provider, request, out output))
                {
                    return output;
                }
            }

            return fallback;
        }

        public static HttpContextBase GetHttpContext(this HttpRequestMessage request)
        {
            object context;
            request.Properties.TryGetValue("MS_HttpContext", out context);

            return context as HttpContextBase;
        }

        public static string GetIPAddress(this HttpRequestMessage request)
        {
            return UserRequestIPAddressController.Instance.GetUserRequestIPAddress(GetHttpContext(request).Request);
        }
    }
}
