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
    public class ExplorerItemRepository
    {
        public static ExplorerItemRepository Instance
        {
            get
            {
                return new ExplorerItemRepository();
            }
        }

        public IEnumerable<ExplorerItemView> GetItems(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ExplorerItemView>();

                var result = rep.Get(scenarioID).OrderBy(i => i.ViewOrder);

                return result;
            }
        }

    }
}