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
    public class ModuleCustomResourceRepository
    {
        public static ModuleCustomResourceRepository Instance
        {
            get
            {
                return new ModuleCustomResourceRepository();
            }
        }

        private const string CachePrefix = "BE_ModuleCustomResources_";

        public Guid AddResource(ModuleCustomResourceInfo objModuleCustomResourceInfo)
        {
            objModuleCustomResourceInfo.ItemID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleCustomResourceInfo>();
                rep.Insert(objModuleCustomResourceInfo);

                DataCache.ClearCache(CachePrefix);

                return objModuleCustomResourceInfo.ItemID;
            }
        }

        public void UpdateResource(ModuleCustomResourceInfo objModuleCustomResourceInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleCustomResourceInfo>();
                rep.Update(objModuleCustomResourceInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteResource(Guid itemID)
        {
            ModuleCustomResourceInfo objModuleCustomResourceInfo = GetResource(itemID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleCustomResourceInfo>();
                rep.Delete(objModuleCustomResourceInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteResources(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleCustomResourceInfo>();
                rep.Delete("Where ModuleID = @0", moduleID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ModuleCustomResourceInfo GetResource(Guid itemID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleCustomResourceInfo>();
                return rep.GetById(itemID);
            }
        }

        public IEnumerable<ModuleCustomResourceInfo> GetResources(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleCustomResourceInfo>();
                return rep.Get(moduleID).OrderBy(l => l.LoadOrder); 
            }
        }
    }
}