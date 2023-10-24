using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_Skins")]
    [PrimaryKey("SkinID", AutoIncrement = false)]
    [Cacheable("BE_Skins_", CacheItemPriority.Default, 20)]
    public class SkinInfo
    {
        public Guid SkinID { get; set; }
        public Guid ExtensionID { get; set; }
        public string SkinName{ get; set; }
        public string Title { get; set; }
        public string SkinImage { get; set; }
        public string SkinPath { get; set; }
        public string Description { get; set; }
    }
}