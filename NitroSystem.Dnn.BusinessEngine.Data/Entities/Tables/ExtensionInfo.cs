using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_Extensions")]
    [PrimaryKey("ExtensionID", AutoIncrement = false)]
    [Cacheable("BE_Extensions_", CacheItemPriority.Default, 20)]
    public class ExtensionInfo
    {
        public Guid ExtensionID { get; set; }
        public string ExtensionType { get; set; }
        public string ExtensionName { get; set; }
        public string ExtensionImage { get; set; }
        public string FolderName { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ReleaseNotes { get; set; }
        public string Owner { get; set; }
        public string Resources { get; set; }
        public string Assemblies { get; set; }
        public string SqlProviders { get; set; }
        public bool IsCommercial { get; set; }
        public string LicenseType { get; set; }
        public string LicenseKey { get; set; }
        public string SourceUrl { get; set; }
        public string VersionType { get; set; }
        public string Version { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
    }
}