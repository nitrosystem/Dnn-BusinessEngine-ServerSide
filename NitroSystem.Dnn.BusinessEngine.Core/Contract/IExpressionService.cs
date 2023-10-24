using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Contract
{
    public interface IExpressionService
    {
        string ParseExpression(string expression, IModuleData moduleData, List<object> blackItems, bool ignoreVariable = false, string expressionParsingType = "", int moreThanOneParse = 0);

        bool IsComputationalExpression(string expression);
    }
}
