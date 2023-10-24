using DotNetNuke.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace NitroSystem.Dnn.BusinessEngine.Api.Base
{
    internal static class HttpConfigurationExtensions
    {
        private const string Key = "BEngineStudioModuleInfoProvider";

        internal static void AddModuleInfoProvider(this HttpConfiguration configuration, IStudioModuleInfoProvider moduleInfoProvider)
        {
            var providers = configuration.Properties.GetOrAdd(Key, InitValue) as ConcurrentQueue<IStudioModuleInfoProvider>;

            if (providers == null)
            {
                providers = new ConcurrentQueue<IStudioModuleInfoProvider>();
                configuration.Properties[Key] = providers;
            }

            providers.Enqueue(moduleInfoProvider);
        }

        internal static IEnumerable<IStudioModuleInfoProvider> GetModuleInfoProviders(this HttpConfiguration configuration)
        {
            var providers = configuration.Properties.GetOrAdd(Key, InitValue) as ConcurrentQueue<IStudioModuleInfoProvider>;

            if (providers == null)
            {
                // shouldn't ever happen outside of unit tests
                return new IStudioModuleInfoProvider[] { };
            }

            return providers.ToArray();
        }

        private static object InitValue(object o)
        {
            return new ConcurrentQueue<IStudioModuleInfoProvider>();
        }
    }
}