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
    public class LibraryResourceRepository
    {
        public static LibraryResourceRepository Instance
        {
            get
            {
                return new LibraryResourceRepository();
            }
        }

        private const string CachePrefix = "BE_LibraryResources_";

        public Guid AddLibraryResource(LibraryResourceInfo objLibraryResourceInfo)
        {
            objLibraryResourceInfo.ResourceID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LibraryResourceInfo>();
                rep.Insert(objLibraryResourceInfo);

                DataCache.ClearCache(CachePrefix);

                return objLibraryResourceInfo.ResourceID;
            }
        }

        public void UpdateLibraryResource(LibraryResourceInfo objLibraryResourceInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LibraryResourceInfo>();
                rep.Update(objLibraryResourceInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteLibraryResource(Guid resourceID)
        {
            LibraryResourceInfo objLibraryResourceInfo = GetLibraryResource(resourceID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LibraryResourceInfo>();
                rep.Delete(objLibraryResourceInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteLibraryResources(string objectName)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LibraryResourceInfo>();
                rep.Delete("Where ObjectName = @0", objectName);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteLibraryResourcesByExtensionID(Guid extensionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LibraryResourceInfo>();
                rep.Delete("Where ExtensionID =@0", extensionID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public LibraryResourceInfo GetLibraryResource(Guid resourceID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LibraryResourceInfo>();
                return rep.GetById(resourceID);
            }
        }

        public IEnumerable<LibraryResourceInfo> GetLibraryResources(string objectName)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LibraryResourceInfo>();
                return rep.Get(objectName).OrderBy(r => r.LoadOrder);
            }
        }
    }
}