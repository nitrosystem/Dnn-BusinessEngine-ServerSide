using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class ScenarioRepository
    {
        public static ScenarioRepository Instance
        {
            get
            {
                return new ScenarioRepository();
            }
        }

        private const string CachePrefix = "BE_Scenarios_";

        public Guid AddScenario(ScenarioInfo objScenarioInfo)
        {
            objScenarioInfo.ScenarioID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ScenarioInfo>();
                rep.Insert(objScenarioInfo);

                DataCache.ClearCache(CachePrefix);

                return objScenarioInfo.ScenarioID;
            }
        }

        public void UpdateScenario(ScenarioInfo objScenarioInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ScenarioInfo>();
                rep.Update(objScenarioInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteScenario(Guid scenarioID)
        {
            ScenarioInfo objScenarioInfo = GetScenario(scenarioID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ScenarioInfo>();
                rep.Delete(objScenarioInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ScenarioInfo GetScenario(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ScenarioInfo>();
                return rep.GetById(scenarioID);
            }
        }

        public ScenarioInfo GetScenario(string scenarioName)
        {
            return GetScenarios().FirstOrDefault(s => s.ScenarioName == scenarioName);
        }

        public string GetScenarioName(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ScenarioInfo>();
                return rep.GetById(scenarioID).ScenarioName;
            }
        }

        public IEnumerable<ScenarioInfo> GetScenarios()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ScenarioInfo>();

                return rep.Get();
            }
        }
    }
}