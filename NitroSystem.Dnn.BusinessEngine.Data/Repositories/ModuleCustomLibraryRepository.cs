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
    public class ModuleCustomLibraryRepository
    {
        public static ModuleCustomLibraryRepository Instance
        {
            get
            {
                return new ModuleCustomLibraryRepository();
            }
        }

        private const string CachePrefix = "BE_ModuleCustomLibraries_";

        public Guid AddLibrary(ModuleCustomLibraryInfo objModuleCustomLibraryInfo)
        {
            objModuleCustomLibraryInfo.ItemID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleCustomLibraryInfo>();
                rep.Insert(objModuleCustomLibraryInfo);

                DataCache.ClearCache(CachePrefix);

                return objModuleCustomLibraryInfo.ItemID;
            }
        }

        public void UpdateLibrary(ModuleCustomLibraryInfo objModuleCustomLibraryInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleCustomLibraryInfo>();
                rep.Update(objModuleCustomLibraryInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteLibrary(Guid itemID)
        {
            ModuleCustomLibraryInfo objModuleCustomLibraryInfo = GetLibrary(itemID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleCustomLibraryInfo>();
                rep.Delete(objModuleCustomLibraryInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteLibraries(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleCustomLibraryInfo>();
                rep.Delete("Where ModuleID = @0", moduleID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ModuleCustomLibraryInfo GetLibrary(Guid itemID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleCustomLibraryInfo>();
                return rep.GetById(itemID);
            }
        }

        public IEnumerable<ModuleCustomLibraryInfo> GetLibraries(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleCustomLibraryInfo>();
                return rep.Get(moduleID).OrderBy(l => l.LoadOrder); 
            }
        }
    }
}