using DotNetNuke.Common.Utilities;
using Newtonsoft.Json;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Core.Appearance
{
    public static class ModuleSkinManager
    {
        public static IEnumerable<ModuleSkinInfo> GetSkins()
        {
            string cachePrefix = "BE_Skins_Items";

            var result = DataCache.GetCache<List<ModuleSkinInfo>>(cachePrefix);
            if (result == null)
            {
                result = new List<ModuleSkinInfo>();

                var skins = SkinRepository.Instance.GetSkins();
                foreach (var objSkinInfo in skins)
                {
                    result.Add(GetSkin(Guid.Empty, string.Empty, null, objSkinInfo));
                }

                DataCache.SetCache(cachePrefix, result);
            }

            return result;
        }

        public static ModuleSkinInfo GetSkin(Guid moduleID, string moduleType, Guid? parentID, SkinInfo objSkinInfo)
        {
            if (objSkinInfo == null) return null;

            string cachePrefix = "BE_Skins_Item_" + objSkinInfo.SkinName;

            var result = DataCache.GetCache<ModuleSkinInfo>(cachePrefix);
            if (result == null)
            {
                string path = objSkinInfo.SkinPath.Replace("[ModulePath]", "~/DesktopModules/BusinessEngine");
                string skinJson = FileUtil.GetFileContent(HttpContext.Current.Request.MapPath(path + "/skin.json"));
                skinJson = skinJson.Replace("[ModulePath]", "/DesktopModules/BusinessEngine");

                result = JsonConvert.DeserializeObject<ModuleSkinInfo>(skinJson);
                result.SkinPath = objSkinInfo.SkinPath.Replace("[ModulePath]", "/DesktopModules/BusinessEngine");
                if (moduleType == "Dashboard")
                {
                    var dashboardType = DashboardRepository.Instance.GetDashboardType(moduleID);
                    result.DashboardTemplates = result.DashboardTemplates.Where(t => (dashboardType == 1 && t.DashboardType == "Standalone") || (dashboardType == 2 && t.DashboardType == "Dnn"));
                }

                else if (moduleType != "Dashboard")
                {
                    result.FormTemplates = result.FormTemplates.Where(t => (!t.IsDashboardTemplate && parentID == null) || (t.IsDashboardTemplate && parentID != null));
                    result.ListTemplates = result.ListTemplates.Where(t => (!t.IsDashboardTemplate && parentID == null) || (t.IsDashboardTemplate && parentID != null));
                    result.DetailsTemplates = result.DetailsTemplates.Where(t => (!t.IsDashboardTemplate && parentID == null) || (t.IsDashboardTemplate && parentID != null));
                }

                DataCache.SetCache(cachePrefix, result);
            }

            return result;
        }

        public static ModuleSkinInfo GetSkin(Guid moduleID, string moduleType, Guid? parentID, string skinName)
        {
            if (string.IsNullOrEmpty(skinName)) return null;

            SkinInfo objSkinInfo = SkinRepository.Instance.GetSkin(skinName);
            return GetSkin(moduleID, moduleType, parentID, objSkinInfo);
        }
    }
}