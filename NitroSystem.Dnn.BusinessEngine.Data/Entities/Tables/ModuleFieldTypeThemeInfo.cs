using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ModuleFieldTypeThemes")]
    [PrimaryKey("ThemeID", AutoIncrement = false)]
    [Cacheable("BE_ModuleFieldTypeThemes_", CacheItemPriority.Default, 20)]
    [Scope("FieldType")]
    public class ModuleFieldTypeThemeInfo
    {
        public Guid ThemeID { get; set; }
        public string FieldType { get; set; }
        public string ThemeName { get; set; }
        public string ThemeImage { get; set; }
        public string ThemeCssPath { get; set; }
        public string ThemeCssClass { get; set; }
        public bool IsDark { get; set; }
        public string Description { get; set; }
        public int ViewOrder { get; set; }
    }
}