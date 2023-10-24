using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class ModuleFieldSettingRepository
    {
        public static ModuleFieldSettingRepository Instance
        {
            get
            {
                return new ModuleFieldSettingRepository();
            }
        }

        private const string CachePrefix = "BE_ModuleFieldSettings_";

        public Guid AddFieldSetting(ModuleFieldSettingInfo objFieldSettingInfo)
        {
            objFieldSettingInfo.SettingID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldSettingInfo>();
                rep.Insert(objFieldSettingInfo);

                DataCache.ClearCache(CachePrefix);
                DataCache.ClearCache("MBD_Fields_");

                return objFieldSettingInfo.FieldID;
            }
        }

        public void DeleteFieldSettings(Guid fieldID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldSettingInfo>();
                rep.Delete("Where (FieldID = @0)", fieldID);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public ModuleFieldSettingInfo GetFieldSetting(Guid settingID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldSettingInfo>();
                return rep.GetById(settingID);
            }
        }

        public ModuleFieldSettingInfo GetFieldSetting(Guid fieldID, Guid settingID)
        {
            return GetFieldSettings(fieldID).SingleOrDefault(a => a.SettingID == settingID);
        }

        public ModuleFieldSettingInfo GetFieldSetting(Guid fieldID, string settingName)
        {
            return GetFieldSettings(fieldID).SingleOrDefault(a => a.SettingName == settingName);
        }

        public IEnumerable<ModuleFieldSettingInfo> GetFieldSettings(Guid fieldID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ModuleFieldSettingInfo>();
                return rep.Get(fieldID);
            }
        }
    }
}