using DotNetNuke.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Views
{
    [TableName("BusinessEngineView_PageResources")]
    [PrimaryKey("ResourceID", AutoIncrement = false)]
    [Cacheable("BE_PageResources_View_", CacheItemPriority.Default, 20)]
    [Scope("CmsPageID")]
    public class PageResourceView
    {
        public Guid ResourceID { get; set; }
        public Guid ModuleID { get; set; }
        public string CmsPageID { get; set; }
        public string ModuleType { get; set; }
        public string ModuleName { get; set; }
        public string ModuleTitle { get; set; }
        public string FieldType { get; set; }
        public string LibraryName { get; set; }
        public string LibraryVersion { get; set; }
        public string LibraryLogo { get; set; }
        public string ResourceType { get; set; }
        public bool IsBaseResource { get; set; }
        public bool IsCustomResource { get; set; }
        public bool IsActive { get; set; }
        public int DisabledByUserID { get; set; }
        public string FilePath { get; set; }
        public bool IsSystemResource { get; set; }
        public string Version { get; set; }
        public int LoadOrder { get; set; }
    }
}
