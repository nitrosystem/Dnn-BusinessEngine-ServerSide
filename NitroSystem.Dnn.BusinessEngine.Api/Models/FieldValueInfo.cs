using NitroSystem.Dnn.BusinessEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Models
{
  public  class FieldValueInfo
    {
        public string ValueExpression { get; set; }
        public IEnumerable<ExpressionInfo> Conditions { get; set; }
        public string ExpressionParsingType { get; set; }
    }
}
