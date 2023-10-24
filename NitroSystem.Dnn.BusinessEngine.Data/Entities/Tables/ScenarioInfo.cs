using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_Scenarios")]
    [PrimaryKey("ScenarioID", AutoIncrement = false)]
    [Cacheable("BE_Scenarios_", CacheItemPriority.Default, 20)]
    public class ScenarioInfo
    {
        public Guid ScenarioID { get; set; }
        public string ScenarioName { get; set; }
        public string ScenarioTitle { get; set; }
        public string DatabaseObjectPrefix { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
    }
}