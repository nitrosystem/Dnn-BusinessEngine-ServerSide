using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class SkinRepository
    {
        public static SkinRepository Instance
        {
            get
            {
                return new SkinRepository();
            }
        }

        private const string CachePrefix = "BE_Skins_";

        public Guid AddSkin(SkinInfo objSkinInfo)
        {
            objSkinInfo.SkinID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SkinInfo>();
                rep.Insert(objSkinInfo);

                DataCache.ClearCache(CachePrefix);

                return objSkinInfo.SkinID;
            }
        }

        public void UpdateSkin(SkinInfo objSkinInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SkinInfo>();
                rep.Update(objSkinInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteSkin(Guid databaseID)
        {
            SkinInfo objSkinInfo = GetSkin(databaseID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SkinInfo>();
                rep.Delete(objSkinInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteSkinsByExtensionID(Guid extensionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SkinInfo>();
                rep.Delete("Where ExtensionID =@0", extensionID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public SkinInfo GetSkin(Guid databaseID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SkinInfo>();
                return rep.GetById(databaseID);
            }
        }

        public SkinInfo GetSkin(string skinName)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SkinInfo>();
                var result = rep.Find("Where SkinName = @0", skinName);
                return result.Any() ? result.First() : null;
            }
        }

        public IEnumerable<SkinInfo> GetSkins()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SkinInfo>();

                return rep.Get();
            }
        }
    }
}