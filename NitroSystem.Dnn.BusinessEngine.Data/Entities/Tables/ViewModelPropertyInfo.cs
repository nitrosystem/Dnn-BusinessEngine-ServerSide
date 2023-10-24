using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ViewModelProperties")]
    [PrimaryKey("PropertyID", AutoIncrement = false)]
    [Cacheable("BE_ViewModelProperties_", CacheItemPriority.Default, 20)]
    [Scope("ViewModelID")]
    public class ViewModelPropertyInfo
    {
        public Guid PropertyID { get; set; }
        public Guid ViewModelID { get; set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public Guid? PropertyTypeID { get; set; }
        public int ViewOrder { get; set; }
    }
}