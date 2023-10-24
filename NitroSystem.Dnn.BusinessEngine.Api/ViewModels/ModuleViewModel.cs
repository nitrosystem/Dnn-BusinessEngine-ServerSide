using NitroSystem.Dnn.BusinessEngine.Core.Appearance;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels
{
    public class ModuleViewModel
    {
        public Guid ModuleID { get; set; }
        public Guid ScenarioID { get; set; }
        public Guid? ParentID { get; set; }
        public Guid? ViewModelID { get; set; }
        public int PortalID { get; set; }
        public int? DnnModuleID { get; set; }
        public string ModuleType { get; set; }
        public string ModuleName { get; set; }
        public string ModuleTitle { get; set; }
        public string Wrapper { get; set; }
        public string Skin { get; set; }
        public string Template { get; set; }
        public string Theme { get; set; }
        public string LayoutTemplate { get; set; }
        public string LayoutCss { get; set; }
        public string ModuleBuilderType { get; set; }
        public bool IsSSR { get; set; }
        public bool IsDisabledFrontFramework { get; set; }
        public string Description { get; set; }
        public int Version { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
        public ModuleSkinInfo ModuleSkin { get; set; }
        public IDictionary<string, object> Settings { get; set; }
        public string CustomHtml { get; set; }
        public string CustomJs { get; set; }
        public string CustomCss { get; set; }
    }
}