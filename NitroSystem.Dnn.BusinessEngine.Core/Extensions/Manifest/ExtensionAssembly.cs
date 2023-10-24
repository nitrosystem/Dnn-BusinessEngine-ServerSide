using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Extensions.Manifest
{
    public class ExtensionAssembly
    {
        public string BasePath { get; set; }
        public IEnumerable<string> Items { get; set; }
    }
}
