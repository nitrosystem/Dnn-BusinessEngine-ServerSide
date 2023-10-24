using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using DotNetNuke.Web.Api;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Api.Base
{
    internal class StudioBaseApiAttributes : FilterAttribute, IFilter
    {
        private readonly string[] _supportedModules;

        internal StudioBaseApiAttributes(string supportedModules)
        {
            this._supportedModules = supportedModules.Split(new[] { ',' });
        }

        protected virtual ModuleInfo FindModuleInfo(HttpRequestMessage request)
        {
            return request.FindModuleInfo();
        }
      
        private bool ModuleIsSupported(ModuleInfo module)
        {
            return this._supportedModules.Contains(module.ModuleName);
        }
    }
}
