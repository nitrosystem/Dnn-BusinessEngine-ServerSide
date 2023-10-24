using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Models
{
    public class ActionEmailInfo
    {
        public List<TagInfo> EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}