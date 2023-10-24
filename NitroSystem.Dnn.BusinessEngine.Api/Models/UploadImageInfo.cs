using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Models
{
    public class UploadImageInfo
    {
        public string FilePath { get; set; }
        public bool IsMain { get; set; }
        public List<string> Thumbnails { get; set; }
    }
}
