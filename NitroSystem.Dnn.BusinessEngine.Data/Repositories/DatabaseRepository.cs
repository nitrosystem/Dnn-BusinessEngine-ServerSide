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
    public class DatabaseRepository
    {
        public static DatabaseRepository Instance
        {
            get
            {
                return new DatabaseRepository();
            }
        }

        private const string CachePrefix = "BE_Databases_";

        public Guid AddDatabase(DatabaseInfo objDatabaseInfo)
        {
            objDatabaseInfo.DatabaseID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DatabaseInfo>();
                rep.Insert(objDatabaseInfo);

                DataCache.ClearCache(CachePrefix);

                return objDatabaseInfo.DatabaseID;
            }
        }

        public void UpdateDatabase(DatabaseInfo objDatabaseInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DatabaseInfo>();
                rep.Update(objDatabaseInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteDatabase(Guid skinID)
        {
            DatabaseInfo objDatabaseInfo = GetDatabase(skinID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DatabaseInfo>();
                rep.Delete(objDatabaseInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public DatabaseInfo GetDatabase(Guid skinID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DatabaseInfo>();
                return rep.GetById(skinID);
            }
        }

        public IEnumerable<DatabaseInfo> GetDatabases()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DatabaseInfo>();

                return rep.Get();
            }
        }
    }
}