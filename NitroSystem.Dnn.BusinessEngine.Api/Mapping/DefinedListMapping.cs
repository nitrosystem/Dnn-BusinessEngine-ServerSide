using AutoMapper;
using Newtonsoft.Json;
using NitroSystem.Dnn.BusinessEngine.Api.ViewModels;
using NitroSystem.Dnn.BusinessEngine.Common.TypeCasting;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Framework.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Mapping
{
    internal static class DefinedListMapping
    {
        private static IEnumerable<DefinedListItemInfo> ListItems { get; set; }

        #region Defined List Mapping

        internal static IEnumerable<DefinedListViewModel> GetListsViewModel(Guid scenarioID)
        {
            var definedLists = DefinedListRepository.Instance.GetLists(scenarioID);

            return GetListsViewModel(definedLists);
        }

        internal static IEnumerable<DefinedListViewModel> GetListsViewModel(IEnumerable<DefinedListInfo> definedLists)
        {
            var result = new List<DefinedListViewModel>();

            if (definedLists != null)
            {
                foreach (var definedList in definedLists)
                {
                    var definedListDTO = GetListViewModel(definedList);
                    result.Add(definedListDTO);
                }
            }

            return result;
        }

        internal static DefinedListViewModel GetListViewModel(DefinedListInfo objDefinedListInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DefinedListInfo, DefinedListViewModel>()
                .ForMember(dest => dest.Items, map => map.MapFrom(source => GetListItems(source.ListID)));
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<DefinedListViewModel>(objDefinedListInfo);

            return result;
        }

        public static IEnumerable<DefinedListItemViewModel> GetListItems(Guid listID)
        {
            var result = new List<DefinedListItemViewModel>();

            var items = DefinedListItemRepository.Instance.GetItems(listID).OrderBy(c => c.ViewOrder);
            foreach (var item in items)
            {
                result.Add(GetListItemViewModel(item));
            }

            return result;
        }

        public static IEnumerable<DefinedListItemInfo> GetListItemAndChilds(Guid listID, Guid? rootID, string prefix = "")
        {
            var result = new List<DefinedListItemInfo>();

            ListItems = DefinedListItemRepository.Instance.GetItems(listID).OrderBy(c => c.ViewOrder);

            IEnumerable<DefinedListItemInfo> items;

            if (rootID == null)
            {
                items = ListItems.Where(c => c.ParentValue == null);
            }
            else
            {
                items = ListItems.Where(c => c.ItemID == rootID);
            }

            foreach (var item in items)
            {
                result = PopulateListItemsInOneLevel(result, item, 0, prefix);
            }

            return result;
        }

        public static DefinedListItemViewModel GetListItemViewModel(DefinedListItemInfo objDefinedListItemInfo)
        {
            var item = new DefinedListItemViewModel()
            {
                ItemID = objDefinedListItemInfo.ItemID,
                Text = objDefinedListItemInfo.Text,
                Value = objDefinedListItemInfo.Value,
                ViewOrder = objDefinedListItemInfo.ViewOrder,
                ParentID = objDefinedListItemInfo.ParentValue,
                Items = new List<DefinedListItemViewModel>()
            };

            if (!string.IsNullOrEmpty(objDefinedListItemInfo.Data))
            {
                try
                {
                    item.Data = JsonConvert.DeserializeObject(objDefinedListItemInfo.Data);
                }
                catch
                {
                }
            }

            return item;
        }

        private static DefinedListItemViewModel PopulateListItems(IEnumerable<DefinedListItemInfo> result, DefinedListItemInfo item)
        {
            var newCategory = GetListItemViewModel(item);

            var childs = result.Where(c => c.ParentValue == item.Value);

            foreach (var i in childs)
            {
                newCategory.Items.Add(PopulateListItems(result, i));
            }

            return newCategory;
        }

        public static List<DefinedListItemInfo> PopulateListItemsInOneLevel(List<DefinedListItemInfo> result, DefinedListItemInfo item, int level = 0, string prefix = "")
        {
            item.ItemLevel = level;

            if (!string.IsNullOrEmpty(prefix))
            {
                string _prefix = string.Empty;
                for (int i = 0; i < level; i++)
                {
                    _prefix += prefix;
                }

                item.Text = _prefix + item.Text;
            }

            if (result == null) result = new List<DefinedListItemInfo>();

            result.Add(item);

            var childs = result.Where(c => c.ParentValue == item.Value);
            foreach (var i in childs)
            {
                PopulateListItemsInOneLevel(result, i, level + 1, prefix);
            }

            return result;
        }

        public static List<DefinedListItemInfo> PopulateListItemsInOneLevel2(List<DefinedListItemInfo> result, DefinedListItemViewModel item, int level = 0, string prefix = "")
        {
            item.ItemLevel = level;

            if (!string.IsNullOrEmpty(prefix))
            {
                string _prefix = string.Empty;
                for (int i = 0; i < level; i++)
                {
                    _prefix += prefix;
                }

                item.Text = _prefix + item.Text;
            }

            if (result == null) result = new List<DefinedListItemInfo>();

            if (item.ItemID == Guid.Empty) item.ItemID = Guid.NewGuid();

            result.Add(new DefinedListItemInfo()
            {
                ItemID = item.ItemID,
                ListID = item.ListID,
                Text = item.Text,
                Value = item.Value,
                Data = JsonConvert.SerializeObject(item.Data),
                ItemLevel = item.ItemLevel,
                ParentValue = item.ParentID,
                ViewOrder = item.ViewOrder,
            });

            var childs = result.Where(c => c.ParentValue == item.Value);
            if (item.Items != null)
            {
                foreach (var i in item.Items)
                {
                    i.ParentID = item.Value;

                    PopulateListItemsInOneLevel2(result, i, level + 1, prefix);
                }
            }

            return result;
        }

        #endregion
    }
}