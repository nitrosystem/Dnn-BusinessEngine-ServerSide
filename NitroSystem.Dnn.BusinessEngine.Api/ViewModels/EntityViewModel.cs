using NitroSystem.Dnn.BusinessEngine.Common.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels
{
    public class EntityViewModel
    {
        public Guid EntityID { get; set; }
        public Guid ScenarioID { get; set; }
        public Guid? DatabaseID { get; set; }
        public Guid? GroupID { get; set; }
        public string EntityName { get; set; }
        public string EntityType { get; set; }
        public string TableName { get; set; }
        public bool IsReadonly { get; set; }
        public bool IsMultipleColumnsForPK { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
        public IDictionary<string, object> Settings { get; set; }
        public IEnumerable<EntityColumnInfo> Columns { get; set; }
        public IEnumerable<TableRelationshipInfo> Relationships { get; set; }
    }
}