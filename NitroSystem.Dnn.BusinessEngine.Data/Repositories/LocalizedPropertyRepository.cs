using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System.Collections;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class LocalizedPropertyRepository
    {
        public static LocalizedPropertyRepository Instance
        {
            get
            {
                return new LocalizedPropertyRepository();
            }
        }

        private const string CachePrefix = "BE_LocalizedProperties_";

        public int AddLocalizedProperty(LocalizedPropertyInfo objLocalizedPropertyInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LocalizedPropertyInfo>();
                rep.Insert(objLocalizedPropertyInfo);

                DataCache.ClearCache(CachePrefix);

                return objLocalizedPropertyInfo.ItemID;
            }
        }

        public void UpdateLocalizedProperty(LocalizedPropertyInfo objLocalizedPropertyInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LocalizedPropertyInfo>();
                rep.Update(objLocalizedPropertyInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteLocalizedProperty(int itemID)
        {
            LocalizedPropertyInfo objLocalizedPropertyInfo = GetLocalizedProperty(itemID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LocalizedPropertyInfo>();
                rep.Delete(objLocalizedPropertyInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteLocalizedProperties(string language, string groupName, int entityID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LocalizedPropertyInfo>();

                rep.Delete("Where Language = @0 and LocaleKeyGroup = @1 and EntityID = @2", language, groupName, entityID);
            }
        }

        public LocalizedPropertyInfo GetLocalizedProperty(int itemID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LocalizedPropertyInfo>();
                return rep.GetById(itemID);
            }
        }

        public IEnumerable<LocalizedPropertyInfo> GetLocalizedProperties(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LocalizedPropertyInfo>();

                return rep.Get(scenarioID);
            }
        }

        public IEnumerable<LocalizedPropertyInfo> GetLocalizedProperties(string language,string groupName, int entityID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LocalizedPropertyInfo>();

                return rep.Find("Where Language = @0 and LocaleKeyGroup = @1 and EntityID = @2", language, groupName, entityID);
            }
        }
    }
}