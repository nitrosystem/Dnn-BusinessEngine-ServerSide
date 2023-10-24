using DotNetNuke.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables
{
    [TableName("BusinessEngine_ActionTypes")]
    [PrimaryKey("TypeID", AutoIncrement = false)]
    [Cacheable("BE_ActionTypes_", CacheItemPriority.Default, 20)]
    public class ActionTypeInfo
    {
        public Guid TypeID { get; set; }
        public Guid ExtensionID { get; set; }
        public Guid GroupID { get; set; }
        public string ActionType { get; set; }
        public string Title { get; set; }
        public int Scope { get; set; }
        public string ActionComponent { get; set; }
        public string ComponentSubParams { get; set; }
        public string ActionJsPath { get; set; }
        public string BusinessControllerClass { get; set; }
        public bool HasResults { get; set; }
        public int IconType { get; set; }
        public string Icon { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public int ViewOrder { get; set; }

        [IgnoreColumn]
        public string GroupName { get; set; }
    }
}
