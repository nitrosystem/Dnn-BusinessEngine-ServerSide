using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ModuleFieldTypeTemplates")]
    [PrimaryKey("TemplateID", AutoIncrement = false)]
    [Cacheable("BE_ModuleFieldTypeTemplates_", CacheItemPriority.Default, 20)]
    [Scope("FieldType")]
    public class ModuleFieldTypeTemplateInfo
    {
        public Guid TemplateID { get; set; }
        public string FieldType { get; set; }
        public string TemplateName { get; set; }
        public string TemplateImage { get; set; }
        public string TemplatePath { get; set; }
        public string Description { get; set; }
        public int ViewOrder { get; set; }

        [IgnoreColumn]
        public bool IsSkinTemplate { get; set; }
    }
}