using System.Web.Routing;
using DotNetNuke.Web.Api;
using System;
using System.Web.Http;
using NitroSystem.Dnn.BusinessEngine.Api.Base;

namespace NitroSystem.Dnn.BusinessEngine.Api.Startup
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute("BusinessEngine", "default", "{controller}/{action}", new[] { "NitroSystem.Dnn.BusinessEngine.Api.Controller" });

            GlobalConfiguration.Configuration.AddModuleInfoProvider(new StudioModuleInfoProvider());
        }
    }
}