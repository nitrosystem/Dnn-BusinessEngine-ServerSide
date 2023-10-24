using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class DashboardPageModuleRepository
    {
        public static DashboardPageModuleRepository Instance
        {
            get
            {
                return new DashboardPageModuleRepository();
            }
        }

        private const string CachePrefix = "BE_DashboardPageModules_";

        public Guid AddModule(DashboardPageModuleInfo objDashboardPageModuleInfo)
        {
            objDashboardPageModuleInfo.PageModuleID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardPageModuleInfo>();
                rep.Insert(objDashboardPageModuleInfo);

                DataCache.ClearCache(CachePrefix);
                DataCache.ClearCache("BE_DashboardPageModules_View_");

                return objDashboardPageModuleInfo.PageModuleID;
            }
        }

        public void UpdateModule(DashboardPageModuleInfo objDashboardPageModuleInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardPageModuleInfo>();
                rep.Update(objDashboardPageModuleInfo);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache("BE_DashboardPageModules_View_");
        }

        public void DeleteModule(Guid pageModuleID)
        {
            DashboardPageModuleInfo objDashboardPageModuleInfo = GetModule(pageModuleID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardPageModuleInfo>();
                rep.Delete(objDashboardPageModuleInfo);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache("BE_DashboardPageModules_View_");
        }

        public void DeleteModuleByModuleID(Guid moduleID)
        {
            DashboardPageModuleInfo objDashboardPageModuleInfo = GetModuleByModuleID(moduleID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardPageModuleInfo>();
                rep.Delete("Where ModuleID = @0", moduleID);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache("BE_DashboardPageModules_View_");
        }

        public DashboardPageModuleInfo GetModule(Guid pageModuleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardPageModuleInfo>();
                return rep.GetById(pageModuleID);
            }
        }

        public DashboardPageModuleInfo GetModuleByModuleID(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardPageModuleInfo>();

                var result = rep.Find("Where ModuleID = @0", moduleID);
                return result.Any() ? result.First() : null;
            }
        }

        public DashboardPageModuleView GetModuleViewByPageID(Guid pageID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardPageModuleView>();

                var result = rep.Find("Where PageID = @0", pageID);
                return result.Any() ? result.First() : null;
            }
        }

        public DashboardPageModuleView GetModuleView(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardPageModuleView>();

                var result = rep.Find("Where ModuleID = @0", moduleID);
                return result.Any() ? result.First() : null;
            }
        }
    }
}