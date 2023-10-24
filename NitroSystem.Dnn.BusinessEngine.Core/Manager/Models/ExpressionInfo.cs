using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Models
{
    public class ExpressionInfo
    {
        public string LeftExpression { get; set; }
        public string EvalType { get; set; }
        public string RightExpression { get; set; }
        public string GroupName { get; set; }
    }
}