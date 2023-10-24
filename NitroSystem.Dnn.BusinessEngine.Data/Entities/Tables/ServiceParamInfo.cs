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
    [TableName("BusinessEngine_ServiceParams")]
    [PrimaryKey("ParamID", AutoIncrement = false)]
    //[Cacheable("BE_ServiceParams_", CacheItemPriority.Default, 20)]
    [Scope("ServiceID")]
    public class ServiceParamInfo : IParamInfo
    {
        public Guid ParamID { get; set; }
        public Guid ServiceID { get; set; }
        public string ParamName { get; set; }
        public string ParamType { get; set; }
        public object ParamValue { get; set; }
        public string DefaultValue { get; set; }
        public string ExpressionParsingType { get; set; }
        public bool IsCustomParam { get; set; }
        public int ViewOrder { get; set; }
    }
}
