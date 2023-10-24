using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Common.Models
{
    public class TableRelationshipInfo
    {
        public string RelationshipName { get; set; }
        public string ParentEntityTableName { get; set; }
        public string ChildEntityTableName { get; set; }
        public bool EnforceForReplication { get; set; }
        public bool EnforceForeignKeyConstraint { get; set; }
        public string DELETESpecification { get; set; }
        public string UPDATESpecification { get; set; }
        public string PrimaryKey { get; set; }
        public string ForeignKey { get; set; }
        public IEnumerable<TableRelationshipColumnInfo> Columns { get; set; }
    }
}
