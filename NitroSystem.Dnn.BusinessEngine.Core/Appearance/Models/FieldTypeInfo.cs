using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Appearance
{
    public class FieldTypeInfo
    {
        public string FieldType { get; set; }
        public IEnumerable<FieldTypeTemplateInfo> Templates { get; set; }
        public IEnumerable<FieldTypeThemeInfo> Themes { get; set; }
    }
}
