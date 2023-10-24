using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Contracts;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Framework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Framework.Dto
{
    public class ActionDto
    {
        public string ActionName { get; set; }
        public string ActionType { get; set; }
        public string Event { get; set; }
        public Guid ActionID { get; set; }
        public Guid ModuleID { get; set; }
        public Guid ParentID { get; set; }
        public Guid? ServiceID { get; set; }
        public Guid? FieldID { get; set; }
        public string ConnectionID { get; set; }
        public bool IsServerSide { get; set; }
        public bool RunChildsInServerSide { get; set; }
        public bool HasPreScript { get; set; }
        public bool HasPostScript { get; set; }
        public bool DisableConditionForPreScript { get; set; }
        public bool CheckConditionsInClientSide { get; set; }
        public string PreScript { get; set; }
        public string PostScript { get; set; }
        public string Settings { get; set; }
        public int ViewOrder { get; set; }
        public bool BufferCreationType { get; set; }
        public ActionResultStatus? ParentResultStatus { get; set; }
        public ActionResultStatus PaymentResultStatus { get; set; }
        public IEnumerable<ActionParamInfo> Params { get; set; }
        public IEnumerable<ActionConditionInfo> Conditions { get; set; }
        public IEnumerable<ActionResultInfo> Results { get; set; }
        public IDictionary<string, object> Form { get; set; }
        public IDictionary<string, object> Field { get; set; }
        public string PageUrl { get; set; }
    }
}
