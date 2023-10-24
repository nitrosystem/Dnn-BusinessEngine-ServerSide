using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ModuleVariables")]
    [PrimaryKey("VariableID", AutoIncrement = false)]
    [Cacheable("BE_ModuleVariables_", CacheItemPriority.Default, 20)]
    [Scope("ModuleID")]
    public class ModuleVariableInfo
    {
        public Guid VariableID { get; set; }
        public Guid ModuleID { get; set; }
        public Guid? ViewModelID { get; set; }
        public string VariableName { get; set; }
        public string VariableType { get; set; }
        public int Scope { get; set; }
        public bool IsSystemVariable { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}