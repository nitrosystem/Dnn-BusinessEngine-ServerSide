using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Dto
{
    public class ExtensionInstallDto
    {
        public string ManifestFilePath { get; set; }
        public string ExtensionUnzipedPath { get; set; }
    }
}
