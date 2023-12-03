using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DotNetNuke.Services.Installer.Log;
using ICSharpCode.SharpZipLib.Zip;
using DotNetNuke.Common;
using DotNetNuke.Services.Localization;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using DotNetNuke.Entities.Content.Workflow.Entities;
using DotNetNuke.Entities.Content.Workflow;
using DotNetNuke.Services.FileSystem.Internal;
using DotNetNuke.Services.Log.EventLog;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using Newtonsoft.Json;
using NitroSystem.Dnn.BusinessEngine.Core.Extensions.Manifest;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using Newtonsoft.Json.Linq;
using System.Security.Policy;
using DotNetNuke.Entities.Users;
using System.Web;
using System.Resources;

namespace NitroSystem.Dnn.BusinessEngine.Core.Extensions.Services
{
    public class ExtensionService
    {
        private readonly Guid _scenarioID;
        private readonly PortalSettings _portalSettings;
        private readonly UserInfo _currentUser;

        public ExtensionService(Guid scenarioID, PortalSettings portalSettings, UserInfo currentUser)
        {
            _portalSettings = portalSettings;
            _scenarioID = scenarioID;
            _currentUser = currentUser;
        }

        public ExtensionManifestInfo Extension { get; set; }
        private string ExtensionUnzipedPath { get; set; }
        private string ModuleMapPath { get; set; }
        private string BaseMapPath { get; set; }
        private bool IsNewExtension { get; set; }

        #region Public Methods

