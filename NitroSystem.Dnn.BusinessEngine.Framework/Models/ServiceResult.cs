using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Framework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Framework.Models
{
    public class ServiceResult
    {
        public ServiceResultType ResultType { get; set; }
        public JObject DataRow { get; set; }
        public JArray DataList { get; set; }
        public object Data { get; set; }
        public long TotalCount { get; set; }
        public string Query { get; set; }
        public bool IsError { get; set; }
        public Exception ErrorException { get; set; }
    }
}
