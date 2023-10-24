using DotNetNuke.Entities.Portals;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using NitroSystem.Dnn.BusinessEngine.Core.Infrastructure.SSR;
using NitroSystem.Dnn.BusinessEngine.Core.ModuleBuilder;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Framework.Services;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Services
{
    internal static class Service
    {
        internal static string RenderSSR(PortalSettings portalSettings, Guid moduleID, string connectionID, string pageUrl, int userID)
        {
            return RenderSSRAsync(portalSettings, moduleID, connectionID, pageUrl, userID).GetAwaiter().GetResult();
        }

        internal static async Task<string> RenderSSRAsync(PortalSettings portalSettings, Guid moduleID, string connectionID, string pageUrl, int userID)
        {
            var expressionService = new ExpressionService();
            var moduleData = new ModuleData(expressionService);
            var actionCondition = new ActionCondition(moduleData, expressionService);
            var serviceWorker = new ServiceWorker(moduleData, expressionService);
            var actionWorker = new ActionWorker(moduleData, expressionService, actionCondition, serviceWorker);

            moduleData.InitModuleData(moduleID, connectionID, userID, null, null, pageUrl, true);

            await actionWorker.CallActions(moduleID, null, "OnPageLoad");

            var module = ModuleRepository.Instance.GetModuleView(moduleID);

            var modulePath = (portalSettings.PortalId == module.PortalID ? portalSettings.HomeSystemDirectoryMapPath : new PortalSettings(module.PortalID).HomeSystemDirectoryMapPath) + @"BusinessEngine\";
            string template = module.ModuleBuilderType != "HtmlEditor" ?
                FileUtil.GetFileContent(string.Format(@"{0}\{1}\module--{2}.html", modulePath, module.ScenarioName, module.ModuleName)) :
                FileUtil.GetFileContent(string.Format(@"{0}\{1}\module--{2}\custom.html", modulePath, module.ScenarioName, module.ModuleName));

            var ssrController = new ServerSideRendering(moduleData, expressionService);
            string renderedSSR = ssrController.Render(template, true);

            //FileUtil.CreateTextFile(string.Format("{0}/BusinessEngine/{1}/module-ssr--{2}.html", PortalSettings.HomeSystemDirectoryMapPath, module.ScenarioName, module.ModuleName), renderedSSR, true);

            return renderedSSR;
        }
    }
}
