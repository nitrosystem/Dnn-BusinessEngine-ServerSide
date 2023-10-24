using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Dto
{
    public class DashboardPageSortDTO
    {
        public Guid DashboardID { get; set; }
        public DashboardPageInfo MovedPage { get; set; }
        public IEnumerable<Guid> SortedPageIDs { get; set; }
    }
}
