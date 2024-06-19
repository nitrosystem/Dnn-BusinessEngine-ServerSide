using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Models
{
    public class FieldDataSourceResult
    {
        public IEnumerable<object> Items { get; set; }
        public long TotalCount { get; set; }
        public string ErrorMessage { get; set; }
        internal string DataSourceJson { get; set; }
    }
}
