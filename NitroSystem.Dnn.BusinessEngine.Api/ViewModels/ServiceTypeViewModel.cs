using NitroSystem.Dnn.BusinessEngine.Core.Enums;
using NitroSystem.Dnn.BusinessEngine.Framework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels
{
  public  class ServiceTypeViewModel
    {
        public Guid TypeID { get; set; }
        public Guid GroupID { get; set; }
        public string GroupName { get; set; }
        public string ServiceType { get; set; }
        public string ServiceSubtype { get; set; }
        public string ServiceComponent { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string JsFile { get; set; }
        public string JsServiceFn { get; set; }
        public string BusinessControllerClass { get; set; }
        public bool HasResult { get; set; }
        public ServiceResultType ResultType { get; set; }
        public bool SubmitApi { get; set; }
        public string DefaultDatabaseObjectType { get; set; }
        public int IconType { get; set; }
        public string Icon { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public int GroupViewOrder { get; set; }
        public int ViewOrder { get; set; }
    }
}
