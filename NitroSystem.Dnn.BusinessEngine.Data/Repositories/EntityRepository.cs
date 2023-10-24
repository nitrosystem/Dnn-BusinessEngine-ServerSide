using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class EntityRepository
    {
        public static EntityRepository Instance
        {
            get
            {
                return new EntityRepository();
            }
        }

        private const string CachePrefix = "BE_Entities_";

        public Guid AddEntity(EntityInfo objEntityInfo)
        {
            objEntityInfo.EntityID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<EntityInfo>();
                rep.Insert(objEntityInfo);

                DataCache.ClearCache(CachePrefix);

                return objEntityInfo.EntityID;
            }
        }

        public void UpdateEntity(EntityInfo objEntityInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<EntityInfo>();
                rep.Update(objEntityInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void UpdateGroup(Guid itemID, Guid? groupID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                ctx.Execute(System.Data.CommandType.Text, "UPDATE dbo.BusinessEngine_Entities SET GroupID = @0 WHERE EntityID = @1", groupID, itemID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteEntity(Guid entityID)
        {
            EntityInfo objEntityInfo = GetEntity(entityID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<EntityInfo>();
                rep.Delete(objEntityInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public EntityInfo GetEntity(Guid entityID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<EntityInfo>();
                return rep.GetById(entityID);
            }
        }

        public EntityInfo GetEntityByModuleID(Guid entityID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<EntityInfo>();
                return rep.GetById(entityID);
            }
        }

        public IEnumerable<EntityInfo> GetEntities(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<EntityInfo>();

                return rep.Get(scenarioID).OrderBy(e => e.ViewOrder);
            }
        }

        public IEnumerable<EntityInfo> GetAllEntities()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<EntityInfo>();

                return rep.Get().OrderBy(e => e.ViewOrder);
            }
        }
    }
}