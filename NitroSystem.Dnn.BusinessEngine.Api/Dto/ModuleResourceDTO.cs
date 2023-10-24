using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Dto
{
    public class ModuleResourceDTO
    {
        public Guid ModuleID { get; set; }
        public Guid? ParentID { get; set; }
        public List<PageResourceInfo> Resources { get; set; }
    }
}
