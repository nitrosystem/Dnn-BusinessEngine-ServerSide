using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_Databases")]
    [PrimaryKey("DatabaseID", AutoIncrement = false)]
    [Cacheable("BE_Databases_", CacheItemPriority.Default, 20)]
    public class DatabaseInfo
    {
        public Guid DatabaseID { get; set; }
        public Guid ScenarioID { get; set; }
        public string DatabaseName { get; set; }
        public string Title { get; set; }
        public string ConnectionString { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}