using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Views
{
    public class PageResourceView
    {
        public string FilePath { get; set; }
        public int Version { get; set; }
        public string ResourceType { get; set; }
        public int LoadOrder { get; set; }
    }
}
