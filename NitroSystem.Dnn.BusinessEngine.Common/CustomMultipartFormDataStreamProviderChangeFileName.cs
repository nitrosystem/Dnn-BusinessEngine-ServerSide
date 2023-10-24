﻿using DotNetNuke.Entities.Host;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Utilities
{
    public class CustomMultipartFormDataStreamProviderChangeFileName : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProviderChangeFileName(string path) : base(path) { }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            if (headers.ContentDisposition.FileName == null) return base.GetStream(parent, headers);

            var filename = headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            var fileExtension = Path.GetExtension(filename).ToLower();

            return Host.AllowedExtensionWhitelist.AllowedExtensions.Contains(fileExtension) ? base.GetStream(parent, headers) : Stream.Null;
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            // override the filename which is stored by the provider (by default is bodypart_x)
            string oldfileName = headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(oldfileName);

            return newFileName;
        }
    }
}