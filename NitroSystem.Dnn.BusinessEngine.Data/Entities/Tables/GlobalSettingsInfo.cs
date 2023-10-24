using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_GlobalSettings")]
    [PrimaryKey("SettingID", AutoIncrement = false)]
    [Cacheable("BE_GlobalSettings_", CacheItemPriority.Default, 20)]
    [Scope("ScenarioID")]
    public class GlobalSettingsInfo
    {
        public Guid SettingID { get; set; }
        public Guid ScenarioID { get; set; }
        public string GroupName { get; set; }
        public string SettingName { get; set; }
        public object SettingValue { get; set; }
    }
}