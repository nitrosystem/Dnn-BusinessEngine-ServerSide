using NitroSystem.Dnn.BusinessEngine.Framework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Framework.Contracts
{
    public interface IActionResult
    {
        ActionResultStatus ResultStatus { get; set; }
        bool IsError { get; set; }
        string ErrorMessage { get; set; }
    }
}
