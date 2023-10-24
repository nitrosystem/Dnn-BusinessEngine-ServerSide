using DotNetNuke.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace NitroSystem.Dnn.BusinessEngine.Models
{
    public class OptionInfo
    {
        public string OptionText { get; set; }
        
        public string OptionValue { get; set; }
    }
}