using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Extensions.Manifest
{
    public class ExtensionPackage
    {
        public string PackageName { get; set; }
        public string Title { get; set; }
        public string PackageType { get; set; }
        public IEnumerable<LibraryResourceInfo> ClientResources { get; set; }
        public IEnumerable<ProviderInfo> Providers { get; set; }
        public IEnumerable<SkinInfo> Skins { get; set; }
        public IEnumerable<ActionTypeInfo> Actions { get; set; }
        public IEnumerable<ServiceTypeInfo> Services { get; set; }
        public IEnumerable<ModuleFieldTypeInfo> Fields { get; set; }
        public IEnumerable<ModuleFieldTypeTemplateInfo> FieldTemplates { get; set; }
    }
}
