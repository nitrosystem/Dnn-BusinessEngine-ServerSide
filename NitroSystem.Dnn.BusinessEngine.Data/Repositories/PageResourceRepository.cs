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
    public class PageResourceRepository
    {
        public static PageResourceRepository Instance
        {
            get
            {
                return new PageResourceRepository();
            }
        }

        private const string CachePrefix = "BE_PageResources_";

        public Guid AddPageResource(PageResourceInfo objPageResourceInfo)
        {
            objPageResourceInfo.ResourceID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PageResourceInfo>();
                rep.Insert(objPageResourceInfo);

                DataCache.ClearCache(CachePrefix);

                return objPageResourceInfo.ResourceID;
            }
        }

        public void UpdatePageResource(PageResourceInfo objPageResourceInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PageResourceInfo>();
                rep.Update(objPageResourceInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeletePageResource(Guid resourceID)
        {
            PageResourceInfo objPageResourceInfo = GetPageResource(resourceID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PageResourceInfo>();
                rep.Delete(objPageResourceInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeletePageResources(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PageResourceInfo>();
                rep.Delete("Where ISNULL(IsCustomResource,0) = 0 and ModuleID = @0", moduleID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteCustomPageResources(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PageResourceInfo>();
                rep.Delete("Where ModuleID = @0 and IsCustomResource is not null and IsCustomResource = 1", moduleID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteScenarioPageResources(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PageResourceInfo>();
                rep.Delete("WHERE ISNULL(IsCustomResource,0) = 0 and ModuleID in (SELECT ModuleID FROM dbo.BusinessEngine_Modules WHERE ScenarioID = @0)", scenarioID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public PageResourceInfo GetPageResource(Guid resourceID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PageResourceInfo>();
                return rep.GetById(resourceID);
            }
        }

        public IEnumerable<PageResourceView> GetPageResources(string cmsPageID)
        {
            var result = DataCache.GetCache<IEnumerable<PageResourceView>>(CachePrefix + cmsPageID);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    result = ctx.ExecuteQuery<PageResourceView>(System.Data.CommandType.Text, "SELECT p.FilePath,p.ResourceType,m.Version FROM dbo.BusinessEngine_PageResources p INNER JOIN dbo.BusinessEngine_Modules m ON p.ModuleID = m.ModuleID WHERE p.CmsPageID = @0 order by LoadOrder", cmsPageID);
                }

                DataCache.SetCache(CachePrefix + cmsPageID, result);
            }

            return result;
        }

        public IEnumerable<PageResourceView> GetPageCustomResources(Guid moduleID)
        {
            string cacheKey = CachePrefix + "CustomResources_" + moduleID;
            var result = DataCache.GetCache<IEnumerable<PageResourceView>>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    result = ctx.ExecuteQuery<PageResourceView>(System.Data.CommandType.Text, "SELECT * FROM dbo.BusinessEngine_PageResources WHERE ModuleID = @0 and IsCustomResource = 1 order by LoadOrder", moduleID);
                }

                DataCache.SetCache(cacheKey, result);
            }

            return result;
        }

        public IEnumerable<PageResourceView> GetPageResources(Guid moduleID)
        {
            var result = DataCache.GetCache<IEnumerable<PageResourceView>>(CachePrefix + moduleID);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    result = ctx.ExecuteQuery<PageResourceView>(System.Data.CommandType.Text, "SELECT p.FilePath,p.ResourceType,m.Version FROM dbo.BusinessEngine_PageResources p INNER JOIN dbo.BusinessEngine_Modules m ON p.ModuleID = m.ModuleID WHERE p.ModuleID = @0", moduleID);
                }

                DataCache.SetCache(CachePrefix + moduleID, result);
            }

            return result;
        }
    }
}