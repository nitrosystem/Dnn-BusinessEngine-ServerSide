using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class ProviderRepository
    {
        public static ProviderRepository Instance
        {
            get
            {
                return new ProviderRepository();
            }
        }

        private const string CachePrefix = "BE_Providers_";

        public Guid AddProvider(ProviderInfo objProviderInfo)
        {
            objProviderInfo.ProviderID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ProviderInfo>();
                rep.Insert(objProviderInfo);

                DataCache.ClearCache(CachePrefix);

                return objProviderInfo.ProviderID;
            }
        }

        public void UpdateProvider(ProviderInfo objProviderInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ProviderInfo>();
                rep.Update(objProviderInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteProvider(Guid providerID)
        {
            ProviderInfo objProviderInfo = GetProvider(providerID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ProviderInfo>();
                rep.Delete(objProviderInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteProvidersByExtensionID(Guid extensionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ProviderInfo>();
                rep.Delete("Where ExtensionID =@0", extensionID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ProviderInfo GetProvider(Guid providerID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ProviderInfo>();
                return rep.GetById(providerID);
            }
        }

        public IEnumerable<ProviderInfo> GetProviders()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ProviderInfo>();

                return rep.Get();
            }
        }

        public IEnumerable<ProviderInfo> GetProviders(string providerType)
        {
            return GetProviders().Where(p => p.ProviderType == providerType);
        }
    }
}