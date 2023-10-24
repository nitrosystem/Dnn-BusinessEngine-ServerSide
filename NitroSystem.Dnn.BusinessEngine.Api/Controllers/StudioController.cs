using DotNetNuke.Common.Utilities;
using DotNetNuke.Security.Roles;
using DotNetNuke.Web.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Api.Dto;
using NitroSystem.Dnn.BusinessEngine.Api.ViewModels;
using NitroSystem.Dnn.BusinessEngine.Components;
using NitroSystem.Dnn.BusinessEngine.Core.ModuleBuilder;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using NitroSystem.Dnn.BusinessEngine.Api.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DotNetNuke.Entities.Host;
using NitroSystem.Dnn.BusinessEngine.Api.Mapping;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Common.Models;
using NitroSystem.Dnn.BusinessEngine.Core.Appearance;
using static Dapper.SqlMapper;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Web.UI;
using NitroSystem.Dnn.BusinessEngine.Api.Models;
using NitroSystem.Dnn.BusinessEngine.Common.TypeCasting;
using System.Web.Http.Results;
using NitroSystem.Dnn.BusinessEngine.Components.Enums;
using System.IO;
using NitroSystem.Dnn.BusinessEngine.Core.Extensions.Services;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Abstractions.Portals;
using NitroSystem.Dnn.BusinessEngine.Core.Extensions.Manifest;
using ICSharpCode.SharpZipLib.Zip;

