using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class ViewModelRepository
    {
        public static ViewModelRepository Instance
        {
            get
            {
                return new ViewModelRepository();
            }
        }

        private const string CachePrefix = "BE_ViewModels_";

        public Guid AddViewModel(ViewModelInfo objViewModelInfo)
        {
            objViewModelInfo.ViewModelID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ViewModelInfo>();
                rep.Insert(objViewModelInfo);

                DataCache.ClearCache(CachePrefix);

                return objViewModelInfo.ViewModelID;
            }
        }

        public void UpdateViewModel(ViewModelInfo objViewModelInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ViewModelInfo>();
                rep.Update(objViewModelInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void UpdateGroup(Guid itemID, Guid? groupID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                ctx.Execute(System.Data.CommandType.Text, "UPDATE dbo.BusinessEngine_ViewModels SET GroupID = @0 WHERE ViewModelID = @1", groupID, itemID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteViewModel(Guid viewModelID)
        {
            ViewModelInfo objViewModelInfo = GetViewModel(viewModelID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ViewModelInfo>();
                rep.Delete(objViewModelInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ViewModelInfo GetViewModel(Guid viewModelID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ViewModelInfo>();
                return rep.GetById(viewModelID);
            }
        }

        public IEnumerable<ViewModelInfo> GetViewModels(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ViewModelInfo>();

                return rep.Get(scenarioID).OrderBy(v => v.ViewOrder);
            }
        }

        public IEnumerable<ViewModelInfo> GetViewModels()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ViewModelInfo>();

                return rep.Get().OrderBy(v => v.ViewOrder);
            }
        }
    }
}