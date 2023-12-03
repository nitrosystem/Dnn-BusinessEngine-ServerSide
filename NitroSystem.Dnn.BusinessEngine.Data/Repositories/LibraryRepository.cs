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
    public class LibraryRepository
    {
        public static LibraryRepository Instance
        {
            get
            {
                return new LibraryRepository();
            }
        }

        private const string CachePrefix = "BE_Libraries_";

        public Guid AddLibrary(LibraryInfo objLibraryInfo)
        {
            objLibraryInfo.LibraryID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LibraryInfo>();
                rep.Insert(objLibraryInfo);

                DataCache.ClearCache(CachePrefix);

                return objLibraryInfo.LibraryID;
            }
        }

        public void UpdateLibrary(LibraryInfo objLibraryInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LibraryInfo>();
                rep.Update(objLibraryInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteLibrary(Guid libraryID)
        {
            LibraryInfo objLibraryInfo = GetLibrary(libraryID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LibraryInfo>();
                rep.Delete(objLibraryInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public LibraryInfo GetLibrary(Guid libraryID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LibraryInfo>();
                return rep.GetById(libraryID);
            }
        }

        public IEnumerable<LibraryView> GetLibraryResources(string libraryName, string version)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LibraryView>();
                return rep.Find("Where LibraryName = @0 and Version = @1", libraryName, version).OrderBy(r => r.LoadOrder);
            }
        }

        public IEnumerable<LibraryInfo> GetLibraries(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LibraryInfo>();
                return rep.Get(scenarioID);
            }
        }
    }
}