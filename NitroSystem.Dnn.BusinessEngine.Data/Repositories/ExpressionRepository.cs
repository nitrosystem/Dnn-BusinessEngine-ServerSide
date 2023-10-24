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
    public class ExpressionRepository
    {
        public static ExpressionRepository Instance
        {
            get
            {
                return new ExpressionRepository();
            }
        }

        private const string CachePrefix = "BE_Expressions_";

        public Guid AddExpression(ExpressionItemInfo  objExpressionInfo)
        {
            objExpressionInfo.ItemID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ExpressionItemInfo>();
                rep.Insert(objExpressionInfo);

                DataCache.ClearCache(CachePrefix);

                return objExpressionInfo.ItemID;
            }
        }

        public void UpdateExpression(ExpressionItemInfo objExpressionInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ExpressionItemInfo>();
                rep.Update(objExpressionInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteExpression(Guid expressionID)
        {
            ExpressionItemInfo objExpressionInfo = GetExpression(expressionID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ExpressionItemInfo>();
                rep.Delete(objExpressionInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ExpressionItemInfo GetExpression(Guid expressionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ExpressionItemInfo>();
                return rep.GetById(expressionID);
            }
        }

        public IEnumerable<ExpressionItemInfo> GetExpressions(Guid typeID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ExpressionItemInfo>();
                return rep.Get(typeID);
            }
        }
    }
}