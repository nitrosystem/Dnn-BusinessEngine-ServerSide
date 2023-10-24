using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_Actions")]
    [PrimaryKey("ActionID", AutoIncrement = false)]
    [Cacheable("BE_Actions_", CacheItemPriority.Default, 20)]
    [Scope("ModuleID")]
    public class ActionInfo
    {
        public Guid ActionID { get; set; }
        public Guid ModuleID { get; set; }
        public Guid? ParentID { get; set; }
        public Guid? FieldID { get; set; }
        public Guid? ServiceID { get; set; }
        public Guid? PaymentMethodID { get; set; }
        [IgnoreColumn]
        public Guid? GroupID { get; set; }
        public string ActionName { get; set; }
        public string ActionType { get; set; }
        public string Event { get; set; }
        public byte? ParentResultStatus { get; set; }
        public byte? PaymentResultStatus { get; set; }
        public bool IsServerSide { get; set; }
        public bool RunChildsInServerSide { get; set; }
        public bool HasPreScript { get; set; }
        public bool HasPostScript { get; set; }
        public bool DisableConditionForPreScript { get; set; }
        public bool CheckConditionsInClientSide { get; set; }
        public string PreScript { get; set; }
        public string PostScript { get; set; }
        public string Settings { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}