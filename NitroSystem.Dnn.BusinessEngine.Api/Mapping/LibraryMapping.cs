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
    internal static class LibraryMapping
    {
        #region Library Mapping

        internal static IEnumerable<LibraryViewModel> GetLibrariesViewModel()
        {
            var libraries = LibraryRepository.Instance.GetLibraries();

            return GetLibrariesViewModel(libraries);
        }

        internal static IEnumerable<LibraryViewModel> GetLibrariesViewModel(IEnumerable<LibraryInfo> libraries)
        {
            var result = new List<LibraryViewModel>();

            if (libraries != null)
            {
                foreach (var library in libraries)
                {
                    var libraryDTO = GetLibraryViewModel(library);
                    result.Add(libraryDTO);
                }
            }

            return result;
        }

        internal static LibraryViewModel GetLibraryViewModel(Guid libraryID)
        {
            var objLibraryInfo = LibraryRepository.Instance.GetLibrary(libraryID);

            return GetLibraryViewModel(objLibraryInfo);
        }

        internal static LibraryViewModel GetLibraryViewModel(LibraryInfo objLibraryInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LibraryInfo, LibraryViewModel>()
                .ForMember(dest => dest.Resources, map => map.MapFrom(source => LibraryResourceRepository.Instance.GetResources(source.LibraryID)));
            });

            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<LibraryViewModel>(objLibraryInfo);

            return result;
        }

        #endregion
    }
}