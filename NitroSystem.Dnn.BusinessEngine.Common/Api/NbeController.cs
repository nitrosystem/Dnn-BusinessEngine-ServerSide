//using System;
//using System.Web.Http;
//using DotNetNuke.Entities.Modules;
//using DotNetNuke.Entities.Portals;
//using DotNetNuke.Entities.Users;
//using DotNetNuke.Web.Api;

//namespace NitroSystem.Dnn.BusinessEngine.Utilities.Api
//{
//    [DnnExceptionFilter]
//    public abstract class NbeApiController : ApiController
//    {
//        private readonly Lazy<NbeModuleInfo> _activeModule;

//        protected NbeApiController()
//        {
//            _activeModule = new Lazy<NbeModuleInfo>(InitModuleInfo);
//        }

//        private NbeModuleInfo InitModuleInfo()
//        {
//            return Request.FindModuleInfo();
//        }

//        /// <summary>
//        /// PortalSettings for the current portal
//        /// </summary>
//        public PortalSettings PortalSettings
//        {
//            get
//            {
//                return PortalController.Instance.GetCurrentPortalSettings();
//            }
//        }

//        /// <summary>
//        /// UserInfo for the current user
//        /// </summary>
//        public UserInfo UserInfo { get { return PortalSettings.UserInfo; } }

//        /// <summary>
//        /// ModuleInfo for the current module
//        /// <remarks>Will be null unless a valid pair of module and tab ids were provided in the request</remarks>
//        /// </summary>
//        public NbeModuleInfo ActiveModule
//        {
//            get { return _activeModule.Value; }
//        }
//    }
//}