using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Models
{
    public class UploadPhotoInfo
    {
        public string FilePath { get; set; }
        public bool IsMain { get; set; }
        public List<string> Thumbnails { get; set; }
    }
}