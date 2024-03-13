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
    public class ModuleFieldTypeLibraryRepository
    {
        public static ModuleFieldTypeLibraryRepository Instance
        {
            get
            {
                return new ModuleFieldTypeLibraryRepository();
            }
        }

        private const string CachePrefix = "BE_ModuleFieldTypeLibraries_";

        public Guid AddLibrary(ModuleFieldTypeLibraryInfo objModuleFieldTypeLibraryInfo)
        {
            objModuleFieldTypeLibraryInfo.ItemID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeLibraryInfo>();
                rep.Insert(objModuleFieldTypeLibraryInfo);

                DataCache.ClearCache(CachePrefix);

                return objModuleFieldTypeLibraryInfo.ItemID;
            }
        }

        public void UpdateLibrary(ModuleFieldTypeLibraryInfo objModuleFieldTypeLibraryInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeLibraryInfo>();
                rep.Update(objModuleFieldTypeLibraryInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteLibrary(Guid itemID)
        {
            ModuleFieldTypeLibraryInfo objModuleFieldTypeLibraryInfo = GetLibrary(itemID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeLibraryInfo>();
                rep.Delete(objModuleFieldTypeLibraryInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ModuleFieldTypeLibraryInfo GetLibrary(Guid itemID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeLibraryInfo>();
                return rep.GetById(itemID);
            }
        }

        public IEnumerable<ModuleFieldTypeLibraryInfo> GetLibraries(string fieldType)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeLibraryInfo>();
                return rep.Get(fieldType).OrderBy(l => l.LoadOrder); 
            }
        }
    }
}