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
    public class ModuleFieldTypeTemplateRepository
    {
        public static ModuleFieldTypeTemplateRepository Instance
        {
            get
            {
                return new ModuleFieldTypeTemplateRepository();
            }
        }

        private const string CachePrefix = "BE_ModuleFieldTypeTemplates_";

        public Guid AddTemplate(ModuleFieldTypeTemplateInfo objModuleFieldTypeTemplate)
        {
            objModuleFieldTypeTemplate.TemplateID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeTemplateInfo>();
                rep.Insert(objModuleFieldTypeTemplate);

                DataCache.ClearCache(CachePrefix);

                return objModuleFieldTypeTemplate.TemplateID;
            }
        }

        public void UpdateTemplate(ModuleFieldTypeTemplateInfo objModuleFieldTypeTemplate)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeTemplateInfo>();
                rep.Update(objModuleFieldTypeTemplate);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteTemplate(int templateID)
        {
            ModuleFieldTypeTemplateInfo objModuleFieldTypeTemplate = GetTemplate(templateID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeTemplateInfo>();
                rep.Delete(objModuleFieldTypeTemplate);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ModuleFieldTypeTemplateInfo GetTemplate(int templateID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeTemplateInfo>();
                return rep.GetById(templateID);
            }
        }

        public IEnumerable<ModuleFieldTypeTemplateInfo> GetTemplates(string fieldType)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeTemplateInfo>();
                var result = rep.Get(fieldType).OrderBy(t => t.ViewOrder);
                return result;
            }
        }
    }
}