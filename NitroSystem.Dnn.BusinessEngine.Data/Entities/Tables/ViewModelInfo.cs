using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ViewModels")]
    [PrimaryKey("ViewModelID", AutoIncrement = false)]
    [Cacheable("BE_ViewModels_", CacheItemPriority.Default, 20)]
    [Scope("ScenarioID")]
    public class ViewModelInfo
    {
        public Guid ViewModelID { get; set; }
        public Guid ScenarioID { get; set; }
        [IgnoreColumn]
        public Guid? GroupID { get; set; }
        public string ViewModelName { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public string Description { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
    }
}