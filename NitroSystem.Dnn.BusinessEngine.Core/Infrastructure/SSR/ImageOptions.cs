using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Infrastructure.SSR
{
    public class ImageOptions
    {
        public ImageType Type { get; set; }
        public string PropertyName { get; set; }
        public bool IsArray { get; set; }
        public bool IsMain { get; set; }
        public byte Index { get; set; }
        public bool IsThumbnail { get; set; }
        public string HandlerService { get; set; }
    }
}
