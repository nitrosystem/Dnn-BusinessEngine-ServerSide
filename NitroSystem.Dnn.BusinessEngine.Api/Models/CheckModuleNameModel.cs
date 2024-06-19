using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Models
{
    public class CheckModuleNameModel
    {
        public Guid ScenarioID{ get; set; }
        public Guid? ModuleID { get; set; }
        public string OldModuleName { get; set; }
        public string NewModuleName { get; set; }
    }
}
