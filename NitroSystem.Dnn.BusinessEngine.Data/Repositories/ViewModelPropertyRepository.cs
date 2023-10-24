using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class ViewModelPropertyRepository
    {
        public static ViewModelPropertyRepository Instance
        {
            get
            {
                return new ViewModelPropertyRepository();
            }
        }

        private const string CachePrefix = "BE_ViewModelProperties_";

        public Guid AddProperty(ViewModelPropertyInfo objViewModelPropertyInfo)
        {
            objViewModelPropertyInfo.PropertyID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ViewModelPropertyInfo>();
                rep.Insert(objViewModelPropertyInfo);

                DataCache.ClearCache(CachePrefix);

                return objViewModelPropertyInfo.PropertyID;
            }
        }

        public void UpdateProperty(ViewModelPropertyInfo objViewModelPropertyInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ViewModelPropertyInfo>();
                rep.Update(objViewModelPropertyInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteProperty(Guid propertyID)
        {
            ViewModelPropertyInfo objViewModelPropertyInfo = GetProperty(propertyID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ViewModelPropertyInfo>();
                rep.Delete(objViewModelPropertyInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteProperties(IEnumerable<Guid> propertyIDs)
        {
            if (propertyIDs != null && propertyIDs.Any())
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<ViewModelPropertyInfo>();
                    rep.Delete("Where PropertyID in (@0)", string.Join(",", propertyIDs));
                }

                DataCache.ClearCache(CachePrefix);
            }
        }

        public ViewModelPropertyInfo GetProperty(Guid propertyID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ViewModelPropertyInfo>();
                return rep.GetById(propertyID);
            }
        }

        public IEnumerable<ViewModelPropertyInfo> GetProperties(Guid viewModelID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ViewModelPropertyInfo>();

                return rep.Get(viewModelID).OrderBy(c => c.ViewOrder);
            }
        }
    }
}