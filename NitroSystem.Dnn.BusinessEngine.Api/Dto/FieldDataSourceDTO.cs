using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Dto
{
    public class FieldDataSourceDTO
    {
        public Guid ModuleID { get; set; }
        public Guid FieldID { get; set; }
        public string ConnectionID { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IDictionary<string, object> Form { get; set; }
        public string PageUrl { get; set; }
    }
}