namespace NitroSystem.Dnn.BusinessEngine.Api.Controller
{
    [DnnAuthorize(StaticRoles = "Administrators")]
    public class StudioController : DnnApiController
    {
        #region Common

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage AddDnnHostVersion()
        {
            try
            {
                DotNetNuke.Entities.Controllers.HostController.Instance.Update("CrmVersion", (Host.CrmVersion + 1).ToString());

                DataCache.ClearCache();

                DataCache.ClearPortalCache(PortalSettings.PortalId, true);

                DataCache.ClearHostCache(true);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region Sidebar Explorer 

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetStudioOptions()
        {
            try
            {
                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                var scenario = ScenarioRepository.Instance.GetScenario(scenarioID);

                var scenarios = ScenarioRepository.Instance.GetScenarios();

                var groups = GroupRepository.Instance.GetGroups(scenarioID).Where(g => g.GroupType == "SidebarExplorer");

                var explorerItems = ExplorerItemRepository.Instance.GetItems(scenarioID);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Scenario = scenario,
                    Scenarios = scenarios,
                    Groups = groups,
                    ExplorerItems = explorerItems
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage RefreshSidebarExplorerItems()
        {
            try
            {
                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                var explorerItems = ExplorerItemRepository.Instance.GetItems(scenarioID);

                return Request.CreateResponse(HttpStatusCode.OK, explorerItems);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveGroup(GroupInfo group)
        {
            try
            {
                var objGroupInfo = new GroupInfo()
                {
                    GroupID = group.GroupID,
                    ScenarioID = group.ScenarioID,
                    GroupType = group.GroupType,
                    ObjectType = group.ObjectType,
                    GroupName = group.GroupName,
                    Description = group.Description,
                    IsSystemGroup = group.IsSystemGroup,
                    ViewOrder = group.ViewOrder,
                };

                objGroupInfo.LastModifiedOnDate = group.LastModifiedOnDate = DateTime.Now;
                objGroupInfo.LastModifiedByUserID = group.LastModifiedByUserID = this.UserInfo.UserID;

                if (group.GroupID == Guid.Empty)
                {
                    objGroupInfo.CreatedOnDate = group.CreatedOnDate = DateTime.Now;
                    objGroupInfo.CreatedByUserID = group.CreatedByUserID = this.UserInfo.UserID;

                    group.GroupID = GroupRepository.Instance.AddGroup(objGroupInfo);
                }
                else
                {
                    objGroupInfo.CreatedOnDate = group.CreatedOnDate == DateTime.MinValue ? DateTime.Now : group.CreatedOnDate;
                    objGroupInfo.CreatedByUserID = group.CreatedByUserID;

                    GroupRepository.Instance.UpdateGroup(objGroupInfo);
                }

                return Request.CreateResponse(HttpStatusCode.OK, group.GroupID);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage DeleteGroup(GuidDTO postData)
        {
            try
            {
                GroupRepository.Instance.DeleteGroup(postData.ID);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage UpdateItemGroup(ExplorerItemDTO item)
        {
            try
            {
                switch (item.ItemType)
                {
                    case "Entity":
                        EntityRepository.Instance.UpdateGroup(item.ItemID, item.GroupID);
                        break;
                    case "ViewModel":
                        ViewModelRepository.Instance.UpdateGroup(item.ItemID, item.GroupID);
                        break;
                    case "Service":
                        ServiceRepository.Instance.UpdateGroup(item.ItemID, item.GroupID);
                        break;
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region Scenario 

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetScenarios()
        {
            try
            {
                var scenarios = ScenarioRepository.Instance.GetScenarios();

                return Request.CreateResponse(HttpStatusCode.OK, scenarios);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetScenario()
        {
            try
            {
                return GetScenario(Guid.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetScenario(Guid scenarioID)
        {
            try
            {
                var scenario = ScenarioRepository.Instance.GetScenario(scenarioID);

                return Request.CreateResponse(HttpStatusCode.OK, scenario);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveScenario(ScenarioInfo scenario)
        {
            try
            {
                var objScenarioInfo = new ScenarioInfo()
                {
                    ScenarioID = scenario.ScenarioID,
                    ScenarioName = scenario.ScenarioName,
                    ScenarioTitle = scenario.ScenarioTitle,
                    DatabaseObjectPrefix = scenario.DatabaseObjectPrefix,
                    Description = scenario.Description,
                };

                objScenarioInfo.LastModifiedOnDate = scenario.LastModifiedOnDate = DateTime.Now;
                objScenarioInfo.LastModifiedByUserID = scenario.LastModifiedByUserID = this.UserInfo.UserID;

                if (scenario.ScenarioID == Guid.Empty)
                {
                    objScenarioInfo.CreatedOnDate = scenario.CreatedOnDate = DateTime.Now;
                    objScenarioInfo.CreatedByUserID = scenario.CreatedByUserID = this.UserInfo.UserID;

                    scenario.ScenarioID = ScenarioRepository.Instance.AddScenario(objScenarioInfo);
                }
                else
                {
                    objScenarioInfo.CreatedOnDate = scenario.CreatedOnDate == DateTime.MinValue ? DateTime.Now : scenario.CreatedOnDate;
                    objScenarioInfo.CreatedByUserID = scenario.CreatedByUserID;

                    ScenarioRepository.Instance.UpdateScenario(objScenarioInfo);
                }

                return Request.CreateResponse(HttpStatusCode.OK, scenario);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetScenarioModulesAndFields(Guid scenarioID)
        {
            try
            {
                var modules = ModuleRepository.Instance.GetScenarioModules(scenarioID);

                var result = new List<object>();

                foreach (var module in modules.Where(m => m.ModuleBuilderType != "HtmlEditor"))
                {
                    result.Add(new
                    {
                        ModuleID = module.ModuleID,
                        ParentID = module.ParentID,
                        ModuleType = module.ModuleType,
                        LayoutTemplate = module.LayoutTemplate,
                        LayoutCss = module.LayoutCss,
                        Fields = ModuleFieldMappings.GetFieldsViewModel(module.ModuleID),
                        Skin = module.Skin,
                        Theme = module.Theme,
                        Template = module.Template
                    });
                }

                var fieldTypes = ModuleFieldMappings.GetFieldTypesViewModel();

                PageResourceRepository.Instance.DeleteScenarioPageResources(scenarioID);

                return Request.CreateResponse(HttpStatusCode.OK, new { Modules = result, FieldTypes = fieldTypes });
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

        #region Entities

        [DnnAuthorize]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetEntities()
        {
            try
            {
                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                var scenarios = ScenarioRepository.Instance.GetScenarios();

                var databases = DatabaseRepository.Instance.GetDatabases();

                var entities = EntityMapping.GetEntitiesViewModel(scenarioID);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Scenarios = scenarios,
                    Databases = databases,
                    Entities = entities
                });
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetDatabaseObjects(Guid? databaseID = null)
        {
            try
            {
                string connectionString = "";

                if (databaseID != null)
                {
                    var database = DatabaseRepository.Instance.GetDatabase(databaseID.Value);
                    connectionString = database != null ? database.ConnectionString : "";
                }

                var tables = DbUtil.GetDatabaseObjects(0, connectionString);

                var views = DbUtil.GetDatabaseObjects(1, connectionString);

                return Request.CreateResponse(HttpStatusCode.OK, new { Tables = tables, Views = views });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetDatabaseObjectColumns(string objectName, Guid? databaseID = null)
        {
            try
            {
                string connectionString = "";

                if (databaseID != null)
                {
                    var database = DatabaseRepository.Instance.GetDatabase(databaseID.Value);
                    connectionString = database != null ? database.ConnectionString : "";
                }

                var columns = DbUtil.GetDatabaseObjectColumns(objectName, connectionString);

                return Request.CreateResponse(HttpStatusCode.OK, columns);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetEntity()
        {
            return GetEntity(Guid.Empty);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetEntity(Guid entityID)
        {
            try
            {
                var scenarios = ScenarioRepository.Instance.GetScenarios();

                var databases = DatabaseRepository.Instance.GetDatabases();

                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());
                var entities = EntityMapping.GetEntitiesViewModel(scenarioID);

                var entity = EntityMapping.GetEntityViewModel(entityID);

                IEnumerable<TableRelationshipInfo> relationships = null;

                if (entity != null)
                    relationships = DbUtil.GetTableRelationships("dbo", entity.TableName);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Scenarios = scenarios,
                    Databases = databases,
                    Entities = entities,
                    Entity = entity,
                    Relationships = relationships
                });
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveEntity(EntityViewModel entity)
        {
            var isNewEntity = entity.EntityID == Guid.Empty;
            var isDbObjectCreated = false;

            try
            {
                if (!entity.IsReadonly) entity.TableName = (entity.Settings["DatabaseObjectPrefixName"].ToString() +
                                                            entity.Settings["DatabaseObjectPostfixName"].ToString());

                var objEntityInfo = new EntityInfo()
                {
                    EntityID = entity.EntityID,
                    ScenarioID = entity.ScenarioID,
                    DatabaseID = entity.DatabaseID,
                    GroupID = entity.GroupID,
                    EntityType = entity.EntityType,
                    EntityName = entity.EntityName,
                    TableName = entity.TableName,
                    IsReadonly = entity.IsReadonly,
                    IsMultipleColumnsForPK = entity.IsMultipleColumnsForPK,
                    Settings = entity.Settings != null ? JsonConvert.SerializeObject(entity.Settings) : null,
                    Description = entity.Description,
                    ViewOrder = entity.ViewOrder,
                };

                objEntityInfo.LastModifiedOnDate = entity.LastModifiedOnDate = DateTime.Now;
                objEntityInfo.LastModifiedByUserID = entity.LastModifiedByUserID = this.UserInfo.UserID;

                if (entity.EntityID == Guid.Empty)
                {
                    if (!entity.IsReadonly)
                    {
                        var primaryColumn = entity.Columns.First();

                        string query = FileUtil.GetFileContent(HttpContext.Current.Server.MapPath("~/DesktopModules/BusinessEngine/data/sql-templates/create-entity.sql"));

                        query = query.Replace("{TableName}", entity.TableName);
                        query = query.Replace("{PrimaryColumnName}", primaryColumn.ColumnName);
                        query = query.Replace("{PrimaryColumnType}", primaryColumn.ColumnType);
                        query = query.Replace("{PrimaryIsIdentity}", primaryColumn.IsIdentity ? "IDENTITY (1, 1)" : "");

                        var result = DbUtil.ExecuteScalarSqlTransaction(query);

                        if (!result.IsSuccess) throw new Exception(result.ResultMessage);

                        isDbObjectCreated = true;
                    }

                    objEntityInfo.CreatedOnDate = entity.CreatedOnDate = DateTime.Now;
                    objEntityInfo.CreatedByUserID = entity.CreatedByUserID = this.UserInfo.UserID;

                    entity.EntityID = EntityRepository.Instance.AddEntity(objEntityInfo);
                }
                else
                {
                    if (!entity.IsReadonly && !isDbObjectCreated)
                    {
                        var oldEntity = EntityRepository.Instance.GetEntity(entity.EntityID);
                        if (oldEntity.TableName != entity.TableName)
                            DbUtil.ExecuteScalarSqlTransaction(string.Format("exec sp_rename '{0}', '{1}'", oldEntity.TableName, entity.TableName));
                    }

                    objEntityInfo.CreatedOnDate = entity.CreatedOnDate == DateTime.MinValue ? DateTime.Now : entity.CreatedOnDate;
                    objEntityInfo.CreatedByUserID = entity.CreatedByUserID;

                    EntityRepository.Instance.UpdateEntity(objEntityInfo);
                }

                var oldColumns = EntityColumnRepository.Instance.GetColumns(entity.EntityID);

                var queries = new StringBuilder();

                // the Columns that must be delete
                foreach (EntityColumnInfo column in oldColumns.Where(c => !entity.Columns.Select(cc => cc.ColumnID).Contains(c.ColumnID)))
                {
                    if (!entity.IsReadonly)
                    {
                        var removeResult = DbUtil.RemoveTableColumn("dbo", entity.TableName, column.ColumnName);

                        if (removeResult.IsSuccess) EntityColumnRepository.Instance.DeleteColumn(column.ColumnID);
                        else
                            throw new Exception(removeResult.ResultMessage);
                    }
                }

                var viewOrder = 0;
                foreach (EntityColumnInfo column in entity.Columns.OrderBy(c => c.ViewOrder))
                {
                    var objEntityColumnInfo = new EntityColumnInfo()
                    {
                        ColumnID = column.ColumnID,
                        EntityID = entity.EntityID,
                        ColumnName = column.ColumnName,
                        ColumnType = column.ColumnType,
                        IsPrimary = column.IsPrimary,
                        IsIdentity = column.IsIdentity,
                        IsComputedColumn = column.ColumnType.Trim().StartsWith("as "),
                        AllowNulls = column.AllowNulls,
                        Description = column.Description,
                        ViewOrder = viewOrder++
                    };

                    objEntityColumnInfo.LastModifiedOnDate = column.LastModifiedOnDate = DateTime.Now;
                    objEntityColumnInfo.LastModifiedByUserID = column.LastModifiedByUserID = this.UserInfo.UserID;

                    bool isNewColumn = column.ColumnID == Guid.Empty;

                    if (!entity.IsReadonly)
                    {
                        if (!column.IsPrimary && isNewColumn) // add new column
                        {
                            //column must be create
                            DbUtil.ExecuteSql(string.Format("ALTER TABLE {0} ADD {1} {2} {3}", entity.TableName, column.ColumnName, column.ColumnType, !column.AllowNulls ? "NOT NULL" : "NULL"));
                        }
                        else if (!isNewColumn) // modify column
                        {
                            var oldColumn = oldColumns.FirstOrDefault(c => c.ColumnID == column.ColumnID);

                            //column name is renamed
                            if (oldColumn != null && column.ColumnName != oldColumn.ColumnName)
                                DbUtil.ExecuteSql(string.Format("EXEC sp_RENAME '{0}.{1}' , '{2}', 'COLUMN'", entity.TableName, oldColumn.ColumnName, column.ColumnName));
                            //column type is changed and new type is formula or old type was formula
                            else if (oldColumn != null && column.ColumnType != oldColumn.ColumnType && (column.ColumnType.ToLower().StartsWith("as ") || oldColumn.ColumnType.ToLower().StartsWith("as ")))
                                DbUtil.ExecuteSql(string.Format("ALTER TABLE {0} DROP COLUMN {1} ALTER TABLE {0} ADD {1} {2} {3}", entity.TableName, column.ColumnName, column.ColumnType, column.ColumnType.ToLower().StartsWith("as ") ? "" : !column.AllowNulls ? "NOT NULL" : "NULL"));
                            //column type is changes
                            else if (oldColumn != null && (column.ColumnType != oldColumn.ColumnType || column.AllowNulls != oldColumn.AllowNulls))
                                DbUtil.ExecuteSql(string.Format("ALTER TABLE {0} ALTER COLUMN {1} {2} {3}", entity.TableName, column.ColumnName, column.ColumnType, !column.AllowNulls ? "NOT NULL" : "NULL"));
                        }
                    }

                    if (isNewColumn)
                    {
                        objEntityColumnInfo.CreatedOnDate = column.CreatedOnDate = DateTime.Now;
                        objEntityColumnInfo.CreatedByUserID = column.CreatedByUserID = this.UserInfo.UserID;

                        column.ColumnID = EntityColumnRepository.Instance.AddColumn(objEntityColumnInfo);
                    }
                    else
                    {
                        objEntityColumnInfo.CreatedOnDate = column.CreatedOnDate == DateTime.MinValue ? DateTime.Now : column.CreatedOnDate;
                        objEntityColumnInfo.CreatedByUserID = column.CreatedByUserID;

                        EntityColumnRepository.Instance.UpdateColumn(objEntityColumnInfo);
                    }
                }

                foreach (var relationship in entity.Relationships ?? Enumerable.Empty<TableRelationshipInfo>())
                {
                    relationship.ChildEntityTableName = entity.TableName;

                    DbUtil.AddRelationship(relationship);
                }

                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                if (isNewEntity && entity.EntityID != Guid.Empty) EntityRepository.Instance.DeleteEntity(entity.EntityID);

                if (isNewEntity && isDbObjectCreated) DbUtil.ExecuteSql(string.Format("DROP TABLE {0}", entity.TableName));

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage DeleteEntity(GuidDTO postData)
        {
            try
            {
                EntityRepository.Instance.DeleteEntity(postData.ID);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region View Models

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetViewModels()
        {
            try
            {
                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                var viewModels = ViewModelMapping.GetViewModelsViewModel(scenarioID);

                var scenarios = ScenarioRepository.Instance.GetScenarios();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    ViewModels = viewModels,
                    Scenarios = scenarios
                });
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetViewModel()
        {
            return GetViewModel(Guid.Empty);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetViewModel(Guid viewModelID)
        {
            try
            {
                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                var viewModel = ViewModelMapping.GetViewModelViewModel(viewModelID);

                var scenarios = ScenarioRepository.Instance.GetScenarios();

                var viewModels = ViewModelRepository.Instance.GetViewModels(scenarioID);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    ViewModel = viewModel,
                    Scenarios = scenarios,
                    ViewModels = viewModels,
                });
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveViewModel(ViewModelViewModel viewModel)
        {
            try
            {
                bool isNew = false;

                var objViewModel = new ViewModelInfo()
                {
                    ViewModelID = viewModel.ViewModelID,
                    ScenarioID = viewModel.ScenarioID,
                    ViewModelName = viewModel.ViewModelName,
                    Description = viewModel.Description,
                    ViewOrder = viewModel.ViewOrder
                };

                objViewModel.LastModifiedOnDate = viewModel.LastModifiedOnDate = DateTime.Now;
                objViewModel.LastModifiedByUserID = viewModel.LastModifiedByUserID = this.UserInfo.UserID;

                if (viewModel.ViewModelID == Guid.Empty)
                {
                    isNew = true;

                    objViewModel.CreatedOnDate = viewModel.CreatedOnDate = DateTime.Now;
                    objViewModel.CreatedByUserID = viewModel.CreatedByUserID = this.UserInfo.UserID;

                    viewModel.ViewModelID = ViewModelRepository.Instance.AddViewModel(objViewModel);
                }
                else
                {
                    objViewModel.CreatedOnDate = viewModel.CreatedOnDate == DateTime.MinValue ? DateTime.Now : viewModel.CreatedOnDate;
                    objViewModel.CreatedByUserID = viewModel.CreatedByUserID;

                    ViewModelRepository.Instance.UpdateViewModel(objViewModel);
                }

                if (viewModel.Properties != null)
                {
                    if (!isNew)
                    {
                        var oldProperties = ViewModelPropertyRepository.Instance.GetProperties(viewModel.ViewModelID);

                        var propertyIDs = oldProperties.Where(p => viewModel.Properties.Select(pp => pp.PropertyID).Contains(p.PropertyID) == false).Select(p => p.PropertyID);
                        ViewModelPropertyRepository.Instance.DeleteProperties(propertyIDs);
                    }

                    foreach (var property in viewModel.Properties)
                    {
                        property.ViewModelID = viewModel.ViewModelID;

                        if (property.PropertyType != "viewModel" && property.PropertyType != "listOfViewModel") property.PropertyTypeID = null;

                        if (property.PropertyID == Guid.Empty)
                            property.PropertyID = ViewModelPropertyRepository.Instance.AddProperty(property);
                        else
                            ViewModelPropertyRepository.Instance.UpdateProperty(property);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage DeleteViewModel(GuidDTO postData)
        {
            try
            {
                ViewModelRepository.Instance.DeleteViewModel(postData.ID);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region Services

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetServices()
        {
            try
            {
                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                var scenarios = ScenarioRepository.Instance.GetScenarios();

                var services = ServiceMapping.GetServicesViewModel(scenarioID);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Scenarios = scenarios,
                    Services = services
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetService()
        {
            return GetService(Guid.Empty);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetService(Guid serviceID)
        {
            try
            {
                var service = ServiceMapping.GetServiceViewModel(serviceID);

                var scenarios = ScenarioRepository.Instance.GetScenarios();

                var databases = DatabaseRepository.Instance.GetDatabases();

                var serviceTypes = ServiceMapping.GetServiceTypesViewModel();

                IEnumerable<string> roles = null;
                roles = RoleController.Instance.GetRoles(PortalSettings.PortalId).Cast<RoleInfo>().Select(r => r.RoleName);
                var allUsers = new List<string>();
                allUsers.Add("All Users");
                roles = allUsers.Concat(roles);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Service = service,
                    Scenarios = scenarios,
                    Databases = databases,
                    Roles = roles,
                    ServiceTypes = serviceTypes
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetServiceDependencies()
        {
            try
            {
                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                var entities = EntityMapping.GetEntitiesViewModel(scenarioID);

                var viewModels = ViewModelMapping.GetViewModelsViewModel(scenarioID);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Entities = entities,
                    ViewModels = viewModels,
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetServiceParams(Guid serviceID)
        {
            try
            {
                var serviceParams = ServiceParamRepository.Instance.GetParams(serviceID);

                return Request.CreateResponse(HttpStatusCode.OK, serviceParams);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveService(ServiceViewModel service)
        {
            try
            {
                var newService = service.ServiceID == Guid.Empty;

                var objServiceInfo = new ServiceInfo()
                {
                    ServiceID = service.ServiceID,
                    ScenarioID = service.ScenarioID,
                    DatabaseID = service.DatabaseID,
                    ServiceType = service.ServiceType,
                    ServiceName = service.ServiceName,
                    ServiceSubtype = service.ServiceSubtype,
                    IsEnabled = service.IsEnabled,
                    HasResult = service.HasResult,
                    ResultType = (byte)service.ResultType,
                    AuthorizationRunService = service.AuthorizationRunService != null ? string.Join(",", service.AuthorizationRunService) : null,
                    Settings = service.Settings != null ? JsonConvert.SerializeObject(service.Settings) : null,
                    Description = service.Description,

                    ViewOrder = service.ViewOrder
                };

                objServiceInfo.LastModifiedOnDate = service.LastModifiedOnDate = DateTime.Now;
                objServiceInfo.LastModifiedByUserID = service.LastModifiedByUserID = this.UserInfo.UserID;

                if (service.ServiceID == Guid.Empty)
                {
                    objServiceInfo.CreatedOnDate = service.CreatedOnDate = DateTime.Now;
                    objServiceInfo.CreatedByUserID = service.CreatedByUserID = this.UserInfo.UserID;

                    service.ServiceID = ServiceRepository.Instance.AddService(objServiceInfo);
                }
                else
                {
                    objServiceInfo.CreatedOnDate = service.CreatedOnDate == DateTime.MinValue ? DateTime.Now : service.CreatedOnDate;
                    objServiceInfo.CreatedByUserID = service.CreatedByUserID;

                    ServiceRepository.Instance.UpdateService(objServiceInfo);
                }

                if (service.Settings.ContainsKey("SaveParams") && bool.Parse(service.Settings["SaveParams"].ToString()))
                    Core.General.SaveServiceParams(service.ServiceID, service.Params);

                return Request.CreateResponse(HttpStatusCode.OK, service);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage DeleteService(GuidDTO postData)
        {
            try
            {
                ServiceRepository.Instance.DeleteService(postData.ID);

                return Request.CreateResponse(HttpStatusCode.OK, new { Success = true });
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetServiceSpScript(Guid serviceID)
        {
            try
            {
                string result = string.Empty;

                var service = ServiceRepository.Instance.GetService(serviceID);

                var connection = new System.Data.SqlClient.SqlConnection(DotNetNuke.Data.DataProvider.Instance().ConnectionString);

                //service.DatabaseObjectParams = Dapper.SqlMapper.Query<string>(connection, string.Format("SELECT [Name] FROM sys.parameters WHERE object_id = object_id('dbo.{0}')", service.DatabaseObjectName));

                // result = Dapper.SqlMapper.QuerySingle<string>(connection, string.Format("SELECT [Definition] FROM sys.sql_modules WHERE objectproperty(OBJECT_ID, 'IsProcedure') = 1 AND OBJECT_NAME(OBJECT_ID) = '{0}'", service.DatabaseObjectName));
                // or use EXEC sp_helptext N'dbo.GetFolders'

                return Request.CreateResponse(HttpStatusCode.OK/*, result*/);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region Extensions

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetExtensions()
        {
            try
            {
                var extensions = ExtensionRepository.Instance.GetExtensions();

                return Request.CreateResponse(HttpStatusCode.OK, extensions);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region Providers

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetProvider(Guid providerID)
        {
            try
            {
                var provider = ProviderRepository.Instance.GetProvider(providerID);

                return Request.CreateResponse(HttpStatusCode.OK, provider);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveProvider(ProviderInfo provider)
        {
            try
            {
                ProviderRepository.Instance.UpdateProvider(provider);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region Global Settings

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetSettings(string groupName)
        {
            try
            {
                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                var settings = GlobalSettingsRepository.Instance.GetSettings(scenarioID, groupName);

                return Request.CreateResponse(HttpStatusCode.OK, settings);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveSettings([FromUri] string groupName, IDictionary<string, object> settings)
        {
            try
            {
                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                foreach (var item in settings)
                {
                    GlobalSettingsRepository.Instance.UpdateSetting(scenarioID, groupName, item.Key, item.Value);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region Payment Methods

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetPaymentMethods()
        {
            try
            {
                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                var paymentMethods = PaymentMethodRepository.Instance.GetPaymentMethods(scenarioID);

                var scenarios = ScenarioRepository.Instance.GetScenarios();

                var providers = ProviderRepository.Instance.GetProviders("PaymentGateway");

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    PaymentMethods = paymentMethods,
                    Scenarios = scenarios,
                    Providers = providers,
                });
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SavePaymentMethod(PaymentMethodInfo paymentMethod)
        {
            try
            {
                var objPaymentMethod = new PaymentMethodInfo()
                {
                    PaymentMethodID = paymentMethod.PaymentMethodID,
                    ScenarioID = paymentMethod.ScenarioID,
                    PaymentMethodName = paymentMethod.PaymentMethodName,
                    SuccessfulPaymentTemplate = paymentMethod.SuccessfulPaymentTemplate,
                    UnsuccessfulPaymentTemplate = paymentMethod.UnsuccessfulPaymentTemplate,
                    Description = paymentMethod.Description,
                    ViewOrder = paymentMethod.ViewOrder
                };

                objPaymentMethod.LastModifiedOnDate = paymentMethod.LastModifiedOnDate = DateTime.Now;
                objPaymentMethod.LastModifiedByUserID = paymentMethod.LastModifiedByUserID = this.UserInfo.UserID;

                if (paymentMethod.PaymentMethodID == Guid.Empty)
                {
                    objPaymentMethod.CreatedOnDate = DateTime.Now;
                    objPaymentMethod.CreatedByUserID = this.UserInfo.UserID;

                    paymentMethod.PaymentMethodID = PaymentMethodRepository.Instance.AddPaymentMethod(objPaymentMethod);
                }
                else
                {
                    objPaymentMethod.CreatedOnDate = paymentMethod.CreatedOnDate == DateTime.MinValue ? DateTime.Now : paymentMethod.CreatedOnDate;
                    objPaymentMethod.CreatedByUserID = paymentMethod.CreatedByUserID;

                    PaymentMethodRepository.Instance.UpdatePaymentMethod(objPaymentMethod);
                }

                return Request.CreateResponse(HttpStatusCode.OK, paymentMethod);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage DeletePaymentMethod(GuidDTO postData)
        {
            try
            {
                PaymentMethodRepository.Instance.DeletePaymentMethod(postData.ID);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region Actions

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetActions(Guid parentID)
        {
            try
            {
                var actions = ActionMapping.GetActionsViewModel(parentID);

                var module = ModuleRepository.Instance.GetModule(parentID);

                return Request.CreateResponse(HttpStatusCode.OK, new { Actions = actions, Module = module });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetFieldActions(Guid parentID)
        {
            try
            {
                var actions = ActionMapping.GetFieldActionsViewModel(parentID);

                var moduleID = ModuleFieldRepository.Instance.GetModuleID(parentID);
                var module = ModuleRepository.Instance.GetModule(moduleID);

                return Request.CreateResponse(HttpStatusCode.OK, new { Actions = actions, Module = module });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetAction(bool isFieldActions, Guid parentID, Guid? id = null)
        {
            try
            {
                var moduleID = !isFieldActions ? parentID : ModuleFieldRepository.Instance.GetModuleID(parentID);
                var module = ModuleRepository.Instance.GetModule(moduleID);

                var scenarioID = ModuleRepository.Instance.GetModuleScenarioID(moduleID);

                var actions = isFieldActions
                    ? ActionMapping.GetFieldActionsViewModel(parentID)
                    : ActionMapping.GetActionsViewModel(moduleID);

                var action = id == null ? null : ActionMapping.GetActionViewModel(id.Value);

                var allActions = ActionRepository.Instance.GetModuleActions(moduleID);

                var fields = ModuleFieldMappings.GetFieldsViewModel(moduleID, null);

                var customEvents = new List<FieldTypeCustomEventInfo>();
                if (isFieldActions)
                {
                    var fieldType = ModuleFieldRepository.Instance.GetFieldType(parentID);
                    var fieldEventTypes = TypeCastingUtil<IEnumerable<FieldTypeCustomEventInfo>>.TryJsonCasting(ModuleFieldTypeRepository.Instance.GetCustomEvents(fieldType));
                    if (fieldEventTypes != null) customEvents.AddRange(fieldEventTypes);
                }

                var variables = ModuleVariableRepository.Instance.GetVariables(moduleID);

                var services = ServiceMapping.GetServicesViewModel(scenarioID);

                var viewModels = ViewModelMapping.GetViewModelsViewModel(scenarioID);

                var paymentMethods = PaymentMethodMapping.GetPaymentMethodsViewModel(scenarioID);

                var paymentGateways = ProviderRepository.Instance.GetProviders("PaymentGateway");

                var actionTypes = ActionMapping.GetActionTypesViewModel();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Module = module,
                    Actions = actions,
                    Action = action,
                    AllActions = allActions,
                    ActionTypes = actionTypes,
                    Fields = fields,
                    CustomEvents = customEvents,
                    Services = services,
                    ViewModels = viewModels,
                    Variables = variables,
                    PaymentMethods = paymentMethods,
                    PaymentGateways = paymentGateways,
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        //[HttpGet]
        //public HttpResponseMessage GetAction(Guid parentID, Guid id)
        //{
        //    try
        //    {
        //        ActionViewModel action = ActionMapping.GetActionViewModel(id);

        //        var moduleID = action != null & action.FieldID != null ? action.ModuleID : parentID;
        //        var module = ModuleRepository.Instance.GetModule(moduleID);

        //        var scenarioID = ModuleRepository.Instance.GetModuleScenarioID(moduleID);

        //        var actions = action != null & action.FieldID != null ? ActionMapping.GetFieldActionsViewModel(action.FieldID.Value) : ActionMapping.GetActionsViewModel(parentID);

        //        var allActions = ActionRepository.Instance.GetModuleActions(moduleID);

        //        var fields = ModuleFieldMappings.GetFieldsViewModel(moduleID, null);

        //        var variables = ModuleVariableRepository.Instance.GetVariables(moduleID);

        //        var services = ServiceMapping.GetServicesViewModel(scenarioID);

        //        var viewModels = ViewModelMapping.GetViewModelsViewModel(scenarioID);

        //        var paymentMethods = PaymentMethodMapping.GetPaymentMethodsViewModel(scenarioID);

        //        var paymentGateways = ProviderRepository.Instance.GetProviders("PaymentGateway");

        //        var actionTypes = ActionMapping.GetActionTypesViewModel();

        //        var customEvents = new List<FieldTypeCustomEventInfo>();
        //        if (action != null & action.FieldID != null)
        //        {
        //            var fieldType = ModuleFieldRepository.Instance.GetFieldType(action.FieldID.Value);
        //            var fieldEventTypes = TypeCastingUtil<IEnumerable<FieldTypeCustomEventInfo>>.TryJsonCasting(ModuleFieldTypeRepository.Instance.GetCustomEvents(fieldType));
        //            if (fieldEventTypes != null) customEvents.AddRange(fieldEventTypes);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, new
        //        {
        //            Module = module,
        //            Action = action,
        //            Actions = actions,
        //            AllActions = allActions,
        //            ActionTypes = actionTypes,
        //            Fields = fields,
        //            CustomEvents = customEvents,
        //            Services = services,
        //            ViewModels = viewModels,
        //            Variables = variables,
        //            PaymentMethods = paymentMethods,
        //            PaymentGateways = paymentGateways,
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
        //    }
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveAction(ActionViewModel action)
        {
            try
            {
                var objActionInfo = new ActionInfo()
                {
                    ActionID = action.ActionID,
                    ParentID = action.Event == "OnActionCompleted" ? action.ParentID : null,
                    ModuleID = action.ModuleID,
                    FieldID = action.FieldID,
                    ServiceID = action.ServiceID,
                    ActionName = action.ActionName,
                    ActionType = action.ActionType,
                    Event = action.Event,
                    IsServerSide = action.IsServerSide,
                    ParentResultStatus = (byte?)action.ParentResultStatus,
                    HasPreScript = action.HasPreScript,
                    HasPostScript = action.HasPostScript,
                    DisableConditionForPreScript = action.DisableConditionForPreScript,
                    CheckConditionsInClientSide = action.CheckConditionsInClientSide,
                    PreScript = action.PreScript,
                    PostScript = action.PostScript,
                    Settings = action.Settings != null && action.Settings.Count > 0 ? JsonConvert.SerializeObject(action.Settings) : null,
                    Description = action.Description,
                    ViewOrder = action.ViewOrder,
                };

                objActionInfo.LastModifiedOnDate = action.LastModifiedOnDate = DateTime.Now;
                objActionInfo.LastModifiedByUserID = action.LastModifiedByUserID = this.UserInfo.UserID;

                if (action.ActionID == Guid.Empty)
                {
                    objActionInfo.CreatedOnDate = DateTime.Now;
                    objActionInfo.CreatedByUserID = this.UserInfo.UserID;

                    action.ActionID = ActionRepository.Instance.AddAction(objActionInfo);
                }
                else
                {
                    objActionInfo.CreatedOnDate = action.CreatedOnDate == DateTime.MinValue ? DateTime.Now : action.CreatedOnDate;
                    objActionInfo.CreatedByUserID = action.CreatedByUserID;

                    ActionRepository.Instance.UpdateAction(objActionInfo);
                }

                ActionParamRepository.Instance.DeleteParams(action.ActionID);

                foreach (var objActionParamInfo in action.Params ?? Enumerable.Empty<ActionParamInfo>())
                {
                    objActionParamInfo.ActionID = action.ActionID;

                    ActionParamRepository.Instance.AddParam(objActionParamInfo);
                }

                ActionResultRepository.Instance.DeleteResults(action.ActionID);

                foreach (var item in action.Results ?? Enumerable.Empty<ActionResultViewModel>())
                {
                    var objActionResultInfo = new ActionResultInfo()
                    {
                        ResultID = item.ResultID,
                        ActionID = action.ActionID,
                        LeftExpression = item.LeftExpression,
                        EvalType = item.EvalType,
                        RightExpression = item.RightExpression,
                        ExpressionParsingType = item.ExpressionParsingType,
                        GroupName = item.GroupName,
                        Conditions = Newtonsoft.Json.JsonConvert.SerializeObject(item.Conditions)
                    };

                    ActionResultRepository.Instance.AddResult(objActionResultInfo);
                }

                ActionConditionRepository.Instance.DeleteConditions(action.ActionID);

                foreach (var objActionConditionInfo in action.Conditions ?? Enumerable.Empty<ActionConditionInfo>())
                {
                    objActionConditionInfo.ActionID = action.ActionID;

                    ActionConditionRepository.Instance.AddCondition(objActionConditionInfo);
                }

                return Request.CreateResponse(HttpStatusCode.OK, action);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage DeleteAction(GuidDTO postData)
        {
            try
            {
                ActionRepository.Instance.DeleteAction(postData.ID);

                return Request.CreateResponse(HttpStatusCode.OK, new { Success = true });
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region Defined Lists

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetDefinedLists()
        {
            try
            {
                var lists = DefinedListRepository.Instance.GetAllLists();

                var result = DefinedListMapping.GetListsViewModel(lists);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetDefinedListByFieldID(Guid fieldID)
        {
            try
            {
                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                var result = DefinedListMapping.GetListsViewModel(scenarioID).Where(l => l.FieldID == fieldID);

                return Request.CreateResponse(HttpStatusCode.OK, result.Any() ? result.First() : null);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage CreateDefinedList(DefinedListInfo list)
        {
            try
            {
                var objDefinedListInfo = new DefinedListInfo()
                {
                    ListID = list.ListID,
                    ScenarioID = list.ScenarioID,
                    ListName = list.ListName,
                    ListType = list.ListType,
                    FieldID = list.FieldID,
                    Description = list.Description,
                    CreatedOnDate = DateTime.Now,
                    CreatedByUserID = this.UserInfo.UserID,
                    LastModifiedOnDate = DateTime.Now,
                    LastModifiedByUserID = this.UserInfo.UserID,
                    ViewOrder = list.ViewOrder
                };

                if (list.FieldID != null)
                {
                    var oldList = DefinedListRepository.Instance.GetListByDependencyID(list.FieldID);
                    if (oldList != null)
                    {
                        oldList.ListName = list.ListName;
                        oldList.ScenarioID = list.ScenarioID;

                        objDefinedListInfo = list = oldList;
                    }
                }

                if (list.ListID == Guid.Empty)
                {
                    list.ListID = DefinedListRepository.Instance.AddList(objDefinedListInfo);
                }

                return Request.CreateResponse(HttpStatusCode.OK, list.ListID);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveDefinedList(DefinedListViewModel list)
        {
            try
            {
                var objDefinedListInfo = DefinedListRepository.Instance.GetList(list.ListID);
                if (objDefinedListInfo == null)
                {
                    objDefinedListInfo = new DefinedListInfo()
                    {
                        ListID = list.ListID,
                        ScenarioID = list.ScenarioID,
                        ListName = list.ListName,
                        ListType = list.ListType,
                        FieldID = list.FieldID,
                        Description = list.Description,
                        CreatedOnDate = list.CreatedOnDate,
                        CreatedByUserID = list.CreatedByUserID,
                        LastModifiedOnDate = DateTime.Now,
                        LastModifiedByUserID = this.UserInfo.UserID,
                        ViewOrder = list.ViewOrder
                    };
                }
                else
                {
                    objDefinedListInfo.ListName = !string.IsNullOrEmpty(list.ListName) ? list.ListName : objDefinedListInfo.ListName;
                    objDefinedListInfo.ViewOrder = list.ViewOrder != 0 ? list.ViewOrder : objDefinedListInfo.ViewOrder;
                }

                if (list.ListID == Guid.Empty)
                {
                    list.CreatedOnDate = DateTime.Now;
                    list.CreatedByUserID = this.UserInfo.UserID;
                    list.ListID = DefinedListRepository.Instance.AddList(objDefinedListInfo);
                }
                else
                    DefinedListRepository.Instance.UpdateList(objDefinedListInfo);

                if (list.Items != null)
                {
                    var oldItems = DefinedListItemRepository.Instance.GetItems(list.ListID);
                    foreach (DefinedListItemInfo item in oldItems.Where(o => list.Items.Select(i => i.ItemID).Contains(o.ItemID) == false))
                    {
                        DefinedListItemRepository.Instance.DeleteItem(item.ItemID);
                    }

                    var items = new List<DefinedListItemInfo>();

                    foreach (DefinedListItemViewModel item in list.Items)
                    {
                        items.AddRange(DefinedListMapping.PopulateListItemsInOneLevel2(null, item));
                    }

                    foreach (DefinedListItemInfo item in items)
                    {
                        item.ListID = list.ListID;

                        if (DefinedListItemRepository.Instance.GetItem(item.ItemID) == null)
                            DefinedListItemRepository.Instance.AddItem(item);
                        else
                            DefinedListItemRepository.Instance.UpdateItem(item);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, list.ListID);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region Module Builder

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetModule(Guid moduleID)
        {
            try
            {
                var scenarios = ScenarioRepository.Instance.GetScenarios();

                var module = ModuleMapping.GetModuleViewModel(moduleID);

                string baseTemplate = string.Empty;
                string adminTemplate = string.Empty;
                string buildedTemplate = string.Empty;

                //if (module != null && module.Skin != null)
                //{
                //    baseTemplate = module.BaseTemplate;

                //    if (string.IsNullOrEmpty(baseTemplate)) baseTemplate = Components.Skins.SkinManager.GetModuleTemplateContent(module.ModuleType, module.Skin.SkinName, module.Template);

                //    adminTemplate = FileUtil.GetFileContent(string.Format("{0}/BusinessEngine/System/Admin-Template-{1}.html", PortalSettings.HomeDirectoryMapPath, module.ModuleID));

                //    buildedTemplate = FileUtil.GetFileContent(string.Format("{0}/BusinessEngine/System/Builded-Template-{1}.html", PortalSettings.HomeDirectoryMapPath, module.ModuleID));

                //    if (string.IsNullOrEmpty(module.CustomCss))
                //        module.CustomCss = FileUtil.GetFileContent(string.Format("{0}/BusinessEngine/System/Template-{1}.css", PortalSettings.HomeDirectoryMapPath, module.ModuleID));
                //}

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Module = module,
                    Scenarios = scenarios,
                    BaseTemplate = baseTemplate,
                    AdminTemplate = adminTemplate,
                    BuildedTemplate = buildedTemplate,
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetModuleBuilder(Guid moduleID)
        {
            try
            {
                var scenarioID = ModuleRepository.Instance.GetModuleScenarioID(moduleID);

                var module = ModuleMapping.GetModuleViewModel(moduleID);

                var parentID = module.ParentID != null ? module.ParentID.Value : module.ModuleID;
                var customResources = PageResourceRepository.Instance.GetPageCustomResources(parentID);

                var skins = SkinRepository.Instance.GetSkins().Select(s =>
                {
                    s.SkinImage = s.SkinImage.Replace("[ModulePath]", "/DesktopModules/BusinessEngine"); return s;
                });

                var fieldTypes = ModuleFieldMappings.GetFieldTypesViewModel();

                var fields = ModuleFieldMappings.GetFieldsViewModel(moduleID);

                var variables = ModuleVariableMapping.GetVariablesViewModel(moduleID);

                var actions = ActionMapping.GetActionsViewModel(moduleID);

                var services = ServiceMapping.GetServicesViewModel(scenarioID);

                var viewModels = ViewModelRepository.Instance.GetViewModels(scenarioID);

                IEnumerable<string> roles = null;
                roles = RoleController.Instance.GetRoles(PortalSettings.PortalId).Cast<RoleInfo>().Select(r => r.RoleName);
                var allUsers = new List<string>();
                allUsers.Add("All Users");
                roles = allUsers.Concat(roles);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Module = module,
                    Skins = skins,
                    FieldTypes = fieldTypes,
                    Fields = fields,
                    Actions = actions,
                    Services = services,
                    ViewModels = viewModels,
                    Variables = variables,
                    Roles = roles,
                    CustomResources = customResources
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetModuleField(Guid fieldID)
        {
            try
            {
                var field = ModuleFieldMappings.GetFieldViewModel(fieldID);

                return Request.CreateResponse(HttpStatusCode.OK, field);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetModuleSkins()
        {
            try
            {
                //var skins = ModuleSkinManager.GetSkins();

                var skins = SkinRepository.Instance.GetSkins().Select(s =>
                {
                    s.SkinImage = s.SkinImage.Replace("[ModulePath]", "/DesktopModules/BusinessEngine"); return s;
                });

                return Request.CreateResponse(HttpStatusCode.OK, skins);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetModuleSkin(string skinName)
        {
            try
            {
                var skin = ModuleSkinManager.GetSkin(skinName);

                return Request.CreateResponse(HttpStatusCode.OK, skin);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetModuleSkinTemplateContents(string layoutTemplatePath = "")
        {
            try
            {
                var layoutTemplate = FileUtil.GetFileContent(HttpContext.Current.Request.MapPath(layoutTemplatePath));

                return Request.CreateResponse(HttpStatusCode.OK, new { LayoutTemplate = layoutTemplate });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetModuleVariables(Guid moduleID)
        {
            try
            {
                var variables = ModuleVariableRepository.Instance.GetVariables(moduleID);

                return Request.CreateResponse(HttpStatusCode.OK, variables);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetModuleViewLinkUrl(Guid moduleID, string moduleWrapper = "", Guid? parentID = null)
        {
            try
            {
                string result = "";

                if (moduleWrapper == "Dashboard" && parentID != null)
                {
                    var dashboard = DashboardRepository.Instance.GetDashboardByModuleID(parentID.Value);
                    var module = DashboardPageModuleRepository.Instance.GetModuleView(moduleID);
                    result = string.Format("/DesktopModules/BusinessEngine/dashboard.aspx?d={0}&page={1}", dashboard.UniqueName, module.PageName);
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveModuleBasicInfo([FromBody] ModuleViewModel module, [FromUri] bool includeAllOptions = true)
        {
            try
            {
                var objModuleInfo = ModuleRepository.Instance.GetModule(module.ModuleID) ?? new ModuleInfo();
                objModuleInfo.ScenarioID = module.ScenarioID;
                objModuleInfo.ParentID = module.ParentID;
                objModuleInfo.PortalID = PortalSettings.PortalId;
                objModuleInfo.Wrapper = module.Wrapper;
                objModuleInfo.ModuleType = module.ModuleType;
                objModuleInfo.ModuleName = module.ModuleName;
                objModuleInfo.ModuleTitle = module.ModuleTitle;
                objModuleInfo.DnnModuleID = module.DnnModuleID;
                objModuleInfo.Skin = includeAllOptions ? module.Skin : objModuleInfo.Skin;
                objModuleInfo.Template = includeAllOptions ? module.Template : objModuleInfo.Template;
                objModuleInfo.Theme = includeAllOptions ? module.Theme : objModuleInfo.Theme;
                objModuleInfo.LayoutTemplate = includeAllOptions ? module.LayoutTemplate : objModuleInfo.LayoutTemplate;
                objModuleInfo.LayoutCss = includeAllOptions ? module.LayoutCss : objModuleInfo.LayoutCss;
                objModuleInfo.ModuleBuilderType = module.ModuleBuilderType;
                objModuleInfo.IsSSR = module.IsSSR;
                objModuleInfo.IsDisabledFrontFramework = module.IsDisabledFrontFramework;
                objModuleInfo.Version = objModuleInfo.Version;
                objModuleInfo.Description = module.Description;
                objModuleInfo.LastModifiedOnDate = DateTime.Now;
                objModuleInfo.LastModifiedByUserID = this.UserInfo.UserID;
                objModuleInfo.Settings = module.Settings != null && module.Settings.Count > 0 ? JsonConvert.SerializeObject(module.Settings) : null;

                if (module.ModuleBuilderType == "HtmlEditor")
                {
                    var scenario = ScenarioRepository.Instance.GetScenario(module.ScenarioID);

                    FileUtil.CreateTextFile(string.Format("{0}/BusinessEngine/{1}/module--{2}/custom.html", PortalSettings.HomeSystemDirectoryMapPath, scenario.ScenarioName, module.ModuleName), module.CustomHtml, true);
                    FileUtil.CreateTextFile(string.Format("{0}/BusinessEngine/{1}/module--{2}/custom.js", PortalSettings.HomeSystemDirectoryMapPath, scenario.ScenarioName, module.ModuleName), module.CustomJs, true);
                    FileUtil.CreateTextFile(string.Format("{0}/BusinessEngine/{1}/module--{2}/custom.css", PortalSettings.HomeSystemDirectoryMapPath, scenario.ScenarioName, module.ModuleName), module.CustomCss, true);

                    if (module.ParentID == null)
                    {
                        FileUtil.DeleteFile(string.Format("{0}/BusinessEngine/{1}/module--{2}.html", PortalSettings.HomeSystemDirectoryMapPath, scenario.ScenarioName, module.ModuleName), false);
                        FileUtil.DeleteFile(string.Format("{0}/BusinessEngine/{1}/module--{2}.js", PortalSettings.HomeSystemDirectoryMapPath, scenario.ScenarioName, module.ModuleName), false);
                        FileUtil.DeleteFile(string.Format("{0}/BusinessEngine/{1}/module--{2}.css", PortalSettings.HomeSystemDirectoryMapPath, scenario.ScenarioName, module.ModuleName), false);
                        FileUtil.DeleteFile(string.Format("{0}/BusinessEngine/{1}/module-fields--{2}.css", PortalSettings.HomeSystemDirectoryMapPath, scenario.ScenarioName, module.ModuleName), false);
                    }

                    module.LayoutTemplate = objModuleInfo.LayoutTemplate = string.Empty;
                    module.LayoutCss = objModuleInfo.LayoutCss = string.Empty;
                }

                if (objModuleInfo.ModuleID == Guid.Empty)
                {
                    objModuleInfo.CreatedOnDate = DateTime.Now;
                    objModuleInfo.CreatedByUserID = this.UserInfo.UserID;

                    objModuleInfo.ModuleID = ModuleRepository.Instance.AddModule(objModuleInfo);
                }
                else
                {
                    ModuleRepository.Instance.UpdateModule(objModuleInfo);
                }

                ModuleRepository.Instance.UpdateModuleVersion(objModuleInfo.ModuleID);

                DataCache.ClearCache("BEModule_" + objModuleInfo.ModuleID);
                DataCache.ClearCache("BEModuleFieldsView_" + objModuleInfo.ModuleID);

                return Request.CreateResponse(HttpStatusCode.OK, objModuleInfo.ModuleID);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveRenderedModule(RenderedModuleDTO postData)
        {
            try
            {
                var moduleGuid = postData.ModuleID;

                var module = ModuleRepository.Instance.GetModuleView(moduleGuid);

                FileUtil.CreateTextFile(string.Format("{0}/BusinessEngine/{1}/module--{2}.html", PortalSettings.HomeSystemDirectoryMapPath, module.ScenarioName, module.ModuleName), postData.ModuleTemplate, true);

                if (postData.IsRenderScriptsAndStyles)
                {
                    var parentModuleID = postData.ParentID != null ? module.ParentID.Value : module.ModuleID;
                    var parentModule = parentModuleID == moduleGuid ? module : ModuleRepository.Instance.GetModuleView(parentModuleID);

                    var moduleStyles = ModuleBuilder.GetModuleStyles(parentModuleID);
                    FileUtil.CreateTextFile(string.Format("{0}/BusinessEngine/{1}/module--{2}.css", PortalSettings.HomeSystemDirectoryMapPath, parentModule.ScenarioName, parentModule.ModuleName), moduleStyles, true);

                    var fieldsStyles = ModuleBuilder.GetModuleFieldsStyles(parentModuleID);
                    FileUtil.CreateTextFile(string.Format("{0}/BusinessEngine/{1}/module-fields--{2}.css", PortalSettings.HomeSystemDirectoryMapPath, parentModule.ScenarioName, parentModule.ModuleName), fieldsStyles, true);

                    var fieldScripts = ModuleBuilder.GetModuleFieldsScripts(parentModuleID);
                    FileUtil.CreateTextFile(string.Format("{0}/BusinessEngine/{1}/module--{2}.js", PortalSettings.HomeSystemDirectoryMapPath, parentModule.ScenarioName, parentModule.ModuleName), fieldScripts, true);

                    var actionScripts = ModuleBuilder.GetModuleActionsScripts(parentModuleID);
                    FileUtil.CreateTextFile(string.Format("{0}/BusinessEngine/{1}/module-action--{2}.js", PortalSettings.HomeSystemDirectoryMapPath, parentModule.ScenarioName, parentModule.ModuleName), actionScripts, true);

                    int? tabID = ModuleRepository.Instance.GetModuleTabID(parentModule.DnnModuleID.Value);

                    var resources = ModuleBuilder.GetModuleResources(parentModule);

                    var fieldResources = ModuleBuilder.GetModuleFieldsLibraryResources(parentModuleID, tabID);
                    foreach (var item in fieldResources)
                    {
                        item.LoadOrder = resources.Count;

                        resources.Add(item);
                    }

                    string moduleStylesFile = string.Format("{0}/BusinessEngine/{1}/module--{2}.css", PortalSettings.HomeSystemDirectory, parentModule.ScenarioName, parentModule.ModuleName);
                    resources.Add(new PageResourceInfo()
                    {
                        ResourceType = "css",
                        FilePath = moduleStylesFile,
                        LoadOrder = resources.Count
                    });

                    string moduleFieldsStylesFile = string.Format("{0}/BusinessEngine/{1}/module-fields--{2}.css", PortalSettings.HomeSystemDirectory, parentModule.ScenarioName, parentModule.ModuleName);
                    resources.Add(new PageResourceInfo()
                    {
                        ResourceType = "css",
                        FilePath = moduleFieldsStylesFile,
                        LoadOrder = resources.Count
                    });

                    string moduleScriptsFile = string.Format("{0}/BusinessEngine/{1}/module--{2}.js", PortalSettings.HomeSystemDirectory, parentModule.ScenarioName, parentModule.ModuleName);
                    resources.Add(new PageResourceInfo()
                    {
                        ResourceType = "js",
                        FilePath = moduleScriptsFile,
                        LoadOrder = resources.Count
                    });

                    PageResourceRepository.Instance.DeletePageResources(parentModuleID);

                    foreach (var item in resources ?? Enumerable.Empty<PageResourceInfo>())
                    {
                        PageResourceRepository.Instance.AddPageResource(new PageResourceInfo()
                        {
                            CmsPageID = tabID != null ? tabID.ToString() : null,
                            ModuleID = parentModuleID,
                            ResourceType = item.ResourceType,
                            FilePath = item.FilePath,
                            Version = module.Version.ToString(),
                            LoadOrder = item.LoadOrder,
                        });
                    }

                    ModuleRepository.Instance.UpdateModuleVersion(parentModuleID);
                }

                ModuleRepository.Instance.UpdateModuleVersion(moduleGuid);

                DataCache.ClearCache("BEModule_");
                DataCache.ClearCache("BEModuleFieldsView_");
                DataCache.ClearCache("BEDashboardModuleTemplate_");

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveCustomResources([FromBody] IEnumerable<PageResourceInfo> resources, [FromUri] Guid moduleID)
        {
            try
            {
                PageResourceRepository.Instance.DeleteCustomPageResources(moduleID);

                var module = ModuleRepository.Instance.GetModule(moduleID);

                int? tabID = ModuleRepository.Instance.GetModuleTabID(module.DnnModuleID.Value);

                foreach (var item in resources ?? Enumerable.Empty<PageResourceInfo>())
                {
                    PageResourceRepository.Instance.AddPageResource(new PageResourceInfo()
                    {
                        ModuleID = moduleID,
                        CmsPageID = tabID != null ? tabID.ToString() : null,
                        ResourceType = item.ResourceType,
                        IsCustomResource = true,
                        FilePath = item.FilePath,
                        Version = module.Version.ToString(),
                        LoadOrder = item.LoadOrder,
                    });
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveModuleVariables([FromUri] Guid moduleID, [FromBody] IEnumerable<ModuleVariableInfo> moduleVariables)
        {
            try
            {
                var oldVariables = ModuleVariableRepository.Instance.GetVariables(moduleID);

                var mustDeleted = oldVariables.Where(v => moduleVariables.Select(vr => vr.VariableID).Contains(v.VariableID) == false).Select(v => v.VariableID);
                ModuleVariableRepository.Instance.DeleteVariables(mustDeleted);

                foreach (var objModuleVariableInfo in moduleVariables)
                {
                    objModuleVariableInfo.ModuleID = moduleID;
                    objModuleVariableInfo.LastModifiedOnDate = DateTime.Now;
                    objModuleVariableInfo.LastModifiedByUserID = this.UserInfo.UserID;

                    if (objModuleVariableInfo.VariableType != "viewModel" && objModuleVariableInfo.VariableType != "listOfViewModel") objModuleVariableInfo.ViewModelID = null;

                    if (objModuleVariableInfo.VariableID == Guid.Empty)
                    {
                        objModuleVariableInfo.CreatedOnDate = DateTime.Now;
                        objModuleVariableInfo.CreatedByUserID = this.UserInfo.UserID;

                        ModuleVariableRepository.Instance.AddVariable(objModuleVariableInfo);
                    }
                    else
                        ModuleVariableRepository.Instance.UpdateVariable(objModuleVariableInfo);
                }

                moduleVariables = ModuleVariableRepository.Instance.GetVariables(moduleID);

                return Request.CreateResponse(HttpStatusCode.OK, moduleVariables);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveModuleField(ModuleFieldViewModel field)
        {
            try
            {
                var objFieldInfo = new ModuleFieldInfo()
                {
                    ModuleID = field.ModuleID,
                    FieldID = field.FieldID,
                    FieldType = field.FieldType,
                    FieldName = field.FieldName,
                    PaneName = field.PaneName,
                    Template = field.Template,
                    ParentID = field.ParentID,
                    FieldText = field.FieldText,
                    IsGroup = field.IsGroup,
                    IsValuable = field.IsValuable,
                    IsRequired = field.IsRequired,
                    IsShow = field.IsShow,
                    IsEnabled = field.IsEnabled,
                    IsSelective = field.IsSelective,
                    IsJsonValue = field.IsJsonValue,
                    AuthorizationViewField = field.AuthorizationViewField != null ? string.Join(",", field.AuthorizationViewField) : string.Empty,
                    ShowConditions = field.ShowConditions != null ? JsonConvert.SerializeObject(field.ShowConditions) : null,
                    FieldValues = field.FieldValues != null ? JsonConvert.SerializeObject(field.FieldValues) : null,
                    Description = field.Description,
                    ViewOrder = field.ViewOrder,
                };

                objFieldInfo.LastModifiedOnDate = field.LastModifiedOnDate = DateTime.Now;
                objFieldInfo.LastModifiedByUserID = field.LastModifiedByUserID = this.UserInfo.UserID;

                if (field.FieldID == Guid.Empty)
                {
                    objFieldInfo.CreatedOnDate = field.CreatedOnDate = DateTime.Now;
                    objFieldInfo.CreatedByUserID = field.CreatedByUserID = this.UserInfo.UserID;

                    field.FieldID = objFieldInfo.FieldID = ModuleFieldRepository.Instance.AddField(objFieldInfo);
                }
                else
                {
                    objFieldInfo.CreatedOnDate = field.CreatedOnDate == DateTime.MinValue ? DateTime.Now : field.CreatedOnDate;
                    objFieldInfo.CreatedByUserID = field.CreatedByUserID;

                    ModuleFieldRepository.Instance.UpdateField(objFieldInfo);
                }

                if (field.Settings != null)
                {
                    ModuleFieldSettingRepository.Instance.DeleteFieldSettings(field.FieldID);
                    foreach (var key in field.Settings.Keys)
                    {
                        if (field.Settings[key] != null)
                        {
                            string value = field.Settings[key].ToString();
                            bool isBoolean = value.ToLower() == "true" || value.ToLower() == "false";

                            var setting = new ModuleFieldSettingInfo()
                            {
                                FieldID = field.FieldID,
                                SettingName = key.ToString(),
                                SettingValue = !isBoolean ? value : value.ToLower(),
                            };

                            if (ModuleFieldSettingRepository.Instance.GetFieldSetting(field.FieldID, key.ToString()) == null)
                                ModuleFieldSettingRepository.Instance.AddFieldSetting(setting);
                        }
                    }
                }

                DataCache.ClearCache("BEModuleFieldsView_" + field.ModuleID);

                return Request.CreateResponse(HttpStatusCode.OK, field);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage DeleteModuleField(GuidDTO postData)
        {
            try
            {
                ModuleFieldRepository.Instance.DeleteField(postData.ID);

                return Request.CreateResponse(HttpStatusCode.OK, new { Success = true });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SortModuleFields(IEnumerable<FieldSortDTO> fields)
        {
            try
            {
                foreach (var field in fields ?? Enumerable.Empty<FieldSortDTO>())
                {
                    var objFieldInfo = ModuleFieldRepository.Instance.GetField(field.FieldID);
                    if (objFieldInfo != null)
                    {
                        objFieldInfo.ViewOrder = field.ViewOrder;
                        ModuleFieldRepository.Instance.UpdateField(objFieldInfo);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { Success = true });
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        //[HttpGet]
        //public HttpResponseMessage GetScenarioModulesAndFields(Guid scenarioID)
        //{
        //    try
        //    {
        //        var modules = ModuleRepository.Instance.GetScenarioModules(scenarioID);

        //        var result = new List<object>();

        //        var skins = SkinManager.GetSkins();

        //        foreach (var module in modules)
        //        {
        //            if (string.IsNullOrEmpty(module.CustomCss))
        //                module.CustomCss = FileUtil.GetFileContent(string.Format("{0}/BusinessEngine/System/Template-{1}.css", PortalSettings.HomeDirectoryMapPath, module.ModuleID));

        //            var skinName = module.ParentID != null ? ModuleRepository.Instance.GetModule(module.ParentID.Value).Skin : module.Skin;

        //            result.Add(new
        //            {
        //                ModuleID = module.ModuleID,
        //                ParentID = module.ParentID,
        //                ModuleType = module.ModuleType,
        //                BaseTemplate = module.BaseTemplate,
        //                CustomCss = module.CustomCss,
        //                Fields = ModuleFieldMappings.GetFieldsViewModel(module.ModuleID),
        //                Skin = skins.FirstOrDefault(s => s.SkinName == skinName),
        //                Theme = module.Theme,
        //                Template = module.Template
        //            });
        //        }

        //        string fieldTypes = FileUtil.GetFileContent(HttpContext.Current.Server.MapPath("~/DesktopModules/BusinessEngine/FieldTypes/_System/_field-types.json"));

        //        return Request.CreateResponse(HttpStatusCode.OK, new { Modules = result, FieldTypes = fieldTypes });
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
        //    }
        //}

        #endregion

        #region Dashboards

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetDashboardSettings()
        {
            return GetDashboardSettings(Guid.Empty);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetDashboardSettings(Guid moduleID)
        {
            try
            {
                var dashboard = DashboardMapping.GetDashboardViewModelByModuleID(moduleID);

                var scenarios = ScenarioRepository.Instance.GetScenarios();

                IEnumerable<string> roles = null;
                roles = RoleController.Instance.GetRoles(PortalSettings.PortalId).Cast<RoleInfo>().Select(r => r.RoleName);
                var allUsers = new List<string>();
                allUsers.Add("All Users");
                roles = allUsers.Concat(roles);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Dashboard = dashboard,
                    Scenarios = scenarios,
                    Roles = roles,
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveDashboard(DashboardViewModel dashboard)
        {
            try
            {
                Guid moduleID;

                SaveModuleBasicInfo(new ModuleViewModel()
                {
                    ModuleID = dashboard.ModuleID,
                    ScenarioID = dashboard.ScenarioID,
                    ModuleType = "Dashboard",
                    Wrapper = "Dnn",
                    ModuleName = dashboard.ModuleName,
                    ModuleTitle = dashboard.ModuleTitle,
                    PortalID = PortalSettings.PortalId,
                    DnnModuleID = dashboard.DnnModuleID,
                    CreatedOnDate = dashboard.CreatedOnDate,
                    CreatedByUserID = dashboard.CreatedByUserID
                }, false).TryGetContentValue<Guid>(out moduleID);

                dashboard.ModuleID = moduleID;

                var objDashboardInfo = new DashboardInfo()
                {
                    DashboardID = dashboard.DashboardID,
                    ModuleID = dashboard.ModuleID,
                    DashboardType = dashboard.DashboardType,
                    AuthorizationViewDashboard = dashboard.AuthorizationViewDashboard != null ? string.Join(",", dashboard.AuthorizationViewDashboard) : null,
                    UniqueName = dashboard.UniqueName,
                };

                if (objDashboardInfo.DashboardID == Guid.Empty)
                    dashboard.DashboardID = DashboardRepository.Instance.AddDashboard(objDashboardInfo);
                else
                    DashboardRepository.Instance.UpdateDashboard(objDashboardInfo);


                return Request.CreateResponse(HttpStatusCode.OK, dashboard);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetDashboardPages(Guid dashboardID)
        {
            try
            {
                var dashboard = DashboardRepository.Instance.GetDashboard(dashboardID);

                var pages = DashboardMapping.GetDashboardPagesViewModel(dashboardID, Guid.Empty);

                IEnumerable<string> roles = null;
                roles = RoleController.Instance.GetRoles(PortalSettings.PortalId).Cast<RoleInfo>().Select(r => r.RoleName);
                var allUsers = new List<string>();
                allUsers.Add("All Users");
                roles = allUsers.Concat(roles);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Dashboard = dashboard,
                    Pages = pages,
                    Roles = roles
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetDashboardPage(Guid moduleID)
        {
            return GetDashboardPage(moduleID, Guid.NewGuid());
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetDashboardPage(Guid moduleID, Guid pageID)
        {
            try
            {
                var dashboard = DashboardRepository.Instance.GetDashboardByModuleID(moduleID);

                var page = DashboardMapping.GetDashboardPageViewModel(pageID);

                var pages = DashboardMapping.GetDashboardPagesViewModel(dashboard.DashboardID, Guid.Empty);

                IEnumerable<string> roles = null;
                roles = RoleController.Instance.GetRoles(PortalSettings.PortalId).Cast<RoleInfo>().Select(r => r.RoleName);
                var allUsers = new List<string>();
                allUsers.Add("All Users");
                roles = allUsers.Concat(roles);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Dashboard = dashboard,
                    Page = page,
                    Pages = pages,
                    Roles = roles
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveDashboardPage(DashboardPageViewModel page)
        {
            try
            {
                var objDashboardPageInfo = new DashboardPageInfo()
                {
                    PageID = page.PageID,
                    DashboardID = page.DashboardID,
                    ParentID = page.ParentID,
                    PageType = page.PageType,
                    PageName = page.PageName,
                    Title = page.Title,
                    Url = page.Url,
                    ExistingPageID = page.ExistingPageID,
                    IsVisible = page.IsVisible,
                    InheritPermissionFromDashboard = page.InheritPermissionFromDashboard,
                    AuthorizationViewPage = page.AuthorizationViewPage != null ? string.Join(",", page.AuthorizationViewPage) : null,
                    Settings = page.Settings != null ? JsonConvert.SerializeObject(page.Settings) : null,
                    Description = page.Description,
                    CreatedOnDate = page.CreatedOnDate,
                    CreatedByUserID = page.CreatedByUserID,
                    LastModifiedOnDate = DateTime.Now,
                    LastModifiedByUserID = this.UserInfo.UserID,
                    ViewOrder = page.ViewOrder
                };

                objDashboardPageInfo.LastModifiedOnDate = page.LastModifiedOnDate = DateTime.Now;
                objDashboardPageInfo.LastModifiedByUserID = page.LastModifiedByUserID = this.UserInfo.UserID;
                if (objDashboardPageInfo.PageID == Guid.Empty)
                {
                    var lastPage = DashboardPageRepository.Instance.GetLastPage(objDashboardPageInfo.ParentID);

                    objDashboardPageInfo.ViewOrder = lastPage != null ? lastPage.ViewOrder + 1 : 1;
                    objDashboardPageInfo.CreatedOnDate = page.CreatedOnDate = DateTime.Now;
                    objDashboardPageInfo.CreatedByUserID = page.CreatedByUserID = this.UserInfo.UserID;


                    page.PageID = DashboardPageRepository.Instance.AddDPage(objDashboardPageInfo);
                }
                else
                {
                    objDashboardPageInfo.CreatedOnDate = page.CreatedOnDate == DateTime.MinValue ? DateTime.Now : page.CreatedOnDate;
                    objDashboardPageInfo.CreatedByUserID = page.CreatedByUserID;

                    DashboardPageRepository.Instance.UpdatePage(objDashboardPageInfo);
                }

                var pages = DashboardMapping.GetDashboardPagesViewModel(page.DashboardID, Guid.Empty);

                return Request.CreateResponse(HttpStatusCode.OK, new { Page = page, Pages = pages });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SortDashboardPages(DashboardPageSortDTO postData)
        {
            try
            {
                if (postData.MovedPage != null)
                {
                    var objDashboardPageInfo = DashboardPageRepository.Instance.GetPage(postData.MovedPage.PageID);
                    objDashboardPageInfo.ParentID = postData.MovedPage.ParentID;
                    objDashboardPageInfo.ViewOrder = postData.MovedPage.ViewOrder;
                    DashboardPageRepository.Instance.UpdatePage(objDashboardPageInfo);
                }

                int index = 0;
                foreach (var pageID in postData.SortedPageIDs ?? Enumerable.Empty<Guid>())
                {
                    var objDashboardPageInfo = DashboardPageRepository.Instance.GetPage(pageID);
                    objDashboardPageInfo.ViewOrder = index++;
                    DashboardPageRepository.Instance.UpdatePage(objDashboardPageInfo);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage DeleteDashboardPage(DashboardPageViewModel page)
        {
            try
            {
                DashboardPageRepository.Instance.DeletePage(page.PageID);

                var pages = DashboardMapping.GetDashboardPagesViewModel(page.DashboardID, Guid.Empty);

                return Request.CreateResponse(HttpStatusCode.OK, pages);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveDashboardPageModule(DashboardPageModuleView pageModule)
        {
            try
            {
                var dashboard = DashboardRepository.Instance.GetDashboardView(pageModule.DashboardID);

                if (pageModule.ModuleID == Guid.Empty)
                    pageModule.ModuleID = ModuleRepository.Instance.AddModule(dashboard.ScenarioID, dashboard.ModuleID, pageModule.ModuleType, pageModule.ModuleName, pageModule.ModuleTitle, PortalSettings.PortalId, null, this.UserInfo.UserID, "Dashboard");
                else
                {
                    var module = ModuleRepository.Instance.GetModule(pageModule.ModuleID);
                    module.ScenarioID = dashboard.ScenarioID;
                    module.ModuleType = pageModule.ModuleType;
                    module.ModuleBuilderType = pageModule.ModuleBuilderType;
                    module.ModuleName = pageModule.ModuleName;
                    module.ModuleTitle = pageModule.ModuleTitle;
                    module.ParentID = dashboard.ModuleID;
                    ModuleRepository.Instance.UpdateModule(module);
                }

                var objDashboardPageModuleInfo = new DashboardPageModuleInfo()
                {
                    PageModuleID = pageModule.PageModuleID,
                    PageID = pageModule.PageID,
                    ModuleID = pageModule.ModuleID,
                    ViewOrder = pageModule.ViewOrder
                };

                if (pageModule.PageModuleID == Guid.Empty)
                    pageModule.PageModuleID = DashboardPageModuleRepository.Instance.AddModule(objDashboardPageModuleInfo);
                else
                    DashboardPageModuleRepository.Instance.UpdateModule(objDashboardPageModuleInfo);

                return Request.CreateResponse(HttpStatusCode.OK, pageModule);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage DeleteDashboardPageModule(GuidDTO postData)
        {
            try
            {
                DashboardPageModuleRepository.Instance.DeleteModuleByModuleID(postData.ID);
                ModuleRepository.Instance.DeleteModule(postData.ID);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region Forms

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetFormModuleSettings()
        {
            return GetFormModuleSettings(Guid.Empty);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetFormModuleSettings(Guid moduleID)
        {
            try
            {
                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                var form = ModuleMapping.GetModuleViewModel(moduleID);

                var scenarios = ScenarioRepository.Instance.GetScenarios();

                var viewModels = ViewModelRepository.Instance.GetViewModels(scenarioID);

                IEnumerable<string> roles = null;
                roles = RoleController.Instance.GetRoles(PortalSettings.PortalId).Cast<RoleInfo>().Select(r => r.RoleName);
                var allUsers = new List<string>();
                allUsers.Add("All Users");
                roles = allUsers.Concat(roles);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Form = form,
                    Scenarios = scenarios,
                    ViewModels = viewModels,
                    Roles = roles,
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveFormModule(ModuleViewModel form)
        {
            try
            {
                Guid moduleID;

                SaveModuleBasicInfo(new ModuleViewModel()
                {
                    ModuleID = form.ModuleID,
                    ScenarioID = form.ScenarioID,
                    ParentID = form.ParentID,
                    ViewModelID = form.ViewModelID,
                    PortalID = PortalSettings.PortalId,
                    DnnModuleID = form.DnnModuleID,
                    ModuleType = "Form",
                    Wrapper = form.Wrapper,
                    ModuleName = form.ModuleName,
                    ModuleTitle = form.ModuleTitle,
                    Settings = form.Settings,
                    ModuleBuilderType = form.ModuleBuilderType,
                    IsSSR = form.IsSSR,
                    IsDisabledFrontFramework = form.IsDisabledFrontFramework,
                    CustomHtml = form.CustomHtml,
                    CustomJs = form.CustomJs,
                    CustomCss = form.CustomCss,
                    CreatedOnDate = form.CreatedOnDate,
                    CreatedByUserID = form.CreatedByUserID
                }, false).TryGetContentValue<Guid>(out moduleID);

                form.ModuleID = moduleID;

                if (form.ViewModelID != null)
                {
                    var viewModel = ViewModelRepository.Instance.GetViewModel(form.ViewModelID.Value);

                    if (!ModuleVariableRepository.Instance.GetVariables(form.ModuleID).Any(v => v.IsSystemVariable))
                    {
                        ModuleVariableRepository.Instance.AddVariable(new ModuleVariableInfo()
                        {
                            ModuleID = form.ModuleID,
                            VariableName = viewModel.ViewModelName,
                            ViewOrder = -1,
                            Scope = 0,
                            IsSystemVariable = true,
                            VariableType = "viewModel",
                            ViewModelID = viewModel.ViewModelID,
                            CreatedOnDate = DateTime.Now,
                            CreatedByUserID = UserInfo.UserID,
                            LastModifiedOnDate = DateTime.Now,
                            LastModifiedByUserID = UserInfo.UserID,
                            Description = "System Variable(Can not be delete)"
                        });
                    }
                }


                return Request.CreateResponse(HttpStatusCode.OK, form.ModuleID);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region Lists

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetListModuleSettings()
        {
            return GetListModuleSettings(Guid.Empty);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetListModuleSettings(Guid moduleID)
        {
            try
            {
                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                var form = ModuleMapping.GetModuleViewModel(moduleID);

                var scenarios = ScenarioRepository.Instance.GetScenarios();

                var viewModels = ViewModelRepository.Instance.GetViewModels(scenarioID);

                IEnumerable<string> roles = null;
                roles = RoleController.Instance.GetRoles(PortalSettings.PortalId).Cast<RoleInfo>().Select(r => r.RoleName);
                var allUsers = new List<string>();
                allUsers.Add("All Users");
                roles = allUsers.Concat(roles);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    List = form,
                    Scenarios = scenarios,
                    ViewModels = viewModels,
                    Roles = roles,
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveListModule(ModuleViewModel list)
        {
            try
            {
                Guid moduleID;

                SaveModuleBasicInfo(new ModuleViewModel()
                {
                    ModuleID = list.ModuleID,
                    ScenarioID = list.ScenarioID,
                    ParentID = list.ParentID,
                    ViewModelID = list.ViewModelID,
                    PortalID = PortalSettings.PortalId,
                    DnnModuleID = list.DnnModuleID,
                    ModuleType = "List",
                    Wrapper = list.Wrapper,
                    ModuleName = list.ModuleName,
                    ModuleTitle = list.ModuleTitle,
                    Settings = list.Settings,
                    ModuleBuilderType = list.ModuleBuilderType,
                    IsSSR = list.IsSSR,
                    IsDisabledFrontFramework = list.IsDisabledFrontFramework,
                    CustomHtml = list.CustomHtml,
                    CustomJs = list.CustomJs,
                    CustomCss = list.CustomCss,
                    CreatedOnDate = list.CreatedOnDate,
                    CreatedByUserID = list.CreatedByUserID
                }, false).TryGetContentValue<Guid>(out moduleID);

                list.ModuleID = moduleID;

                if (list.ViewModelID != null)
                {
                    var viewModel = ViewModelRepository.Instance.GetViewModel(list.ViewModelID.Value);

                    if (!ModuleVariableRepository.Instance.GetVariables(list.ModuleID).Any(v => v.IsSystemVariable))
                    {
                        ModuleVariableRepository.Instance.AddVariable(new ModuleVariableInfo()
                        {
                            ModuleID = list.ModuleID,
                            VariableName = viewModel.ViewModelName,
                            ViewOrder = -1,
                            Scope = 0,
                            IsSystemVariable = true,
                            VariableType = "viewModel",
                            ViewModelID = viewModel.ViewModelID,
                            CreatedOnDate = DateTime.Now,
                            CreatedByUserID = UserInfo.UserID,
                            LastModifiedOnDate = DateTime.Now,
                            LastModifiedByUserID = UserInfo.UserID,
                            Description = "System Variable(Can not be delete)"
                        });
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, list.ModuleID);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region Details

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetDetailsModuleSettings()
        {
            return GetDetailsModuleSettings(Guid.Empty);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetDetailsModuleSettings(Guid moduleID)
        {
            try
            {
                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                var details = ModuleMapping.GetModuleViewModel(moduleID);

                var scenarios = ScenarioRepository.Instance.GetScenarios();

                var viewModels = ViewModelRepository.Instance.GetViewModels(scenarioID);

                IEnumerable<string> roles = null;
                roles = RoleController.Instance.GetRoles(PortalSettings.PortalId).Cast<RoleInfo>().Select(r => r.RoleName);
                var allUsers = new List<string>();
                allUsers.Add("All Users");
                roles = allUsers.Concat(roles);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Details = details,
                    Scenarios = scenarios,
                    ViewModels = viewModels,
                    Roles = roles,
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveDetailsModule(ModuleViewModel details)
        {
            try
            {
                Guid moduleID;

                SaveModuleBasicInfo(new ModuleViewModel()
                {
                    ModuleID = details.ModuleID,
                    ScenarioID = details.ScenarioID,
                    ParentID = details.ParentID,
                    ViewModelID = details.ViewModelID,
                    PortalID = PortalSettings.PortalId,
                    DnnModuleID = details.DnnModuleID,
                    ModuleType = "Details",
                    Wrapper = details.Wrapper,
                    ModuleName = details.ModuleName,
                    ModuleTitle = details.ModuleTitle,
                    Settings = details.Settings,
                    ModuleBuilderType = details.ModuleBuilderType,
                    IsSSR = details.IsSSR,
                    IsDisabledFrontFramework = details.IsDisabledFrontFramework,
                    CustomHtml = details.CustomHtml,
                    CustomJs = details.CustomJs,
                    CustomCss = details.CustomCss,
                    CreatedOnDate = details.CreatedOnDate,
                    CreatedByUserID = details.CreatedByUserID,
                }, false).TryGetContentValue<Guid>(out moduleID);

                details.ModuleID = moduleID;

                if (details.ViewModelID != null)
                {
                    var viewModel = ViewModelRepository.Instance.GetViewModel(details.ViewModelID.Value);

                    if (!ModuleVariableRepository.Instance.GetVariables(details.ModuleID).Any(v => v.IsSystemVariable))
                    {
                        ModuleVariableRepository.Instance.AddVariable(new ModuleVariableInfo()
                        {
                            ModuleID = details.ModuleID,
                            VariableName = viewModel.ViewModelName,
                            ViewOrder = -1,
                            Scope = 0,
                            IsSystemVariable = true,
                            VariableType = "viewModel",
                            ViewModelID = viewModel.ViewModelID,
                            CreatedOnDate = DateTime.Now,
                            CreatedByUserID = UserInfo.UserID,
                            LastModifiedOnDate = DateTime.Now,
                            LastModifiedByUserID = UserInfo.UserID,
                            Description = "System Variable(Can not be delete)"
                        });
                    }
                }


                return Request.CreateResponse(HttpStatusCode.OK, details.ModuleID);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        #region Extensions

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> UploadExtensionPackage()
        {
            try
            {
                if (!this.UserInfo.IsSuperUser)
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Problem!. the current user for installing extension must be superuser!");

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    string fileName = HttpContext.Current.Request.Files[0].FileName;
                    if (!Host.AllowedExtensionWhitelist.AllowedExtensions.Contains(Path.GetExtension(fileName).ToLower()))
                        throw new Exception("File type not allowed");
                }

                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());
                var scenarioName = ScenarioRepository.Instance.GetScenarioName(scenarioID);

                if (Request.Content.IsMimeMultipartContent())
                {
                    var uploadPath = this.PortalSettings.HomeSystemDirectoryMapPath + @"BusinessEngine\Temp";
                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                    var streamProvider = new CustomMultipartFormDataStreamProviderChangeFileName(uploadPath);

                    await Request.Content.ReadAsMultipartAsync(streamProvider);

                    string filename = streamProvider.FileData[0].LocalFileName;

                    var extensionUnzipedPath = this.PortalSettings.HomeSystemDirectoryMapPath + @"BusinessEngine\Temp\" + scenarioName + @"\" + Path.GetFileNameWithoutExtension(filename);
                    FastZip fastZip = new FastZip();
                    fastZip.ExtractZip(filename, extensionUnzipedPath, null); //Will always overwrite if target filenames already exist

                    File.Delete(filename);

                    var files = Directory.GetFiles(extensionUnzipedPath);
                    var manifestFile = files.FirstOrDefault(f => Path.GetFileName(f) == "manifest.json");

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        ExtensionJson = FileUtil.GetFileContent(manifestFile),
                        ManifestFilePath = manifestFile,
                        ExtensionUnzipedPath = extensionUnzipedPath
                    });
                }
                else
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Request!");
                    throw new HttpResponseException(response);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage InstallExtension([FromUri] string extensionUnzipedPath, [FromUri] string manifestFilePath)
        {
            try
            {
                var extensionJson = FileUtil.GetFileContent(manifestFilePath);
                var extension = JsonConvert.DeserializeObject<ExtensionManifestInfo>(extensionJson);

                var scenarioID = Guid.Parse(Request.Headers.GetValues("ScenarioID").First());

                var extensionController = new ExtensionService(scenarioID, this.PortalSettings, this.UserInfo);
                extensionController.InstallExtension(extension, extensionUnzipedPath);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion
    }
}
