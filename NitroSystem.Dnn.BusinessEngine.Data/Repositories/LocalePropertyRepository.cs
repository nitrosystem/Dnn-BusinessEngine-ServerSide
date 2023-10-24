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
    public class LocalePropertyRepository
    {
        public static LocalePropertyRepository Instance
        {
            get
            {
                return new LocalePropertyRepository();
            }
        }

        private const string CachePrefix = "BE_LocaleProperties_";

        public Guid AddLocaleProperty(LocalePropertyInfo objLocalePropertyInfo)
        {
            objLocalePropertyInfo.PropertyID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LocalePropertyInfo>();
                rep.Insert(objLocalePropertyInfo);

                DataCache.ClearCache(CachePrefix);

                return objLocalePropertyInfo.PropertyID;
            }
        }

        public void UpdateLocaleProperty(LocalePropertyInfo objLocalePropertyInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LocalePropertyInfo>();
                rep.Update(objLocalePropertyInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteLocaleProperty(Guid propertyID)
        {
            LocalePropertyInfo objLocalePropertyInfo = GetLocaleProperty(propertyID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LocalePropertyInfo>();
                rep.Delete(objLocalePropertyInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public LocalePropertyInfo GetLocaleProperty(Guid propertyID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LocalePropertyInfo>();
                return rep.GetById(propertyID);
            }
        }

        public IEnumerable<LocalePropertyInfo> GetLocalePropertiesByGroupName(Guid scenarioID, string groupName)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LocalePropertyInfo>();
                return rep.Get(scenarioID).Where(p => p.GroupName == groupName);
            }
        }

        public IEnumerable<LocalePropertyInfo> GetLocaleProperties(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LocalePropertyInfo>();

                return rep.Get(scenarioID);
            }
        }
    }
}