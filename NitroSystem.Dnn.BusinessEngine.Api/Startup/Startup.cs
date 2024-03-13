using Microsoft.Extensions.DependencyInjection;
using DotNetNuke.DependencyInjection;
using NitroSystem.Dnn.BusinessEngine.Framework.Contracts;
using NitroSystem.Dnn.BusinessEngine.Framework.Services;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using NitroSystem.Dnn.BusinessEngine.Core.ModuleBuilder;


namespace NitroSystem.Dnn.BusinessEngine.Api.Startup
{
    internal class Startup : IDnnStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IModuleData, ModuleData>();

            services.AddTransient<IActionWorker, ActionWorker>();
            services.AddTransient<IActionCondition, ActionCondition>();
            services.AddTransient<IServiceWorker, ServiceWorker>();
            services.AddTransient<IExpressionService, ExpressionService>();

            //GlobalConfiguration.Configuration.Filters.Add(new BasicAuthenticationAttribute());
        }
    }
}
