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
    public class ServiceTypeRepository
    {
        public static ServiceTypeRepository Instance
        {
            get
            {
                return new ServiceTypeRepository();
            }
        }

        private const string CachePrefix = "BE_ServiceTypes_";

        public Guid AddServiceType(ServiceTypeInfo objServiceTypeInfo)
        {
            objServiceTypeInfo.TypeID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceTypeInfo>();
                rep.Insert(objServiceTypeInfo);

                DataCache.ClearCache(CachePrefix);

                return objServiceTypeInfo.TypeID;
            }
        }

        public void UpdateServiceType(ServiceTypeInfo objServiceTypeInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceTypeInfo>();
                rep.Update(objServiceTypeInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteServiceType(Guid serviceTypeID)
        {
            ServiceTypeInfo objServiceTypeInfo = GetServiceType(serviceTypeID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceTypeInfo>();
                rep.Delete(objServiceTypeInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteServiceTypesByExtensionID(Guid extensionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceTypeInfo>();
                rep.Delete("Where ExtensionID =@0", extensionID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ServiceTypeInfo GetServiceType(Guid serviceTypeID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceTypeInfo>();
                return rep.GetById(serviceTypeID);
            }
        }

        public ServiceTypeInfo GetServiceTypeByName(string serviceSubtype)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceTypeInfo>();
                var result = rep.Find("Where ServiceSubtype = @0", serviceSubtype);
                return result.Any() ? result.First() : null;
            }
        }

        public string GetServiceTypeIcon(string serviceType)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteScalar<string>(System.Data.CommandType.Text, "Select Icon From dbo.BusinessEngine_ServiceTypes Where ServiceType = @0", serviceType);
            }
        }

        public IEnumerable<ServiceTypeView> GetServiceTypes(string groupType)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceTypeView>();
                return rep.Get(groupType);
            }
        }

        public IEnumerable<ServiceTypeView> GetServiceTypes()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceTypeView>();
                return rep.Get().OrderBy(st => st.GroupViewOrder).ThenBy(st => st.ViewOrder);
            }
        }
    }
}