using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Extensions.Manifest.PackageModels
{
    public class LibraryInfo
    {
        public string LibraryName { get; set; }
        public string Type { get; set; }
        public string Logo { get; set; }
        public string Summary { get; set; }
        public string Version { get; set; }
        public string LocalPath { get; set; }
        public bool IsSystemLibrary { get; set; }
        public bool IsCDN { get; set; }
        public bool IsCommercial { get; set; }
        public bool IsOpenSource { get; set; }
        public bool IsStable { get; set; }
        public string LicenseJson { get; set; }
        public string GithubPage { get; set; }
        public IEnumerable<LibraryResourceInfo> Resources { get; set; }

    }
}
