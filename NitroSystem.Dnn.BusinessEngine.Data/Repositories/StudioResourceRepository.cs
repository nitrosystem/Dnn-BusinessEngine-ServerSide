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
    public class StudioResourceRepository
    {
        public static StudioResourceRepository Instance
        {
            get
            {
                return new StudioResourceRepository();
            }
        }

        private const string CachePrefix = "BE_StudioResources_";

        public Guid AddStudioResource(StudioResourceInfo objStudioResourceInfo)
        {
            objStudioResourceInfo.ResourceID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioResourceInfo>();
                rep.Insert(objStudioResourceInfo);

                DataCache.ClearCache(CachePrefix);

                return objStudioResourceInfo.ResourceID;
            }
        }

        public void UpdateStudioResource(StudioResourceInfo objStudioResourceInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioResourceInfo>();
                rep.Update(objStudioResourceInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteStudioResource(Guid resourceID)
        {
            StudioResourceInfo objStudioResourceInfo = GetStudioResource(resourceID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioResourceInfo>();
                rep.Delete(objStudioResourceInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteStudioResources(Guid extensionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioResourceInfo>();
                rep.Delete("Where ISNULL(IsCustomResource,0) = 0 and ModuleID = @0", extensionID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteCustomStudioResources(Guid extensionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioResourceInfo>();
                rep.Delete("Where ModuleID = @0 and IsCustomResource is not null and IsCustomResource = 1", extensionID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteScenarioStudioResources(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioResourceInfo>();
                rep.Delete("WHERE ISNULL(IsCustomResource,0) = 0 and ModuleID in (SELECT ModuleID FROM dbo.BusinessEngine_Modules WHERE ScenarioID = @0)", scenarioID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public StudioResourceInfo GetStudioResource(Guid resourceID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioResourceInfo>();
                return rep.GetById(resourceID);
            }
        }

        public IEnumerable<StudioResourceInfo> GetStudioResources(string cmsStudioID)
        {
            var result = DataCache.GetCache<IEnumerable<StudioResourceInfo>>(CachePrefix + cmsStudioID);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<StudioResourceInfo>();
                    result = rep.Get(cmsStudioID);
                }

                DataCache.SetCache(CachePrefix + cmsStudioID, result);
            }

            return result;
        }

        public IEnumerable<StudioResourceInfo> GetActiveStudioResources()
        {
            string cacheKey = CachePrefix + "_IsActived";
            var result = DataCache.GetCache<IEnumerable<StudioResourceInfo>>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<StudioResourceInfo>();
                    result = rep.Get().Where(p => p.IsActive).OrderBy(p => p.Priority).ThenBy(p => p.LoadOrder);
                }

                DataCache.SetCache(cacheKey, result);
            }

            return result;
        }

        public IEnumerable<string> GetActiveStudioResourceFilePaths(string cmsStudioID)
        {
            string cacheKey = CachePrefix + cmsStudioID + "_IsActived_FilePaths";
            var result = DataCache.GetCache<IEnumerable<string>>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<StudioResourceInfo>();
                    result = rep.Get(cmsStudioID).Where(p => p.IsActive).OrderBy(p => p.Priority).ThenBy(p => p.LoadOrder).Select(p => p.FilePath);
                }

                DataCache.SetCache(cacheKey, result);
            }

            return result;
        }

        public IEnumerable<StudioResourceInfo> GetStudioCustomResources(Guid extensionID)
        {
            string cacheKey = CachePrefix + "CustomResources_" + extensionID;
            var result = DataCache.GetCache<IEnumerable<StudioResourceInfo>>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    result = ctx.ExecuteQuery<StudioResourceInfo>(System.Data.CommandType.Text, "SELECT * FROM dbo.BusinessEngine_StudioResources WHERE ModuleID = @0 and IsCustomResource = 1 order by LoadOrder", extensionID);
                }

                DataCache.SetCache(cacheKey, result);
            }

            return result;
        }

        public IEnumerable<StudioResourceInfo> GetStudioResources(Guid extensionID)
        {
            var result = DataCache.GetCache<IEnumerable<StudioResourceInfo>>(CachePrefix + extensionID);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    result = ctx.ExecuteQuery<StudioResourceInfo>(System.Data.CommandType.Text, "SELECT p.FilePath,p.ResourceType,m.Version FROM dbo.BusinessEngine_StudioResources p INNER JOIN dbo.BusinessEngine_Modules m ON p.ModuleID = m.ModuleID WHERE p.ModuleID = @0", extensionID);
                }

                DataCache.SetCache(CachePrefix + extensionID, result);
            }

            return result;
        }
    }
}