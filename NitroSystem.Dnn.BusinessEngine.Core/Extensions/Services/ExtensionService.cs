using DotNetNuke.Entities.Portals;
using System;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using Newtonsoft.Json;
using NitroSystem.Dnn.BusinessEngine.Core.Extensions.Manifest;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using DotNetNuke.Entities.Users;
using System.Web;
using NitroSystem.Dnn.BusinessEngine.Core.Extensions.Manifest.PackageModels;
using NitroSystem.Dnn.BusinessEngine.Common.Models;
using System.Web.UI;
using System.Threading;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Extensions.Services
{
    public class ExtensionService
    {
        private readonly Guid _scenarioID;
        private readonly PortalSettings _portalSettings;
        private readonly UserInfo _currentUser;

        public ExtensionService()
        {
        }

        public ExtensionService(Guid scenarioID, PortalSettings portalSettings, UserInfo currentUser)
        {
            _portalSettings = portalSettings;
            _scenarioID = scenarioID;
            _currentUser = currentUser;
        }

        #region  Properties

        public static ExtensionService Instance
        {
            get
            {
                return new ExtensionService();
            }
        }

        private ExtensionManifest Extension { get; set; }

        private string ExtensionUnzipedPath { get; set; }

        private string ModuleMapPath { get; set; }

        private string BaseMapPath { get; set; }

        private string MonitoringFile { get; set; }

        private bool BeginMonitoring { get; set; }

        private bool IsNewExtension { get; set; }

        #endregion

        #region Install Extension

        public void InstallExtension(ExtensionManifest extension, string extensionUnzipedPath)
        {
            this.Extension = extension;
            this.ExtensionUnzipedPath = extensionUnzipedPath;
            this.ModuleMapPath = HttpContext.Current.Server.MapPath("~/DesktopModules/BusinessEngine/");
            this.BaseMapPath = HttpContext.Current.Server.MapPath("~/");
            this.MonitoringFile = HttpContext.Current.Server.MapPath(string.Format("~{0}/BusinessEngine/Temp/monitoring.txt", this._portalSettings.HomeSystemDirectory));

            this.IsNewExtension = ExtensionRepository.Instance.GetExtensionByName(this.Extension.ExtensionName) == null;

            MonitorProgress(string.Format(@"Srart Install Extension {0} by {1}({2})", this.Extension.ExtensionName, _currentUser.DisplayName, _currentUser.Username));

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
                File.Delete(this.MonitoringFile);
                Directory.Delete(this.ExtensionUnzipedPath, true);
            }
            catch (Exception ex)
            {
            }
        }

        /*-----------------------------------------------------------------------
         * ------------------ Step 1 ==> Create or Update Extension ----------------------
         * -------------------------------------------------------------*/
        private void CreateExtension()
        {
            var objExtensionInfo = new ExtensionInfo()
            {
                ExtensionName = this.Extension.ExtensionName,
                ExtensionType = this.Extension.ExtensionType,
                ExtensionImage = this.Extension.ExtensionImage.Replace("[EXTPATH]", "/DesktopModules/BusinessEngine/extensions"),
                FolderName = this.Extension.FolderName,
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
                MonitorProgress("Create extension record in database");
            }
            else
            {
                objExtensionInfo.ExtensionID = this.Extension.ExtensionID = extension.ExtensionID;
                objExtensionInfo.CreatedByUserID = extension.CreatedByUserID;
                objExtensionInfo.CreatedOnDate = extension.CreatedOnDate;

                ExtensionRepository.Instance.UpdateExtension(objExtensionInfo);
                MonitorProgress("extension exists and update record in database");
            }
        }

        /*-----------------------------------------------------------------------
         * ------------------ Step 2 ==> Execute Sql Providers ----------------------
         * -------------------------------------------------------------*/
        private void ExecuteSqlProviders()
        {
            MonitorProgress("Start extension sql queries...");

            string sqlProviderFolder = this.ExtensionUnzipedPath + @"\sql-providers\";
            StringBuilder queries = new StringBuilder();
            foreach (var item in (this.Extension.SqlProviders ?? Enumerable.Empty<ExtensionSqlProvider>()).Where(p => p.Type == "Install" && IsValidVersion(p.Version)))
            {
                var query = FileUtil.GetFileContent(sqlProviderFolder + item.Name);
                queries.AppendLine(query);
                queries.AppendLine(Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(queries.ToString()))
            {
                queries = queries.Replace("[EXTENSIONID]", this.Extension.ExtensionID.ToString());
                queries = queries.Replace("GO", Environment.NewLine);

                MonitorProgress(queries.ToString());

                var queryResult = DbUtil.ExecuteScalarSqlTransaction(queries.ToString());

                if (queryResult.IsSuccess)
                    MonitorProgress("the Sql Queries has been executed successfully");
                else
                {
                    MonitorProgress("the Sql Queries has been error");
                    MonitorProgress(queryResult.ResultMessage);
                }
            }

            FileUtil.MoveDirectory(sqlProviderFolder, this.BaseMapPath);
            MonitorProgress("Sql provider directory moved to new directory has been successfully");
        }

        /*-----------------------------------------------------------------------
         * ------------------ Step 3 ==> Create Extension Items ----------------------
         * -------------------------------------------------------------*/
        private void CreateExtensionItems()
        {
            //1-Action
            ActionTypeRepository.Instance.DeleteActionTypesByExtensionID(this.Extension.ExtensionID);
            MonitorProgress("Removed the old action type that were child of the extension");
            foreach (var package in this.Extension.Packages.Where(p => p.PackageType == "Action"))
            {
                foreach (var item in package.Actions ?? Enumerable.Empty<ActionTypeInfo>())
                {
                    item.ExtensionID = this.Extension.ExtensionID;
                    item.GroupID = GroupRepository.Instance.GetGroupID("ActionType", item.GroupName).Value;
                    //item.GroupID = GroupRepository.Instance.CheckExistsGroupOrCreateGroup(_scenarioID,this._currentUser.UserID, "ActionType", item.GroupName,item.);
                    //MonitorProgress("Checking action type group is exists or no");

                    item.TypeID = ActionTypeRepository.Instance.AddActionType(item);
                    MonitorProgress(string.Format("{0} action type added to the ActionTypes", item.ActionType));
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
                    item.GroupID = GroupRepository.Instance.GetGroupID("ServiceType", item.GroupName).Value;
                    //item.GroupID = GroupRepository.Instance.CheckExistsGroupOrCreateGroup(_scenarioID,this._currentUser.UserID, "ServiceType", item.GroupName);
                    //MonitorProgress("Checking service type group is exists or no");

                    item.TypeID = ServiceTypeRepository.Instance.AddServiceType(item);
                    MonitorProgress(string.Format("{0} service type added to the ServiceType", item.ServiceSubtype));
                }
            }
            //--------------------------------------------------------------------------------

            //3-Field
            ModuleFieldTypeRepository.Instance.DeleteFieldTypesByExtensionID(this.Extension.ExtensionID);
            foreach (var package in this.Extension.Packages.Where(p => p.PackageType == "Field"))
            {
                foreach (var item in package.Fields ?? Enumerable.Empty<FieldTypeInfo>())
                {
                    var objFieldTypeInfo = new ModuleFieldTypeInfo()
                    {
                        ExtensionID = this.Extension.ExtensionID,
                        GroupID = GroupRepository.Instance.GetGroupID("FieldType", item.GroupName).Value,
                        FieldType = item.FieldType,
                        Title = item.Title,
                        FieldComponent = item.FieldComponent,
                        FieldJsPath = item.FieldJsPath,
                        DirectiveJsPath = item.DirectiveJsPath,
                        CustomEvents = item.CustomEvents,
                        IsGroup = item.IsGroup,
                        IsValuable = item.IsValuable,
                        IsSelective = item.IsSelective,
                        IsJsonValue = item.IsJsonValue,
                        IsContent = item.IsContent,
                        DefaultSettings = item.DefaultSettings,
                        ValidationPattern = item.ValidationPattern,
                        Icon = item.Icon,
                        IsEnabled = item.IsEnabled,
                        Description = item.Description,
                        ViewOrder = item.ViewOrder
                    };

                    objFieldTypeInfo.TypeID = ModuleFieldTypeRepository.Instance.AddFieldType(objFieldTypeInfo);
                    MonitorProgress(string.Format("{0} field type added to the FieldType", item.FieldType));

                    //4-Field Template
                    foreach (var template in item.Templates ?? Enumerable.Empty<ModuleFieldTypeTemplateInfo>())
                    {
                        template.FieldType = objFieldTypeInfo.FieldType;
                        ModuleFieldTypeTemplateRepository.Instance.AddTemplate(template);
                        MonitorProgress(string.Format("{0} field type template added to the {0} field type templates", item.FieldType));
                    }
                }
            }
            //--------------------------------------------------------------------------------

            //We replaced this section with a sql query. Maybe he will come back to these solutions in the future
            ////5-Library(Client) Resource
            //LibraryRepository.Instance.DeleteLibrariesByExtensionID(this.Extension.ExtensionID);

            //foreach (var package in this.Extension.Packages.Where(p => p.PackageType == "Library"))
            //{
            //    foreach (var item in package.Libraries ?? Enumerable.Empty<Manifest.PackageModels.LibraryInfo>())
            //    {
            //        var objLibraryInfo = new Data.Entities.Tables.LibraryInfo()
            //        {
            //            ExtensionID = this.Extension.ExtensionID,
            //            Type = item.Type,
            //            LibraryName = item.LibraryName,
            //            Logo = item.Logo,
            //            Summary = item.Summary,
            //            Version = item.Version,
            //            LocalPath = item.LocalPath,
            //            IsSystemLibrary = item.IsSystemLibrary,
            //            IsCDN = item.IsCDN,
            //            IsCommercial = item.IsCommercial,
            //            IsOpenSource = item.IsOpenSource,
            //            IsStable = item.IsStable,
            //            LicenseJson = item.LicenseJson,
            //            GithubPage = item.GithubPage
            //        };

            //        objLibraryInfo.LibraryID = LibraryRepository.Instance.AddLibrary(objLibraryInfo);

            //        //Library Resources
            //        foreach (var resource in item.Resources ?? Enumerable.Empty<LibraryResourceInfo>())
            //        {
            //            resource.LibraryID = objLibraryInfo.LibraryID;
            //            LibraryResourceRepository.Instance.AddResource(resource);
            //        }
            //    }
            //}
            //--------------------------------------------------------------------------------

            //6-Skins(Templates&Themes)
            SkinRepository.Instance.DeleteSkinsByExtensionID(this.Extension.ExtensionID);
            foreach (var package in this.Extension.Packages.Where(p => p.PackageType == "Skin"))
            {
                foreach (var item in package.Skins ?? Enumerable.Empty<SkinInfo>())
                {
                    item.ExtensionID = this.Extension.ExtensionID;
                    item.SkinID = SkinRepository.Instance.AddSkin(item);
                    MonitorProgress(string.Format("{0} skin added to the skins", item.SkinName));
                }
            }
            //--------------------------------------------------------------------------------

            //7-Providers(Payment Gateway&Sms Gateway)
            ProviderRepository.Instance.DeleteProvidersByExtensionID(this.Extension.ExtensionID);
            foreach (var package in this.Extension.Packages.Where(p => p.PackageType == "Provider"))
            {
                foreach (var item in package.Providers ?? Enumerable.Empty<ProviderInfo>())
                {
                    item.ExtensionID = this.Extension.ExtensionID;
                    item.ProviderID = ProviderRepository.Instance.AddProvider(item);
                    MonitorProgress(string.Format("{0} provider added to the provider", item.ProviderName));
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
                MonitorProgress(string.Format("{0} resource files unziped", item.ZipFile));
            }

            //2-Unzip Assemblies And Copy To bin folder Them
            foreach (var item in this.Extension.Assemblies ?? Enumerable.Empty<ExtensionAssembly>())
            {
                string targetDir = this.BaseMapPath + item.BasePath;
                foreach (var file in item.Items)
                {
                    string source = this.ExtensionUnzipedPath + @"\bin\" + file;
                    File.Copy(source, targetDir + @"\" + file, true);
                    MonitorProgress(string.Format("{0} files copy to the destitions path", file));
                }
            }
        }

        #endregion

        #region Uninstall Extension 

        /*-----------------------------------------------------------------------
         * ------------------ Uninstall Extension ----------------------
         * -------------------------------------------------------------*/
        public void UninstallExtension(Guid extensionID)
        {
            var extension = ExtensionRepository.Instance.GetExtension(extensionID);

            var sqlResult = UninstallQueries(extension.FolderName);

            if (sqlResult.IsSuccess)
            {
                ExtensionRepository.Instance.DeleteExtension(extensionID);
            }
            else
            {
                throw new Exception(sqlResult.ResultMessage);
            }
        }

        private SqlResultInfo UninstallQueries(string foldername)
        {
            var uninstallFilePath = HttpContext.Current.Server.MapPath(string.Format("~/DesktopModules/BusinessEngine/extensions/{0}/sql-providers/uninstall.sql", foldername));
            var uninstallQueries = FileUtil.GetFileContent(uninstallFilePath);
            var queryResult = DbUtil.ExecuteScalarSqlTransaction(uninstallQueries);

            return queryResult;

        }

        #endregion

        #region Private Methods

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

        private void MonitorProgress(string message, byte state = 1)
        {
            if (!BeginMonitoring)
            {
                FileUtil.CreateTextFile(this.MonitoringFile, message, true);
                this.BeginMonitoring = true;
            }
            else
                FileUtil.AppendTextFile(this.MonitoringFile, Environment.NewLine + message);

            System.Threading.Thread.Sleep(100);
        }

        #endregion

    }
}
