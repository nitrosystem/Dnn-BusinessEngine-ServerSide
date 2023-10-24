using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels
{
    public class ViewModelViewModel
    {
        public Guid ViewModelID { get; set; }
        public Guid ScenarioID { get; set; }
        public string ViewModelName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
        public IEnumerable<ViewModelPropertyInfo> Properties { get; set; }
    }
}