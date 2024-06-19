using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Dto
{
    public class BuildModuleDTO
    {
        public Guid ModuleID { get; set; }
        public Guid? ParentID { get; set; }
        public string ModuleBuilderType { get; set; }
        public string CustomHtml { get; set; }
        public string CustomJs { get; set; }
        public string CustomCss { get; set; }
        public string ModuleTemplate { get; set; }
        public string MonitoringFileID { get; set; }
    }
}
