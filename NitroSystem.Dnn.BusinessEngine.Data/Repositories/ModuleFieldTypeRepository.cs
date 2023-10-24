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
    public class ModuleFieldTypeRepository
    {
        public static ModuleFieldTypeRepository Instance
        {
            get
            {
                return new ModuleFieldTypeRepository();
            }
        }

        private const string CachePrefix = "BE_ModuleFieldTypes_";

        public Guid AddFieldType(ModuleFieldTypeInfo objFieldTypeInfo)
        {
            objFieldTypeInfo.TypeID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeInfo>();
                rep.Insert(objFieldTypeInfo);

                DataCache.ClearCache(CachePrefix);

                return objFieldTypeInfo.TypeID;
            }
        }

        public void UpdateFieldType(ModuleFieldTypeInfo objFieldTypeInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeInfo>();
                rep.Update(objFieldTypeInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteFieldType(int fieldTypeID)
        {
            ModuleFieldTypeInfo objFieldTypeInfo = GetFieldType(fieldTypeID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeInfo>();
                rep.Delete(objFieldTypeInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteFieldTypesByExtensionID(Guid extensionID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeInfo>();
                rep.Delete("Where ExtensionID =@0", extensionID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ModuleFieldTypeInfo GetFieldType(int fieldTypeID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeInfo>();
                return rep.GetById(fieldTypeID);
            }
        }

        public string GetCustomEvents(string fieldType)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteScalar<string>(System.Data.CommandType.Text, "Select CustomEvents From dbo.BusinessEngine_ModuleFieldTypes Where FieldType = @0", fieldType);
            }
        }

        public ModuleFieldTypeInfo GetFieldTypeByName(string fieldType)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeInfo>();
                var result = rep.Find("Where FieldType = @0", fieldType);
                return result.Any() ? result.First() : null;
            }
        }

        public IEnumerable<ModuleFieldTypeView> GetFieldTypes()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldTypeView>();
                var result = rep.Get().OrderBy(f => f.GroupViewOrder).ThenBy(f => f.ViewOrder);
                return result;
            }
        }
    }
}