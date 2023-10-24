using DotNetNuke.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NitroSystem.Dnn.BusinessEngine.Framework.Contracts;
using NitroSystem.Dnn.BusinessEngine.Framework.Services;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using NitroSystem.Dnn.BusinessEngine.Core.ModuleBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using NitroSystem.Dnn.BusinessEngine.Api.Base;
using DotNetNuke.Abstractions.Portals;
using DotNetNuke.Entities.Portals;

namespace NitroSystem.Dnn.BusinessEngine.Api.Startup
{
    class Startup : IDnnStartup
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
