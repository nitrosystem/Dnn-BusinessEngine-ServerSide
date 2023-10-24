using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class ModuleFieldRepository
    {
        public static ModuleFieldRepository Instance
        {
            get
            {
                return new ModuleFieldRepository();
            }
        }

        private const string CachePrefix = "BE_ModuleFields_";
        private const string ActionsCachePrefix = "BE_Actions_";

        public Guid AddField(ModuleFieldInfo objFieldInfo)
        {
            objFieldInfo.FieldID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldInfo>();
                rep.Insert(objFieldInfo);

                DataCache.ClearCache(CachePrefix);
                DataCache.ClearCache(ActionsCachePrefix);

                return objFieldInfo.FieldID;
            }
        }

        public void UpdateField(ModuleFieldInfo objFieldInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldInfo>();
                rep.Update(objFieldInfo);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache(ActionsCachePrefix);
        }

        public void DeleteField(Guid fieldID)
        {
            ModuleFieldInfo objFieldInfo = GetField(fieldID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldInfo>();
                rep.Delete(objFieldInfo);
            }

            DataCache.ClearCache(CachePrefix);
            DataCache.ClearCache(ActionsCachePrefix);
        }

        public ModuleFieldInfo GetField(Guid fieldID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldInfo>();
                return rep.GetById(fieldID);
            }
        }

        public string GetFieldName(Guid fieldID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteScalar<string>(System.Data.CommandType.Text, "Select FieldName From dbo.BusinessEngine_ModuleFields Where FieldID = @0", fieldID);
            }
        }

        public string GetFieldType(Guid fieldID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteScalar<string>(System.Data.CommandType.Text, "Select FieldType From dbo.BusinessEngine_ModuleFields Where FieldID = @0", fieldID);
            }
        }

        public Guid GetModuleID(Guid fieldID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteScalar<Guid>(System.Data.CommandType.Text, "Select ModuleID From dbo.BusinessEngine_ModuleFields Where FieldID = @0", fieldID);
            }
        }

        public IEnumerable<ModuleFieldInfo> GetFields(Guid moduleID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldInfo>();
                return rep.Get(moduleID).OrderBy(f => f.ViewOrder);
            }
        }

        public IEnumerable<string> GetModulesFieldTypes(string modules)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteQuery<string>(System.Data.CommandType.Text, "Select Distinct FieldType From dbo.BusinessEngine_ModuleFields Where ModuleID in (Select [RowValue] From dbo.ConvertListToTable(',',@0))", modules);
            }
        }

        public IEnumerable<string> GetModulesStyles(string modules)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteQuery<string>(System.Data.CommandType.Text, "Select Distinct LayoutCss From dbo.BusinessEngine_Modules Where LayoutCss is not null and ModuleID in (Select [RowValue] From dbo.ConvertListToTable(',',@0))", modules);
            }
        }

        public IEnumerable<string> GetModulesFieldsCss(string modules)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteQuery<string>(System.Data.CommandType.Text, "Select Distinct CssPath From dbo.BusinessEngine_ModuleFieldTypeTemplates t inner join dbo.BusinessEngine_ModuleFields f on t.FieldType=f.FieldType Where t.CssPath is not null and f.ModuleID in (Select [RowValue] From dbo.ConvertListToTable(',',@0))", modules);
            }
        }
    }
}