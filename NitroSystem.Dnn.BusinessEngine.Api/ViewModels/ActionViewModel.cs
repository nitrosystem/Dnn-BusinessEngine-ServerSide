using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Framework.Enums;
using NitroSystem.Dnn.BusinessEngine.Core.Models;

namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels
{
    public class ActionViewModel
    {
        public Guid ActionID { get; set; }
        public Guid? ParentID { get; set; }
        public Guid ModuleID { get; set; }
        public Guid? FieldID { get; set; }
        public Guid? ServiceID { get; set; }
        public Guid? PaymentMethodID { get; set; }
        public string ActionName { get; set; }
        public string ActionType { get; set; }
        public string Event { get; set; }
        public ActionResultStatus? ParentResultStatus { get; set; }
        public ActionResultStatus PaymentResultStatus { get; set; }
        public bool IsServerSide { get; set; }
        public bool RunChildsInServerSide { get; set; }
        public bool HasPreScript { get; set; }
        public bool HasPostScript { get; set; }
        public bool DisableConditionForPreScript { get; set; }
        public bool CheckConditionsInClientSide { get; set; }
        public string PreScript { get; set; }
        public string PostScript { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
        public IDictionary<string, object> Settings { get; set; }
        public IEnumerable<ActionParamInfo> Params { get; set; }
        public IEnumerable<ActionConditionInfo> Conditions { get; set; }
        public IEnumerable<ActionResultViewModel> Results { get; set; }
        public string FieldName { get; set; }
        public string CreatedByUserDisplayName { get; set; }
        public string LastModifiedByUserDisplayName { get; set; }
        public string ActionTypeIcon { get; set; }
    }
}