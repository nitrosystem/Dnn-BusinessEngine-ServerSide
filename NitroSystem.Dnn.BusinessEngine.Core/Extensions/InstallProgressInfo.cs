using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Extensions
{
    internal class InstallProgressInfo
    {
        public int Step { get; set; }
        public int ProgressPercent { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
