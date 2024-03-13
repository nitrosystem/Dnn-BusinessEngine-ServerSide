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
    public class DashboardRepository
    {
        public static DashboardRepository Instance
        {
            get
            {
                return new DashboardRepository();
            }
        }

        private const string CachePrefix = "BE_Dashboards_";

        public Guid AddDashboard(DashboardInfo objDashboardInfo)
        {
            objDashboardInfo.DashboardID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardInfo>();
                rep.Insert(objDashboardInfo);

                DataCache.ClearCache(CachePrefix);

                return objDashboardInfo.DashboardID;
            }
        }

        public void UpdateDashboard(DashboardInfo objDashboardInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardInfo>();
                rep.Update(objDashboardInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteDashboard(Guid dashboardID)
        {
            DashboardInfo objDashboardInfo = GetDashboard(dashboardID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardInfo>();
                rep.Delete(objDashboardInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public DashboardInfo GetDashboard(Guid dashboardID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardInfo>();
                return rep.GetById(dashboardID);
            }
        }

        public DashboardView GetDashboardView(Guid dashboardID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardView>();

                var list = rep.Find("Where DashboardID = @0", dashboardID);
                return list.Any() ? list.First() : null;
            }
        }

        public DashboardView GetDashboardByModuleID(Guid moduleID)
        {
            var result = DataCache.GetCache<DashboardView>(CachePrefix + moduleID);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<DashboardView>();

                    var list = rep.Find("Where ModuleID = @0", moduleID);
                    result = list.Any() ? list.First() : null;
                }

                DataCache.SetCache(CachePrefix + moduleID, result);
            }

            return result;
        }

        public DashboardView GetDashboardByUniqueName(string uniqueName)
        {
            var result = DataCache.GetCache<DashboardView>(CachePrefix + uniqueName);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<DashboardView>();

                    var list = rep.Find("Where UniqueName = @0", uniqueName);
                    result = list.Any() ? list.First() : null;
                }

                DataCache.SetCache(CachePrefix + uniqueName, result);
            }

            return result;
        }

        public byte GetDashboardType(Guid moduleID)
        {
            return (this.GetDashboardByModuleID(moduleID) ?? new DashboardView()).DashboardType;
        }
    }
}