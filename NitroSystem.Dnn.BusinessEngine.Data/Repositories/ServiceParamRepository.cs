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
    public class ServiceParamRepository
    {
        public static ServiceParamRepository Instance
        {
            get
            {
                return new ServiceParamRepository();
            }
        }

        private const string CachePrefix = "BE_ServiceParams_";

        public Guid AddParam(ServiceParamInfo objServiceParamInfo)
        {
            objServiceParamInfo.ParamID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceParamInfo>();
                rep.Insert(objServiceParamInfo);

                DataCache.ClearCache(CachePrefix);

                return objServiceParamInfo.ParamID;
            }
        }

        public void DeleteParams(Guid serviceID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceParamInfo>();

                rep.Delete("Where ServiceID = @0", serviceID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public IEnumerable<ServiceParamInfo> GetParams(Guid serviceID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ServiceParamInfo>();

                return rep.Get(serviceID);
            }
        }
    }
}