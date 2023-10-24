using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Dto
{
    public class ModuleVariableDto
    {
        public JObject _B { get; set; }
        public int PortalID { get; set; }
        public int UserID { get; set; }
        public int TabID { get; set; }
        public Guid ModuleID { get; set; }
    }
}