        public void InstallExtension(ExtensionManifestInfo extension, string extensionUnzipedPath)
        {
            this.Extension = extension;
            this.ExtensionUnzipedPath = extensionUnzipedPath;
            this.ModuleMapPath = HttpContext.Current.Server.MapPath("~/DesktopModules/BusinessEngine/");
            this.BaseMapPath = HttpContext.Current.Server.MapPath("~/");

            this.IsNewExtension = ExtensionRepository.Instance.GetExtensionByName(this.Extension.ExtensionName) == null;

            /* 1- */
            CreateExtension();
            /* 2- */
            ExecuteSqlProviders();
            /* 3- */
            CreateExtensionItems();
            /* 4- */
            CopyResourcesAndAssemblies();

            try
            {
                // Remove temp folder & files
                if (Directory.Exists(this.ExtensionUnzipedPath)) Directory.Delete(this.ExtensionUnzipedPath, true);
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Common Methods

        private void ExtractFiles(string zipFile, string targetDir, string fileFilter = null)
        {
            FastZip fastZip = new FastZip();
            fastZip.ExtractZip(zipFile, targetDir, fileFilter); //Will always overwrite if target filenames already exist
        }

        private bool IsValidVersion(string version)
        {
            if (this.IsNewExtension || string.IsNullOrEmpty(version)) return true;

            var extVersion = ExtensionRepository.Instance.GetExtensionVersion(this.Extension.ExtensionName);

            return new Version(extVersion) < new Version(version);
        }

        #endregion

        #region Extension Install Steps Methods

        /*-----------------------------------------------------------------------
         * ------------------ Step 1 ==> Create or Update Extension ----------------------
         * -------------------------------------------------------------*/
        private void CreateExtension()
        {
            var objExtensionInfo = new ExtensionInfo()
            {
                ExtensionName = this.Extension.ExtensionName,
                ExtensionType= this.Extension.ExtensionType,
                ExtensionImage = this.Extension.ExtensionImage,
                Title = this.Extension.Title,
                Summary = this.Extension.Summary,
                Description = this.Extension.Description,
                ReleaseNotes = this.Extension.ReleaseNotes,
                Owner = JsonConvert.SerializeObject(this.Extension.Owner),
                Resources = JsonConvert.SerializeObject(this.Extension.Resources),
                Assemblies = JsonConvert.SerializeObject(this.Extension.Assemblies),
                SqlProviders = JsonConvert.SerializeObject(this.Extension.SqlProviders),
                IsCommercial = this.Extension.IsCommercial,
                LicenseType = this.Extension.LicenseType,
                LicenseKey = this.Extension.LicenseKey,
                SourceUrl = this.Extension.SourceUrl,
                VersionType = this.Extension.VersionType,
                Version = this.Extension.Version,
                LastModifiedByUserID = _currentUser.UserID,
                LastModifiedOnDate = DateTime.Now,
            };

            var extension = ExtensionRepository.Instance.GetExtensionByName(this.Extension.ExtensionName);
            if (extension == null)
            {
                objExtensionInfo.CreatedByUserID = _currentUser.UserID;
                objExtensionInfo.CreatedOnDate = DateTime.Now;

                this.Extension.ExtensionID = ExtensionRepository.Instance.AddExtension(objExtensionInfo);
            }
            else
            {
                objExtensionInfo.ExtensionID = this.Extension.ExtensionID = extension.ExtensionID;
                objExtensionInfo.CreatedByUserID = extension.CreatedByUserID;
                objExtensionInfo.CreatedOnDate = extension.CreatedOnDate;

                ExtensionRepository.Instance.UpdateExtension(objExtensionInfo);
            }
        }

        /*-----------------------------------------------------------------------
         * ------------------ Step 2 ==> Execute Sql Providers ----------------------
         * -------------------------------------------------------------*/
        private void ExecuteSqlProviders()
        {
            StringBuilder queries = new StringBuilder();

            foreach (var item in (this.Extension.SqlProviders ?? Enumerable.Empty<ExtensionSqlProvider>()).Where(p => p.Type == "Install" && IsValidVersion(p.Version)))
            {
                var query = FileUtil.GetFileContent(this.ExtensionUnzipedPath + @"\SqlProviders\" + item.Name);
                queries.AppendLine(query);
                queries.AppendLine(Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(queries.ToString()))
            {
                queries = queries.Replace("GO", Environment.NewLine);
                var queryResult = DbUtil.ExecuteScalarSqlTransaction(queries.ToString());
            }
        }

        /*-----------------------------------------------------------------------
         * ------------------ Step 3 ==> Create Extension Items ----------------------
         * -------------------------------------------------------------*/
        private void CreateExtensionItems()
        {
            //1-Action
            ActionTypeRepository.Instance.DeleteActionTypesByExtensionID(this.Extension.ExtensionID);

            foreach (var package in this.Extension.Packages.Where(p => p.PackageType == "Action"))
            {
                foreach (var item in package.Actions ?? Enumerable.Empty<ActionTypeInfo>())
                {
                    item.ExtensionID = this.Extension.ExtensionID;
                    item.GroupID = GroupRepository.Instance.CheckExistsGroupOrCreateGroup(_scenarioID, "ActionType", item.GroupName);

                    item.TypeID = ActionTypeRepository.Instance.AddActionType(item);

                }
            }
            //--------------------------------------------------------------------------------

            //2-Service
            ServiceTypeRepository.Instance.DeleteServiceTypesByExtensionID(this.Extension.ExtensionID);

            foreach (var package in this.Extension.Packages.Where(p => p.PackageType == "Service"))
            {
                foreach (var item in package.Services ?? Enumerable.Empty<ServiceTypeInfo>())
                {
                    item.ExtensionID = this.Extension.ExtensionID;
                    item.GroupID = GroupRepository.Instance.CheckExistsGroupOrCreateGroup(_scenarioID, "ServiceType", item.GroupName);

                    item.TypeID = ServiceTypeRepository.Instance.AddServiceType(item);
                }
            }
            //--------------------------------------------------------------------------------

            //3-Field
            ModuleFieldTypeRepository.Instance.DeleteFieldTypesByExtensionID(this.Extension.ExtensionID);

            foreach (var package in this.Extension.Packages.Where(p => p.PackageType == "Field"))
            {
                foreach (var item in package.Fields ?? Enumerable.Empty<ModuleFieldTypeInfo>())
                {
                    item.ExtensionID = this.Extension.ExtensionID;
                    item.GroupID = GroupRepository.Instance.CheckExistsGroupOrCreateGroup(_scenarioID, "FieldType", item.GroupName);

                    item.TypeID = ModuleFieldTypeRepository.Instance.AddFieldType(item);
                }

                //4-Field Template
                foreach (var item in package.FieldTemplates ?? Enumerable.Empty<ModuleFieldTypeTemplateInfo>())
                {
                    item.TemplateID = ModuleFieldTypeTemplateRepository.Instance.AddTemplate(item);
                }
            }
            //--------------------------------------------------------------------------------

            //5-Library(Client) Resource
            LibraryResourceRepository.Instance.DeleteResourcesByExtensionID(this.Extension.ExtensionID);

            foreach (var package in this.Extension.Packages.Where(p => p.PackageType == "ClientResource"))
            {
                foreach (var item in package.ClientResources ?? Enumerable.Empty<LibraryResourceInfo>())
                {
                    //item.ExtensionID = this.Extension.ExtensionID;
                    //item.ResourceID = LibraryResourceRepository.Instance.AddResource(item);
                }
            }
            //--------------------------------------------------------------------------------

            //6-Skins(Templates&Themes)
            SkinRepository.Instance.DeleteSkinsByExtensionID(this.Extension.ExtensionID);

            foreach (var package in this.Extension.Packages.Where(p => p.PackageType == "Skin"))
            {
                foreach (var item in package.Skins ?? Enumerable.Empty<SkinInfo>())
                {
                    item.ExtensionID = this.Extension.ExtensionID;
                    item.SkinID = SkinRepository.Instance.AddSkin(item);
                }
            }
            //--------------------------------------------------------------------------------

            //7-Providers(Payment Gateway&Sms Gateway)
            ProviderRepository.Instance.DeleteSkinsByExtensionID(this.Extension.ExtensionID);

            foreach (var package in this.Extension.Packages.Where(p => p.PackageType == "Provider"))
            {
                foreach (var item in package.Providers ?? Enumerable.Empty<ProviderInfo>())
                {
                    item.ExtensionID = this.Extension.ExtensionID;
                    item.ProviderID = ProviderRepository.Instance.AddProvider(item);
                }
            }
            //--------------------------------------------------------------------------------
        }

        /*-----------------------------------------------------------------------
         * ------------------ Step 4 ==> Copy Resources And Assemblies ----------------------
         * -------------------------------------------------------------*/
        private void CopyResourcesAndAssemblies()
        {
            //1-Unzip Resources And Copy To Path Them
            foreach (var item in this.Extension.Resources ?? Enumerable.Empty<ExtensionResource>())
            {
                string filename = this.ExtensionUnzipedPath + @"\" + item.ZipFile;
                string targetDir = this.ModuleMapPath + item.BasePath;

                ExtractFiles(filename, targetDir);
            }

            //2-Unzip Assemblies And Copy To bin folder Them
            foreach (var item in this.Extension.Assemblies ?? Enumerable.Empty<ExtensionAssembly>())
            {
                string targetDir = this.BaseMapPath + item.BasePath;
                foreach (var file in item.Items)
                {
                    string source = this.ExtensionUnzipedPath + @"\bin\" + file;
                    File.Copy(source, targetDir + @"\" + file, true);
                }
            }
        }

        private void BroadcastInstallProgress(int step, object data, PortalSettings portalSettings)
        {
            var result = new InstallProgressInfo();

            switch (step)
            {
                case 1:
                    result.Title = "Read release file info";
                    result.Description = (data ?? string.Empty).ToString();
                    break;
                default:
                    break;
            }

            string json = JsonConvert.SerializeObject(result);

            FileUtil.CreateTextFile(portalSettings.HomeSystemDirectoryMapPath + @"\BusinessEngine\Temp\install-extension.json", json, true);
        }

        #endregion
    }
}
