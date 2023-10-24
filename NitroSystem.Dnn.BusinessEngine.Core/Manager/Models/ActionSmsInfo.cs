using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Models
{
    public class ActionSmsInfo
    {
        public List<TagInfo> SmsTo { get; set; }
        public string Body { get; set; }
    }
}