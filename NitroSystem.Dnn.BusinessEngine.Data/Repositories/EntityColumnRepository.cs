using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class EntityColumnRepository
    {
        public static EntityColumnRepository Instance
        {
            get
            {
                return new EntityColumnRepository();
            }
        }

        private const string CachePrefix = "BE_EntityColumns_";

        public Guid AddColumn(EntityColumnInfo objEntityColumnInfo)
        {
            objEntityColumnInfo.ColumnID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<EntityColumnInfo>();
                rep.Insert(objEntityColumnInfo);

                DataCache.ClearCache(CachePrefix);

                return objEntityColumnInfo.ColumnID;
            }
        }

        public void UpdateColumn(EntityColumnInfo objEntityColumnInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<EntityColumnInfo>();
                rep.Update(objEntityColumnInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteColumn(Guid entityColumnID)
        {
            EntityColumnInfo objEntityColumnInfo = GetColumn(entityColumnID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<EntityColumnInfo>();
                rep.Delete(objEntityColumnInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public EntityColumnInfo GetColumn(Guid entityColumnID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<EntityColumnInfo>();
                return rep.GetById(entityColumnID);
            }
        }

        public IEnumerable<EntityColumnInfo> GetColumns(Guid entityID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<EntityColumnInfo>();

                return rep.Get(entityID).OrderBy(c => c.ViewOrder);
            }
        }
    }
}