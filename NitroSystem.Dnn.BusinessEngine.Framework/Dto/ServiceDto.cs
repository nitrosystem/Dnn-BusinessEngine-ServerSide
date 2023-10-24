using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Framework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Framework.Dto
{
  public  class ServiceDto
    {
        public Guid ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceType { get; set; }
        public string ServiceSubtype { get; set; }
        public bool HasResult { get; set; }
        public ServiceResultType ResultType { get; set; }
        public IEnumerable<ServiceParamInfo> Params { get; set; }
        public string Settings { get; set; }
    }
}
