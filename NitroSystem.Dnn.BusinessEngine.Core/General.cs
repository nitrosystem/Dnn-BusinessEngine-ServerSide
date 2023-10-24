using DotNetNuke.Common.Utilities;
using NitroSystem.Dnn.BusinessEngine.Common.Models;
using NitroSystem.Dnn.BusinessEngine.Common.TypeCasting;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Contracts;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core
{
    public static class General
    {
        public static ExpressionInfo CastExpressionInfo<T>(T property)
        {
            ExpressionInfo result = new ExpressionInfo()
            {
                LeftExpression = property.GetType().GetProperty("LeftExpression").GetValue(property, null) as string,
                EvalType = property.GetType().GetProperty("EvalType").GetValue(property, null) as string,
                RightExpression = property.GetType().GetProperty("RightExpression").GetValue(property, null) as string,
                GroupName = property.GetType().GetProperty("GroupName").GetValue(property, null) as string,
                ExpressionParsingType = property.GetType().GetProperty("ExpressionParsingType").GetValue(property, null) as string,
                ViewOrder = property.GetType().GetProperty("ViewOrder").GetValue(property, null) == null ? 0 :
                    (property.GetType().GetProperty("ViewOrder").GetValue(property, null) as int?).Value,
            };

            return result;
        }

        public static IEnumerable<ServiceParamInfo> GetSpParams(string spName)
        {
            var result = DbUtil.GetSpParams("dbo", spName) ?? Enumerable.Empty<SpParamInfo>();

            var serviceParams = result.OrderBy(p => p.ViewOrder).Select(p => new ServiceParamInfo() { ParamName = p.ParamName, ParamType = p.ParamType });

            return serviceParams;
        }

        public static string GetSpScript(string spName)
        {
            var connection = new System.Data.SqlClient.SqlConnection(DotNetNuke.Data.DataProvider.Instance().ConnectionString);

            var result = Dapper.SqlMapper.QuerySingle<string>(connection, string.Format("SELECT [Definition] FROM sys.sql_modules WHERE objectproperty(OBJECT_ID, 'IsProcedure') = 1 AND OBJECT_NAME(OBJECT_ID) = '{0}'", spName));

            return result;
        }

        public static void SaveServiceParams(Guid serviceID, IEnumerable<ServiceParamInfo> paramList)
        {
            ServiceParamRepository.Instance.DeleteParams(serviceID);

            foreach (var objServiceParamInfo in paramList ?? Enumerable.Empty<ServiceParamInfo>())
            {
                objServiceParamInfo.ServiceID = serviceID;

                ServiceParamRepository.Instance.AddParam(objServiceParamInfo);
            }
        }
    }
}
