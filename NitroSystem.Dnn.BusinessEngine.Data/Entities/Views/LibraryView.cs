using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Views
{
    [TableName("BusinessEngineView_Libraries")]
    [PrimaryKey("LibraryID", AutoIncrement = false)]
    [Cacheable("BusinessEngine_Libraries_View_", CacheItemPriority.Default, 20)]
    public class LibraryView
    {
        public Guid LibraryID { get; set; }
        public string Type { get; set; }
        public string LibraryName { get; set; }
        public string LibraryLogo { get; set; }
        public string Version { get; set; }
        public string ResourceType { get; set; }
        public string ResourcePath { get; set; }
        public int LoadOrder { get; set; }
    }
}