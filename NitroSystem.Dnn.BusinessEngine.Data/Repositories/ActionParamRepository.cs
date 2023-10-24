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
    public class ActionParamRepository
    {
        public static ActionParamRepository Instance
        {
            get
            {
                return new ActionParamRepository();
            }
        }

        private const string CachePrefix = "BE_ActionParams_";

        public Guid AddParam(ActionParamInfo objActionParamInfo)
        {
            objActionParamInfo.ParamID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionParamInfo>();
                rep.Insert(objActionParamInfo);

                DataCache.ClearCache(CachePrefix);

                return objActionParamInfo.ParamID;
            }
        }

        public void DeleteParams(Guid actionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionParamInfo>();

                rep.Delete("Where ActionID = @0", actionID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public IEnumerable<ActionParamInfo> GetParams(Guid actionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionParamInfo>();

                return rep.Get(actionID);
            }
        }
    }
}