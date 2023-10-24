using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Api.ViewModels
{
    public class DashboardPageViewModel
    {
        public Guid PageID { get; set; }
        public Guid DashboardID { get; set; }
        public Guid? ParentID { get; set; }
        public int PageType { get; set; }
        public string PageName { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Guid? ExistingPageID { get; set; }
        public bool IsVisible { get; set; }
        public bool InheritPermissionFromDashboard { get; set; }
        public int ViewOrder { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserID { get; set; }
        public bool IsChild { get; set; }
        public IEnumerable<string> AuthorizationViewPage { get; set; }
        public DashboardPageModuleView Module { get; set; }
        public List<DashboardPageViewModel> Pages { get; set; }
        public IDictionary<string, object> Settings { get; set; }
    }
}