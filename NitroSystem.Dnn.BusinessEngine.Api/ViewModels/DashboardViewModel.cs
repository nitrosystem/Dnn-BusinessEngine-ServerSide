using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels
{
    public class DashboardViewModel
    {
        public Guid DashboardID { get; set; }
        public Guid ScenarioID { get; set; }
        public Guid ModuleID { get; set; }
        public int DashboardType { get; set; }
        public string UniqueName { get; set; }
        public string ModuleName { get; set; }
        public string ModuleTitle { get; set; }
        public string Skin { get; set; }
        public string Template { get; set; }
        public string Theme { get; set; }
        public int PortalID { get; set; }
        public int DnnModuleID { get; set; }
        public string ModuleBuilderType { get; set; }
        public bool IsSSR { get; set; }
        public bool IsDisabledFrontFramework { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
        public IEnumerable<string> AuthorizationViewDashboard { get; set; }
        public IDictionary<string, object> Settings { get; set; }
    }
}