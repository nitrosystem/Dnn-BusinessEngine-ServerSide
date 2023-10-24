using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Data.Contracts
{
    public interface IExpressionInfo
    {
        string LeftExpression { get; set; }
        string EvalType { get; set; }
        string RightExpression { get; set; }
        string GroupName { get; set; }
        string ExpressionParsingType { get; set; }
        int ViewOrder { get; set; }
    }
}
