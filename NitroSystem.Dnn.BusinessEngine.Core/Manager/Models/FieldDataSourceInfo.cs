using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Models
{
    public class FieldDataSourceInfo
    {
        public int Type { get; set; }
        public Guid? ListID { get; set; }
        public string ListName { get; set; }
        public Guid? ActionID { get; set; }
        public IEnumerable<ExpressionInfo> ListFilters { get; set; }
        public string TextField { get; set; }
        public string ValueField { get; set; }
        public int ListStructure { get; set; }
        public IEnumerable<object> CustomFields { get; set; }
    }
}