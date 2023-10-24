using NitroSystem.Dnn.BusinessEngine.Api.Enums;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Contracts;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Models
{
    public class FieldDataSourceInfo
    {
        public Guid? ListID { get; set; }
        public Guid? ServiceID { get; set; }
        public Guid? ActionID { get; set; }
        public FieldDataSourceType Type { get; set; }
        public string ListName { get; set; }
        public string TextField { get; set; }
        public string ValueField { get; set; }
        public long TotalCount { get; set; }
        public bool? RunServiceClientSide { get; set; }
        public IEnumerable<ParamInfo> ServiceParams { get; set; }
        public IEnumerable<ExpressionInfo> ListFilters { get; set; }
    }
}
