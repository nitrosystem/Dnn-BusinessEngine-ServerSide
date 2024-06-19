using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Framework.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels
{
    public class ServiceViewModel
    {
        public Guid ServiceID { get; set; }
        public Guid ScenarioID { get; set; }
        public Guid? DatabaseID { get; set; }
        public Guid? GroupID { get; set; }
        public Guid? ViewModelID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceType { get; set; }
        public string ServiceSubtype { get; set; }
        public bool IsEnabled { get; set; }
        public bool HasResult { get; set; }
        public ServiceResultType ResultType { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
        public IEnumerable<string> AuthorizationRunService { get; set; }
        public IEnumerable<ServiceParamInfo> Params { get; set; }
        public IDictionary<string, object> Settings { get; set; }
        public string ServiceTypeIcon { get; set; }
    }
}