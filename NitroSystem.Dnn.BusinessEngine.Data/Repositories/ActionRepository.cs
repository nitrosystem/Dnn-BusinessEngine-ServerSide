using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class ActionRepository
    {
        public static ActionRepository Instance
        {
            get
            {
                return new ActionRepository();
            }
        }

        private const string CachePrefix = "BE_Actions_";
        private const string FieldCachePrefix = "BE_ModuleFields_";

        public Guid AddAction(ActionInfo objActionInfo)
        {
            objActionInfo.ActionID = Guid.NewGuid();

            objActionInfo.ParentID = objActionInfo.ParentID != Guid.Empty ? objActionInfo.ParentID : null;
            objActionInfo.FieldID = objActionInfo.FieldID != Guid.Empty ? objActionInfo.FieldID : null;

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionInfo>();
                rep.Insert(objActionInfo);

                DataCache.ClearCache(CachePrefix);
                DataCache.ClearCache(FieldCachePrefix);

                return objActionInfo.ActionID;
            }
        }

        public void UpdateAction(ActionInfo objActionInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionInfo>();
                rep.Update(objActionInfo);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache(FieldCachePrefix);
        }

        public void UpdateGroup(Guid itemID, Guid? groupID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                ctx.Execute(System.Data.CommandType.Text, "UPDATE dbo.BusinessEngine_Actions SET GroupID = @0 WHERE ViewModelID = @1", groupID, itemID);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache(FieldCachePrefix);
        }

        public void DeleteAction(Guid actionID)
        {
            ActionInfo objActionInfo = GetAction(actionID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionInfo>();
                rep.Delete(objActionInfo);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache(FieldCachePrefix);
        }

        public ActionInfo GetAction(Guid actionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionInfo>();
                return rep.GetById(actionID);
            }
        }

        public ActionInfo GetAction(string actionName)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionInfo>();
                var result = rep.Find("Where ActionName = @0", actionName);

                return result.Any() ? result.First() : null;
            }
        }

        public IEnumerable<ActionInfo> GetActions(Guid moduleID)
        {
            string cacheKey = CachePrefix + moduleID;

            var result = DataCache.GetCache<IEnumerable<ActionInfo>>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<ActionInfo>();

                    result = rep.Get(moduleID);
                }

                DataCache.SetCache(cacheKey, result);
            }

            return result;
        }

        public IEnumerable<ActionInfo> GetServerActions(Guid moduleID)
        {
            string cacheKey = CachePrefix + moduleID + "_ServerSide";

            var result = DataCache.GetCache<IEnumerable<ActionInfo>>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<ActionInfo>();

                    result = rep.Find("Where ModuleID = @0 and IsServerSide = 1", moduleID);
                }

                DataCache.SetCache(cacheKey, result);
            }

            return result;
        }

        public IEnumerable<ActionInfo> GetActions(Guid moduleID, Guid? fieldID = null)
        {
            string cacheKey = CachePrefix + moduleID + fieldID;

            var result = DataCache.GetCache<IEnumerable<ActionInfo>>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<ActionInfo>();

                    result = rep.Find("Where ModuleID = @0 and ((FieldID is not null and FieldID = @1) or (FieldID is null and @1 is null)) Order By ViewOrder", moduleID, fieldID);
                }

                DataCache.SetCache(cacheKey, result);
            }

            return result;
        }

        public IEnumerable<ActionInfo> GetFieldActions(Guid fieldID)
        {
            string cacheKey = CachePrefix + "Field_" + fieldID;

            var result = DataCache.GetCache<IEnumerable<ActionInfo>>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<ActionInfo>();

                    result = rep.Find("Where FieldID = @0 Order By ViewOrder", fieldID);
                }

                DataCache.SetCache(cacheKey, result);
            }

            return result;
        }

        public IEnumerable<ActionInfo> GetActions(Guid moduleID, Guid? fieldID, string eventName, bool? isServerSide)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionInfo>();

                return rep.Find("Where ModuleID = @0 and (@1 is null or FieldID = @1) and Event = @2 and (@3 is null or IsServerSide= @3) Order By ViewOrder", moduleID, fieldID, eventName, isServerSide).OrderBy(a => a.ViewOrder);
            }
        }

        public IEnumerable<ActionInfo> GetActions(Guid parentID, byte parentResultStatus, bool isServerSide)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionInfo>();

                return rep.Find("Where ParentID = @0 and ParentResultStatus = @1 and IsServerSide= @2 Order By ViewOrder", parentID, parentResultStatus, isServerSide).OrderBy(a => a.ViewOrder); ;
            }
        }

        public IEnumerable<string> GetActionTypes(string modules)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteQuery<string>(System.Data.CommandType.Text, "Select Distinct ActionType From dbo.BusinessEngine_Actions Where IsNull(IsServerSide,0) = 0 and ModuleID in (Select [RowValue] From dbo.ConvertListToTable(',',@0))", modules);
            }
        }

        public IEnumerable<ActionInfo> GetModuleActions(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionInfo>();
                return rep.Get(moduleID);
            }
        }
    }
}