using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_Sitemap")]
    [PrimaryKey("NodeID")]
    [Cacheable("BE_Sitemaps_", CacheItemPriority.Default, 20)]
    [Scope("GroupName")]
    public class SitemapInfo
    {
        public int NodeID { get; set; }
        public string GroupName { get; set; }
        public string EntityID { get; set; }
        public DateTime LastModified { get; set; }
        public string ChangeFrequency { get; set; }
        public float Priority { get; set; }
        public string Url { get; set; }
    }
}