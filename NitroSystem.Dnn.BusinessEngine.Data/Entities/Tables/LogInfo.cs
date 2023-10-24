using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_Logs")]
    [PrimaryKey("LogID", AutoIncrement = false)]
    [Cacheable("BE_Logs_", CacheItemPriority.Default, 20)]
    [Scope("ScenarioID")]
    public class LogInfo
    {
        public Guid LogID { get; set; }
        public Guid PrevLogID { get; set; }
        public Guid ScenarioID { get; set; }
        public Guid EntityID { get; set; }
        public string EntityType { get; set; }
        public string LogKey { get; set; }
        public bool IsError { get; set; }
        public int UserID { get; set; }
        public DateTime LogDate { get; set; }
        public string LogDetails { get; set; }
    }
}