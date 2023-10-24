using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Utilities.Models
{
    public class UploadVideoInfo
    {
        public string Duration { get; set; }
        public string ThumbnailPath { get; set; }
        public string FilePath { get; set; }
        public string Watermark { get; set; }
        public string Preloader { get; set; }
    }
}