using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ModuleFieldSettings")]
    [PrimaryKey("SettingID", AutoIncrement = false)]
    [Cacheable("BE_ModuleFieldSettings_", CacheItemPriority.Default, 20)]
    [Scope("FieldID")]
    public class ModuleFieldSettingInfo
    {
        public Guid SettingID { get; set; }
        public Guid FieldID { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
    }
}