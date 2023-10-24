using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels
{
    public class DefinedListViewModel
    {
        public Guid ListID { get; set; }
        public Guid? ScenarioID { get; set; }
        public Guid? ParentID { get; set; }
        public Guid? FieldID { get; set; }
        public string ListName { get; set; }
        public string ListType { get; set; }
        public bool IsMultiLevelList { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public int ViewOrder { get; set; }
        public IEnumerable<DefinedListItemViewModel> Items { get; set; }
    }
}