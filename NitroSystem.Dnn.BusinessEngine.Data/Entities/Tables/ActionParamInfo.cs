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
    [TableName("BusinessEngine_ActionParams")]
    [PrimaryKey("ParamID", AutoIncrement = false)]
    //[Cacheable("BE_ActionParams_", CacheItemPriority.Default, 20)]
    [Scope("ActionID")]
    public class ActionParamInfo : IParamInfo
    {
        public Guid ParamID { get; set; }
        public Guid ActionID { get; set; }
        public string ParamName { get; set; }
        public string ParamType { get; set; }
        public object ParamValue { get; set; }
        public string DefaultValue { get; set; }
        public string ExpressionParsingType { get; set; }
        public bool IsCustomParam { get; set; }
        public int ViewOrder { get; set; }
    }
}
