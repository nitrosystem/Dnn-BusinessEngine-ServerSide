using NitroSystem.Dnn.BusinessEngine.Common.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels
{
    public class LibraryViewModel
    {
        public Guid LibraryID { get; set; }
        public Guid ExtensionID { get; set; }
        public string Type { get; set; }
        public string LibraryName { get; set; }
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