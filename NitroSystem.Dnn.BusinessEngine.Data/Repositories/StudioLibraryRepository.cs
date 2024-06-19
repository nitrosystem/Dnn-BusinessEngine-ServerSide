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
    public class StudioLibraryRepository
    {
        public static StudioLibraryRepository Instance
        {
            get
            {
                return new StudioLibraryRepository();
            }
        }

        private const string CachePrefix = "BE_StudioLibraries_";

        public Guid AddStudioLibrary(StudioLibraryInfo objStudioLibraryInfo)
        {
            objStudioLibraryInfo.ItemID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioLibraryInfo>();
                rep.Insert(objStudioLibraryInfo);

                DataCache.ClearCache(CachePrefix);

                return objStudioLibraryInfo.ItemID;
            }
        }

        public void UpdateStudioLibrary(StudioLibraryInfo objStudioLibraryInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioLibraryInfo>();
                rep.Update(objStudioLibraryInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteStudioLibrary(Guid itemID)
        {
            StudioLibraryInfo objStudioLibraryInfo = GetStudioLibrary(itemID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioLibraryInfo>();
                rep.Delete(objStudioLibraryInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteStudioLibraries(Guid extensionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioLibraryInfo>();
                rep.Delete("Where ISNULL(IsCustomResource,0) = 0 and ModuleID = @0", extensionID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteCustomStudioLibraries(Guid extensionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioLibraryInfo>();
                rep.Delete("Where ModuleID = @0 and IsCustomResource is not null and IsCustomResource = 1", extensionID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteScenarioStudioLibraries(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioLibraryInfo>();
                rep.Delete("WHERE ISNULL(IsCustomResource,0) = 0 and ModuleID in (SELECT ModuleID FROM dbo.BusinessEngine_Modules WHERE ScenarioID = @0)", scenarioID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public StudioLibraryInfo GetStudioLibrary(Guid itemID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioLibraryInfo>();
                return rep.GetById(itemID);
            }
        }

        public IEnumerable<StudioLibraryInfo> GetStudioLibraries(string cmsStudioID)
        {
            var result = DataCache.GetCache<IEnumerable<StudioLibraryInfo>>(CachePrefix + cmsStudioID);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<StudioLibraryInfo>();
                    result = rep.Get(cmsStudioID);
                }

                DataCache.SetCache(CachePrefix + cmsStudioID, result);
            }

            return result;
        }

        public IEnumerable<StudioLibraryInfo> GetStudioLibraries()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<StudioLibraryInfo>();
                return rep.Get().OrderBy(l => l.LoadOrder);
            }
        }
    }
}