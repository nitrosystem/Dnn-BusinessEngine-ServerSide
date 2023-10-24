using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Dto
{
    public class RenderedModuleDTO
    {
        public Guid ModuleID { get; set; }
        public Guid? ParentID { get; set; }
        public string ModuleTemplate { get; set; }
        public string ModuleStyles { get; set; }
        public string ModueScripts { get; set; }
        public bool IsRenderScriptsAndStyles { get; set; }

    }
}
