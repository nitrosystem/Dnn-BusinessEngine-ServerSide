using DotNetNuke.ComponentModel.DataAnnotations;
using NitroSystem.Dnn.BusinessEngine.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ActionResults")]
    [PrimaryKey("ResultID", AutoIncrement = false)]
    //[Cacheable("BE_ActionResults_", CacheItemPriority.Default, 20)]
    [Scope("ActionID")]
    public class ActionResultInfo: IExpressionInfo
    {
        public Guid ResultID { get; set; }
        public Guid ActionID { get; set; }
        public string LeftExpression { get; set; }
        public string EvalType { get; set; }
        public string RightExpression { get; set; }
        public string GroupName { get; set; }
        public string ExpressionParsingType { get; set; }
        public string Conditions { get; set; }
        public int ViewOrder { get; set; }
    }
}
