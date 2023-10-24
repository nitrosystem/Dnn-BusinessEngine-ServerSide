using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels
{
    public class ModuleVariableViewModel
    {
        public Guid VariableID { get; set; }
        public Guid ModuleID { get; set; }
        public string VariableName { get; set; }
        public string VariableType { get; set; }
        public int Scope { get; set; }
        public bool IsSystemVariable { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
        public ViewModelViewModel ViewModel { get; set; }
    }
}