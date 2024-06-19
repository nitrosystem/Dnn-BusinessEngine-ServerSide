using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Appearance
{
    public class FieldTypeThemeInfo
    {
        public string ThemeName { get; set; }
        public string ThemeImage { get; set; }
        public string ThemeCssPath { get; set; }
        public string ThemeCssClass { get; set; }
        public bool IsDark { get; set; }
        public string Description { get; set; }
        public int ViewOrder { get; set; }

    }
}
