using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Dto
{
    public class FieldSortDTO
    {
        public Guid FieldID { get; set; }
        public string GroupName { get; set; }
        public int ViewOrder { get; set; }
    }
}
