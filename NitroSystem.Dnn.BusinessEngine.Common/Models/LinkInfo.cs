using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Utilities.Models
{
    public class LinkInfo {
        public string URL { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ImageInfo> Images { get; set; }
    }
}