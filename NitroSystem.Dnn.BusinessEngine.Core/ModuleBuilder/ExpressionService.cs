using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Common;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using NitroSystem.Dnn.BusinessEngine.Core.Dto;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.ModuleBuilder
{
    public class ExpressionService : IExpressionService
    {
        public ExpressionService()
        {
        }

        public string ParseExpression(string expression, IModuleData moduleData, List<object> blackItems, bool ignoreVariable = false, string expressionParsingType = "", int moreThanOneParse = 0)
        {
            if (string.IsNullOrWhiteSpace(expression)) return string.Empty;

            bool ignoreVariableMatch = false;
            bool foundExpression = false;
            string expressionWithoutString = expression;

            /*------------------------------------*/
            /* Parse {{XXX}} Expressions  */
            /*------------------------------------*/
            var matches = Regex.Matches(expression, @"{{((\w+)\.?([^=?\s()!'""&@#$%\^\*]+)?)}}", RegexOptions.Multiline);
            foreach (Match match in matches)
            {
                string propertyPath = match.Groups[1].Value;
                var value = moduleData.GetData(propertyPath);
                value = ParseValue(value, "word", expressionParsingType);

                expression = expression.Replace(match.Value, value != null ? value.ToString() : "null");

                ignoreVariable = true;
            }

            /*------------------------------------*/
            /* Parse Page Params  */
            /*------------------------------------*/
            matches = Regex.Matches(expression, @"_PageParam\.\w+", RegexOptions.Multiline);
            foreach (Match match in matches)
            {
                ignoreVariableMatch = true;

                var propMatch = Regex.Match(match.Value, @"(?<=_PageParam\.)(\w*)$");
                var paramName = propMatch.Value;

                object value = moduleData.GetPageParam(paramName);
                value = ParseValue(value, "word", expressionParsingType);

                if (value == null && match.Value == expression)
                {
                    expression = null;
                    foundExpression = false;
                }
                else
                {
                    expression = expression.Replace(match.Value, value != null ? value.ToString() : "null");

                    foundExpression = true;
                }
            }

            if (!ignoreVariableMatch && expressionParsingType != "static-expression")
            {
                //Black items are expressions that once parsed and not must be again parsed
                if (blackItems == null) blackItems = new List<object>();

                //Remove wordss from expression that inside between cotation or double cotation
                matches = Regex.Matches(expressionWithoutString, @"([""|'](\w+)\.?([^=?\s()'""&@#$%\^\*]+)?[""|'])", RegexOptions.Multiline);
                foreach (Match match in matches)
                {
                    expressionWithoutString = expressionWithoutString.Replace(match.Value, string.Empty);
                }

                /*------------------------------------*/
                /* Parse Module Variables  
                /*------------------------------------*/
                matches = Regex.Matches(expressionWithoutString, @"(\w+)\.?([^,=?\s()!'""&@#$%\^\*]+)?", RegexOptions.Multiline);
                foreach (Match match in matches)
                {
                    string propertyPath = match.Value;

                    if (blackItems.Contains(propertyPath)) continue;

                    string variableName = match.Groups[1].Value;
                    if (moduleData.IsVariable(variableName) || (ignoreVariable && (moduleData as JObject).SelectToken(variableName) != null))
                    {
                        var value = moduleData.GetData(propertyPath);
                        value = ParseValue(value, "word", expressionParsingType);

                        if (value != null && value != "") blackItems.Add(value);

                        if (propertyPath == expression)
                            return value != null ? value .ToString(): null;
                        else
                            expression = expression.Replace(propertyPath, value != null ? value.ToString() : "null");

                        foundExpression = true;
                    }
                }
            }

            if (moreThanOneParse <= 0 || !foundExpression)
            {
                if (expressionParsingType == "eval-expression")
                    return Evaluator.Eval<object>(expression).ToString();
                if (expressionParsingType == "custom-server-method")
                    return Evaluator.EvalCustomMethod(expression).ToString();
                else if (expressionParsingType == "parse-int-number")
                    return Evaluator.EvalToInteger(expression).ToString();
                else
                    return (string)ParseValue(expression, "expression", expressionParsingType);
            }
            else
            {
                return ParseExpression(expression, moduleData, blackItems, ignoreVariable, expressionParsingType, moreThanOneParse - 1);
            }
        }

        public bool IsComputationalExpression(string expression)
        {
            return string.IsNullOrEmpty(expression) ? false : Regex.IsMatch(expression, @"(\+|\-|\*|\/|\\|\^|\().+");
        }

        private object ParseValue(object value, string type, string expressionParsingType)
        {
            if ((expressionParsingType == "get-value" || expressionParsingType == "custom-server-method") && value == "")
                value = null;
            else if (type == "word" && expressionParsingType == "parse-string-with-add-cotation-to-any-word")
                value = string.Format(@"""{0}""", value);
            else if (type == "expression" && expressionParsingType == "parse-string-with-add-cotation-to-expression")
                value = string.Format(@"""{0}""", value);

            return value;
        }
    }
}
