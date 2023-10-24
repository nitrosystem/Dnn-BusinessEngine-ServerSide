using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace NitroSystem.Dnn.BusinessEngine.Data.Entities.Views
{
    [TableName("BusinessEngineView_ActionTypes")]
    [PrimaryKey("TypeID", AutoIncrement = false)]
    [Cacheable("BE_ActionType_Views_", CacheItemPriority.Default, 20)]
    public class ActionTypeView
    {
        public Guid TypeID { get; set; }
        public Guid GroupID { get; set; }
        public string GroupName { get; set; }
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
        public int GroupViewOrder { get; set; }
        public int ViewOrder { get; set; }
    }
}