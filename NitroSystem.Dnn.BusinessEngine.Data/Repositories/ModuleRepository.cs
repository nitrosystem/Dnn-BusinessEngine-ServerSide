using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class ModuleRepository
    {
        public static ModuleRepository Instance
        {
            get
            {
                return new ModuleRepository();
            }
        }

        private const string CachePrefix = "BE_Modules_";
        private const string FieldCachePrefix = "BE_ModuleFields_";
        private const string ActionsCachePrefix = "BE_Actions_";


        public Guid AddModule(ModuleInfo objModuleInfo)
        {
            objModuleInfo.ModuleID = Guid.NewGuid();
            //objModuleInfo.ModuleName = Regex.Replace(objModuleInfo.ModuleName, @"[^a-zA-Z0-9]", "");

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleInfo>();
                rep.Insert(objModuleInfo);

                DataCache.ClearCache(CachePrefix);
                DataCache.ClearCache(FieldCachePrefix);
                DataCache.ClearCache(ActionsCachePrefix);

                return objModuleInfo.ModuleID;
            }
        }

        public Guid AddModule(Guid scenarioID, Guid? parentID, string moduleType, string moduleName, string moduleTitle, int portalID, int? dnnModuleID, int userID, string wrapper = "Dnn")
        {
            return AddModule(new ModuleInfo()
            {
                ScenarioID = scenarioID,
                ParentID = parentID,
                Wrapper = wrapper,
                ModuleType = moduleType,
                ModuleName = moduleName,
                ModuleTitle = moduleTitle,
                PortalID = portalID,
                DnnModuleID = dnnModuleID,
                CreatedByUserID = userID,
                CreatedOnDate = DateTime.Now,
                LastModifiedByUserID = userID,
                LastModifiedOnDate = DateTime.Now
            });
        }

        public void AddModuleVersion(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                ctx.Execute(System.Data.CommandType.Text, "UPDATE dbo.BusinessEngine_Modules SET Version = ISNULL(Version,0) + 1 WHERE ModuleID = @0", moduleID);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache(FieldCachePrefix);
            DataCache.ClearCache(ActionsCachePrefix);
        }

        public void UpdateModule(ModuleInfo objModuleInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleInfo>();
                rep.Update(objModuleInfo);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache(FieldCachePrefix);
            DataCache.ClearCache(ActionsCachePrefix);
        }

        public void UpdateModuleVersion(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                ctx.Execute(System.Data.CommandType.Text, "UPDATE dbo.BusinessEngine_Modules SET Version = Version + 1 WHERE ModuleID = @0", moduleID);

                DataCache.ClearCache(CachePrefix);
                DataCache.ClearCache(FieldCachePrefix);
                DataCache.ClearCache(ActionsCachePrefix);
            }
        }

        public void DeleteModule(Guid moduleID)
        {
            ModuleInfo objModuleInfo = GetModule(moduleID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleInfo>();
                rep.Delete(objModuleInfo);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache(FieldCachePrefix);
            DataCache.ClearCache(ActionsCachePrefix);
        }

        public ModuleInfo GetModule(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleInfo>();
                return rep.GetById(moduleID);
            }
        }

        public ModuleView GetModuleView(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleView>();
                return rep.GetById(moduleID);
            }
        }

        public string GetModuleName(Guid moduleID)
        {
            string cacheKey = CachePrefix + "ModuleName_" + moduleID;

            var result = DataCache.GetCache<string>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    result = ctx.ExecuteScalar<string>(System.Data.CommandType.Text, "SELECT ModuleName FROM dbo.BusinessEngine_Modules WHERE ModuleID = @0", moduleID);

                    DataCache.SetCache(cacheKey, result);
                }
            }

            return result;
        }

        public bool? IsSSR(Guid moduleID)
        {
            string cacheKey = CachePrefix + "IsSSR_" + moduleID;

            var result = DataCache.GetCache<bool?>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    result = ctx.ExecuteScalar<bool?>(System.Data.CommandType.Text, "SELECT IsSSR FROM dbo.BusinessEngine_Modules WHERE ModuleID = @0", moduleID);

                    DataCache.SetCache(cacheKey, result);
                }
            }

            return result;
        }

        public bool? IsDisabledFrontFramework(Guid moduleID)
        {
            string cacheKey = CachePrefix + "IsDisabledFrontFramework_" + moduleID;

            var result = DataCache.GetCache<bool?>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    result = ctx.ExecuteScalar<bool?>(System.Data.CommandType.Text, "SELECT IsDisabledFrontFramework FROM dbo.BusinessEngine_Modules WHERE ModuleID = @0", moduleID);

                    DataCache.SetCache(cacheKey, result);
                }
            }

            return result;
        }

        public string GetModuleScenarioName(Guid moduleID)
        {
            string cacheKey = CachePrefix + "ScenarioName_" + moduleID;

            var result = DataCache.GetCache<string>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    result = ctx.ExecuteScalar<string>(System.Data.CommandType.Text, "SELECT ScenarioName FROM dbo.BusinessEngineView_Modules WHERE ModuleID = @0", moduleID);

                    DataCache.SetCache(cacheKey, result);
                }
            }

            return result;
        }

        public Guid GetModuleScenarioID(Guid moduleID)
        {
            string cacheKey = CachePrefix + "ScenarioID_" + moduleID;

            var result = DataCache.GetCache<Guid>(cacheKey);
            if (result == Guid.Empty)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<ModuleInfo>();

                    var modules = rep.Find("WHERE ModuleID = @0", moduleID);

                    result = modules.Any() ? modules.First().ScenarioID : Guid.Empty;
                }

                DataCache.SetCache(cacheKey, result);
            }

            return result;
        }

        public IEnumerable<ModuleInfo> GetModules(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleInfo>();

                return rep.Get(scenarioID);
            }
        }

        public Guid? GetModuleGuidByDnnModuleID(int dnnModuleID)
        {
            string cacheKey = CachePrefix + "ModuleGuid_" + dnnModuleID;

            var result = DataCache.GetCache<Guid?>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<ModuleInfo>();

                    var modules = rep.Find("WHERE DnnModuleID = @0", dnnModuleID);

                    result = modules.Any() ? modules.First().ModuleID : (Guid?)null;
                }

                DataCache.SetCache(cacheKey, result);
            }

            return result;
        }

        public IEnumerable<Guid> GetModuleIdsByParentID(Guid moduleID)
        {
            string cacheKey = CachePrefix + "ModuleIDs_" + moduleID;

            var result = DataCache.GetCache<IEnumerable<Guid>>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    result = ctx.ExecuteQuery<Guid>(System.Data.CommandType.Text, "SELECT ModuleID FROM dbo.BusinessEngine_Modules WHERE ParentID = @0", moduleID);

                    DataCache.SetCache(cacheKey, result);
                }
            }

            return result;
        }

        public bool IsModuleParent(Guid moduleID)
        {
            string cacheKey = CachePrefix + "IsParent_" + moduleID;

            var result = DataCache.GetCache<bool?>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    result = ctx.ExecuteScalar<bool?>(System.Data.CommandType.Text, "SELECT 1 FROM dbo.BusinessEngine_Modules WHERE ParentID = @0", moduleID);

                    DataCache.SetCache(cacheKey, result);
                }
            }

            return result != null ? result.Value : false;
        }

        public int? GetModuleTabID(int dnnModuleID)
        {
            return DataContext.Instance().ExecuteScalar<int?>(System.Data.CommandType.Text, "Select Top 1 TabID From TabModules Where ModuleID = @0", dnnModuleID);
        }

        public IEnumerable<ModuleInfo> GetAllModules()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleInfo>();

                return rep.Get();
            }
        }

        public IEnumerable<ModuleInfo> GetScenarioModules(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleInfo>();

                return rep.Get(scenarioID);
            }
        }
    }
}