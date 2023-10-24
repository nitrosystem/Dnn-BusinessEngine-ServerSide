using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class ExtensionRepository
    {
        public static ExtensionRepository Instance
        {
            get
            {
                return new ExtensionRepository();
            }
        }

        private const string CachePrefix = "BE_Extensions_";

        public Guid AddExtension(ExtensionInfo objExtensionInfo)
        {
            objExtensionInfo.ExtensionID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ExtensionInfo>();
                rep.Insert(objExtensionInfo);

                DataCache.ClearCache(CachePrefix);

                return objExtensionInfo.ExtensionID;
            }
        }

        public void UpdateExtension(ExtensionInfo objExtensionInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ExtensionInfo>();
                rep.Update(objExtensionInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteExtension(Guid extensionID)
        {
            ExtensionInfo objExtensionInfo = GetExtension(extensionID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ExtensionInfo>();
                rep.Delete(objExtensionInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ExtensionInfo GetExtension(Guid extensionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ExtensionInfo>();
                return rep.GetById(extensionID);
            }
        }

        public IEnumerable<ExtensionInfo> GetExtensions()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ExtensionInfo>();

                return rep.Get();
            }
        }

        public ExtensionInfo GetExtensionByName(string extensionName)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ExtensionInfo>();

                var result = rep.Find("Where ExtensionName = @0", extensionName);
                return result.Any() ? result.First() : null;
            }
        }

        public string GetExtensionVersion(string extensionName)
        {
            return (GetExtensionByName(extensionName) ?? new ExtensionInfo()).Version;
        }

        public IEnumerable<ExtensionInfo> GetExtensions(string extensionType)
        {
            return GetExtensions();
        }   
    }
}