using DotNetNuke.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Data.Contracts
{
    public interface IParamInfo
    {
        string ParamName { get; set; }
        string ParamType { get; set; }
        object ParamValue { get; set; }
        string DefaultValue { get; set; }
        string ExpressionParsingType { get; set; }
        bool IsCustomParam { get; set; }
        int ViewOrder { get; set; }
    }
}
