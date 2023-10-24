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
    public class ActionResultRepository
    {
        public static ActionResultRepository Instance
        {
            get
            {
                return new ActionResultRepository();
            }
        }

        private const string CachePrefix = "BE_ActionResults_";

        public Guid AddResult(ActionResultInfo objActionResultInfo)
        {
            objActionResultInfo.ResultID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionResultInfo>();
                rep.Insert(objActionResultInfo);

                DataCache.ClearCache(CachePrefix);

                return objActionResultInfo.ResultID;
            }
        }

        public void UpdateResult(ActionResultInfo objActionResultInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionResultInfo>();
                rep.Update(objActionResultInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteResults(Guid actionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionResultInfo>();

                rep.Delete("Where ActionID = @0", actionID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ActionResultInfo GetResult(Guid resultID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionResultInfo>();
                return rep.GetById(resultID);
            }
        }

        public IEnumerable<ActionResultInfo> GetResults(Guid actionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionResultInfo>();

                return rep.Get(actionID);
            }
        }
    }
}