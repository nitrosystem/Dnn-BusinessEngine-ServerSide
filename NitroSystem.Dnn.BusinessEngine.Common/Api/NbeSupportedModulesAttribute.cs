//using System;
//using System.Linq;
//using System.Net.Http;
//using System.Web.Http.Controllers;

//using DotNetNuke.Entities.Modules;
//using DotNetNuke.Web.Api;

//namespace NitroSystem.Dnn.BusinessEngine.Utilities.Api
//{
//    public class NbeSupportedModulesAttribute : AuthorizeAttributeBase
//    {
//        private readonly string[] _supportedModules;

//        public NbeSupportedModulesAttribute(string supportedModules)
//        {
//            _supportedModules = supportedModules.Split(new[] { ',' });
//        }

//        public override bool IsAuthorized(AuthFilterContext context)
//        {
//            var module = FindModuleInfo(context.ActionContext.Request);

//            if (module != null)
//            {
//                return ModuleIsSupported(module);
//            }

//            return false;
//        }

//        private bool ModuleIsSupported(NbeModuleInfo module)
//        {
//            return _supportedModules.Contains(module.DesktopModule.ModuleName);
//        }

//        protected virtual NbeModuleInfo FindModuleInfo(HttpRequestMessage request)
//        {
//            var result = new NbeModuleInfo();

//            string moduleID = string.Empty;
//            if(request.Headers.GetValues("ModuleId").FirstOrDefault(), out moduleID))
//            {
//                result.ModuleID = Guid.Parse(moduleID);
//            }

//            return request.Headers;
//        }

//        protected override bool SkipAuthorization(HttpActionContext actionContext)
//        {
//            return false;
//        }
//    }
//}