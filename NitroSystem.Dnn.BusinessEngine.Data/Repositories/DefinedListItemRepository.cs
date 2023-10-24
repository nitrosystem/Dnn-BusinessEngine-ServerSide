using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class DefinedListItemRepository
    {
        public static DefinedListItemRepository Instance
        {
            get
            {
                return new DefinedListItemRepository();
            }
        }

        private const string CachePrefix = "BE_DefinedListItems_";
        private const string FieldCachePrefix = "BE_ModuleFields_";

        public Guid AddItem(DefinedListItemInfo objDefinedListItemInfo)
        {
            if (objDefinedListItemInfo.ItemID == Guid.Empty) objDefinedListItemInfo.ItemID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListItemInfo>();
                rep.Insert(objDefinedListItemInfo);

                DataCache.ClearCache(CachePrefix);
                DataCache.ClearCache(FieldCachePrefix);

                return objDefinedListItemInfo.ItemID;
            }
        }

        public void UpdateItem(DefinedListItemInfo objDefinedListItemInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListItemInfo>();
                rep.Update(objDefinedListItemInfo);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache(FieldCachePrefix);
        }

        public void DeleteItem(Guid listID)
        {
            DefinedListItemInfo objDefinedListItemInfo = GetItem(listID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListItemInfo>();
                rep.Delete(objDefinedListItemInfo);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache(FieldCachePrefix);
        }

        public DefinedListItemInfo GetItem(Guid listID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListItemInfo>();
                return rep.GetById(listID);
            }
        }

        public IEnumerable<DefinedListItemInfo> GetItems(Guid listID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListItemInfo>();

                return rep.Get(listID).OrderBy(i => i.ViewOrder);
            }
        }

        public IEnumerable<DefinedListItemInfo> GetItemsWithFilters(Guid listID, string filters)
        {
            if (string.IsNullOrEmpty(filters)) filters = "1=1";

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListItemInfo>();

                return rep.Find(string.Format("Where ListID = '{0}' and {1}", listID, filters)).OrderBy(i => i.ViewOrder); ;
            }
        }

        public IEnumerable<DefinedListItemInfo> GetAllItems()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<DefinedListItemInfo>();

                return rep.Get();
            }
        }
    }
}