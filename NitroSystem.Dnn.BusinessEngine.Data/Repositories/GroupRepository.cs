using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;

namespace NitroSystem.Dnn.BusinessEngine.Data.Repositories
{
    public class GroupRepository
    {
        public static GroupRepository Instance
        {
            get
            {
                return new GroupRepository();
            }
        }

        private const string CachePrefix = "BE_Groups_";

        public Guid AddGroup(GroupInfo objGroupInfo)
        {
            objGroupInfo.GroupID = Guid.NewGuid();

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<GroupInfo>();
                rep.Insert(objGroupInfo);

                DataCache.ClearCache(CachePrefix);

                return objGroupInfo.GroupID;
            }
        }

        public Guid CheckExistsGroupOrCreateGroup(Guid scenarioID, string groupType, string groupName)
        {
            var group = GetGroupByName(scenarioID, groupType, groupName);

            if (group == null)
            {
                using (IDataContext ctx = DataContext.Instance())
                {
                    var objGroupInfo = new GroupInfo()
                    {
                        GroupID = Guid.NewGuid(),
                        ScenarioID = scenarioID,
                        GroupType = groupType,
                        GroupName = groupName,
                        CreatedOnDate = DateTime.Now,
                        LastModifiedOnDate = DateTime.Now
                    };

                    var rep = ctx.GetRepository<GroupInfo>();
                    rep.Insert(objGroupInfo);

                    DataCache.ClearCache(CachePrefix);

                    return objGroupInfo.GroupID;
                }
            }
            else
                return group.GroupID;
        }

        public void UpdateGroup(GroupInfo objGroupInfo)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<GroupInfo>();
                rep.Update(objGroupInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public void DeleteGroup(Guid groupID)
        {
            GroupInfo objGroupInfo = GetGroup(groupID);
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<GroupInfo>();
                rep.Delete(objGroupInfo);
            }

            DataCache.ClearCache(CachePrefix);
        }

        public GroupInfo GetGroup(Guid groupID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<GroupInfo>();
                return rep.GetById(groupID);
            }
        }

        public GroupInfo GetGroupByName(Guid scenarioID, string groupType, string groupName)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<GroupInfo>();
                var result = rep.Find("Where ScenarioID =@0 and GroupType =@1 and GroupName =@2", scenarioID, groupType, groupName);
                return result.Any() ? result.First() : null;
            }
        }

        public IEnumerable<GroupInfo> GetGroups(Guid scenarioID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<GroupInfo>();

                return rep.Get(scenarioID);
            }
        }

        public IEnumerable<GroupInfo> GetGroups()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<GroupInfo>();

                return rep.Get();
            }
        }
    }
}