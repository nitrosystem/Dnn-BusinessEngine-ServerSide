using NitroSystem.Dnn.BusinessEngine.Framework.Contracts;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Contracts;

namespace NitroSystem.Dnn.BusinessEngine.Framework.Services
{
    public class ActionCondition : IActionCondition
    {
        private readonly IModuleData _moduleData;
        private readonly IExpressionService _expressionService;

        public ActionCondition(IModuleData moduleData, IExpressionService expressionService)
        {
            this._moduleData = moduleData;
            this._expressionService = expressionService;
        }

        public bool IsTrueConditions(IEnumerable<IExpressionInfo> conditions)
        {
            if (conditions == null || !conditions.Any()) return true;

            bool andResult = true;

            var groups = conditions.GroupBy(c => c.GroupName);
            foreach (var group in groups)
            {
                bool orResult = false;

                foreach (var condition in group)
                {
                    var leftValue = this._expressionService.ParseExpression(condition.LeftExpression, this._moduleData, new List<object>());

                    //var leftValue = leftExp;
                    //if (this._expressionService.IsComputationalExpression(leftExp)) leftValue = this._expressionService.Evaluate(leftExp).ToString();

                    var rightValue = this._expressionService.ParseExpression(condition.RightExpression, this._moduleData, new List<object>());

                    //var rightValue = rightExp;
                    //if (this._expressionService.IsComputationalExpression(rightExp)) rightValue = this._expressionService.Evaluate(rightExp).ToString();

                    bool compareResult = CompareTwoValue(leftValue, rightValue, condition.EvalType);

                    if (!orResult && compareResult) orResult = true;
                }

                if (!orResult)
                {
                    andResult = false;
                    break;
                }
            }

            return andResult;
        }

        public bool CompareTwoValue(string leftValue, string rightValue, string operatorType)
        {
            switch (operatorType)
            {
                case "=":
                    return leftValue.ToLower() == rightValue.ToLower();
                    break;
                case "!=":
                    return leftValue != rightValue;
                    break;
                case ">":
                    return decimal.Parse(leftValue.ToString()) > decimal.Parse(rightValue.ToString());
                    break;
                case "<":
                    return decimal.Parse(leftValue.ToString()) < decimal.Parse(rightValue.ToString());
                    break;
                case "isfilled":
                    return leftValue != null;
                    break;
                case "isnull":
                    return leftValue == null;
                    break;
                default:
                    return false;
                    break;
            }
        }
    }
}
