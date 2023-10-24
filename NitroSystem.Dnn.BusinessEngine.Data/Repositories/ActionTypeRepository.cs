using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using DotNetNuke.Entities.Urls;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class ActionTypeRepository
    {
        public static ActionTypeRepository Instance
        {
            get
            {
                return new ActionTypeRepository();
            }
        }

        private const string CachePrefix = "BE_ActionTypes_";

        public Guid AddActionType(ActionTypeInfo objActionTypeInfo)
        {
            objActionTypeInfo.TypeID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionTypeInfo>();
                rep.Insert(objActionTypeInfo);

                DataCache.ClearCache(CachePrefix);

                return objActionTypeInfo.TypeID;
            }
        }

        public void UpdateActionType(ActionTypeInfo objActionTypeInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionTypeInfo>();
                rep.Update(objActionTypeInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteActionType(Guid actionTypeID)
        {
            ActionTypeInfo objActionTypeInfo = GetActionType(actionTypeID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionTypeInfo>();
                rep.Delete(objActionTypeInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteActionTypesByExtensionID(Guid extensionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionTypeInfo>();
                rep.Delete("Where ExtensionID =@0", extensionID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ActionTypeInfo GetActionType(Guid actionTypeID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionTypeInfo>();
                return rep.GetById(actionTypeID);
            }
        }

        public string GetActionTypeIcon(string actionType)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteScalar<string>(System.Data.CommandType.Text, "Select Icon From dbo.BusinessEngine_ActionTypes Where ActionType = @0", actionType);
            }
        }

        public ActionTypeInfo GetActionTypeByName(string actionType)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionTypeInfo>();
                var result = rep.Find("Where ActionType = @0", actionType);
                return result.Any() ? result.First() : null;
            }
        }

        public IEnumerable<ActionTypeView> GetActionTypes()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ActionTypeView>();

                return rep.Get().OrderBy(a => a.GroupViewOrder).ThenBy(a => a.ViewOrder);
            }
        }
    }
}