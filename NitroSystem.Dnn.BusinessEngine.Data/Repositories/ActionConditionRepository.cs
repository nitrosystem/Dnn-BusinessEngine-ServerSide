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
    public class ActionConditionRepository
    {
        public static ActionConditionRepository Instance
        {
            get
            {
                return new ActionConditionRepository();
            }
        }

        private const string CachePrefix = "BE_ActionConditions_";

        public Guid AddCondition(ActionConditionInfo objActionConditionInfo)
        {
            objActionConditionInfo.ConditionID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionConditionInfo>();
                rep.Insert(objActionConditionInfo);

                DataCache.ClearCache(CachePrefix);

                return objActionConditionInfo.ConditionID;
            }
        }

        public void UpdateCondition(ActionConditionInfo objActionConditionInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionConditionInfo>();
                rep.Update(objActionConditionInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteConditions(Guid actionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionConditionInfo>();

                rep.Delete("Where ActionID = @0", actionID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ActionConditionInfo GetCondition(Guid conditionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionConditionInfo>();
                return rep.GetById(conditionID);
            }
        }

        public IEnumerable<ActionConditionInfo> GetConditions(Guid actionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionConditionInfo>();

                return rep.Get(actionID);
            }
        }
    }
}