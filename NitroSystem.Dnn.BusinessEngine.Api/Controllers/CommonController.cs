using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Controllers;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Web.Api;
using NitroSystem.Dnn.BusinessEngine.Api.Models;
using NitroSystem.Dnn.BusinessEngine.Components.Enums;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using NitroSystem.Dnn.BusinessEngine.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace NitroSystem.Dnn.BusinessEngine.Api.Controller
{
    public class CommonController : DnnApiController
    {
        #region Common

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> UploadImage()
        {
            var result = new UploadImageInfo() { Thumbnails = new List<string>() };

            try
            {
                var currentRequest = HttpContext.Current.Request;
                string fileName = string.Empty;

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    fileName = HttpContext.Current.Request.Files[0].FileName;
                    if (!Host.AllowedExtensionWhitelist.AllowedExtensions.Contains(Path.GetExtension(fileName).ToLower()))
                        throw new Exception("File type not allowed");
                }

                if (Request.Content.IsMimeMultipartContent())
                {
                    string uploadPath = PortalSettings.HomeDirectory + "BusinessEngine/Images/";

                    var mapPath = HttpContext.Current.Server.MapPath(uploadPath);
                    if (!Directory.Exists(mapPath)) Directory.CreateDirectory(mapPath);

                    if (Path.GetExtension(fileName).ToLower() == ".webp")
                    {
                        var streamProvider = new CustomMultipartFormDataStreamProviderChangeFileName(mapPath);

                        await Request.Content.ReadAsMultipartAsync(streamProvider);

                        foreach (var file in streamProvider.FileData)
                        {
                            result.FilePath = uploadPath + Path.GetFileName(file.LocalFileName);
                        }
                    }
                    else
                    {
                        var streamProvider = new MultipartMemoryStreamProvider();
                        await Request.Content.ReadAsMultipartAsync(streamProvider);

                        var content = streamProvider.Contents[streamProvider.Contents.Count - 1];
                        var stream = content.ReadAsStreamAsync().Result;

                        string imageFile = mapPath + "\\" + Guid.NewGuid().ToString() + Path.GetExtension(fileName);

                        var image = Image.FromStream(stream);
                        int imageWidth = image.Size.Width;
                        int imageHeight = image.Size.Height;

                        //resize large image
                        bool resizeLargeImage = false;
                        bool.TryParse(currentRequest.Params["ResizeLargeImage"], out resizeLargeImage);
                        if (resizeLargeImage)
                        {
                            if (resizeLargeImage)
                            {
                                int largeImageWidth = 1024;
                                int largeImageHeight = 0;

                                int.TryParse(currentRequest.Params["LargeImageWidth"], out largeImageWidth);
                                int.TryParse(currentRequest.Params["LargeImageHeight"], out largeImageHeight);

                                if (imageWidth > largeImageWidth || (largeImageHeight != 0 && imageHeight > largeImageHeight))
                                {
                                    imageWidth = largeImageWidth;
                                    imageHeight = largeImageHeight;
                                }
                            }
                        }

                        image = ImageUtil.ResizeImage(stream, imageFile, imageWidth, imageHeight);

                        result.FilePath = uploadPath + Path.GetFileName(imageFile);

                        //create thumbnail 1
                        bool createThumbnail = false;
                        bool.TryParse(currentRequest.Params["CreateThumbnail1"], out createThumbnail);
                        if (createThumbnail)
                        {
                            int thumbnailWidth = 150;
                            int thumbnailHeight = 0;
                            int.TryParse(currentRequest.Params["Thumbnail1Width"], out thumbnailWidth);
                            int.TryParse(currentRequest.Params["Thumbnail1Height"], out thumbnailHeight);

                            string thumbnailPath = Path.GetDirectoryName(imageFile) + "\\Thumbnails\\";
                            if (!Directory.Exists(thumbnailPath)) Directory.CreateDirectory(thumbnailPath);

                            string thumbnailFileName = Guid.NewGuid() + Path.GetExtension(imageFile);
                            string thumbnailFilePath = thumbnailPath + thumbnailFileName;

                            if (ImageUtil.ResizeImage(stream, thumbnailFilePath, thumbnailWidth, thumbnailHeight) != null)
                                result.Thumbnails.Add(uploadPath + "Thumbnails/" + thumbnailFileName);
                        }

                        //create thumbnail 2
                        createThumbnail = false;
                        bool.TryParse(currentRequest.Params["CreateThumbnail2"], out createThumbnail);
                        if (createThumbnail)
                        {
                            int thumbnailWidth = 150;
                            int thumbnailHeight = 0;
                            int.TryParse(currentRequest.Params["Thumbnail2Width"], out thumbnailWidth);
                            int.TryParse(currentRequest.Params["Thumbnail2Height"], out thumbnailHeight);

                            string thumbnailPath = Path.GetDirectoryName(imageFile) + "\\Thumbnails\\";
                            if (!Directory.Exists(thumbnailPath)) Directory.CreateDirectory(thumbnailPath);

                            string thumbnailFileName = Guid.NewGuid() + Path.GetExtension(imageFile);
                            string thumbnailFilePath = thumbnailPath + thumbnailFileName;

                            if (ImageUtil.ResizeImage(stream, thumbnailFilePath, thumbnailWidth, thumbnailHeight) != null)
                                result.Thumbnails.Add(uploadPath + "Thumbnails/" + thumbnailFileName);
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, result);
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

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> UploadPhoto()
        {
            var result = new UploadImageInfo() { Thumbnails = new List<string>() };

            try
            {
                var currentRequest = HttpContext.Current.Request;
                string fileName = string.Empty;

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    fileName = HttpContext.Current.Request.Files[0].FileName;
                    if (!Host.AllowedExtensionWhitelist.AllowedExtensions.Contains(Path.GetExtension(fileName).ToLower()))
                        throw new Exception("File type not allowed");
                }

                if (Request.Content.IsMimeMultipartContent())
                {
                    string uploadPath = PortalSettings.HomeDirectory + "BusinessEngine/Images/";

                    var mapPath = HttpContext.Current.Server.MapPath(uploadPath);
                    if (!Directory.Exists(mapPath)) Directory.CreateDirectory(mapPath);

                    //Mahmoud => Add .svg
                    if (Path.GetExtension(fileName).ToLower() == ".webp" || Path.GetExtension(fileName).ToLower() == ".svg")
                    {
                        var streamProvider = new CustomMultipartFormDataStreamProviderChangeFileName(mapPath);

                        await Request.Content.ReadAsMultipartAsync(streamProvider);

                        foreach (var file in streamProvider.FileData)
                        {
                            result.FilePath = uploadPath + Path.GetFileName(file.LocalFileName);
                        }
                    }
                    else
                    {
                        var streamProvider = new MultipartMemoryStreamProvider();
                        await Request.Content.ReadAsMultipartAsync(streamProvider);

                        var content = streamProvider.Contents[streamProvider.Contents.Count - 1];
                        var stream = content.ReadAsStreamAsync().Result;

                        string imageFile = mapPath + "\\" + Guid.NewGuid().ToString() + Path.GetExtension(fileName);

                        var image = Image.FromStream(stream);
                        int imageWidth = image.Size.Width;
                        int imageHeight = image.Size.Height;

                        //resize large image
                        bool resizeLargeImage = false;
                        bool.TryParse(currentRequest.Params["ResizeLargeImage"], out resizeLargeImage);
                        if (resizeLargeImage)
                        {
                            if (resizeLargeImage)
                            {
                                int largeImageWidth = 1024;
                                int largeImageHeight = 0;

                                int.TryParse(currentRequest.Params["LargeImageWidth"], out largeImageWidth);
                                int.TryParse(currentRequest.Params["LargeImageHeight"], out largeImageHeight);

                                if (imageWidth > largeImageWidth || (largeImageHeight != 0 && imageHeight > largeImageHeight))
                                {
                                    imageWidth = largeImageWidth;
                                    imageHeight = largeImageHeight;
                                }
                            }
                        }

                        image = ImageUtil.ResizeImage(stream, imageFile, imageWidth, imageHeight);

                        result.FilePath = uploadPath + Path.GetFileName(imageFile);

                        //create thumbnail 1
                        bool createThumbnail = false;
                        bool.TryParse(currentRequest.Params["CreateThumbnail1"], out createThumbnail);
                        if (createThumbnail)
                        {
                            int thumbnailWidth = 150;
                            int thumbnailHeight = 0;
                            int.TryParse(currentRequest.Params["Thumbnail1Width"], out thumbnailWidth);
                            int.TryParse(currentRequest.Params["Thumbnail1Height"], out thumbnailHeight);

                            string thumbnailPath = Path.GetDirectoryName(imageFile) + "\\Thumbnails\\";
                            if (!Directory.Exists(thumbnailPath)) Directory.CreateDirectory(thumbnailPath);

                            string thumbnailFileName = Guid.NewGuid() + Path.GetExtension(imageFile);
                            string thumbnailFilePath = thumbnailPath + thumbnailFileName;

                            if (ImageUtil.ResizeImage(stream, thumbnailFilePath, thumbnailWidth, thumbnailHeight) != null)
                                result.Thumbnails.Add(uploadPath + "Thumbnails/" + thumbnailFileName);
                        }

                        //create thumbnail 2
                        createThumbnail = false;
                        bool.TryParse(currentRequest.Params["CreateThumbnail2"], out createThumbnail);
                        if (createThumbnail)
                        {
                            int thumbnailWidth = 150;
                            int thumbnailHeight = 0;
                            int.TryParse(currentRequest.Params["Thumbnail2Width"], out thumbnailWidth);
                            int.TryParse(currentRequest.Params["Thumbnail2Height"], out thumbnailHeight);

                            string thumbnailPath = Path.GetDirectoryName(imageFile) + "\\Thumbnails\\";
                            if (!Directory.Exists(thumbnailPath)) Directory.CreateDirectory(thumbnailPath);

                            string thumbnailFileName = Guid.NewGuid() + Path.GetExtension(imageFile);
                            string thumbnailFilePath = thumbnailPath + thumbnailFileName;

                            if (ImageUtil.ResizeImage(stream, thumbnailFilePath, thumbnailWidth, thumbnailHeight) != null)
                                result.Thumbnails.Add(uploadPath + "Thumbnails/" + thumbnailFileName);
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, result);
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


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> UploadFile()
        {
            var result = new UploadFileInfo();

            try
            {
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    var fileName = HttpContext.Current.Request.Files[0].FileName;
                    if (!Host.AllowedExtensionWhitelist.AllowedExtensions.Contains(Path.GetExtension(fileName).ToLower()))
                        throw new Exception("File type not allowed");

                    result.FileName = fileName;
                    result.FileType = MimeMapping.GetMimeMapping(fileName);
                }

                if (Request.Content.IsMimeMultipartContent())
                {
                    string uploadPath = PortalSettings.HomeDirectory + "BusinessEngine/Files/";
                    var mapPath = HttpContext.Current.Server.MapPath(uploadPath);
                    if (!Directory.Exists(mapPath)) Directory.CreateDirectory(mapPath);

                    var streamProvider = new CustomMultipartFormDataStreamProviderChangeFileName(mapPath);

                    await Request.Content.ReadAsMultipartAsync(streamProvider);

                    foreach (var file in streamProvider.FileData)
                    {
                        result.FilePath = uploadPath + Path.GetFileName(file.LocalFileName);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, result);
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

        //public IFileInfo UploadDnnFile(string fileName, PortalSettings portalSetting, Stream stream)
        //{
        //    string rootPath = "BusinessEngine";
        //    int portalId = portalSetting.PortalId;

        //    IFolderInfo folder = null;
        //    if (FolderManager.Instance.FolderExists(portalId, rootPath))
        //        folder = FolderManager.Instance.GetFolder(portalId, rootPath);
        //    else
        //    {
        //        var folderMapping = FolderMappingController.Instance.GetFolderMapping(portalId, "Standard");
        //        folder = FolderManager.Instance.AddFolder(new FolderMappingInfo
        //        {
        //            FolderProviderType = folderMapping.FolderProviderType,
        //            FolderMappingID = folderMapping.FolderMappingID,
        //            Priority = 1,
        //            PortalID = portalId,
        //        }, rootPath);
        //    }

        //    var file = FileManager.Instance.AddFile(folder, fileName, stream);
        //    return file;
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<HttpResponseMessage> UploadVideo()
        //{
        //    var result = new UploadVideoInfo();

        //    try
        //    {
        //        var currentRequest = HttpContext.Current.Request;

        //        if (HttpContext.Current.Request.Files.Count > 0)
        //        {
        //            var fileName = HttpContext.Current.Request.Files[0].FileName;
        //            if (!Host.AllowedExtensionWhitelist.AllowedExtensions.Contains(Path.GetExtension(fileName)))
        //                throw new Exception("File type not allowed");
        //        }

        //        if (Request.Content.IsMimeMultipartContent())
        //        {
        //            int id = new Random().Next(1, 9999);
        //            string uploadPath = PortalSettings.HomeDirectory + "BusinessEngine/SubmitEntity/Videos/" + id + "/";

        //            var mapPath = HttpContext.Current.Server.MapPath(uploadPath);
        //            if (!Directory.Exists(mapPath)) Directory.CreateDirectory(mapPath);

        //            string watermark = HttpContext.Current.Request.Params["WatermarkImagePath"];
        //            if (!string.IsNullOrEmpty(watermark)) watermark = currentRequest.MapPath(watermark);

        //            var streamProvider = new CustomMultipartFormDataStreamProviderChangeFileName(mapPath);

        //            await Request.Content.ReadAsMultipartAsync(streamProvider);

        //            foreach (var file in streamProvider.FileData)
        //            {
        //                result = new VideoUtil().SetVideo(currentRequest, file.LocalFileName, watermark);

        //                result.FilePath = uploadPath + Path.GetFileName(file.LocalFileName);
        //                result.ThumbnailPath = uploadPath + "thumbnail.png";
        //                result.Watermark = uploadPath + "watermark.mp4";
        //                result.Preloader = uploadPath + "preloader.mp4";
        //            }

        //            return Request.CreateResponse(HttpStatusCode.OK, result);
        //        }
        //        else
        //        {
        //            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Request!");
        //            throw new HttpResponseException(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<HttpResponseMessage> UploadExcel()
        //{
        //    try
        //    {
        //        var currentRequest = HttpContext.Current.Request;

        //        if (HttpContext.Current.Request.Files.Count > 0)
        //        {
        //            var fileName = HttpContext.Current.Request.Files[0].FileName;
        //            if (!Host.AllowedExtensionWhitelist.AllowedExtensions.Contains(Path.GetExtension(fileName)))
        //                throw new Exception("File type not allowed");
        //        }

        //        if (Request.Content.IsMimeMultipartContent())
        //        {
        //            string uploadPath = PortalSettings.HomeDirectory + "BusinessEngine/SubmitEntity/Excels/";

        //            var mapPath = HttpContext.Current.Server.MapPath(uploadPath);
        //            if (!Directory.Exists(mapPath)) Directory.CreateDirectory(mapPath);

        //            string columns = HttpContext.Current.Request.Params["Columns"];

        //            var streamProvider = new CustomMultipartFormDataStreamProviderChangeFileName(mapPath);

        //            await Request.Content.ReadAsMultipartAsync(streamProvider);

        //            var file = streamProvider.FileData[0];

        //            var result = "";// populateExcelData(file.LocalFileName, columns.Split(',').ToList());

        //            return Request.CreateResponse(HttpStatusCode.OK, result);
        //        }
        //        else
        //        {
        //            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Request!");
        //            throw new HttpResponseException(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        #endregion

        #region Localization

        [DnnAuthorize]
        [ValidateAntiForgeryToken]
        [HttpGet]
        public HttpResponseMessage GetLocaleProperties(string groupName)
        {
            try
            {
                var moduleGuid = Guid.Parse(Request.Headers.GetValues("ModuleGuid").First());

                var module = ModuleRepository.Instance.GetModule(moduleGuid);

                var properties = LocalePropertyRepository.Instance.GetLocalePropertiesByGroupName(Guid.Empty, groupName);

                return Request.CreateResponse(HttpStatusCode.OK, properties);
            }
            catch (Exception ex)
            {
                //Logger.Error(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [DnnAuthorize]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage GetLocalizedProperties(string language, string groupName, int entityID)
        {
            try
            {
                var moduleGuid = Guid.Parse(Request.Headers.GetValues("ModuleGuid").First());

                var module = ModuleRepository.Instance.GetModule(moduleGuid);

                var properties = LocalizedPropertyRepository.Instance.GetLocalizedProperties(language, groupName, entityID);

                return Request.CreateResponse(HttpStatusCode.OK, properties);
            }
            catch (Exception ex)
            {
                //Logger.Error(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [DnnAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveLocalizedProperties([FromBody] IEnumerable<LocalizedPropertyInfo> properties)
        {
            try
            {
                if (properties != null && properties.Any())
                {
                    var firstProperty = properties.First();

                    LocalizedPropertyRepository.Instance.DeleteLocalizedProperties(firstProperty.Language, firstProperty.LocaleKeyGroup, firstProperty.EntityID);

                    foreach (var property in properties)
                    {
                        LocalizedPropertyRepository.Instance.AddLocalizedProperty(property);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //Logger.Error(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion
    }
}