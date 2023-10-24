using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class DashboardPageRepository
    {
        public static DashboardPageRepository Instance
        {
            get
            {
                return new DashboardPageRepository();
            }
        }

        private const string CachePrefix = "BE_DashboardPages_";

        public Guid AddDPage(DashboardPageInfo objDashboardPageInfo)
        {
            objDashboardPageInfo.PageID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardPageInfo>();
                rep.Insert(objDashboardPageInfo);

                DataCache.ClearCache(CachePrefix);
                DataCache.ClearCache("BE_Dashboards_View_");

                return objDashboardPageInfo.PageID;
            }
        }

        public void UpdatePage(DashboardPageInfo objDashboardPageInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardPageInfo>();
                rep.Update(objDashboardPageInfo);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache("BE_Dashboards_View_");
        }

        public void DeletePage(Guid pageID)
        {
            DashboardPageInfo objDashboardPageInfo = GetPage(pageID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardPageInfo>();
                rep.Delete(objDashboardPageInfo);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache("BE_Dashboards_View_");
        }

        public DashboardPageInfo GetPage(Guid pageID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardPageInfo>();
                return rep.GetById(pageID);
            }
        }

        public DashboardPageInfo GetLastPage(Guid? parentID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DashboardPageInfo>();
                var result = rep.Find(string.Format("Where ParentID {0}", parentID == null ? "is null" : ("='" + parentID + "'"))).OrderByDescending(p => p.ViewOrder);
                return result.Any() ? result.First() : null;
            }
        }

        public IEnumerable<DashboardPageInfo> GetPages(Guid dashboardID)
        {
            var result = DataCache.GetCache<IEnumerable<DashboardPageInfo>>(CachePrefix + dashboardID);
            if (1 == 1 || result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<DashboardPageInfo>();

                    result = rep.Get(dashboardID).OrderBy(p => p.ViewOrder);
                }

                DataCache.SetCache(CachePrefix + dashboardID, result);
            }

            return result;
        }
    }
}