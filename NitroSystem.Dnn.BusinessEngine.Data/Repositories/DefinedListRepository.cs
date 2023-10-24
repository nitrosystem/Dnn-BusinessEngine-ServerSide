using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class DefinedListRepository
    {
        public static DefinedListRepository Instance
        {
            get
            {
                return new DefinedListRepository();
            }
        }

        private const string CachePrefix = "BE_DefinedLists_";

        public Guid AddList(DefinedListInfo objDefinedListInfo)
        {
            objDefinedListInfo.ListID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListInfo>();
                rep.Insert(objDefinedListInfo);

                DataCache.ClearCache(CachePrefix);

                return objDefinedListInfo.ListID;
            }
        }

        public void UpdateList(DefinedListInfo objDefinedListInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListInfo>();
                rep.Update(objDefinedListInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteList(Guid listID)
        {
            DefinedListInfo objDefinedListInfo = GetList(listID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListInfo>();
                rep.Delete(objDefinedListInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public DefinedListInfo GetList(Guid listID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListInfo>();
                return rep.GetById(listID);
            }
        }

        public DefinedListInfo GetListByDependencyID(Guid? id)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListInfo>();
                var result = rep.Find("Where FieldID = @0", id);
                return result.Any() ? result.First() : null;
            }
        }

        public IEnumerable<DefinedListInfo> GetLists(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListInfo>();

                return rep.Get(scenarioID);
            }
        }

        public IEnumerable<DefinedListInfo> GetLists(Guid scenarioID,bool includePublicLists)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListInfo>();

                return rep.Find("Where ScenarioID = @0 or (ScenarioID is null and @1 = 1)", scenarioID, includePublicLists);
            }
        }

        public IEnumerable<DefinedListInfo> GetAllLists()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListInfo>();

                return rep.Get();
            }
        }
    }
}