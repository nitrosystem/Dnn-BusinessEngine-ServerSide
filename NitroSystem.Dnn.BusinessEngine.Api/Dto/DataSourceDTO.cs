using NitroSystem.Dnn.BusinessEngine.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Dto
{
   public  class DataSourceDTO
    {
        public Guid FieldID { get; set; }
        public FieldDataSourceInfo DataSource { get; set; }
    }
}
