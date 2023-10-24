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
    public class ModuleFieldTypeResourceRepository
    {
        public static ModuleFieldTypeResourceRepository Instance
        {
            get
            {
                return new ModuleFieldTypeResourceRepository();
            }
        }

        private const string CachePrefix = "BE_ModuleFieldTypeResources_";

        public Guid AddResource(ModuleFieldTypeResourceInfo objModuleFieldTypeResource)
        {
            objModuleFieldTypeResource.ResourceID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeResourceInfo>();
                rep.Insert(objModuleFieldTypeResource);

                DataCache.ClearCache(CachePrefix);

                return objModuleFieldTypeResource.ResourceID;
            }
        }

        public void UpdateResource(ModuleFieldTypeResourceInfo objModuleFieldTypeResource)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeResourceInfo>();
                rep.Update(objModuleFieldTypeResource);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteResource(int resourceID)
        {
            ModuleFieldTypeResourceInfo objModuleFieldTypeResource = GetResource(resourceID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeResourceInfo>();
                rep.Delete(objModuleFieldTypeResource);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ModuleFieldTypeResourceInfo GetResource(int resourceID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeResourceInfo>();
                return rep.GetById(resourceID);
            }
        }

        public IEnumerable<ModuleFieldTypeResourceInfo> GetResources(string fieldType)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeResourceInfo>();
                var result = rep.Get(fieldType).OrderBy(t => t.ViewOrder);
                return result;
            }
        }
    }
}