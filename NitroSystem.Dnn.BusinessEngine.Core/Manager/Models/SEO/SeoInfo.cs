using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Models.SEO
{
    public class SeoInfo
    {
        public PageInfo Page { get; set; }
        public IEnumerable<string> StructuredData { get; set; }
        public string MetaTags { get; set; }
    }
}