using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System.Collections;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class GlobalSettingsRepository
    {
        public static GlobalSettingsRepository Instance
        {
            get
            {
                return new GlobalSettingsRepository();
            }
        }

        private const string CachePrefix = "BE_GlobalSettings_";

        public void UpdateSetting(Guid scenarioID, string groupName, string settingName, object settingValue)
        {
            string cacheKey = CachePrefix + scenarioID;

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<GlobalSettingsInfo>();
                var setting = GetSetting(scenarioID, groupName, settingName);
                if (setting != null)
                {
                    setting.SettingValue = settingValue;
                    rep.Update(setting);
                }
                else
                {
                    setting = new GlobalSettingsInfo();
                    setting.SettingID = Guid.NewGuid();
                    setting.ScenarioID = scenarioID;
                    setting.GroupName = groupName;
                    setting.SettingName = settingName;
                    setting.SettingValue = settingValue;
                    rep.Insert(setting);
                }

                DataCache.ClearCache(cacheKey);
            }
        }

        public void DeleteSetting(int settingID)
        {
            GlobalSettingsInfo objSettingInfo = GetSetting(settingID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<GlobalSettingsInfo>();
                rep.Delete(objSettingInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public GlobalSettingsInfo GetSetting(int settingID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<GlobalSettingsInfo>();
                return rep.GetById(settingID);
            }
        }

        public GlobalSettingsInfo GetSetting(Guid scenarioID, string groupName, string settingName)
        {
            var settings = GetScenarioSettings(scenarioID);
            return settings.FirstOrDefault(s => s.GroupName == groupName && s.SettingName == settingName);
        }

        public Hashtable GetSettings(Guid scenarioID, string groupName)
        {
            string cacheKey = CachePrefix + scenarioID;

            var settings = (Hashtable)DataCache.GetCache(cacheKey);

            if (settings == null)
            {
                settings = new Hashtable();

                using (IDataContext ctx = DataContext.Instance())
                {
                    var rep = ctx.GetRepository<GlobalSettingsInfo>();

                    var items = rep.Get(scenarioID).Where(s => s.GroupName == groupName);
                    foreach (var item in items)
                    {
                        settings[item.SettingName] = item.SettingValue;
                    }

                    DataCache.SetCache(cacheKey, settings);
                }
            }

            return settings;
        }

        public IEnumerable<GlobalSettingsInfo> GetScenarioSettings(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<GlobalSettingsInfo>();

                return rep.Get(scenarioID);
            }
        }
    }
}