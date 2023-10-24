using NitroSystem.Dnn.BusinessEngine.Common.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Models
{
    public class ParamInfo: IParamInfo
    {
        public string ParamName { get; set; }
        public string ParamType { get; set; }
        public object ParamValue { get; set; }
        public string DefaultValue { get; set; }
        public string ExpressionParsingType { get; set; }
        public bool IsCustomParam { get; set; }
        public int ViewOrder { get; set; }
    }
}
