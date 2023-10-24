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
    public class LogRepository
    {
        public static LogRepository Instance
        {
            get
            {
                return new LogRepository();
            }
        }

        private const string CachePrefix = "BE_Logs_";

        public Guid AddLog(LogInfo objLogInfo)
        {
            objLogInfo.LogID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LogInfo>();
                rep.Insert(objLogInfo);

                DataCache.ClearCache(CachePrefix);

                return objLogInfo.LogID;
            }
        }

        public void UpdateLog(LogInfo objLogInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LogInfo>();
                rep.Update(objLogInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteLog(Guid logID)
        {
            LogInfo objLogInfo = GetLog(logID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LogInfo>();
                rep.Delete(objLogInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public LogInfo GetLog(Guid logID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LogInfo>();
                return rep.GetById(logID);
            }
        }

        public IEnumerable<LogInfo> GetLogs(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LogInfo>();
                return rep.Get(scenarioID);
            }
        }
    }
}