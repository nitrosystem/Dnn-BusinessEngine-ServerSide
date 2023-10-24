using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class SitemapRepository
    {
        public static SitemapRepository Instance
        {
            get
            {
                return new SitemapRepository();
            }
        }

        private const string CachePrefix = "BE_Sitemaps_";

        public int AddSitemap(SitemapInfo objSitemapInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SitemapInfo>();
                rep.Insert(objSitemapInfo);

                DataCache.ClearCache(CachePrefix);

                return objSitemapInfo.NodeID;
            }
        }

        public void UpdateSitemap(SitemapInfo objSitemapInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SitemapInfo>();
                rep.Update(objSitemapInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteSitemap(Guid nodeID)
        {
            SitemapInfo objSitemapInfo = GetSitemap(nodeID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SitemapInfo>();
                rep.Delete(objSitemapInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public SitemapInfo GetSitemap(Guid nodeID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SitemapInfo>();
                return rep.GetById(nodeID);
            }
        }

        public IEnumerable<SitemapInfo> GetSitemaps(string groupName)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SitemapInfo>();

                return rep.Get(groupName);
            }
        }
    }
}