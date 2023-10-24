using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Common.Models
{
    public class SqlResultInfo
    {
        public bool IsSuccess { get; set; }
        public object Result { get; set; }
        public string ResultMessage { get; set; }
        public string Query { get; set; }
    }
}
