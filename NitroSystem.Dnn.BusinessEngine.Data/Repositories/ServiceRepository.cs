using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class ServiceRepository
    {
        public static ServiceRepository Instance
        {
            get
            {
                return new ServiceRepository();
            }
        }

        private const string CachePrefix = "BE_Services_";

        public Guid AddService(ServiceInfo objServiceInfo)
        {
            objServiceInfo.ServiceID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceInfo>();
                rep.Insert(objServiceInfo);

                DataCache.ClearCache(CachePrefix);

                return objServiceInfo.ServiceID;
            }
        }

        public void UpdateService(ServiceInfo objServiceInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceInfo>();
                rep.Update(objServiceInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void UpdateGroup(Guid itemID, Guid? groupID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                ctx.Execute(System.Data.CommandType.Text, "UPDATE dbo.BusinessEngine_Services SET GroupID = @0 WHERE ServiceID = @1", groupID, itemID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteService(Guid serviceID)
        {
            ServiceInfo objServiceInfo = GetService(serviceID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceInfo>();
                rep.Delete(objServiceInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ServiceInfo GetService(Guid serviceID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceInfo>();
                return rep.GetById(serviceID);
            }
        }

        public ServiceInfo GetServiceByName(Guid scenarioID, string serviceName, bool isEnabled)
        {
            return GetServices().FirstOrDefault(s => s.ScenarioID == scenarioID && s.ServiceName == serviceName && (!isEnabled || s.IsEnabled));
        }

        public ServiceInfo GetServiceByID(int portalid, Guid serviceID, bool isEnabled)
        {
            return GetServices().FirstOrDefault(t => t.ServiceID == serviceID && (!isEnabled || t.IsEnabled));
        }

        public IEnumerable<ServiceInfo> GetServices(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceInfo>();

                return rep.Get(scenarioID);
            }
        }

        public IEnumerable<ServiceInfo> GetServices(string serviceType = "")
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceInfo>();

                return rep.Find("Where @0 = '' or ServiceType = @0", serviceType).OrderBy(s => s.ViewOrder);
            }
        }
    }
}