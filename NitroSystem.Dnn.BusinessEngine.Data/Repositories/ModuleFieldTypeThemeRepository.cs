using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class ModuleFieldTypeThemeRepository
    {
        public static ModuleFieldTypeThemeRepository Instance
        {
            get
            {
                return new ModuleFieldTypeThemeRepository();
            }
        }

        private const string CachePrefix = "BE_ModuleFieldTypeThemes_";

        public Guid AddTheme(ModuleFieldTypeThemeInfo objModuleFieldTypeTheme)
        {
            objModuleFieldTypeTheme.ThemeID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeThemeInfo>();
                rep.Insert(objModuleFieldTypeTheme);

                DataCache.ClearCache(CachePrefix);

                return objModuleFieldTypeTheme.ThemeID;
            }
        }

        public void UpdateTheme(ModuleFieldTypeThemeInfo objModuleFieldTypeTheme)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeThemeInfo>();
                rep.Update(objModuleFieldTypeTheme);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteTheme(int themeID)
        {
            ModuleFieldTypeThemeInfo objModuleFieldTypeTheme = GetTheme(themeID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeThemeInfo>();
                rep.Delete(objModuleFieldTypeTheme);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ModuleFieldTypeThemeInfo GetTheme(int themeID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeThemeInfo>();
                return rep.GetById(themeID);
            }
        }

        public IEnumerable<ModuleFieldTypeThemeInfo> GetThemes(string fieldType)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeThemeInfo>();
                var result = rep.Get(fieldType).OrderBy(t => t.ViewOrder);
                return result;
            }
        }

        public IEnumerable<string> GetFieldsThemeCss(string modules)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteQuery<string>(System.Data.CommandType.Text, "Select Distinct ThemeCssPath From dbo.BusinessEngine_ModuleFieldTypeThemes t inner join dbo.BusinessEngine_ModuleFields f on t.FieldType=f.FieldType Where t.ThemeCssPath is not null and f.ModuleID in (Select [RowValue] From dbo.ConvertListToTable(',',@0))", modules);
            }
        }

        public string GetThemeCssClass(string fieldType, string theme)
        {
            string cacheKey = CachePrefix + fieldType + "_" + theme;

            var result = DataCache.GetCache<string>(cacheKey);
            if (result == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    result = ctx.ExecuteScalar<string>(System.Data.CommandType.Text, "Select Distinct Top 1 ThemeCssClass From dbo.BusinessEngine_ModuleFieldTypeThemes Where FieldType = @0 and ThemeName = @1", fieldType, theme);

                    DataCache.SetCache(cacheKey, result);
                }
            }

            return result;
        }
    }
}