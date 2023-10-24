using AutoMapper;
using Newtonsoft.Json;
using NitroSystem.Dnn.BusinessEngine.Api.ViewModels;
using NitroSystem.Dnn.BusinessEngine.Common.TypeCasting;
using NitroSystem.Dnn.BusinessEngine.Core.Models;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Views;
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
    internal static class DashboardMapping
    {
        private static IEnumerable<DashboardPageInfo> DashboardPages { get; set; }

        #region Dashboard Mapping

        internal static DashboardViewModel GetDashboardViewModel(Guid dashboardID)
        {
            var objDashboardView = DashboardRepository.Instance.GetDashboardView(dashboardID);

            return GetDashboardViewModel(objDashboardView);
        }

        internal static DashboardViewModel GetDashboardViewModelByModuleID(Guid moduleID)
        {
            var objDashboardView = DashboardRepository.Instance.GetDashboardByModuleID(moduleID);

            return GetDashboardViewModel(objDashboardView);
        }

        internal static DashboardViewModel GetDashboardViewModel(DashboardView objDashboardView)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DashboardView, DashboardViewModel>()
                .ForMember(dest => dest.AuthorizationViewDashboard, map => map.MapFrom(source => source.AuthorizationViewDashboard.Split(',')));
                //.ForMember(dest => dest.Skin, map => map.MapFrom(source => new SkinInfo()));
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<DashboardViewModel>(objDashboardView);

            return result;
        }

        #endregion

        #region Dashboard Pages Mapping

        public static IEnumerable<DashboardPageViewModel> GetDashboardPagesViewModel(Guid dashboardID, Guid rootItemID, IEnumerable<DashboardPageInfo> pages = null)
        {
            var result = new List<DashboardPageViewModel>();

            if (pages == null)
                DashboardPages = DashboardPageRepository.Instance.GetPages(dashboardID);
            else
                DashboardPages = pages;

            IEnumerable<DashboardPageInfo> items;
            if (rootItemID == Guid.Empty)
            {
                items = DashboardPages.Where(c => c.ParentID == null);
            }
            else
            {
                items = DashboardPages.Where(c => c.PageID == rootItemID);
            }

            foreach (var item in items)
            {
                result.Add(PopulateDashboardPageItems(item));
            }

            return result;
        }

        public static DashboardPageViewModel GetDashboardPageViewModel(Guid pageID)
        {
            var page = DashboardPageRepository.Instance.GetPage(pageID);

            return page != null ? GetDashboardPageViewModel(page) : null;
        }

        public static DashboardPageViewModel GetDashboardPageViewModel(DashboardPageInfo objDashboardPageInfo)
        {
            Guid pageID = objDashboardPageInfo.PageID;
            if (objDashboardPageInfo.PageType == 2 && objDashboardPageInfo.ExistingPageID != null)
                pageID = objDashboardPageInfo.ExistingPageID.Value;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DashboardPageInfo, DashboardPageViewModel>()
                .ForMember(dest => dest.AuthorizationViewPage, map => map.MapFrom(source => source.AuthorizationViewPage.Split(',')))
                .ForMember(dest => dest.Module, map => map.MapFrom(source => DashboardPageModuleRepository.Instance.GetModuleViewByPageID(pageID)))
                .ForMember(dest => dest.Pages, map => map.MapFrom(source => new List<DashboardPageViewModel>()))
                .ForMember(dest => dest.Settings, map => { map.PreCondition(source => !string.IsNullOrEmpty(source.Settings)); map.MapFrom(source => TypeCastingUtil<IDictionary<string, object>>.TryJsonCasting(source.Settings)); });
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<DashboardPageViewModel>(objDashboardPageInfo);

            return result;
        }

        private static DashboardPageViewModel PopulateDashboardPageItems(DashboardPageInfo page)
        {
            var newPage = GetDashboardPageViewModel(page);

            var childs = DashboardPages.Where(c => c.ParentID == page.PageID);
            foreach (var item in childs)
            {
                var pageViewModel = PopulateDashboardPageItems(item);
                pageViewModel.IsChild = true;
                newPage.Pages.Add(pageViewModel);
            }

            return newPage;
        }

        #endregion
    }
}