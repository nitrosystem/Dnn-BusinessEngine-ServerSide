using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Infrastructure.SSR
{
    public class DateTimeType
    {
        public bool IsRelativeData { get; set; }
        public bool IsDateOnly { get; set; }
        public bool IsTimeOnly { get; set; }
        public string Format { get; set; }
    }
}
