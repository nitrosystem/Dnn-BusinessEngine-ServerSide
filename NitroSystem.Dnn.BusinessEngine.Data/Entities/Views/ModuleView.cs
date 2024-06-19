using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Views
{
    [TableName("BusinessEngineView_Modules")]
    [PrimaryKey("ModuleID", AutoIncrement = false)]
    [Cacheable("BE_Modules_View_", CacheItemPriority.Default, 20)]
    [Scope("ScenarioID")]
    public class ModuleView
    {
        public Guid ModuleID { get; set; }
        public Guid ScenarioID { get; set; }
        public Guid? ParentID { get; set; }
        public string ScenarioName { get; set; }
        public string Wrapper { get; set; }
        public string ModuleType { get; set; }
        public string ModuleName { get; set; }
        public string ModuleTitle { get; set; }
        public string Skin { get; set; }
        public string Template { get; set; }
        public string Theme { get; set; }
        public bool EnableFieldsDefaultTemplate { get; set; }
        public bool EnableFieldsDefaultTheme { get; set; }
        public string FieldsDefaultTemplate { get; set; }
        public string FieldsDefaultTheme { get; set; }
        public string LayoutTemplate { get; set; }
        public string LayoutCss { get; set; }
        public string ModuleBuilderType { get; set; }
        public bool IsSSR { get; set; }
        public bool IsDisabledFrontFramework { get; set; }
        public int PortalID { get; set; }
        public int? DnnModuleID { get; set; }
        public string Settings { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}