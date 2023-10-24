using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class ModuleVariableRepository
    {
        public static ModuleVariableRepository Instance
        {
            get
            {
                return new ModuleVariableRepository();
            }
        }

        private const string CachePrefix = "BE_ModuleVariables_";

        public Guid AddVariable(ModuleVariableInfo objModuleVariableInfo)
        {
            objModuleVariableInfo.VariableID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleVariableInfo>();
                rep.Insert(objModuleVariableInfo);

                DataCache.ClearCache(CachePrefix);

                return objModuleVariableInfo.VariableID;
            }
        }

        public void UpdateVariable(ModuleVariableInfo objModuleVariableInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleVariableInfo>();
                rep.Update(objModuleVariableInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteVariable(Guid variableID)
        {
            ModuleVariableInfo objModuleVariableInfo = GetVariable(variableID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleVariableInfo>();
                rep.Delete(objModuleVariableInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteVariables(IEnumerable<Guid> variablesID)
        {
            if (variablesID != null && variablesID.Any())
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<ModuleVariableInfo>();
                    rep.Delete("Where VariableID in (@0)", string.Join(",", variablesID));
                }

                DataCache.ClearCache(CachePrefix);
            }
        }

        public ModuleVariableInfo GetVariable(Guid variableID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleVariableInfo>();
                return rep.GetById(variableID);
            }
        }

        public IEnumerable<ModuleVariableInfo> GetVariables(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleVariableInfo>();

                return rep.Get(moduleID);
            }
        }
    }
}