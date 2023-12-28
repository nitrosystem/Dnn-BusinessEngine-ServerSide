using Microsoft.JScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Extensions.Manifest
{
    public class ExtensionManifest
    {
        public Guid ExtensionID { get; set; }
        public string ExtensionName { get; set; }
        public string ExtensionType { get; set; }
        public string ExtensionImage { get; set; }
        public string FolderName { get; set; }
        public string VersionType { get; set; }
        public string Version { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ReleaseNotes { get; set; }
        public ExtensionOwner Owner { get; set; }
        public IEnumerable<ExtensionResource> Resources { get; set; }
        public IEnumerable<ExtensionPackage> Packages { get; set; }
        public IEnumerable<ExtensionAssembly> Assemblies { get; set; }
        public IEnumerable<ExtensionSqlProvider> SqlProviders { get; set; }
        public bool IsCommercial { get; set; }
        public string LicenseType { get; set; }
        public string LicenseKey { get; set; }
        public string SourceUrl { get; set; }
    }
}
