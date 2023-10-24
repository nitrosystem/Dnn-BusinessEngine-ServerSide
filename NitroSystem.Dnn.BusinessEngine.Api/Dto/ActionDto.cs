using NitroSystem.Dnn.BusinessEngine.Api.ViewModels;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Framework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Dto
{
    public class ActionDto
    {
        public string ActionName { get; set; }
        public string ActionType { get; set; }
        public string Event { get; set; }
        public Guid ActionID { get; set; }
        public Guid ModuleID { get; set; }
        public Guid? FieldID { get; set; }
        public Guid ConnectionID { get; set; }
        public bool IsServerSide { get; set; }
        public ActionResultStatus? ParentResultStatus { get; set; }
        public ActionResultStatus PaymentResultStatus { get; set; }
        public IEnumerable<ParamInfo> Params { get; set; }
        public IEnumerable<ExpressionInfo> Conditions { get; set; }
        public IEnumerable<ExpressionInfo> Results { get; set; }
        public IDictionary<string, object> Form { get; set; }
        public IDictionary<string, ModuleFieldViewModel> Field { get; set; }
        public string PageUrl { get; set; }
    }
}
